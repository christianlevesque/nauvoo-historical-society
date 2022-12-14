using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Services;

namespace Services;

public interface IRepository<TEntity, TKey>
	where TEntity : class
    where TKey : IEquatable<TKey>
{
	/// <summary>
	/// Searches a repository for paged results based on a search filter
	/// </summary>
	/// <param name="filter">The <see cref="Filter"/> describing the paging pattern to return</param>
	/// <param name="include">An optional function to include only the desired entities</param>
	/// <returns>The pagified search results</returns>
	public Task<IEnumerable<TEntity>> GetPagified(Filter filter, Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null);

	/// <summary>
	/// Returns the total number of entries in the repository table
	/// </summary>
	/// <param name="filter">The <see cref="Filter"/> describing the paging pattern to return</param>
	/// <returns>The entry count</returns>
	public Task<int> GetCount(Filter? filter = null);

	/// <summary>
	/// Pulls from a repository with optional search features
	/// </summary>
	/// <remarks>
	/// This method accepts an optional search predicate, an optional number of results to skip, and an optional number of results to take. If no parameters are provided, this method returns the entire result set. 
	/// </remarks>
	/// <param name="predicate">An EF Core-style search predicate</param>
	/// <param name="skip">The number of results to skip</param>
	/// <param name="take">The number of results to take</param>
	/// <param name="include">An optional function to include only the desired entities</param>
	/// <returns>The search results</returns>
	public Task<IEnumerable<TEntity>> Find(Expression<Func<TEntity, bool>>? predicate = null, int skip = 0, int take = 0, Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null);

	/// <summary>
	/// Returns a <c>TEntity</c> by primary key
	/// </summary>
	/// <param name="id">The primary key of the <c>TEntity</c> to find</param>
	/// <param name="include">An optional function to include only the desired entities</param>
	/// <returns>The requested <c>TEntity</c></returns>
	public Task<TEntity> FindById(TKey id, Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null);

	/// <summary>
	/// Creates a new <c>TEntity</c> in the repository table
	/// </summary>
	/// <param name="entity">The <c>TEntity</c> to create</param>
	/// <returns>The primary key of the new entity</returns>
	public Task<TKey> Create(TEntity entity);

	/// <summary>
	/// Updates an existing <c>TEntity</c> in the repository table
	/// </summary>
	/// <param name="entity">The <c>TEntity</c> to update</param>
	/// <returns><c>Task</c></returns>
	public Task Update(TEntity entity);

	/// <summary>
	/// Deletes a <c>TEntity</c> by primary key
	/// </summary>
	/// <param name="id">The primary key of the <c>TEntity</c> to delete</param>
	/// <returns><c>Task</c></returns>
	public Task Delete(TKey id);
}

public interface IRepository<TEntity> : IRepository<TEntity, Guid>
	where TEntity : EntityBase
{}