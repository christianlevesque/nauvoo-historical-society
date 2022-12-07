using System;
using System.Threading.Tasks;
using Core.Account;
using Autoinjector;
using Microsoft.Extensions.DependencyInjection;
using Repositories.Identity;

namespace Services.Identity;

[Service(ServiceLifetime.Transient, typeof(IDtoAdapter<ApplicationUser, ApplicationUserDto>))]
public class UserDtoAdapter : IDtoAdapter<ApplicationUser, ApplicationUserDto>
{
	private readonly UserService _userService;

	public UserDtoAdapter(UserService userService)
	{
		_userService = userService;
	}

	/// <inheritdoc />
	public Task<ApplicationUser> MapAddDto(ApplicationUserDto dto) => throw new InvalidOperationException($"Model {typeof(ApplicationUser)} does not have an appropriate add DTO");

	/// <inheritdoc />
	public Task MapEditDto(ApplicationUserDto dto, ApplicationUser entity) => throw new InvalidOperationException($"Model {typeof(ApplicationUser)} does not have an appropriate edit DTO");

	/// <inheritdoc />
	public async Task<ApplicationUserDto> MapToDto(ApplicationUser user)
	{
		var roles = await _userService.GetRolesAsync(user);

		return new ApplicationUserDto
		{
			Id = user.Id,
			UserName = user.UserName,
			Email = user.Email,
			IsVerified = user.EmailConfirmed,
			Roles = roles ?? Array.Empty<string>()
		};
	}
}