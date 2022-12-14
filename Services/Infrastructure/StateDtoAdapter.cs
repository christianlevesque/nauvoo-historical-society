using System.Threading.Tasks;
using Autoinjector;
using Microsoft.Extensions.DependencyInjection;

namespace Services.Infrastructure;

[Service(ServiceLifetime.Transient, typeof(IDtoAdapter<State, StateDto>))]
public class StateDtoAdapter : IDtoAdapter<State, StateDto>
{
	/// <inheritdoc />
	public Task<State> MapAddDto(StateDto dto)
	{
		var state = new State
		{
			Name = dto.Name,
			Abbreviation = dto.Abbreviation
		};

		return Task.FromResult(state);
	}

	/// <inheritdoc />
	public Task MapEditDto(StateDto dto, State entity)
	{
		entity.Name = dto.Name;
		entity.Abbreviation = dto.Abbreviation;
		entity.ConcurrencyStamp = entity.ConcurrencyStamp;
		return Task.CompletedTask;
	}

	/// <inheritdoc />
	public Task<StateDto> MapToDto(State entity)
	{
		var state = new StateDto
		{
			Id = entity.Id,
			Name = entity.Name,
			Abbreviation = entity.Abbreviation,
			ConcurrencyStamp = entity.ConcurrencyStamp
		};

		return Task.FromResult(state);
	}
}