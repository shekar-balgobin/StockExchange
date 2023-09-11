using FluentValidation;
using MediatR;
using Tyl.CommandQuery.Command;
using Tyl.StockExchange.PricingEngine.Model;
using Tyl.StockExchange.PricingEngine.Model.MsSqlServer;

namespace Tyl.StockExchange.PricingEngine.Transaction.Command;

internal sealed class TransactionHandler :
	IRequestHandler<PostEntityCommand<Model.Transaction>, int> {
	private readonly PricingEngineDatabaseContext databaseContext;

	private readonly IMediator mediator;

	private static readonly TransactionValidator validator = new();

	public TransactionHandler(PricingEngineDatabaseContext databaseContext, IMediator mediator) =>
		(this.databaseContext, this.mediator) = (databaseContext, mediator);

	public async Task<int> Handle(PostEntityCommand<Model.Transaction> request, CancellationToken cancellationToken) {
		var transaction = request.Entity;
		validator.ValidateAndThrow(transaction);
		databaseContext.TransactionCollection.Add(transaction);

		var numberOfStateEntries = await databaseContext.SaveChangesAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);

		var command = new PostEntityCommand<string>(transaction.TickerSymbol);
		await mediator.Send(command, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);

		return numberOfStateEntries;
	}
}
