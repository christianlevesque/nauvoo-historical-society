using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core;

namespace Client.Services;

public interface ICrudService<TViewDto, in TAddDto, in TEditDto, TKey>
	where TEditDto : EntityBase<TKey>
	where TKey : struct, IEquatable<TKey>
{
	/// <summary>
	/// Creates a new entry in the backend
	/// </summary>
	/// <param name="model">The DTO representing the entity to create</param>
	/// <returns>the primary key of the created entity</returns>
	Task<TKey> Add(TAddDto model);

	/// <summary>
	/// Updates an existing entity in the backend
	/// </summary>
	/// <param name="model">The DTO representing the entity to update</param>
	/// <returns>whether the edit operation was successful</returns>
	Task<bool> Edit(TEditDto model);

	/// <summary>
	/// Gets an entity by primary key
	/// </summary>
	/// <param name="id">The primary key of the entity to retrieve</param>
	/// <returns>the requested <c>TModel</c></returns>
	Task<TViewDto?> Get(TKey id);

	/// <summary>
	/// Gets a list of all entities in the backend
	/// </summary>
	/// <param name="filter">The </param>
	/// <returns>a list of all entities in the database</returns>
	Task<PagedDto<TViewDto>> Get(Filter? filter = null);

	/// <summary>
	/// Deletes an entity by primary key
	/// </summary>
	/// <param name="id">The primary key of the entity to delete</param>
	/// <returns>whether the delete operation was successful</returns>
	Task<bool> Delete(TKey id);
}

public interface ICrudService<TViewDto, TAddDto, TEditDto> : ICrudService<TViewDto, TAddDto, TEditDto, Guid>
	where TEditDto : EntityBase
{}