using MediatR;
using inputform.Common.Result;

namespace inputform.Service.Inputform;

public sealed class CreateInputformCommand : IRequest<Result<InputformResponse>>
{
    public InputformRequest Request { get; }
    public CreateInputformCommand(InputformRequest request) => Request = request;
}

public sealed class GetOccupationsQuery
    : IRequest<IReadOnlyList<string>>
{
}

public sealed class InputformHandler : IRequestHandler<CreateInputformCommand, Result<InputformResponse>>
{
    private readonly InputformRepository _repo;

    public InputformHandler(InputformRepository repo) => _repo = repo;

    public async Task<Result<InputformResponse>> Handle(CreateInputformCommand request, CancellationToken ct)
    {
        var dto = await _repo.CreateAsync(request.Request, ct);
        return dto is null
            ? Result<InputformResponse>.Fail("Create failed.")
            : Result<InputformResponse>.Ok(dto);
    }
}


public sealed class GetOccupationsHandler
    : IRequestHandler<GetOccupationsQuery, IReadOnlyList<string>>
{
    public Task<IReadOnlyList<string>> Handle(
        GetOccupationsQuery request,
        CancellationToken ct)
    {
        IReadOnlyList<string> data =
            new[] { "Engineer", "Teacher", "Designer" };

        return Task.FromResult(data);
    }
}