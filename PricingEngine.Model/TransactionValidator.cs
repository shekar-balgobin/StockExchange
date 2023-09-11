using FluentValidation;

namespace Tyl.StockExchange.PricingEngine.Model;

public sealed class TransactionValidator :
	AbstractValidator<Transaction> {
	public TransactionValidator() {
		RuleFor(t => t.Quantity).GreaterThan(valueToCompare: 0);
		RuleFor(t => t.TickerSymbol).MaximumLength(maximumLength: Transaction.TickerSymbolLength);
		RuleFor(t => t.TickerSymbol).MinimumLength(minimumLength: 1);
	}
}
