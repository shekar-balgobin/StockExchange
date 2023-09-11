using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Tyl.StockExchange.Exchange.ViewModel;

public record class Trade {
	private string tickerSymbol;

	public Trade(int brokerIdentifier, decimal price, double quantity, string tickerSymbol) =>
		(BrokerIdentifier, Price, Quantity, this.tickerSymbol) = (brokerIdentifier, price, quantity, tickerSymbol);

	[DefaultValue("1234")]
	[Required]
	public int BrokerIdentifier { get; init; }

	[DefaultValue("0.99")]
	[Required]
	public decimal Price { get; init; }

	[DefaultValue("1000")]
	[Required]
	public double Quantity { get; init; }

	[DefaultValue("MSFT")]
	[Required]
	[StringLength(maximumLength: TickerSymbolLength, MinimumLength = 1)]
	public string TickerSymbol {
		get => tickerSymbol;
		init => tickerSymbol = value.Trim().ToUpperInvariant();
	}

	public const int TickerSymbolLength = Model.Trade.TickerSymbolLength;
}
