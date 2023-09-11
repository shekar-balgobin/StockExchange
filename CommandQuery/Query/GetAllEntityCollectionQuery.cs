using MediatR;

namespace Tyl.CommandQuery.Query;

public sealed class GetAllEntityCollectionQuery<TEntity> :
	IStreamRequest<TEntity> {
}
