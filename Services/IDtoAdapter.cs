using System.Threading.Tasks;

namespace Services;

public interface IDtoAdapter<TEntity, TDto>
{
	/// <summary>
	/// Maps a DTO to a new <c>TEntity</c>
	/// </summary>
	/// <param name="dto">The DTO to map to a new <c>TEntity</c></param>
	/// <returns>a new <c>TEntity</c> for persisting to the repository</returns>
	public Task<TEntity> MapAddDto(TDto dto);

	/// <summary>
	/// Maps a DTO to an existing <c>TEntity</c>
	/// </summary>
	/// <param name="dto">The DTO to map to an existing <c>TEntity</c></param>
	/// <param name="entity">The <c>TEntity</c> to map the DTO to</param>
	public Task MapEditDto(TDto dto, TEntity entity);

	/// <summary>
	/// Maps a <c>TEntity</c> to a new DTO
	/// </summary>
	/// <param name="entity">The <c>TEntity</c> to map to a DTO</param>
	/// <returns>the mapped DTO</returns>
	public Task<TDto> MapToDto(TEntity entity);
}