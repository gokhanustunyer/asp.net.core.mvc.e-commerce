using MediatR;
using Microsoft.AspNetCore.Http;

namespace eticaret.business.Features.Commands.Audience.UploadAudience
{
    public class UploadAudienceCommandRequest : IRequest<UploadAudienceCommandResponse>
    {
        public IFormFile audienceData { get; set; }
    }
}