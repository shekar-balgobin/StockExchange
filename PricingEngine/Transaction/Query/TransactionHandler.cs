using MediatR;
using Microsoft.EntityFrameworkCore;
using Tyl.CommandQuery.Query;
using Tyl.StockExchange.PricingEngine.Model.MsSqlServer;

namespace Tyl.StockExchange.PricingEngine.Transaction.Query;

internal sealed class TransactionHandler :
	IRequestHandler<GetEntityQuery<long, Model.Transaction>, Model.Transaction?> {
	private readonly PricingEngineDatabaseContext databaseContext;

	public TransactionHandler(PricingEngineDatabaseContext databaseContext) =>
		this.databaseContext = databaseContext;

	public async Task<Model.Transaction?> Handle(GetEntityQuery<long, Model.Transaction> request, CancellationToken cancellationToken) =>
		await databaseContext.TransactionCollection.SingleOrDefaultAsync(t => t.Identifier == request.Identifier, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
}
