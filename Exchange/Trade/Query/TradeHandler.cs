using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;
using Tyl.CommandQuery.Query;
using Tyl.StockExchange.Exchange.Model.MsSqlServer;

namespace Tyl.StockExchange.Exchange.Trade.Query;

internal sealed class TradeHandler :
	IRequestHandler<GetEntityQuery<long, Model.Trade>, Model.Trade?>,
	IStreamRequestHandler<GetAllEntityCollectionQuery<Model.Trade>, Model.Trade> {
	private readonly ExchangeDatabaseContext databaseContext;

	public TradeHandler(ExchangeDatabaseContext databaseContext) =>
		this.databaseContext = databaseContext;

	public async IAsyncEnumerable<Model.Trade> Handle(GetAllEntityCollectionQuery<Model.Trade> request, [EnumeratorCancellation] CancellationToken cancellationToken) {
		foreach (var trade in databaseContext.TradeCollection) {
			if (cancellationToken.IsCancellationRequested) {
				yield break;
			}

			yield return trade;
		}

		await Task.CompletedTask;
	}

	public async Task<Model.Trade?> Handle(GetEntityQuery<long, Model.Trade> request, CancellationToken cancellationToken) =>
		await databaseContext.TradeCollection.SingleOrDefaultAsync(t => t.Identifier == request.Identifier, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
}
