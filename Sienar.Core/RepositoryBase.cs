using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Sienar;

namespace Sienar;

/// <summary>
/// A generic implementation of <see cref="IRepository{TEntity,TKey}"/>
/// </summary>
/// <remarks>
/// This implementation does not directly implement <c>Create(...)</c> or <c>FindById(...)</c>. This allows any class to be used for <c>TEntity</c>. This exists primarily because the default <see cref="Microsoft.AspNetCore.Identity.IdentityUser"/> doesn't extend from <see cref="EntityBase"/>, so we can't use that class as a type constraint for <c>TEntity</c>. <see cref="Microsoft.AspNetCore.Identity.IdentityUser"/> has string implementations of all the properties on <see cref="EntityBase"/>, so one workaround would be to make an interface <c>IEntityBase</c> which encapsulates the properties on <see cref="EntityBase"/>, but the current approach offers more flexibility in case we decide we need an entity that is NOT derived from <see cref="EntityBase"/> for whatever reason.
///
/// This implementation also does not implement a concurrency conflict detection strategy. Microsoft Identity has its own concurrency conflict strategy, and most other entities should use the <see cref="RepositoryBase{TEntity}"/> implementation, which <c>does</c> implement concurrency conflict detection.
/// </remarks>
/// <typeparam name="TEntity"></typeparam>
/// <typeparam name="TKey"></typeparam>
public abstract class RepositoryBase<TEntity, TKey> : IRepository<TEntity, TKey>
	where TEntity : class
	where TKey : IEquatable<TKey>
{
	protected readonly ApplicationDbContext Context;

	protected RepositoryBase(ApplicationDbContext context)
	{
		Context = context;
	}

	/// <inheritdoc />
	public virtual async Task<IEnumerable<TEntity>> Find(Expression<Func<TEntity, bool>>? predicate = null, int skip = 0, int take = 0, Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null)
	{
		include ??= Include;
		var result = include(Context.Set<TEntity>());
		if (predicate is not null)
		{
			result = result.Where(predicate);
		}

		if (skip > 0)
		{
			result = result.Skip(skip);
		}

		if (take > 0)
		{
			result = result.Take(take);
		}

		return await result.ToListAsync();
	}

	/// <inheritdoc />
	public virtual async Task<int> GetCount(Filter? filter = null)
	{
		var results = GetQueryable();
		if (filter is not null && !string.IsNullOrWhiteSpace(filter.SearchTerm))
		{
			results = Search(results, filter.SearchTerm);
		}

		return await results.CountAsync();
	}

	/// <inheritdoc />
	public virtual async Task<IEnumerable<TEntity>> GetPagified(Filter filter, Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null)
	{
		include ??= Include;
		var results = include(Context.Set<TEntity>());

		if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
		{
			results = Search(results, filter.SearchTerm);
		}

		var sortName = filter.SortName ?? string.Empty;
		var predicate = GetSortPredicate(sortName);
		results = filter.SortDescending
			          ? results.OrderByDescending(predicate)
			          : results.OrderBy(predicate);

		results = results.Skip(filter.PageSize * (filter.Page - 1))
		                 .Take(filter.PageSize);

		return await results.ToListAsync();
	}

	/// <inheritdoc />
	public virtual async Task Update(TEntity entity)
	{
		Context.Set<TEntity>()
			   .Update(entity);
		await Context.SaveChangesAsync();
	}

	/// <inheritdoc />
	public virtual async Task Delete(TKey id)
	{
		var entity = await Context.Set<TEntity>()
								  .FindAsync(id);
		if (entity == null)
		{
			return;
		}

		Context.Set<TEntity>()
			   .Remove(entity);
		await Context.SaveChangesAsync();
	}

	/// <inheritdoc />
	public abstract Task<TEntity> FindById(TKey id, Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null);

	/// <inheritdoc />
	public abstract Task<TKey> Create(TEntity entity);

	/// <summary>
	/// Sets the default set of related entities that should be included on a lookup
	/// </summary>
	/// <param name="results">The current data set</param>
	/// <returns>The current data set with related entities included</returns>
	protected virtual IQueryable<TEntity> Include(IQueryable<TEntity> results) => results;

	/// <summary>
	/// Returns the <see cref="ApplicationDbContext"/> as an <see cref="IQueryable{TEntity}"/>
	/// </summary>
	/// <returns></returns>
	protected IQueryable<TEntity> GetQueryable() => Context.Set<TEntity>();

	/// <summary>
	/// Applies search criteria on the database query
	/// </summary>
	/// <param name="results">The existing result set</param>
	/// <param name="searchTerm">The term to search for in the database</param>
	/// <returns>The result set with any applicable filters applied</returns>
	protected abstract IQueryable<TEntity> Search(IQueryable<TEntity> results, string searchTerm);

	/// <summary>
	/// Creates a predicate for use in sorting by columns
	/// </summary>
	/// <param name="sortName">The name of the column to sort by</param>
	/// <returns></returns>
	protected abstract Expression<Func<TEntity, object>> GetSortPredicate(string sortName);

	/// <summary>
	/// Generates a <see cref="NotSupportedException"/> to indicate a column cannot be sorted against
	/// </summary>
	/// <param name="name">The name of the column that can't be sorted</param>
	/// <returns>The exception for throwing</returns>
	protected NotSupportedException ColumnNotSupported(string name)
	{
		return new NotSupportedException($"The column {name} is not supported for sorting");
	}

	/// <summary>
	/// Creates a <see cref="KeyNotFoundException"/>
	/// </summary>
	/// <param name="key">The primary key of the entity that was not found</param>
	/// <returns>The exception for throwing</returns>
	protected KeyNotFoundException KeyNotFound(object key)
	{
		return new KeyNotFoundException($"Entity with ID {key} was not found");
	}

	/// <summary>
	/// Creates a <see cref="DbUpdateConcurrencyException"/>
	/// </summary>
	/// <param name="message"></param>
	/// <returns></returns>
	protected DbUpdateConcurrencyException ConcurrencyFailure(string message = "The database entry has been updated since you last accessed it. Please reload the page and try again.")
	{
		return new DbUpdateConcurrencyException(message);
	}
}

