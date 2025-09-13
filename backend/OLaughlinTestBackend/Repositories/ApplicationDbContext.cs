using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Models.Entities;

public partial class ApplicationDbContext : DbContext
{
    private string _connectionString;
    public IConfiguration Configuration { get; }
    public ApplicationDbContext()
    {
    }

    public ApplicationDbContext(string connectionString)
    {
        _connectionString = connectionString;
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : base(options)
    {
    }

    public virtual DbSet<Customer> Customers { get; set; }
    public virtual DbSet<Appointment> Appointments { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            if (string.IsNullOrEmpty(_connectionString))
            {
                throw new InvalidOperationException("No connection string provided for ApplicationDbContext.");
            }

            // 🔹 SQL Server (ya no MySQL/Pomelo)
            optionsBuilder.UseSqlServer(_connectionString);
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(120);

            entity.Property(e => e.Email)
                .HasMaxLength(255);
        });

        // Appointments
        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(120);

            entity.Property(e => e.Email)
                .HasMaxLength(255);
        });

        // Appointments
        modelBuilder.Entity<Appointment>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.DateTime)
                .IsRequired();

            entity.Property(e => e.Status)
                .IsRequired()
                .HasMaxLength(20);

            entity.HasOne(a => a.Customer)
                .WithMany(c => c.Appointments)
                .HasForeignKey(a => a.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            // Constraint único para evitar doble booking
            entity.HasIndex(a => new { a.CustomerId, a.DateTime })
                .IsUnique();
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}