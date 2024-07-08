using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Context
{
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseSqlServer("Data Source=PLAYSTATION;Initial Catalog=TajimaSoftwareTest;Integrated Security=True;Encrypt=True;Trust Server Certificate=True");

            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}
//Data Source=PLAYSTATION;Initial Catalog=TajimaSoftwareTest;Integrated Security=True;Encrypt=True;Trust Server Certificate=True
//Data Source=Dev-dw44-n;Initial Catalog=TajimaSoftwareTest;Integrated Security=True;Encrypt=True;Trust Server Certificate=True