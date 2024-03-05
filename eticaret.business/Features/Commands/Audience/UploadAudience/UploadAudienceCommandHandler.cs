using eticaret.data.Abstract.Audience;
using eticaret.entity.Audience;
using eticaret.entity.Identity;
using ExcelDataReader;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eticaret.business.Features.Commands.Audience.UploadAudience
{
    public class UploadAudienceCommandHandler : IRequestHandler<UploadAudienceCommandRequest, UploadAudienceCommandResponse>
    {
        private readonly IAudienceRepository _audienceRepository;
        private readonly UserManager<AppUser> _userManager;
        public UploadAudienceCommandHandler(IAudienceRepository audienceRepository,
                                            UserManager<AppUser> userManager)
        {
            _audienceRepository = audienceRepository;
            _userManager = userManager;
        }

        public async Task<UploadAudienceCommandResponse> Handle(UploadAudienceCommandRequest request, CancellationToken cancellationToken)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            using (var stream = new MemoryStream())
            {
                request.audienceData.CopyTo(stream);
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    do
                    {
                        while (reader.Read())
                        {
                            var rowData = new List<object>();
                            for (int column = 0; column < reader.FieldCount; column++)
                            {
                                rowData.Add(reader.GetValue(column));
                            }
                            AppUser user = await _audienceRepository.Context.Users.FirstOrDefaultAsync(u => u.Email == rowData[2]);
                            if (user == null || rowData[rowData.Count - 1] == null) continue;

                            var segment = await _audienceRepository.Table.Include(a => a.Users).FirstOrDefaultAsync(a => a.SegmentTitle == rowData[rowData.Count - 1].ToString());
                            if (segment == null)
                            {
                                await _audienceRepository.AddAsync(new UserSegment()
                                {
                                    SegmentTitle = rowData[rowData.Count - 1].ToString()
                                });
                                await _audienceRepository.SaveAsync();
                                segment = await _audienceRepository.Table.Include(a => a.Users).FirstOrDefaultAsync(a => a.SegmentTitle == rowData[rowData.Count - 1].ToString());
                            }
                            segment.Users.Add(user);
                            _audienceRepository.Update(segment);
                        }
                    } while (reader.NextResult());
                }
            }
            await _audienceRepository.SaveAsync();
            return new ();
        }
    }
}
