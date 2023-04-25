﻿using KatifiWebServer.Models.DatabaseModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace KatifiWebServer.Data
{
    public class MicrosoftEFContext : DbContext
    {
        public MicrosoftEFContext(DbContextOptions<MicrosoftEFContext> options) : base(options) { }

        public DbSet<Address> Addresses { get; set; }
        public DbSet<Church> Churches { get; set; }
        public DbSet<Community> Communities { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<Participant> Participants { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Mess> Messes { get; set; }

        //TODO: History táblák később
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Address>().Property(p => p.Id).UseIdentityColumn(seed: 338, increment: 1);
            modelBuilder.Entity<Church>().Property(p => p.Id).UseIdentityColumn(seed: 50, increment: 1);
            modelBuilder.Entity<Community>().Property(p => p.Id).UseIdentityColumn(seed: 630, increment: 1);
            modelBuilder.Entity<Event>().Property(p => p.Id).UseIdentityColumn(seed: 1140, increment: 1);
            modelBuilder.Entity<Member>().Property(p => p.Id).UseIdentityColumn(seed: 4740, increment: 1);
            modelBuilder.Entity<Participant>().Property(p => p.Id).UseIdentityColumn(seed: 8305, increment: 1);
            modelBuilder.Entity<Role>().Property(p => p.Id).UseIdentityColumn(seed: 20, increment: 3);
            modelBuilder.Entity<User>().Property(p => p.Id).UseIdentityColumn(seed: 5170, increment: 1);
            //Mess starting id not required
            base.OnModelCreating(modelBuilder);
        }

    }
}
