using System.Collections.Generic;
using MediatR;
using MicroservicesSample.Identity.Dto;

namespace MicroservicesSample.Identity.Application.Queries
{
    /// <summary>
    /// Команда получения списка всех пользователей
    /// </summary>
    public sealed class GetAllUsersQuery : IRequest<List<UserDto>>
    {
        private static GetAllUsersQuery? _instance;
        /// <summary>
        /// 
        /// </summary>
        public static GetAllUsersQuery Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new GetAllUsersQuery();
                return _instance;
            }
        }
    }
}
