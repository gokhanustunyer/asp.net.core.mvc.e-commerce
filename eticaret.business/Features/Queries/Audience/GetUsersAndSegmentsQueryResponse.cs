using eticaret.entity.EntityRefrences.AudienceReferences;

namespace eticaret.business.Features.Queries.Audience
{
    public class GetUsersAndSegmentsQueryResponse
    {
        public List<UserSegmentReference> UserSegmentReferences { get; set; }
    }
}