using AutoFixture.Xunit2;
using AutoMapper;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Tyl.CommandQuery.Command;
using Tyl.CommandQuery.Query;
using Tyl.StockExchange.Exchange.WebApplication.Controller;

namespace Tyl.StockExchange.Exchange.WebApplication.TestProject.Controller;

public sealed class TradeControllerTest {
	private readonly IMapper mapper;

	private readonly Mock<IMediator> mediator = new();

	private readonly TradeController controller;

	public TradeControllerTest() {
		mapper = new MapperConfiguration(mce => {
			mce.CreateMap<Model.Trade, ViewModel.Trade>().ReverseMap();
			mce.CreateMap<Model.Trade, ViewModel.TradeEntity>().ReverseMap();
		}).CreateMapper();

		controller = new TradeController(mapper, mediator.Object);
	}

	[AutoData]
	[Theory]
	public async Task Get(Model.Trade trade) {
		mediator.Setup(m => m.CreateStream(It.IsAny<GetAllEntityCollectionQuery<Model.Trade>>(), It.IsAny<CancellationToken>())).Returns(new[] { trade }.ToAsyncEnumerable());

		await foreach (var tradeEntity in controller.Get().ConfigureAwait(continueOnCapturedContext: false)) {
			tradeEntity.BrokerIdentifier.Should().Be(trade.BrokerIdentifier);
			tradeEntity.Identifier.Should().Be(trade.Identifier);
			tradeEntity.Price.Should().Be(trade.Price);
			tradeEntity.Quantity.Should().Be(trade.Quantity);
			tradeEntity.TickerSymbol.Should().Be(trade.TickerSymbol);
		};
	}

	[AutoData]
	[Theory]
	public async Task GetByIdentifier(Model.Trade trade) {
		mediator.Setup(m => m.Send(It.IsAny<GetEntityQuery<long, Model.Trade>>(), It.IsAny<CancellationToken>())).ReturnsAsync(trade);

		var actual = await controller.GetByIdentifier(trade.Identifier).ConfigureAwait(continueOnCapturedContext: false);

		actual.Should().BeOfType(typeof(OkObjectResult));

		var actionResult = (OkObjectResult)actual;
		actionResult.StatusCode.Should().Be(StatusCodes.Status200OK);
		actionResult.Value.Should().BeOfType<ViewModel.TradeEntity>();
	}

	[AutoData]
	[Theory]
	public async Task GetByIdentifier_NotFound(long identifier) {
		var actual = await controller.GetByIdentifier(identifier).ConfigureAwait(continueOnCapturedContext: false);

		actual.Should().BeOfType(typeof(NotFoundResult));
	}

	[AutoData]
	[Theory]
	public async Task Post(ViewModel.Trade tradeEntity) {
		mediator.Setup(m => m.Send(It.IsAny<PostEntityCommand<Model.Trade>>(), It.IsAny<CancellationToken>())).ReturnsAsync(1);

		var actual = await controller.Post(tradeEntity).ConfigureAwait(continueOnCapturedContext: false);

		actual.Should().BeOfType(typeof(CreatedAtActionResult));

		var actionResult = (CreatedAtActionResult)actual;
		actionResult.ActionName.Should().Be(nameof(TradeController.GetByIdentifier));
		actionResult.StatusCode.Should().Be(StatusCodes.Status201Created);
	}
}
