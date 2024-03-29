﻿using Microsoft.EntityFrameworkCore;
using ms_userCrud.Data.Entity;
using ms_userCrud.Data.Mapping;

namespace ms_userCrud.Data
{
    public class DBContext : DbContext
    {
        public virtual DbSet<UserDTO> User { get; set; }

        public DBContext(DbContextOptions options) : base(options) { }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<UserDTO>(new UserMap().Configure);
        }
    }
}
