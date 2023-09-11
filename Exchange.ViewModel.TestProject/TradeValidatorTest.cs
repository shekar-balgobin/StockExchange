using AutoFixture.Xunit2;
using FluentAssertions;
using FluentValidation.Validators;

namespace Tyl.StockExchange.Exchange.ViewModel.TestProject;

public sealed class TradeValidatorTest {
	private readonly TradeValidator validator = new();

	[InlineAutoData("?")]
	[Theory]
	public void Validate(string tickerSymbol, int brokerIdentifier, decimal price, double quantity) {
		var trade = new Trade(brokerIdentifier, price, quantity, tickerSymbol);

		var actual = validator.Validate(trade);

		actual.IsValid.Should().BeTrue();
	}

	[InlineAutoData(1, "?")]
	[InlineAutoData(int.MaxValue, "?")]
	[Theory]
	public void Validate_BrokerIdentifier_GreaterThanZero(int brokerIdentifier, string tickerSymbol, decimal price, double quantity) {
		var trade = new Trade(brokerIdentifier, price, quantity, tickerSymbol);

		var actual = validator.Validate(trade);

		actual.IsValid.Should().BeTrue();
	}

	[InlineAutoData(0, "?")]
	[InlineAutoData(int.MinValue, "?")]
	[Theory]
	public void Validate_BrokerIdentifier_LessThanOrEqualToZero(int brokerIdentifier, string tickerSymbol, decimal price, double quantity) {
		var trade = new Trade(brokerIdentifier, price, quantity, tickerSymbol);

		var actual = validator.Validate(trade);

		actual.Errors.Should().ContainSingle(vf => vf.PropertyName == nameof(Trade.BrokerIdentifier) && vf.ErrorCode == nameof(GreaterThanValidator<Trade, decimal>));
		actual.IsValid.Should().BeFalse();
	}

	[InlineAutoData(1, "?")]
	[InlineAutoData(double.MaxValue, "?")]
	[Theory]
	public void Validate_Quantity_GreaterThanZero(double quantity, string tickerSymbol, int brokerIdentifier, decimal price) {
		var trade = new Trade(brokerIdentifier, price, quantity, tickerSymbol);

		var actual = validator.Validate(trade);

		actual.IsValid.Should().BeTrue();
	}

	[InlineAutoData(0, "?")]
	[InlineAutoData(double.MinValue, "?")]
	[Theory]
	public void Validate_Quantity_LessThanOrEqualToZero(double quantity, string tickerSymbol, int brokerIdentifier, decimal price) {
		var trade = new Trade(brokerIdentifier, price, quantity, tickerSymbol);

		var actual = validator.Validate(trade);

		actual.Errors.Should().ContainSingle(vf => vf.PropertyName == nameof(Trade.Quantity) && vf.ErrorCode == nameof(GreaterThanValidator<Trade, double>));
		actual.IsValid.Should().BeFalse();
	}

	[AutoData]
	[Theory]
	public void Validate_TickerSymbol_Length_MaximumLength(int brokerIdentifier, decimal price, double quantity) {
		var trade = new Trade(brokerIdentifier, price, quantity, new string('?', Trade.TickerSymbolLength + 1));

		var actual = validator.Validate(trade);

		actual.Errors.Should().ContainSingle(vf => vf.PropertyName == nameof(Trade.TickerSymbol) && vf.ErrorCode == nameof(MaximumLengthValidator<Trade>));
		actual.IsValid.Should().BeFalse();
	}

	[InlineAutoData("")]
	[Theory]
	public void Validate_TickerSymbol_Length_MinimumLength(string tickerSymbol, int brokerIdentifier, decimal price, double quantity) {
		var trade = new Trade(brokerIdentifier, price, quantity, tickerSymbol);

		var actual = validator.Validate(trade);

		actual.Errors.Should().ContainSingle(vf => vf.PropertyName == nameof(Trade.TickerSymbol) && vf.ErrorCode == nameof(MinimumLengthValidator<Trade>));
		actual.IsValid.Should().BeFalse();
	}
}