/// <summary>
/// A generic implementation of <see cref="IRepository{TEntity}"/> with a GUID primary key and concurrency conflict detection
/// </summary>
/// <remarks>
/// This repository only works with entities subclassed from <see cref="EntityBase"/>.
/// </remarks>
/// <typeparam name="TEntity"></typeparam>
public abstract class RepositoryBase<TEntity> : RepositoryBase<TEntity, Guid>, IRepository<TEntity>
	where TEntity : EntityBase
{
	/// <inheritdoc />
	protected RepositoryBase(ApplicationDbContext context) : base(context)
	{
	}

	/// <inheritdoc />
	public override async Task Update(TEntity entity)
	{
		// No tracking is needed to force EF to get the latest value from the database
		// If tracking is on, the old ConcurrencyStamp as applied by the Adapter will be returned from the query
		// and then the ConcurrencyStamp values will always be equal
		var original = await Context.Set<TEntity>()
		                            .AsNoTracking()
		                            .FirstOrDefaultAsync(e => e.Id == entity.Id);
		if (original is null)
		{
			throw KeyNotFound(entity.Id);
		}

		if (original.ConcurrencyStamp != entity.ConcurrencyStamp)
		{
			throw ConcurrencyFailure();
		}

		entity.ConcurrencyStamp = Guid.NewGuid();

		Context.Set<TEntity>()
		       .Update(entity);
		await Context.SaveChangesAsync();
	} 

	/// <inheritdoc />
	public override async Task<TEntity> FindById(Guid id, Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null)
	{
		include ??= Include;
		var set = await include(Context.Set<TEntity>())
			          .FirstOrDefaultAsync(e => e.Id.Equals(id));

		return set ?? throw KeyNotFound(id);
	}

	/// <inheritdoc />
	public override async Task<Guid> Create(TEntity entity)
	{
		entity.ConcurrencyStamp = Guid.NewGuid();

		await Context.Set<TEntity>()
		             .AddAsync(entity);
		await Context.SaveChangesAsync();

		return entity.Id;
	}
}