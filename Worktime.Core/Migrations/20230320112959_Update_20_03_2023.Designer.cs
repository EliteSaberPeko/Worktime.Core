﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Worktime.Core.Models;

#nullable disable

namespace Worktime.Core.Migrations
{
    [DbContext(typeof(ApplicationContext))]
    [Migration("20230320112959_Update_20_03_2023")]
    partial class Update_20_03_2023
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Worktime.Core.Models.WTLine", b =>
                {
                    b.Property<decimal>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("numeric(20,0)");

                    b.Property<DateTime>("BeginTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("EndTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<double>("Time")
                        .HasColumnType("double precision");

                    b.Property<int>("WTTaskId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("WTTaskId");

                    b.ToTable("Lines");
                });

            modelBuilder.Entity("Worktime.Core.Models.WTTask", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<bool>("Completed")
                        .HasColumnType("boolean");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<double>("TotalTime")
                        .HasColumnType("double precision");

                    b.Property<Guid>("WTUserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("WTUserId");

                    b.ToTable("Tasks");
                });

            modelBuilder.Entity("Worktime.Core.Models.WTUser", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Worktime.Core.Models.WTLine", b =>
                {
                    b.HasOne("Worktime.Core.Models.WTTask", "Task")
                        .WithMany("Lines")
                        .HasForeignKey("WTTaskId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Task");
                });

            modelBuilder.Entity("Worktime.Core.Models.WTTask", b =>
                {
                    b.HasOne("Worktime.Core.Models.WTUser", "User")
                        .WithMany("Tasks")
                        .HasForeignKey("WTUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Worktime.Core.Models.WTTask", b =>
                {
                    b.Navigation("Lines");
                });

            modelBuilder.Entity("Worktime.Core.Models.WTUser", b =>
                {
                    b.Navigation("Tasks");
                });
#pragma warning restore 612, 618
        }
    }
}
