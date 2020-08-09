using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MicroservicesSample.Common.Database;
using MicroservicesSample.Common.Exceptions;
using MicroservicesSample.Identity.Core.Entities;
using MicroservicesSample.Identity.Core.Exceptions;
using MicroservicesSample.Identity.Dto;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace MicroservicesSample.Identity.Application.Services
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly PasswordPolicy _passwordPolicy;
        private readonly IUnitOfWork _ufw;
        private IRepository<User> _userRepo => _ufw.Repository<User>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userManager"></param>
        /// <param name="roleManager"></param>
        /// <param name="passwordOptions"></param>
        /// <param name="ufw"></param>
        public UserService(
            UserManager<User> userManager,
            RoleManager<Role> roleManager,
            IOptions<PasswordPolicy> passwordOptions,
            IUnitOfWork ufw)
        {
            _ufw = ufw;
            _userManager = userManager;
            _roleManager = roleManager;
            _passwordPolicy = passwordOptions.Value;
        }

        /// <inheritdoc />
        public async Task<UserDto> CreateAsync(UserAddDto model)
        {
            var allUsers = await _userRepo.GetAllAsync().ConfigureAwait(false);
            if (allUsers.Any(_ => _.UserName.ToLower() == model.UserName.ToLower()))
            {
                throw new IdentityBaseException("Field 'UserName' must be unique");
            }

            CheckModel(model, true);

            var user = new User
            {
                UserName = model.UserName,
                Description = model.Description,
            };
            var identityResult = await _userManager.CreateAsync(user);
            if (!identityResult.Succeeded)
            {
                throw new IdentityBaseException(identityResult.Errors.First().Description);
            }
            var role = await _roleManager.FindByIdAsync(model.Role);
            identityResult = await _userManager.AddToRoleAsync(user, role.Name);
            if (!identityResult.Succeeded)
            {
                throw new IdentityBaseException(identityResult.Errors.First().Description);
            }
            identityResult = await _userManager.AddPasswordAsync(user, model.Password);
            if (!identityResult.Succeeded)
            {
                throw new IdentityBaseException(identityResult.Errors.First().Description);
            }
            var dto =  GetDto(user);
            
            
            
            return dto;
        }


        /// <inheritdoc />
        public async Task<UserDto> UpdateAsync(UserUpdateDto model)
        {
            var user = await _userManager.FindByIdAsync(model.Id).ConfigureAwait(false);
            if (user == null)
                throw new EntityNotFoundException();
            var allUsers = await _userRepo.GetAllAsync().ConfigureAwait(false);
            if (allUsers.Any(_ => _.UserName.ToLower() == model.UserName.ToLower() && _.Id != user.Id))
            {
                throw new IdentityBaseException("Field 'UserName' must be unique");
            }

            var oldUser = await _userManager.FindByIdAsync(model.Id);
            if (oldUser.UserName.ToLower() == User.DefaultAdmin)
            {
                throw new IdentityBaseException("Editing default administrator is prohibited");
            }

            CheckModel(model, false);

            user.UserName = model.UserName;
            user.Description = model.Description;
            user.UpdatedAt = DateTime.UtcNow;

            var identityResult = await _userManager.UpdateAsync(user);
            if (!identityResult.Succeeded)
            {
                throw new IdentityBaseException(identityResult.Errors.First().Description);
            }

            identityResult = await _userManager.RemoveFromRoleAsync(user, user.Roles.First().Role.Name);
            if (!identityResult.Succeeded)
            {
                throw new IdentityBaseException(identityResult.Errors.First().Description);
            }

            var role = await _roleManager.FindByIdAsync(model.Role);
            identityResult = await _userManager.AddToRoleAsync(user, role.Name);
            if (!identityResult.Succeeded)
            {
                throw new IdentityBaseException(identityResult.Errors.First().Description);
            }

            if (!string.IsNullOrWhiteSpace(model.Password))
            {
                identityResult = await _userManager.RemovePasswordAsync(user);
                if (!identityResult.Succeeded)
                {
                    throw new IdentityBaseException(identityResult.Errors.First().Description);
                }

                identityResult = await _userManager.AddPasswordAsync(user, model.Password);
                if (!identityResult.Succeeded)
                {
                    throw new IdentityBaseException(identityResult.Errors.First().Description);
                }
            }
            return GetDto(user);
        }

        /// <inheritdoc />
        public ValueTask<List<UserDto>> GetAll()
        {
            var users = _userManager.Users.ToList();
            var dtos = users.Select(_ => GetDto(_)).ToList();
            return new ValueTask<List<UserDto>>(dtos);
        }

        /// <inheritdoc />
        public async Task<UserDto> GetAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId).ConfigureAwait(false);
            if (user == null)
                throw new EntityNotFoundException();
            return GetDto(user);
        }

        /// <inheritdoc />
        public async Task DeleteAsync(string userId)
        {
            var oldUser = await _userManager.FindByIdAsync(userId);
            if (oldUser.UserName.ToLower() == User.DefaultAdmin)
            {
                throw new IdentityBaseException("Deleting default administrator is prohibited");
            }

            var user = await _userManager.FindByIdAsync(userId).ConfigureAwait(false);
            if (user == null)
                return;
            await _userManager.DeleteAsync(user).ConfigureAwait(false);
        }

        private UserDto GetDto(User entity)
        {
            return new UserDto
            {
                Id = entity.Id,
                UserName = entity.UserName,
                Description = entity.Description,
                Role = new RoleDto
                {
                    Id = entity.Roles.First().RoleId,
                    Name = entity.Roles.First().Role.Name,
                    Description = entity.Roles.First().Role.Description
                },
                CreatedAt = entity.CreatedAt.ToLocalTime(),
                UpdatedAt = entity.UpdatedAt.ToLocalTime()
            };
        }

        private void CheckModel(UserAddDto model, bool passwordRequired)
        {
            if (string.IsNullOrWhiteSpace(model.UserName))
            {
                throw new IdentityBaseException("Field 'UserName' is required");
            }
            if (model.UserName.Length > 50)
                throw new IdentityBaseException("Length of field 'UserName' must be less or equal 50");

            if (!string.IsNullOrWhiteSpace(model.Description) &&
                model.Description.Length > 250)
                throw new IdentityBaseException("Length of field 'Description' must be less or equal 250");

            if (!UserNameIsValid(model.UserName))
                throw new IdentityBaseException("Field 'UserName' must be contains only latin symbols, '_', '-' and digits");

            if (string.IsNullOrWhiteSpace(model.Password) && passwordRequired)
                throw new IdentityBaseException("Field 'Password' if required");

            if (string.IsNullOrWhiteSpace(model.Password) && !passwordRequired)
                return;

            if (_passwordPolicy.RequireDigit && !ContainsDigits(model.Password))
                throw new IdentityBaseException("Field 'Password' must be contains digits");

            if (model.Password!.Length < _passwordPolicy.RequiredLength)
                throw new IdentityBaseException("Length of field 'Password' must be greater or equal " + _passwordPolicy.RequiredLength);

            if (model.Password!.Length > 50)
                throw new IdentityBaseException("Length of field 'Password' must be less or equal 50");

            if (_passwordPolicy.RequireLowercase && !ContainsLowerCase(model.Password))
                throw new IdentityBaseException("Field 'Password' must be contains symbols in lower case");

            if (_passwordPolicy.RequireUppercase && !ContainsUpperCase(model.Password))
                throw new IdentityBaseException("Field 'Password' must be contains symbols in upper case");

        }

        /// <summary>
        /// Проверка названия сущности на корректность:
        /// - содержит только буквы или цифры латинского алфавита;
        /// - содержит знаки "-", "_"
        /// </summary>
        /// <param name="objectName">Строка для проверки</param>
        /// <returns>Результат проверки</returns>
        private bool UserNameIsValid(string? objectName) =>
            !string.IsNullOrEmpty(objectName) &&
            Regex.IsMatch(objectName, "^([a-zA-Z0-9_-]*)$", RegexOptions.Compiled);

        /// <summary>
        /// Проверка строки на то, что она содержит кириллические буквы
        /// </summary>
        /// <param name="template"></param>
        /// <returns></returns>
        public static bool NotContainsCyrillic(string? template)
        {
            if (string.IsNullOrEmpty(template))
            {
                return true;
            }
            string pattern = "[а-яА-Я]";
            return !Regex.IsMatch(template, pattern);
        }

        /// <summary>
        /// Проверка наличия символов в верхнем регистре
        /// </summary>
        /// <param name="str">Строка для проверки</param>
        /// <returns>Флаг наличия</returns>
        public static bool ContainsUpperCase(string? str)
            => string.IsNullOrEmpty(str) || str.Any(char.IsUpper);

        /// <summary>
        /// Проверка наличия символов в верхнем регистре
        /// </summary>
        /// <param name="str">Строка для проверки</param>
        /// <returns>Флаг наличия</returns>
        public static bool ContainsLowerCase(string? str)
            => string.IsNullOrEmpty(str) || str.Any(char.IsLower);

        /// <summary>
        /// Проверка наличия нечисловых и нецифровых символов в верхнем регистре
        /// </summary>
        /// <param name="str">Строка для проверки</param>
        /// <returns>Флаг наличия</returns>
        public static bool ContainsNonAlphaNumeric(string? str)
            => string.IsNullOrEmpty(str) || !str.All(char.IsLetterOrDigit);

        /// <summary>
        /// Проверка наличия цифр
        /// </summary>
        /// <param name="str">Строка для проверки</param>
        /// <returns>Флаг наличия</returns>
        public static bool ContainsDigits(string? str)
            => string.IsNullOrEmpty(str) || str.Any(char.IsDigit);
    }
}
