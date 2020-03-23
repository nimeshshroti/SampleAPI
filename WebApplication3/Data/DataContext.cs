using Microsoft.EntityFrameworkCore;
using SampleAPI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleAPI.Data
{
    public class DataContext: DbContext 
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<Value> Values { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
