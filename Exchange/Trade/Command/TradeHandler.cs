using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Json;
using Tyl.CommandQuery.Command;
using Tyl.StockExchange.Exchange.Model;
using Tyl.StockExchange.Exchange.Model.MsSqlServer;

namespace Tyl.StockExchange.Exchange.Trade.Command;

public sealed class TradeHandler :
	IRequestHandler<PostEntityCommand<Model.Trade>, int> {
	private readonly ExchangeDatabaseContext databaseContext;

	private static readonly TradeValidator validator = new();

	private readonly HttpClient httpClient;

	public TradeHandler(IConfiguration configuration, ExchangeDatabaseContext databaseContext, HttpClient httpClient) {
		this.databaseContext = databaseContext;
		this.httpClient = httpClient;

		httpClient.BaseAddress = new Uri(configuration.GetSection("PricingEngine").GetValue<string>("BaseUrl")!);
	}

	public async Task<int> Handle(PostEntityCommand<Model.Trade> request, CancellationToken cancellationToken) {
		var trade = request.Entity;
		validator.ValidateAndThrow(trade);
		databaseContext.TradeCollection.Add(trade);

		var numberOfStateEntries = await databaseContext.SaveChangesAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);

		var transaction = new PricingEngine.Model.Transaction(trade.Price, trade.Quantity, trade.TickerSymbol);
		var httpResponseMessage = await httpClient.PostAsJsonAsync(nameof(PricingEngine.Model.Transaction), transaction, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
		httpResponseMessage.EnsureSuccessStatusCode();

		return numberOfStateEntries;
	}
}
