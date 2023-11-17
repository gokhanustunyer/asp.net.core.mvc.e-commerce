using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eticaret.business.Features.Commands.Product.GetComment
{
    public class GetCommentCommandRequest: IRequest<GetCommentCommandResponse>
    {
        public string ProductId { get; set; }
        public string UserName { get; set; }
        public int Rate { get; set; }
        public string Comment { get; set; }
        public IFormFileCollection? CommentImages { get; set; }
    }
}
