using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace Tyl.StockExchange.Exchange.Model.MsSqlServer;

[ExcludeFromCodeCoverage]
public sealed class ExchangeDatabaseContext :
	DbContext {
	public DbSet<Trade> TradeCollection { get; init; }

	public ExchangeDatabaseContext(DbContextOptions<ExchangeDatabaseContext> databaseContextOptions) :
		base(databaseContextOptions) {
		ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder) {
		base.OnModelCreating(modelBuilder);

		modelBuilder.ApplyConfiguration(new TradeTypeConfiguration());
	}

	public override Task<int> SaveChangesAsync(CancellationToken cancellationToken) {
		var entityEntryCollection = ChangeTracker.Entries().Where(ee => ee.Entity is IAudit);
		foreach (var entityEntry in entityEntryCollection.Where(ee => ee.State == EntityState.Added)) {
			entityEntry.Property(nameof(IAudit.CreatedAt)).CurrentValue = DateTime.UtcNow;
		}

		return base.SaveChangesAsync(cancellationToken);
	}
}
