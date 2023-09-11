using AutoFixture.Xunit2;
using FluentAssertions;
using FluentValidation.Validators;

namespace Tyl.StockExchange.PricingEngine.Model.TestProject;

public sealed class TransactionValidatorTest {
	private readonly TransactionValidator validator = new();

	[InlineAutoData("?")]
	[Theory]
	public void Validate(string tickerSymbol, decimal price, double quantity) {
		var transaction = new Transaction(price, quantity, tickerSymbol);

		var actual = validator.Validate(transaction);

		actual.IsValid.Should().BeTrue();
	}

	[InlineAutoData(1, "?")]
	[InlineAutoData(double.MaxValue, "?")]
	[Theory]
	public void Validate_Quantity_GreaterThanZero(double quantity, string tickerSymbol, decimal price) {
		var transaction = new Transaction(price, quantity, tickerSymbol);

		var actual = validator.Validate(transaction);

		actual.IsValid.Should().BeTrue();
	}

	[InlineAutoData(0, "?")]
	[InlineAutoData(double.MinValue, "?")]
	[Theory]
	public void Validate_Quantity_LessThanOrEqualToZero(double quantity, string tickerSymbol, decimal price) {
		var transaction = new Transaction(price, quantity, tickerSymbol);

		var actual = validator.Validate(transaction);

		actual.Errors.Should().ContainSingle(vf => vf.PropertyName == nameof(Transaction.Quantity) && vf.ErrorCode == nameof(GreaterThanValidator<Transaction, double>));
		actual.IsValid.Should().BeFalse();
	}

	[AutoData]
	[Theory]
	public void Validate_TickerSymbol_Length_MaximumLength(decimal price, double quantity) {
		var trade = new Transaction(price, quantity, new string('?', Transaction.TickerSymbolLength + 1));

		var actual = validator.Validate(trade);

		actual.Errors.Should().ContainSingle(vf => vf.PropertyName == nameof(Transaction.TickerSymbol) && vf.ErrorCode == nameof(MaximumLengthValidator<Transaction>));
		actual.IsValid.Should().BeFalse();
	}

	[InlineAutoData("")]
	[Theory]
	public void Validate_TickerSymbol_Length_MinimumLength(string tickerSymbol, decimal price, double quantity) {
		var trade = new Transaction(price, quantity, tickerSymbol);

		var actual = validator.Validate(trade);

		actual.Errors.Should().ContainSingle(vf => vf.PropertyName == nameof(Transaction.TickerSymbol) && vf.ErrorCode == nameof(MinimumLengthValidator<Transaction>));
		actual.IsValid.Should().BeFalse();
	}
}
