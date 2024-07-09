using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Data.Context
{
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        private readonly IConfiguration _configuration;
        public ApplicationDbContextFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseSqlServer(_configuration.GetConnectionString("DefaultConnection"));

            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}
//Data Source=PLAYSTATION;Initial Catalog=TajimaSoftwareTest;Integrated Security=True;Encrypt=True;Trust Server Certificate=True
//Data Source=Dev-dw44-n;Initial Catalog=TajimaSoftwareTest;Integrated Security=True;Encrypt=True;Trust Server Certificate=True