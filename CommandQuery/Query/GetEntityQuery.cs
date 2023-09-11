using MediatR;

namespace Tyl.CommandQuery.Query;

public sealed class GetEntityQuery<TIdentifier, TEntity> :
	IRequest<TEntity> {
	public GetEntityQuery(TIdentifier identifier) =>
		Identifier = identifier;

	public TIdentifier Identifier { get; init; }
}
