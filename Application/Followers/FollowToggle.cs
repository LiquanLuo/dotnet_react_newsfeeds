using Application.Core;
using Application.Interface;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Followers
{
    public class FollowToggle
    {
        public class Command : IRequest<Result<Unit>>
        {
            public string TargetUsername { get; set; }
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly DataContext _dataContext;
            private readonly IUserAccessor _userAccessor;
            public Handler(DataContext dataContext, IUserAccessor userAccessor)
            {
                _userAccessor = userAccessor;
                _dataContext = dataContext;

            }
            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var target = await _dataContext
                    .Users
                    .FirstOrDefaultAsync(x => x.UserName == request.TargetUsername);

                var observer = await _dataContext
                    .Users
                    .FirstOrDefaultAsync(x => x.UserName == _userAccessor.GetUsername());

                if (target == null) return null;

                var following = await _dataContext.UserFollowings.FindAsync(observer.Id, target.Id);

                if (following == null)
                {
                    _dataContext.UserFollowings.Add(new UserFollowing
                    {
                        Observer = observer,
                        Target = target
                    });

                }
                else
                {
                    _dataContext.Remove(following);
                }


                var success = await _dataContext.SaveChangesAsync() > 0;

                if (success) return Result<Unit>.Success(Unit.Value);

                return Result<Unit>.Failure("Failed to update following");

            }
        }
    }
}