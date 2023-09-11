using AutoFixture.Xunit2;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Tyl.CommandQuery.Command;
using Tyl.CommandQuery.Query;
using Tyl.StockExchange.PricingEngine.WebApplication.Controller;

namespace Tyl.StockExchange.PricingEngine.WebApplication.TestProject.Controller;

public sealed class TransactionControllerTest {
	private readonly Mock<IMediator> mediator = new();

	private readonly TransactionController controller;

	public TransactionControllerTest() {
		controller = new TransactionController(mediator.Object);
	}

	[AutoData]
	[Theory]
	public async Task GetByIdentifier(Model.Transaction transaction) {
		mediator.Setup(m => m.Send(It.IsAny<GetEntityQuery<long, Model.Transaction>>(), It.IsAny<CancellationToken>())).ReturnsAsync(transaction);

		var actual = await controller.GetByIdentifier(transaction.Identifier).ConfigureAwait(continueOnCapturedContext: false);

		actual.Should().BeOfType(typeof(OkObjectResult));

		var actionResult = (OkObjectResult)actual;
		actionResult.StatusCode.Should().Be(StatusCodes.Status200OK);
		actionResult.Value.Should().BeOfType<Model.Transaction>();
	}

	[AutoData]
	[Theory]
	public async Task GetByIdentifier_NotFound(long identifier) {
		var actual = await controller.GetByIdentifier(identifier).ConfigureAwait(continueOnCapturedContext: false);

		actual.Should().BeOfType(typeof(NotFoundResult));
	}

	[AutoData]
	[Theory]
	public async Task Post(Model.Transaction transaction) {
		mediator.Setup(m => m.Send(It.IsAny<PostEntityCommand<Model.Transaction>>(), It.IsAny<CancellationToken>())).ReturnsAsync(1);

		var actual = await controller.Post(transaction).ConfigureAwait(continueOnCapturedContext: false);

		actual.Should().BeOfType(typeof(CreatedAtActionResult));

		var actionResult = (CreatedAtActionResult)actual;
		actionResult.ActionName.Should().Be(nameof(TransactionController.GetByIdentifier));
		actionResult.StatusCode.Should().Be(StatusCodes.Status201Created);
	}
}
