﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using vkrobot.application.data;

#nullable disable

namespace vkrobot.application.webapi.Migrations
{
    [DbContext(typeof(ApplicationData))]
    [Migration("20240131042508_GroupExtended")]
    partial class GroupExtended
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("vkrobot.application.data.Group", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("Id");

                    b.Property<string>("ErrorText")
                        .HasColumnType("text")
                        .HasColumnName("ErrorText");

                    b.Property<string>("GroupId")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("GroupId");

                    b.Property<string>("GroupName")
                        .HasColumnType("text")
                        .HasColumnName("GroupName");

                    b.Property<DateTime?>("LastScan")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("LastScan");

                    b.Property<string>("Password")
                        .HasColumnType("text")
                        .HasColumnName("Password");

                    b.Property<bool?>("Private")
                        .HasColumnType("boolean")
                        .HasColumnName("Private");

                    b.Property<string>("User")
                        .HasColumnType("text")
                        .HasColumnName("User");

                    b.HasKey("Id");

                    b.ToTable("Groups", (string)null);
                });

            modelBuilder.Entity("vkrobot.application.data.Message", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("Id");

                    b.Property<string>("GroupId")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("GroupId");

                    b.Property<DateTime>("MessageDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("MessageDate");

                    b.Property<string>("MessageText")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("MessageText");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("UserId");

                    b.HasKey("Id");

                    b.ToTable("Messages", (string)null);
                });
#pragma warning restore 612, 618
        }
    }
}
