using MicroservicesSample.Identity.Infrastructure.PostgreSql;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MicroservicesSample.Identity.Infrastructure.PostgreSql.EntityTypeConfigurations
{
    /// <summary>
    /// Entity configuration for <see cref="IdentityUserLogin{TKey}"/>
    /// </summary>
    /// <seealso cref="IEntityTypeConfiguration{TEntity}" />
    class UserLoginEntityTypeConfiguration
        : IEntityTypeConfiguration<IdentityUserLogin<string>>
    {
        /// <inheritdoc />
        public void Configure(EntityTypeBuilder<IdentityUserLogin<string>> builder)
        {
            builder.ToTable("user_logins", IdentitySampleDbContext.PublicSchemeName);
        }
    }
}
