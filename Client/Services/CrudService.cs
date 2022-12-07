using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Core;
using Client.Services.Infrastructure;

namespace Client.Services;

public abstract class CrudService<TViewDto, TAddDto, TEditDto, TKey, TService> : BaseHttpService<TService>, ICrudService<TViewDto, TAddDto, TEditDto, TKey>
	where TEditDto : EntityBase<TKey>
	where TKey : struct, IEquatable<TKey>
{
	protected string AddSuccessMessage = string.Empty;
	protected string EditSuccessMessage = string.Empty;
	protected string DeleteSuccessMessage = string.Empty;

	protected CrudService(string endpoint,
	                   HttpClient client,
	                   ILogger<TService> logger,
	                   ISnackbarService snackbar)
		: base(endpoint, client, logger, snackbar)
	{
	}

	public Task<TKey> Add(TAddDto model) => SendRequest<TKey>(Endpoint, AddSuccessMessage, input: model, method: HttpMethod.Post);

	public Task<bool> Edit(TEditDto model) => SendRequest($"{Endpoint}/{model.Id}", EditSuccessMessage, input: model, method: HttpMethod.Put);

	public Task<TViewDto?> Get(TKey id) => SendRequest<TViewDto>($"{Endpoint}/{id}");

	public async Task<PagedDto<TViewDto>> Get(Filter? filter = null)
	{
		if (filter?.SearchTerm != null)
		{
			filter.SearchTerm = filter.SearchTerm.ToLower();
		}

		return await SendRequest<PagedDto<TViewDto>>(Endpoint, input: filter) ?? new PagedDto<TViewDto>();
	}

	public Task<bool> Delete(TKey id) => SendRequest($"{Endpoint}/{id}", DeleteSuccessMessage, method: HttpMethod.Delete);
}

public abstract class CrudService<TViewDto, TAddDto, TEditDto, TService> : CrudService<TViewDto, TAddDto, TEditDto, Guid, TService>
	where TEditDto : EntityBase
{
	/// <inheritdoc />
	protected CrudService(string endpoint,
	                      HttpClient client,
	                      ILogger<TService> logger,
	                      ISnackbarService snackbar)
		: base(endpoint, client, logger, snackbar)
	{
	}
}