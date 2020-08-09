using MicroservicesSample.Identity.Infrastructure.PostgreSql;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MicroservicesSample.Identity.Infrastructure.PostgreSql.EntityTypeConfigurations
{
    /// <summary>
    /// Entity configuration for <see cref="IdentityUserToken{TKey}"/>
    /// </summary>
    /// <seealso cref="IEntityTypeConfiguration{TEntity}" />
    class UserTokenTypeConfiguration
        : IEntityTypeConfiguration<IdentityUserToken<string>>
    {
        /// <inheritdoc />
        public void Configure(EntityTypeBuilder<IdentityUserToken<string>> builder)
        {
            builder.ToTable("user_tokens", IdentitySampleDbContext.PublicSchemeName);
        }
    }
}
