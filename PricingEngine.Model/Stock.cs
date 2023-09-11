namespace Tyl.StockExchange.PricingEngine.Model;

public sealed class Stock {
	private string tickerSymbol;

	public Stock(decimal price, string tickerSymbol) =>
		(Price, this.tickerSymbol) = (price, tickerSymbol);

	public decimal Price { get; init; }

	public string TickerSymbol {
		get => tickerSymbol;
		init => tickerSymbol = value.Trim().ToUpperInvariant();
	}

	public const int TickerSymbolLength = 8;
}
