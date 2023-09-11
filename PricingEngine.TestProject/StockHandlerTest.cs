using AutoFixture.Xunit2;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Tyl.CommandQuery.Command;
using Tyl.StockExchange.PricingEngine.Model.MsSqlServer;
using Tyl.StockExchange.PricingEngine.Transaction.Command;

namespace Tyl.StockExchange.PricingEngine.TestProject;

public sealed class StockHandlerTest {
	[InlineAutoData("MSFT")]
	[Theory]
	public async Task Handle_PostEntityCommand(string tickerSymbol) {
		var databaseContextOptions = new DbContextOptionsBuilder<PricingEngineDatabaseContext>()
			.UseInMemoryDatabase(databaseName: "PricingEngine")
			.Options;

		using var databaseContext = new PricingEngineDatabaseContext(databaseContextOptions);
		await databaseContext.TransactionCollection
			.AddRangeAsync(new[] {
				new Model.Transaction(price: 100, quantity: 50, tickerSymbol),
				new Model.Transaction(price:100, quantity: 50, tickerSymbol),
				new Model.Transaction(price:90, quantity: 100, tickerSymbol),
				new Model.Transaction(price:10, quantity: 100, tickerSymbol: "?"),
				new Model.Transaction(price:9, quantity: 100, tickerSymbol: "?"),
			})
			.ConfigureAwait(continueOnCapturedContext: false);

		databaseContext.SaveChanges();

		var handler = new StockHandler(databaseContext);
		var command = new PostEntityCommand<string>(tickerSymbol);
		await handler.Handle(command, CancellationToken.None);

		var stock = databaseContext.StockCollection.Single(s => s.TickerSymbol == tickerSymbol);
		stock.Price.Should().Be(95);
		stock.TickerSymbol.Should().Be(tickerSymbol);
	}
}
