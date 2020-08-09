using MicroservicesSample.Identity.Core.Entities;
using MicroservicesSample.Identity.Infrastructure.PostgreSql;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MicroservicesSample.Identity.Infrastructure.PostgreSql.EntityTypeConfigurations
{
    /// <summary>
    /// Entity configuration for <see cref="UserToRoleLink"/>
    /// </summary>
    /// <seealso cref="IEntityTypeConfiguration{TEntity}" />
    class UserToRoleLinkEntityTypeConfiguration
        : IEntityTypeConfiguration<UserToRoleLink>
    {

        /// <inheritdoc />
        public void Configure(EntityTypeBuilder<UserToRoleLink> builder)
        {
            builder.HasKey(_ => new { _.UserId, _.RoleId });
            builder.HasOne(_ => _.User)
                .WithMany(_ => _.Roles)
                .HasForeignKey(_ => _.UserId);

            builder.HasOne(_ => _.Role)
                .WithMany(_ => _.Users)
                .HasForeignKey(_ => _.RoleId);

            builder.ToTable("user_to_role_links", IdentitySampleDbContext.PublicSchemeName);
        }
    }
}
