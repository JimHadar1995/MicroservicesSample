using System.Reflection;
using MicroservicesSample.Common.Extensions;
using MicroservicesSample.Identity.Core.Entities;
using MicroservicesSample.Identity.Core.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MicroservicesSample.Identity.Infrastructure.PostgreSql
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class IdentitySampleDbContext : IdentityDbContext<User, Role, string,
        IdentityUserClaim<string>,
        UserToRoleLink, IdentityUserLogin<string>,
        IdentityRoleClaim<string>,
        IdentityUserToken<string>>
    {
        /// <summary>
        /// Название схемы для Microsoft Identity Store
        /// </summary>
        internal const string IdentitySchemeName = "asp_identity";

        /// <summary>
        /// Название схемы для других таблиц БД
        /// </summary>
        internal const string PublicSchemeName = "public";

        /// <inheritdoc />
#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
        public IdentitySampleDbContext(DbContextOptions<IdentitySampleDbContext> options)
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
                throw new IdentityBaseException("DbContextOptionsBuilder not configured");
            }

            // optionsBuilder.EnableSensitiveDataLogging();
        }

        /// <inheritdoc />
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // TODO: только для PostgreSQL 9.6 и ниже
            //modelBuilder.UseSerialColumns();

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetAssembly(typeof(IdentitySampleDbContext))!);

            EntitiesToSnackcase(modelBuilder);
        }

        #region [ Help methods ]

        /// <summary>
        /// Преобразование всех сущностей БД к ноатции snackcase.
        /// </summary>
        /// <param name="builder">The builder.</param>
        private void EntitiesToSnackcase(ModelBuilder builder)
        {
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
