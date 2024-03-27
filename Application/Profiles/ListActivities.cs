using Application.Core;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Profiles
{
    public class ListActivities
    {
        public class Query : IRequest<Result<List<UserActivityDto>>>
        {
            public string Predicate { get; set; }
            public string Username { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<List<UserActivityDto>>>
        {
            private readonly IMapper _mapper;
            private readonly DataContext _context;
            public Handler(DataContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<Result<List<UserActivityDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var query = _context.ActivityAttendees
                .Where(x => x.AppUser.UserName == request.Username)
                .OrderBy(d => d.Activity.Date)
                .ProjectTo<UserActivityDto>(_mapper.ConfigurationProvider)
                .AsQueryable();

                query = request.Predicate switch
                {
                    "past" =>
                        query.Where(d => d.Date <= DateTime.Now),
                    "hosting" =>
                        query.Where(h => h.HostUsername == request.Username),
                    _ =>
                        query.Where(d => d.Date >= DateTime.Now)
                };


                var activites = await query.ToListAsync();

                return Result<List<UserActivityDto>>.Success(activites);

            }
        }
    }
}