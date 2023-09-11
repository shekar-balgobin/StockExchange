using FluentValidation;

namespace Tyl.StockExchange.PricingEngine.Model;

public sealed class StockValidator :
	AbstractValidator<Stock> {
	public StockValidator() {
		RuleFor(s => s.TickerSymbol).MaximumLength(maximumLength: Stock.TickerSymbolLength);
		RuleFor(s => s.TickerSymbol).MinimumLength(minimumLength: 1);
	}
}
