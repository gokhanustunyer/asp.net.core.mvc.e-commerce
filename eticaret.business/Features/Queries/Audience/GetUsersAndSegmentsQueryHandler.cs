using eticaret.data.Abstract.Audience;
using eticaret.entity.Audience;
using eticaret.entity.EntityRefrences.AudienceReferences;
using eticaret.entity.Identity;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eticaret.business.Features.Queries.Audience
{
    public class GetUsersAndSegmentsQueryHandler : IRequestHandler<GetUsersAndSegmentsQueryRequest, GetUsersAndSegmentsQueryResponse>
    {
        private readonly IAudienceRepository _audienceRepository;
        public GetUsersAndSegmentsQueryHandler(IAudienceRepository audienceRepository)
        {
            _audienceRepository = audienceRepository;
        }

        public async Task<GetUsersAndSegmentsQueryResponse> Handle(GetUsersAndSegmentsQueryRequest request, CancellationToken cancellationToken)
        {
            DbSet<UserSegment> table = _audienceRepository.Table;
            IEnumerable<UserSegment> segments = table.Include(s => s.Users).Where(s => true);
            
            if (request.SegmentOrEmail is not null)
            {
                AppUser user = await _audienceRepository.Context?.Users.FirstOrDefaultAsync(u => u.Email == request.SegmentOrEmail);
                if (user is not null)
                {
                    segments = segments.Where(s => s.Users.Contains(user));
                }
                else
                {
                    ICollection<string> targetSegments = request.SegmentOrEmail.ToLower().Split(",");
                    segments = segments.Where(s => targetSegments.Contains(s.SegmentTitle.ToLower()));
                }
            }

            int page = request.Page ?? 1;
            int pageSize = request.PageSize ?? 30;

            segments = segments.Skip((page - 1) * pageSize).Take(pageSize);
            List<UserSegmentReference> userSegmentReference = new() {  };
            foreach (var segment in segments)
            {
                foreach(var user in segment.Users)
                {
                    if (user != null)
                    {
                        userSegmentReference.Add(new UserSegmentReference()
                        {
                            Email = user.Email,
                            Name = user.FirstName,
                            Surname = user.LastName,
                            UserId = user.Id,
                            SegmentId = segment.Id.ToString(),
                            SegmentTitle = segment.SegmentTitle
                        });
                    }
                }
            }

            return new()
            {
                UserSegmentReferences = userSegmentReference.ToList()
            };
        }
    }
}
