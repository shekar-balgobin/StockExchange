using MediatR;

namespace Tyl.CommandQuery.Command;

public sealed class PostEntityCommand<TEntity> :
	IRequest<int> {
	public PostEntityCommand(TEntity entity) =>
		Entity = entity;

	public TEntity Entity { get; init; }
}
