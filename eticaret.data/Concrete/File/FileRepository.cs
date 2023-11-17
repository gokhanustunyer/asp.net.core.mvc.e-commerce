using eticaret.data.Abstract.File;
using eticaret.data.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using et = eticaret.entity;

namespace eticaret.data.Concrete.File
{
    public class FileRepository : Repository<et.File>, IFileRepository
    {
        public FileRepository(ETicaretDbContext context) : base(context)
        {
        }
    }
}
