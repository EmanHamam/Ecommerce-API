using E_Commerce.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Infrastructure.Repositories.Interfaces
{
    public interface IAccountService
    {
        Task<AuthModel> RegisterAsync(RegisterDto model);
        Task<AuthModel> LoginAsync(LoginDto model);
        Task<AuthModel> GetCurrentUserAsync();
        Task<AddressDto> GetCurrentUserAddressAsync();
        Task<AddressDto> UpdateCurrentUserAddressAsync(AddressDto addressDto);



        Task<string> AddRoleAsync(AddRoleDto model);
    }
}
