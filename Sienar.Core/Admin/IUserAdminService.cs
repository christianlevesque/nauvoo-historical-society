using System.Collections.Generic;
using System.Threading.Tasks;
using Sienar.Identity;

namespace Sienar.Admin;

public interface IUserAdminService
{
	Task<ServiceResult<PagedDto<ApplicationUserDto>>> GetAllUsers(Filter filter);
	Task<ServiceResult<ApplicationUserDto>> GetUserData(string userId);
	Task<ServiceResult<bool>> UpdateUserEmail(UserChangeEmailDto data);
	Task<ServiceResult<bool>> UpdateUserPassword(UserChangePasswordDto data);
	Task<ServiceResult<IEnumerable<string>>> GetAvailableRoles();
	Task<ServiceResult<bool>> AddUserToRole(UserUpdateRolesDto data);
}