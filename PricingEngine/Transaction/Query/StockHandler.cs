using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;
using Tyl.CommandQuery.Query;
using Tyl.StockExchange.PricingEngine.Model;
using Tyl.StockExchange.PricingEngine.Model.MsSqlServer;

namespace Tyl.StockExchange.PricingEngine.Transaction.Query;

internal sealed class StockHandler :
	IRequestHandler<GetEntityQuery<string, Stock>, Stock?>,
	IStreamRequestHandler<GetAllEntityCollectionQuery<Stock>, Stock>,
	IStreamRequestHandler<GetEntityCollectionQuery<string, Stock>, Stock> {
	private readonly PricingEngineDatabaseContext databaseContext;

	public StockHandler(PricingEngineDatabaseContext databaseContext) =>
		this.databaseContext = databaseContext;

	public async IAsyncEnumerable<Stock> Handle(GetAllEntityCollectionQuery<Stock> request, [EnumeratorCancellation] CancellationToken cancellationToken) {
		foreach (var trade in databaseContext.StockCollection) {
			if (cancellationToken.IsCancellationRequested) {
				yield break;
			}

			yield return trade;
		}

		await Task.CompletedTask;
	}

	public async Task<Stock?> Handle(GetEntityQuery<string, Stock> request, CancellationToken cancellationToken) =>
		await databaseContext.StockCollection.SingleOrDefaultAsync(t => t.TickerSymbol == request.Identifier, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);

	public async IAsyncEnumerable<Stock> Handle(GetEntityCollectionQuery<string, Stock> request, [EnumeratorCancellation] CancellationToken cancellationToken) {
		var identifierCollection = request.Identifier.ToUpperInvariant().Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
		foreach (var trade in databaseContext.StockCollection.Where(s => identifierCollection.Contains(s.TickerSymbol))) {
			if (cancellationToken.IsCancellationRequested) {
				yield break;
			}

			yield return trade;
		}

		await Task.CompletedTask;
	}
}
