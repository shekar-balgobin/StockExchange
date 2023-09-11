using AutoFixture.Xunit2;
using FluentAssertions;
using FluentValidation.Validators;

namespace Tyl.StockExchange.PricingEngine.Model.TestProject;

public sealed class StockValidatorTest {
	private readonly StockValidator validator = new();

	[InlineAutoData("?")]
	[Theory]
	public void Validate(string tickerSymbol, decimal price) {
		var stock = new Stock(price, tickerSymbol);

		var actual = validator.Validate(stock);

		actual.IsValid.Should().BeTrue();
	}

	[AutoData]
	[Theory]
	public void Validate_TickerSymbol_Length_TooLong(decimal price) {
		var stock = new Stock(price, new string('?', Stock.TickerSymbolLength + 1));

		var actual = validator.Validate(stock);

		actual.Errors.Should().ContainSingle(vf => vf.PropertyName == nameof(Stock.TickerSymbol) && vf.ErrorCode == nameof(MaximumLengthValidator<Stock>));
		actual.IsValid.Should().BeFalse();
	}

	[InlineAutoData("")]
	[Theory]
	public void Validate_TickerSymbol_Length_TooShort(string tickerSymbol, decimal price) {
		var stock = new Stock(price, tickerSymbol);

		var actual = validator.Validate(stock);

		actual.Errors.Should().ContainSingle(vf => vf.PropertyName == nameof(Stock.TickerSymbol) && vf.ErrorCode == nameof(MinimumLengthValidator<Stock>));
		actual.IsValid.Should().BeFalse();
	}
}
