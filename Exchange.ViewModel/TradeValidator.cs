using FluentValidation;

namespace Tyl.StockExchange.Exchange.ViewModel;

public sealed class TradeValidator :
	AbstractValidator<Trade> {
	public TradeValidator() {
		RuleFor(t => t.BrokerIdentifier).GreaterThan(valueToCompare: 0);
		RuleFor(t => t.Quantity).GreaterThan(valueToCompare: 0);
		RuleFor(t => t.TickerSymbol).MaximumLength(maximumLength: Trade.TickerSymbolLength);
		RuleFor(t => t.TickerSymbol).MinimumLength(minimumLength: 1);
	}
}
