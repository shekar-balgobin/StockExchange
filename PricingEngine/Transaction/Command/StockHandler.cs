using Tyl.StockExchange.PricingEngine.Model.MsSqlServer;
using Tyl.StockExchange.PricingEngine.Model;
using MediatR;
using Tyl.CommandQuery.Command;
using Microsoft.EntityFrameworkCore;
using FluentValidation;

namespace Tyl.StockExchange.PricingEngine.Transaction.Command;

public sealed class StockHandler :
	IRequestHandler<PostEntityCommand<string>, int> {
	private readonly PricingEngineDatabaseContext databaseContext;

	private static readonly StockValidator validator = new();

	public StockHandler(PricingEngineDatabaseContext databaseContext) =>
		this.databaseContext = databaseContext;

	public async Task<int> Handle(PostEntityCommand<string> request, CancellationToken cancellationToken) {
		var tickerSymbol = request.Entity;

		var transactionCollection = databaseContext.TransactionCollection
			.Where(t => t.TickerSymbol == tickerSymbol)
			.GroupBy(t => t.Price)
			.Select(g => new { Price = g.Key * (decimal)g.Sum(t => t.Quantity), Quantity = g.Sum(t => t.Quantity) });

		var price = transactionCollection.Sum(x => x.Price) / (decimal)transactionCollection.Sum(x => x.Quantity);
		var stock = new Stock(price, tickerSymbol);
		validator.ValidateAndThrow(stock);

		databaseContext.StockCollection.Update(stock);

		int numberOfStateEntries;
		try {
			numberOfStateEntries = await databaseContext.SaveChangesAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
		} catch (DbUpdateConcurrencyException) {
			databaseContext.StockCollection.Add(stock);
			numberOfStateEntries = await databaseContext.SaveChangesAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
		}

		return numberOfStateEntries;
	}
}
