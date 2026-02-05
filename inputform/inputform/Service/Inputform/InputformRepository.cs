using System.Globalization;
using Microsoft.EntityFrameworkCore;
using inputform.Persistence;
using inputform.Persistence.Entities;

namespace inputform.Service.Inputform;

public sealed class InputformRepository
{
    private readonly AppDbContext _db;
    public InputformRepository(AppDbContext db) => _db = db;

    public async Task<InputformResponse?> CreateAsync(InputformRequest request, CancellationToken ct)
    {
        if (request.profile is null || request.profile.Length == 0) return null;

        if (!DateOnly.TryParseExact(request.birth_day, "yyyy-MM-dd",
                CultureInfo.InvariantCulture, DateTimeStyles.None, out var birth))
            return null;

        byte[] imageBytes;
        await using (var ms = new MemoryStream(capacity: (int)Math.Min(request.profile.Length, int.MaxValue)))
        {
            await request.profile.CopyToAsync(ms, ct);
            imageBytes = ms.ToArray();
        }

        var contentType = request.profile.ContentType ?? "application/octet-stream";

        var strategy = _db.Database.CreateExecutionStrategy();
        return await strategy.ExecuteAsync(async () =>
        {
            await using var tx = await _db.Database.BeginTransactionAsync(ct);
            try
            {
                var id = Guid.NewGuid();
                var now = DateTime.UtcNow;

                var entry = new input_form_entry
                {
                    id = id,
                    first_name = request.first_name,
                    last_name = request.last_name,
                    email = request.email,
                    phone = request.phone,
                    occupation = request.occupation,
                    sex = request.sex,
                    birth_day = birth,
                    created_at = now
                };
                _db.input_form_entries.Add(entry);

                var img = new input_form_entry_image
                {
                    entry_id = id,
                    profile_image = imageBytes,
                    content_type = contentType,
                    file_size = imageBytes.Length,
                    created_at = now
                };
                _db.input_form_entry_images.Add(img);

                await _db.SaveChangesAsync(ct);
                await tx.CommitAsync(ct);

                return new InputformResponse { Id = id };
            }
            catch
            {
                await tx.RollbackAsync(ct);
                throw;
            }
        });
    }
}
