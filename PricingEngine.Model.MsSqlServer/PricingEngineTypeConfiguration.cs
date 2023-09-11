using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Diagnostics.CodeAnalysis;

namespace Tyl.StockExchange.PricingEngine.Model.MsSqlServer;

[ExcludeFromCodeCoverage]
internal sealed class PricingEngineTypeConfiguration :
	IEntityTypeConfiguration<Stock> {
	public void Configure(EntityTypeBuilder<Stock> builder) {
		builder.Property(s => s.Price).HasColumnType(typeName: "decimal(19, 4)");
		builder.Property(s => s.TickerSymbol).HasMaxLength(Stock.TickerSymbolLength);
		builder.ToTable(nameof(Stock), tb => tb.IsTemporal()).HasKey(s => s.TickerSymbol);
	}
}
