using MicroservicesSample.Identity.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MicroservicesSample.Identity.Infrastructure.PostgreSql.EntityTypeConfigurations
{
    /// <summary>
    /// Entity configuration for <see cref="User"/>
    /// </summary>
    /// <seealso cref="IEntityTypeConfiguration{TEntity}" />
    class UserEntityTypeConfiguration :
        IEntityTypeConfiguration<User>
    {
        /// <inheritdoc />
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(_ => _.Id)
                .ValueGeneratedOnAdd();

            builder.Ignore(_ => _.DomainEvents);

            builder.ToTable("users", IdentitySampleDbContext.PublicSchemeName);
        }
    }
}
