using eticaret.data.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eticaret.data
{
    public class DesginTimeDbContextFactory : IDesignTimeDbContextFactory<ETicaretDbContext>
    {
        public ETicaretDbContext CreateDbContext(string[] args)
        {
            var serverVersion = new MySqlServerVersion(new Version(8, 0, 29));
            DbContextOptionsBuilder<ETicaretDbContext> dbContextOptionsBuilder = new();
            dbContextOptionsBuilder.UseMySql("server=localhost;port=3306;user=root;password=gokhan949;database=eticaretdb;", serverVersion);
            return new ETicaretDbContext(dbContextOptionsBuilder.Options);
        }
    }
}
