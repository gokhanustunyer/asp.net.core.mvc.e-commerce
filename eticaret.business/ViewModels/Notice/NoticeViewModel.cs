using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eticaret.business.ViewModels.Notice
{
    public class NoticeViewModel
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public NoticeTypes MessageType { get; set; }
    }
}
