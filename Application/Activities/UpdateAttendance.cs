using Application.Core;
using Application.Interface;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Activities
{
    public class UpdateAttendance
    {
        public class Command : IRequest<Result<Unit>>
        {
            public Guid Id { get; set; }
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
                var activity = await _dataContext.Activities
                    .Include(a => a.Attendees)
                    .ThenInclude(u => u.AppUser)
                    .SingleOrDefaultAsync(x => x.Id == request.Id);
                
                if (activity == null) return null;

                var user = await _dataContext.Users
                    .FirstOrDefaultAsync(x => x.UserName == _userAccessor.GetUsername());

                if (user == null) return null;

                var hostUsername = activity.Attendees.FirstOrDefault(x => x.IsHost)?.AppUser.UserName;
                var attendance = activity.Attendees.FirstOrDefault(x => x.AppUser.UserName == user.UserName);

                // user is the host of this activity, cancel the activity
                if (attendance != null && hostUsername == user.UserName)
                    activity.IsCancelled = !activity.IsCancelled;

                // user attend the activity, quit the activity
                if (attendance != null && hostUsername != user.UserName)
                    activity.Attendees.Remove(attendance);

                // user hasnt joined the activity, join the activity
                if (attendance == null)
                {
                    attendance = new ActivityAttendee
                    {
                        AppUser = user,
                        Activity = activity,
                        IsHost = false
                    };

                    activity.Attendees.Add(attendance);
                }

                var result = await _dataContext.SaveChangesAsync() > 0;

                return result? Result<Unit>.Success(Unit.Value) : Result<Unit>.Failure("Problem updating attendance");

            }
        }

    }
}