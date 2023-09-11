using AutoFixture.Xunit2;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Tyl.CommandQuery.Query;
using Tyl.StockExchange.PricingEngine.WebApplication.Controller;

namespace Tyl.StockExchange.PricingEngine.WebApplication.TestProject.Controller;

public sealed class StockControllerTest {
	private readonly Mock<IMediator> mediator = new();

	private readonly StockController controller;

	public StockControllerTest() {
		controller = new StockController(mediator.Object);
	}

	[AutoData]
	[Theory]
	public async Task Get(Model.Stock stock) {
		mediator.Setup(m => m.CreateStream(It.IsAny<GetAllEntityCollectionQuery<Model.Stock>>(), It.IsAny<CancellationToken>())).Returns(new[] { stock }.ToAsyncEnumerable());

		await foreach (var item in controller.Get().ConfigureAwait(continueOnCapturedContext: false)) {
			item.Price.Should().Be(stock.Price);
			item.TickerSymbol.Should().Be(stock.TickerSymbol);
		};
	}

	[AutoData]
	[Theory]
	public async Task GetByIdentifier(Model.Stock stock) {
		mediator.Setup(m => m.Send(It.IsAny<GetEntityQuery<string, Model.Stock>>(), It.IsAny<CancellationToken>())).ReturnsAsync(stock);

		var actual = await controller.GetByIdentifier(stock.TickerSymbol).ConfigureAwait(continueOnCapturedContext: false);

		actual.Should().BeOfType(typeof(OkObjectResult));

		var actionResult = (OkObjectResult)actual;
		actionResult.StatusCode.Should().Be(StatusCodes.Status200OK);
		actionResult.Value.Should().BeOfType<Model.Stock>();
	}

	[AutoData]
	[Theory]
	public async Task GetByIdentifier_NotFound(string identifier) {
		var actual = await controller.GetByIdentifier(identifier).ConfigureAwait(continueOnCapturedContext: false);

		actual.Should().BeOfType(typeof(NotFoundResult));
	}
}
