using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Diagnostics.CodeAnalysis;

namespace Tyl.StockExchange.PricingEngine.Model.MsSqlServer;

[ExcludeFromCodeCoverage]
internal sealed class TransactionTypeConfiguration :
	IEntityTypeConfiguration<Transaction> {
	public void Configure(EntityTypeBuilder<Transaction> builder) {
		builder.Property(t => t.Price).HasColumnType(typeName: "decimal(19, 4)");
		builder.Property(t => t.TickerSymbol).HasMaxLength(Transaction.TickerSymbolLength);
		builder.ToTable(nameof(Transaction)).HasKey(t => t.Identifier);
	}
}
