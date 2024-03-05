using MediatR;

namespace eticaret.business.Features.Queries.Audience
{
    public class GetUsersAndSegmentsQueryRequest : IRequest<GetUsersAndSegmentsQueryResponse>
    {
        public string? SegmentOrEmail { get; set; }
        public int? Page { get; set; }
        public int? PageSize { get; set; }
    }
}