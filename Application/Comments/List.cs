using Application.Core;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Comments
{
    public class List
    {
        public class Query : IRequest<Result<List<CommentDto>>>
        {
            public Guid ActivityId { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<List<CommentDto>>>
        {
            private readonly DataContext _dataContext;
            private readonly IMapper _mapper;
            public Handler(DataContext dataContext, IMapper mapper)
            {
                _mapper = mapper;
                _dataContext = dataContext;

            }
            public async Task<Result<List<CommentDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var activity = await _dataContext.Activities.FindAsync(request.ActivityId);

                var comments = await _dataContext
                            .Comments
                            .Where(x => x.Activity.Id == request.ActivityId)
                            .OrderByDescending(x => x.CreatedAt)
                            .ProjectTo<CommentDto>(_mapper.ConfigurationProvider)
                            .ToListAsync();

                return Result<List<CommentDto>>.Success(comments);
            }
        }


    }
}