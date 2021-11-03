using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GesitConsole.Models
{
    class DbContextGlobal : GesitDbContext
    {
        public DbContextGlobal() : base()
        {
            var connectionStr = ConfigurationManager.ConnectionStrings["DbContextGlobal"].ConnectionString;
        }
    }
}
