using Microsoft.EntityFrameworkCore;
using ms_userCrud.Data.Mapping;
using ms_userCrud.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ms_userCrud.Data
{
    public class MysqlDBContext : DbContext
    {
        public virtual DbSet<User> User { get; set; }

        public MysqlDBContext(DbContextOptions options) : base(options) { }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>(new UserMap().Configure);
        }
    }
}
