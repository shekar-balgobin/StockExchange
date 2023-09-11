using System.ComponentModel.DataAnnotations;

namespace Tyl.StockExchange.Exchange.ViewModel;

public sealed record class TradeEntity :
	Trade {
	public TradeEntity(int brokerIdentifier, long identifier, decimal price, double quantity, string tickerSymbol) :
		base(brokerIdentifier, price, quantity, tickerSymbol) =>
		Identifier = identifier;

	[Required]
	public long? Identifier { get; init; }
}
