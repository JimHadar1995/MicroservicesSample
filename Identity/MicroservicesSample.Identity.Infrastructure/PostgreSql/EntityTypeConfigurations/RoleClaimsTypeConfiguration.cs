using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MicroservicesSample.Identity.Infrastructure.PostgreSql.EntityTypeConfigurations
{
    /// <summary>
    /// Entity configuration for <see cref="IdentityRoleClaim{TKey}"/>
    /// </summary>
    /// <seealso cref="IEntityTypeConfiguration{TEntity}" />
    class RoleClaimsTypeConfiguration
        : IEntityTypeConfiguration<IdentityRoleClaim<string>>
    {
        /// <inheritdoc />
        public void Configure(EntityTypeBuilder<IdentityRoleClaim<string>> builder)
        {
            builder.ToTable("role_claims", IdentitySampleDbContext.PublicSchemeName);
        }
    }
}
