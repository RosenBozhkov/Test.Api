using Business.Models.v1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces.v1;

public interface IUserService
{
    Task<UserResponse> RegisterAsync(UserRequest model);
    Task<UserResponse> GetByIdAsync(Guid id);
    Task<string> LoginAsync(UserRequest model);

}
