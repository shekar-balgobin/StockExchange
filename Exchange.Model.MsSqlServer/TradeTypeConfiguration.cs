using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Diagnostics.CodeAnalysis;

namespace Tyl.StockExchange.Exchange.Model.MsSqlServer;

[ExcludeFromCodeCoverage]
internal sealed class TradeTypeConfiguration :
	IEntityTypeConfiguration<Trade> {
	public void Configure(EntityTypeBuilder<Trade> builder) {
		builder.Property(t => t.Price).HasColumnType(typeName: "decimal(19, 4)");
		builder.Property(t => t.TickerSymbol).HasMaxLength(Trade.TickerSymbolLength);
		builder.ToTable(nameof(Trade)).HasKey(t => t.Identifier);
	}
}
