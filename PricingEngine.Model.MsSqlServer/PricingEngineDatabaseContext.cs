using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace Tyl.StockExchange.PricingEngine.Model.MsSqlServer;

[ExcludeFromCodeCoverage]
public sealed class PricingEngineDatabaseContext :
	DbContext {
	public DbSet<Stock> StockCollection { get; init; }

	public DbSet<Transaction> TransactionCollection { get; init; }

	public PricingEngineDatabaseContext(DbContextOptions<PricingEngineDatabaseContext> databaseContextOptions) :
	base(databaseContextOptions) {
		ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder) {
		base.OnModelCreating(modelBuilder);

		modelBuilder.ApplyConfiguration(new PricingEngineTypeConfiguration())
			.ApplyConfiguration(new TransactionTypeConfiguration());
	}

	public override Task<int> SaveChangesAsync(CancellationToken cancellationToken) {
		var entityEntryCollection = ChangeTracker.Entries().Where(ee => ee.Entity is IAudit);
		foreach (var entityEntry in entityEntryCollection.Where(ee => ee.State == EntityState.Added)) {
			entityEntry.Property(nameof(IAudit.CreatedAt)).CurrentValue = DateTime.UtcNow;
		}

		return base.SaveChangesAsync(cancellationToken);
	}
}
