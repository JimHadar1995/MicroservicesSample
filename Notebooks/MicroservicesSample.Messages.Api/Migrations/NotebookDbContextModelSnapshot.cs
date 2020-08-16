﻿// <auto-generated />
using System;
using MicroservicesSample.Notebooks.Api.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace MicroservicesSample.Notebooks.Api.Migrations
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    [DbContext(typeof(NotebookDbContext))]
    partial class NotebookDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .HasAnnotation("ProductVersion", "3.1.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("MicroservicesSample.Messages.Api.Entities.Notebook", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnName("id")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnName("created_at")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("SenderId")
                        .IsRequired()
                        .HasColumnName("sender_id")
                        .HasColumnType("text");

                    b.Property<string>("SenderName")
                        .IsRequired()
                        .HasColumnName("sender_name")
                        .HasColumnType("text");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnName("text")
                        .HasColumnType("text");

                    b.HasKey("Id")
                        .HasName("pk_notebooks");

                    b.ToTable("notebooks");
                });
#pragma warning restore 612, 618
        }
    }
}
