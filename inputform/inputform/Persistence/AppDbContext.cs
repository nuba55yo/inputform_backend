using inputform.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace inputform.Persistence;

public partial class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
       : base(options)
    {
    }

    public virtual DbSet<input_form_entry> input_form_entries { get; set; }

    public virtual DbSet<input_form_entry_image> input_form_entry_images { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasPostgresExtension("citext")
            .HasPostgresExtension("pgcrypto");

        modelBuilder.Entity<input_form_entry>(entity =>
        {
            entity.HasKey(e => e.id).HasName("input_form_entries_pkey");

            entity.Property(e => e.id).HasDefaultValueSql("gen_random_uuid()");
            entity.Property(e => e.created_at).HasDefaultValueSql("now()");
        });

        modelBuilder.Entity<input_form_entry_image>(entity =>
        {
            entity.HasKey(e => e.entry_id).HasName("input_form_entry_images_pkey");

            entity.Property(e => e.entry_id).ValueGeneratedNever();
            entity.Property(e => e.created_at).HasDefaultValueSql("now()");

            entity.HasOne(d => d.entry).WithOne(p => p.input_form_entry_image).HasConstraintName("input_form_entry_images_entry_id_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
