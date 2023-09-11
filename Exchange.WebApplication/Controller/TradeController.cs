using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using System.Runtime.CompilerServices;
using Tyl.CommandQuery.Command;
using Tyl.CommandQuery.Query;

namespace Tyl.StockExchange.Exchange.WebApplication.Controller;

/// <summary>
/// Trade API.
/// </summary>
[ApiController]
[Consumes(MediaTypeNames.Application.Json)]
[Produces(MediaTypeNames.Application.Json)]
[Route("api/[controller]")]
public sealed class TradeController :
	ControllerBase {
	private readonly IMapper mapper;

	private readonly IMediator mediator;

	/// <summary>
	/// Dependencies.
	/// </summary>
	/// <param name="mapper"></param>
	/// <param name="mediator"></param>
	public TradeController(IMapper mapper, IMediator mediator) =>
		(this.mapper, this.mediator) = (mapper, mediator);

	/// <summary>
	/// Get all trades.
	/// </summary>
	/// <param name="cancellationToken"></param>
	[HttpGet]
	[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IAsyncEnumerable<ViewModel.TradeEntity>))]
	public async IAsyncEnumerable<ViewModel.TradeEntity> Get([EnumeratorCancellation] CancellationToken cancellationToken = default) {
		var query = new GetAllEntityCollectionQuery<Model.Trade>();

		await foreach (var trade in mediator.CreateStream(query, cancellationToken).ConfigureAwait(continueOnCapturedContext: false)) {
			yield return mapper.Map<ViewModel.TradeEntity>(trade);
		}
	}

	/// <summary>
	/// Get trade by identifier.
	/// </summary>
	/// <param name="identifier">Trade identifier</param>
	/// <param name="cancellationToken"></param>
	[HttpGet("{identifier:long}")]
	[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ViewModel.TradeEntity))]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<IActionResult> GetByIdentifier(long identifier, CancellationToken cancellationToken = default) {
		var query = new GetEntityQuery<long, Model.Trade>(identifier);

		var trade = await mediator.Send(query, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);

		return trade is null ? NotFound() : Ok(mapper.Map<ViewModel.TradeEntity>(trade));
	}

	/// <summary>
	/// Post a new trade.
	/// </summary>
	/// <param name="trade"></param>
	/// <param name="cancellationToken"></param>
	[HttpPost]
	[ProducesResponseType(StatusCodes.Status201Created)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<IActionResult> Post(ViewModel.Trade trade, CancellationToken cancellationToken = default) {
		var tradeModel = mapper.Map<Model.Trade>(trade);
		var command = new PostEntityCommand<Model.Trade>(tradeModel);

		await mediator.Send(command, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);

		return CreatedAtAction(nameof(GetByIdentifier), routeValues: new { identifier = tradeModel.Identifier }, null);
	}
}
