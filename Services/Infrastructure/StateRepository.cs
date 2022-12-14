using System;
using System.Linq;
using System.Linq.Expressions;
using Autoinjector;
using Microsoft.Extensions.DependencyInjection;

namespace Services.Infrastructure;

[Service(ServiceLifetime.Scoped, typeof(IRepository<State>))]
public class StateRepository : RepositoryBase<State>
{
	public StateRepository(ApplicationDbContext context) : base(context)
	{
	}

	/// <inheritdoc />
	protected override IQueryable<State> Search(IQueryable<State> results, string searchTerm)
	{
		return results.Where(s => s.Name.ToLower().Contains(searchTerm)
		                       || s.Abbreviation.ToLower().Contains(searchTerm));
	}

	/// <inheritdoc />
	protected override Expression<Func<State, object>> GetSortPredicate(string sortName) => sortName switch
	{
		nameof(State.Name) => s => s.Name,
		nameof(State.Abbreviation) => s => s.Abbreviation,
		_ => s => s.Name
	};
}