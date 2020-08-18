using MicroservicesSample.Common.Extensions;
using MicroservicesSample.Notebooks.Api.Entities;
using MicroservicesSample.Notebooks.Api.Exceptions;
using Microsoft.EntityFrameworkCore;
using Npgsql.NameTranslation;

namespace MicroservicesSample.Notebooks.Api.Repositories
{
    /// <summary>
    /// 
    /// </summary>
    public class NotebookDbContext : DbContext
    {
        /// <inheritdoc/>
#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
        public NotebookDbContext(DbContextOptions<NotebookDbContext> options)
#pragma warning restore CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
            : base(options)
        {
        }

        /// <inheritdoc />
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            if (!optionsBuilder.IsConfigured)
            {
                throw new ApiNotebooksException("DbContextOptionsBuilder not configured");
            }

            // optionsBuilder.EnableSensitiveDataLogging();
        }

        /// <inheritdoc />
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // TODO: только для PostgreSQL 9.6 и ниже
            //modelBuilder.UseSerialColumns();            

            EntitiesToSnackcase(modelBuilder);
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public DbSet<Notebook> Notebooks { get; set; }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

        #region [ Help methods ]

        /// <summary>
        /// Преобразование всех сущностей БД к ноатции snackcase.
        /// </summary>
        /// <param name="builder">The builder.</param>
        private void EntitiesToSnackcase(ModelBuilder builder)
        {
            var mapper = new NpgsqlSnakeCaseNameTranslator();
            foreach (var entity in builder.Model.GetEntityTypes())
            {
                // Replace table names
                entity.SetTableName(entity.GetTableName().ToSnakeCase());

                // Replace column names            
                foreach (var property in entity.GetProperties())
                {
                    property.SetColumnName(property.GetColumnName().ToSnakeCase());
                }

                foreach (var key in entity.GetKeys())
                {
                    key.SetName(key.GetName().ToSnakeCase());
                }

                foreach (var key in entity.GetForeignKeys())
                {
                    key.SetConstraintName(key.GetConstraintName().ToSnakeCase());
                }

                foreach (var index in entity.GetIndexes())
                {
                    index.SetName(index.GetName().ToSnakeCase());
                }
            }
        }

        #endregion
    }
}
