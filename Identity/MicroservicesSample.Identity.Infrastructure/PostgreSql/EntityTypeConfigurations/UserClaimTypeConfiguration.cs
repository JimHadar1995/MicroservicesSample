using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MicroservicesSample.Identity.Infrastructure.PostgreSql.EntityTypeConfigurations
{
    // <summary>
    /// Entity configuration for <see cref="IdentityUserClaim{TKey}"/>
    /// </summary>
    /// <seealso cref="IEntityTypeConfiguration{TEntity}" />
    class UserClaimTypeConfiguration
        : IEntityTypeConfiguration<IdentityUserClaim<string>>
    {
        /// <inheritdoc />
        public void Configure(EntityTypeBuilder<IdentityUserClaim<string>> builder)
        {
            builder.ToTable("user_claims", IdentitySampleDbContext.PublicSchemeName);
        }
    }
}
