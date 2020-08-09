using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicroservicesSample.Common.Database;
using MicroservicesSample.Identity.Application.Services;
using MicroservicesSample.Identity.Core.Entities;
using MicroservicesSample.Identity.Infrastructure.PostgreSql;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis.Extensions.Core.Abstractions;

namespace MicroservicesSample.Identity.Infrastructure.Code
{
    /// <summary>
    /// Инициализатор БД.
    /// </summary>
    internal static class DbInitializer
    {
        /// <summary>
        /// Применение миграций
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
        public static async Task MigrateAsync(IServiceProvider serviceProvider)
        {
            try
            {
                var context = serviceProvider.GetRequiredService<IdentitySampleDbContext>();
                var migrations = (await context.Database.GetPendingMigrationsAsync()
                        .ConfigureAwait(false))
                    .ToList();
                if (migrations.Any())
                {
                    await context.Database.MigrateAsync().ConfigureAwait(false);
                }

                Console.WriteLine($"Db deployed successfull");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Initialize migrations error: {ex.Message}");
            }
        }

        /// <summary>
        /// Инициализация БД начальными данными
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <returns></returns>
        public static async Task InitializeData(this IServiceCollection serviceCollection)
        {
            try
            {
                var sp = serviceCollection.BuildServiceProvider();
                using var scope = sp.CreateScope();

                await MigrateAsync(scope.ServiceProvider).ConfigureAwait(false);

                var ufw = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<Role>>();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
                var roles = roleManager.Roles.ToList();

                if (!roles.Any())
                {
                    var adminRole = new Role {Name = Role.AdministratorRole, Description = "Администратор",};
                    var userRole = new Role {Name = Role.UserRole, Description = "Пользователь"};
                    await roleManager.CreateAsync(adminRole);
                    await roleManager.CreateAsync(userRole);
                }

                if (!ufw.Repository<User>().Query.Any())
                {
                    var user = new User {UserName = User.DefaultAdmin, Description = "Администратор по умолчанию"};
                    var result = await userManager.CreateAsync(user, "Qwerty7").ConfigureAwait(false);
                    if (result == IdentityResult.Success)
                    {
                        await userManager.AddToRoleAsync(user, Role.AdministratorRole);
                    }
                }

                await InitRedis(serviceCollection);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
        }

        private static async Task InitRedis(this IServiceCollection services)
        {
            try
            {
                var sp = services.BuildServiceProvider();
                using var scope = sp.CreateScope();
                var redisCache = scope.ServiceProvider.GetRequiredService<IRedisCacheClient>();
                var ufw = scope.ServiceProvider.GetRequiredService<IUserService>();
                var db = redisCache.GetDbFromConfiguration();
                var users = await ufw.GetAll();
                foreach (var user in users)
                {
                    if (!(await db.ExistsAsync($"user-{user.Id}")))
                    {
                        await db.AddAsync($"user-{user.Id}", user);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
