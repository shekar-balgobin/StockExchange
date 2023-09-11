namespace Tyl.StockExchange.Exchange.Model;

public sealed class Trade : 
	IAudit {
	private string tickerSymbol;

	public Trade(int brokerIdentifier, decimal price, double quantity, string tickerSymbol) =>
		(BrokerIdentifier, Price, Quantity, this.tickerSymbol) = (brokerIdentifier, price, quantity, tickerSymbol);

	public int BrokerIdentifier { get; init; }

	public DateTime CreatedAt { get; init; }

	public long Identifier { get; init; }

	public decimal Price { get; init; }

	public double Quantity { get; init; }

	public string TickerSymbol {
		get => tickerSymbol;
		init => tickerSymbol = value.Trim().ToUpperInvariant();
	}

	public const int TickerSymbolLength = 8;
}
