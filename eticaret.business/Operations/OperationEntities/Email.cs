using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eticaret.business.Operations.OperationEntities
{
    public class Email
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Host { get; set; }
        public bool EnableSSL { get; set; }
        public int Port { get; set; }
    }
}
