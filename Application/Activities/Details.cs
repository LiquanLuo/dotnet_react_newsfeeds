using Application.Core;
using Application.Interface;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Activities
{
    public class Details
    {
        public class Query : IRequest<Result<ActivityDto>>
        {

            public Guid Id { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<ActivityDto>>
        {
            private readonly DataContext _dataContext;
            private readonly IMapper _mapper;
            private readonly IUserAccessor _userAccessor;
            public Handler(DataContext dataContext, IMapper mapper, IUserAccessor userAccessor)
            {
                _userAccessor = userAccessor;
                _mapper = mapper;
                _dataContext = dataContext;
            }

            public async Task<Result<ActivityDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var activity = await _dataContext.Activities
                    .ProjectTo<ActivityDto>(_mapper.ConfigurationProvider, 
                        new{currentUsername = _userAccessor.GetUsername()})
                    .FirstOrDefaultAsync(x => x.Id == request.Id);

                return Result<ActivityDto>.Success(activity);
            }
        }
    }


}