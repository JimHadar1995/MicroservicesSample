using MicroservicesSample.Identity.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MicroservicesSample.Identity.Infrastructure.PostgreSql.EntityTypeConfigurations
{
    /// <summary>
    /// Entity configuration for <see cref="Role"/>
    /// </summary>
    /// <seealso cref="IEntityTypeConfiguration{TEntity}" />
    class RoleEntityTypeConfiguration
        : IEntityTypeConfiguration<Role>
    {

        /// <inheritdoc />
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.Property(_ => _.Id).ValueGeneratedOnAdd();
            builder.ToTable("roles", IdentitySampleDbContext.PublicSchemeName);
        }
    }
}
