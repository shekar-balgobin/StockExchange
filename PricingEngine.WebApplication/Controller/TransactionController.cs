using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using Tyl.CommandQuery.Command;
using Tyl.CommandQuery.Query;

namespace Tyl.StockExchange.PricingEngine.WebApplication.Controller;

/// <summary>
/// Transaction API.
/// </summary>
[ApiController]
[Consumes(MediaTypeNames.Application.Json)]
[Produces(MediaTypeNames.Application.Json)]
[Route("api/[controller]")]
[Tags("Transaction - For internal use only")]
public sealed class TransactionController :
	ControllerBase {
	private readonly IMediator mediator;

	/// <summary>
	/// Dependencies.
	/// </summary>
	/// <param name="mediator"></param>
	public TransactionController(IMediator mediator) =>
		this.mediator = mediator;

	/// <summary>
	/// Get transaction by identifier.
	/// </summary>
	/// <param name="identifier"></param>
	/// <param name="cancellationToken"></param>
	[HttpGet("{identifier:long}")]
	[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Model.Transaction))]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<IActionResult> GetByIdentifier(long identifier, CancellationToken cancellationToken = default) {
		var query = new GetEntityQuery<long, Model.Transaction>(identifier);

		var transaction = await mediator.Send(query, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);

		return transaction is null ? NotFound() : Ok(transaction);
	}

	/// <summary>
	/// Post a new transaction.
	/// </summary>
	/// <param name="transaction"></param>
	/// <param name="cancellationToken"></param>
	[HttpPost]
	[ProducesResponseType(StatusCodes.Status201Created)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<IActionResult> Post(Model.Transaction transaction, CancellationToken cancellationToken = default) {
		var command = new PostEntityCommand<Model.Transaction>(transaction);

		await mediator.Send(command, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);

		return CreatedAtAction(nameof(GetByIdentifier), routeValues: new { identifier = transaction.Identifier }, null);
	}
}
