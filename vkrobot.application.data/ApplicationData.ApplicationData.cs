﻿//------------------------------------------------------------------------------
// This is auto-generated code.
//------------------------------------------------------------------------------
// This code was generated by Entity Developer tool using EF Core template.
// Code is generated on: 31.01.2024 05:59:08
//
// Changes to this file may cause incorrect behavior and will be lost if
// the code is regenerated.
//------------------------------------------------------------------------------

#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata;

namespace vkrobot.application.data
{

    public partial class ApplicationData : DbContext
    {

        public ApplicationData() :
            base()
        {
            OnCreated();
        }

        public ApplicationData(DbContextOptions<ApplicationData> options) :
            base(options)
        {
            OnCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured ||
                (!optionsBuilder.Options.Extensions.OfType<RelationalOptionsExtension>().Any(ext => !string.IsNullOrEmpty(ext.ConnectionString) || ext.Connection != null) &&
                 !optionsBuilder.Options.Extensions.Any(ext => !(ext is RelationalOptionsExtension) && !(ext is CoreOptionsExtension))))
            {
            }
            CustomizeConfiguration(ref optionsBuilder);
            base.OnConfiguring(optionsBuilder);
        }

        partial void CustomizeConfiguration(ref DbContextOptionsBuilder optionsBuilder);

        public virtual DbSet<Group> Groups
        {
            get;
            set;
        }

        public virtual DbSet<Message> Messages
        {
            get;
            set;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            this.GroupMapping(modelBuilder);
            this.CustomizeGroupMapping(modelBuilder);

            this.MessageMapping(modelBuilder);
            this.CustomizeMessageMapping(modelBuilder);

            RelationshipsMapping(modelBuilder);
            CustomizeMapping(ref modelBuilder);
        }

        #region Group Mapping

        private void GroupMapping(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Group>().ToTable(@"Groups");
            modelBuilder.Entity<Group>().Property(x => x.Id).HasColumnName(@"Id").IsRequired().ValueGeneratedOnAdd();
            modelBuilder.Entity<Group>().Property(x => x.GroupId).HasColumnName(@"GroupId").IsRequired().ValueGeneratedNever();
            modelBuilder.Entity<Group>().Property(x => x.User).HasColumnName(@"User").ValueGeneratedNever();
            modelBuilder.Entity<Group>().Property(x => x.Password).HasColumnName(@"Password").ValueGeneratedNever();
            modelBuilder.Entity<Group>().Property(x => x.LastScan).HasColumnName(@"LastScan").ValueGeneratedNever();
            modelBuilder.Entity<Group>().Property(x => x.ErrorText).HasColumnName(@"ErrorText").ValueGeneratedNever();
            modelBuilder.Entity<Group>().Property(x => x.GroupName).HasColumnName(@"GroupName").ValueGeneratedNever();
            modelBuilder.Entity<Group>().Property(x => x.Private).HasColumnName(@"Private").ValueGeneratedNever();
            modelBuilder.Entity<Group>().HasKey(@"Id");
        }

        partial void CustomizeGroupMapping(ModelBuilder modelBuilder);

        #endregion

        #region Message Mapping

        private void MessageMapping(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Message>().ToTable(@"Messages");
            modelBuilder.Entity<Message>().Property(x => x.Id).HasColumnName(@"Id").IsRequired().ValueGeneratedOnAdd();
            modelBuilder.Entity<Message>().Property(x => x.UserId).HasColumnName(@"UserId").IsRequired().ValueGeneratedNever();
            modelBuilder.Entity<Message>().Property(x => x.GroupId).HasColumnName(@"GroupId").IsRequired().ValueGeneratedNever();
            modelBuilder.Entity<Message>().Property(x => x.MessageText).HasColumnName(@"MessageText").IsRequired().ValueGeneratedNever();
            modelBuilder.Entity<Message>().Property(x => x.MessageDate).HasColumnName(@"MessageDate").IsRequired().ValueGeneratedNever();
            modelBuilder.Entity<Message>().HasKey(@"Id");
        }

        partial void CustomizeMessageMapping(ModelBuilder modelBuilder);

        #endregion

        private void RelationshipsMapping(ModelBuilder modelBuilder)
        {
        }

        partial void CustomizeMapping(ref ModelBuilder modelBuilder);

        public bool HasChanges()
        {
            return ChangeTracker.Entries().Any(e => e.State == Microsoft.EntityFrameworkCore.EntityState.Added || e.State == Microsoft.EntityFrameworkCore.EntityState.Modified || e.State == Microsoft.EntityFrameworkCore.EntityState.Deleted);
        }

        partial void OnCreated();
    }
}