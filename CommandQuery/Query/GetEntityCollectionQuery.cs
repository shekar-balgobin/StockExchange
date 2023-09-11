using MediatR;

namespace Tyl.CommandQuery.Query;

public sealed class GetEntityCollectionQuery<TIdentifier, TEntity> :
	IStreamRequest<TEntity> {
	public GetEntityCollectionQuery(TIdentifier identifier) =>
		Identifier = identifier;

	public TIdentifier Identifier { get; init; }
}
