using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using System.Runtime.CompilerServices;
using Tyl.CommandQuery.Query;

namespace Tyl.StockExchange.PricingEngine.WebApplication.Controller;

/// <summary>
/// Stock API.
/// </summary>
[ApiController]
[Consumes(MediaTypeNames.Application.Json)]
[Produces(MediaTypeNames.Application.Json)]
[Route("api/[controller]")]
public sealed class StockController :
	ControllerBase {
	private readonly IMediator mediator;

	/// <summary>
	/// Dependencies.
	/// </summary>
	/// <param name="mediator"></param>
	public StockController(IMediator mediator) =>
		this.mediator = mediator;

	/// <summary>
	/// Get all stocks.
	/// </summary>
	/// <param name="cancellationToken"></param>
	[HttpGet]
	[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IAsyncEnumerable<Model.Stock>))]
	public async IAsyncEnumerable<Model.Stock> Get([EnumeratorCancellation] CancellationToken cancellationToken = default) {
		var query = new GetAllEntityCollectionQuery<Model.Stock>();

		await foreach (var stock in mediator.CreateStream(query, cancellationToken).ConfigureAwait(continueOnCapturedContext: false)) {
			yield return stock;
		}
	}

	/// <summary>
	/// Get stock by ticker symbol.
	/// </summary>
	/// <param name="tickerSymbol"></param>
	/// <param name="cancellationToken"></param>
	[HttpGet("{tickerSymbol:length(1, 8)}")]
	[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Model.Stock))]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<IActionResult> GetByIdentifier(string tickerSymbol, CancellationToken cancellationToken = default) {
		var query = new GetEntityQuery<string, Model.Stock>(tickerSymbol);

		var stock = await mediator.Send(query, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);

		return stock is null ? NotFound() : Ok(stock);
	}

	/// <summary>
	/// Get multiple stocks by ticker symbol.
	/// </summary>
	/// <param name="tickerSymbolCollection">Comma separated list of ticker symbols</param>
	/// <param name="cancellationToken"></param>
	[HttpGet("list/{tickerSymbolCollection}")]
	[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Model.Stock))]
	public async IAsyncEnumerable<Model.Stock> GetCollectionByIdentifier(string tickerSymbolCollection, [EnumeratorCancellation] CancellationToken cancellationToken = default) {
		var query = new GetEntityCollectionQuery<string, Model.Stock>(tickerSymbolCollection);

		await foreach (var stock in mediator.CreateStream(query, cancellationToken).ConfigureAwait(continueOnCapturedContext: false)) {
			yield return stock;
		}
	}
}
