using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Tyl.StockExchange.PricingEngine.Model;

public sealed class Transaction :
	IAudit {
	private string tickerSymbol;

	public Transaction(decimal price, double quantity, string tickerSymbol) =>
		(Price, Quantity, this.tickerSymbol) = (price, quantity, tickerSymbol);

	[JsonIgnore]
	public DateTime CreatedAt { get; init; }

	[JsonIgnore]
	public long Identifier { get; init; }

	[DefaultValue("0.99")]
	[Required]
	public decimal Price { get; init; }

	[DefaultValue("1000")]
	[Required]
	public double Quantity { get; init; }

	[DefaultValue("MSFT")]
	[Required]
	public string TickerSymbol {
		get => tickerSymbol;
		init => tickerSymbol = value.Trim().ToUpperInvariant();
	}

	public const int TickerSymbolLength = 8;
}
