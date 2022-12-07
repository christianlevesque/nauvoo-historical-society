using System.Net.Http;
using System.Threading.Tasks;
using Core;
using Microsoft.Extensions.Logging;
using Core.Account;
using Autoinjector;
using Core.Users;
using Microsoft.Extensions.DependencyInjection;
using Client.Services.Infrastructure;

namespace Client.Services;

[Service(ServiceLifetime.Scoped, typeof(IAdminUserService))]
public class AdminUserService : BaseHttpService<AdminUserService>, IAdminUserService
{
	public AdminUserService(HttpClient client, ILogger<AdminUserService> logger, ISnackbarService snackbar) : base("users", client, logger, snackbar) {}

	public async Task<PagedDto<ApplicationUserDto>> GetUsers(Filter? filter = null) => await SendRequest<PagedDto<ApplicationUserDto>>(Endpoint, input: filter) ?? new PagedDto<ApplicationUserDto>();

	public Task<ApplicationUserDto?> GetUser(string id) => SendRequest<ApplicationUserDto>($"{Endpoint}/{id}");

	public Task<bool> UpdateUserEmail(UserChangeEmailDto user) => SendRequest($"{Endpoint}/email", "Updated user email successfully!", input: user, method: HttpMethod.Patch);

	public Task<bool> UpdateUserPassword(UserChangePasswordDto user) => SendRequest($"{Endpoint}/password", "Updated user password successfully!", input: user, method: HttpMethod.Patch);

	public Task<bool> UpdateUserRoles(UserUpdateRolesDto user) => SendRequest($"{Endpoint}/roles", "Updated user roles successfully!", input: user, method: HttpMethod.Put);
}

public interface IAdminUserService
{
	Task<PagedDto<ApplicationUserDto>> GetUsers(Filter? filter = null);
	Task<ApplicationUserDto?> GetUser(string id);
	Task<bool> UpdateUserEmail(UserChangeEmailDto user);
	Task<bool> UpdateUserPassword(UserChangePasswordDto user);
	Task<bool> UpdateUserRoles(UserUpdateRolesDto user);
}