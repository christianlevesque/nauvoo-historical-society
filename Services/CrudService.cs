using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Repositories;
using Services.Errors;
using Core;
using Microsoft.EntityFrameworkCore;

namespace Services;

public abstract class CrudService<TEntity, TDto, TKey, TRepo> : ICrudService<TDto, TKey>
	where TEntity : EntityBase<TKey>, new()
	where TDto : EntityBase<TKey>
	where TKey : struct, IEquatable<TKey>
	where TRepo : IRepository<TEntity, TKey>
{
	protected bool UsesHoneypot { get; init; }
	protected readonly TRepo Repo;
	protected readonly IDtoAdapter<TEntity, TDto> Adapter;

	public CrudService(TRepo repo, IDtoAdapter<TEntity, TDto> adapter)
	{
		Repo = repo;
		Adapter = adapter;
	}

	/// <inheritdoc />
	public virtual async Task<ServiceResult> Add(TDto model)
	{
		// Verify honeypot
		if (UsesHoneypot && model is HoneypotDto<TKey> { IsSpambot: true })
		{
			return ServiceResult.Ok();
		}

		var entity = await Adapter.MapAddDto(model);
		try
		{
			return ServiceResult.Ok(await Repo.Create(entity));
		}
		catch (Exception e)
		{
			return CreateErrorResult(e);
		}
	}

	/// <inheritdoc />
	public virtual async Task<ServiceResult> Edit(TDto model)
	{
		TEntity entity;
		try
		{
			entity = await Repo.FindById(model.Id);
		}
		catch (Exception e)
		{
			return CreateErrorResult(e);
		}

		await Adapter.MapEditDto(model, entity);
		entity.ConcurrencyStamp = model.ConcurrencyStamp;

		try
		{
			await Repo.Update(entity);
			return ServiceResult.Ok();
		}
		catch (Exception e)
		{
			return CreateErrorResult(e);
		}
	}

	/// <inheritdoc />
	public virtual async Task<ServiceResult> Get(TKey id)
	{
		TEntity entity;
		try
		{
			entity = await Repo.FindById(id);
		}
		catch (Exception e)
		{
			return CreateErrorResult(e);
		}

		return ServiceResult.Ok((await Adapter.MapToDto(entity))!);
	}

	/// <inheritdoc />
	public virtual async Task<ServiceResult> Get(Filter? filter = null)
	{
		try
		{
			var entries = await (filter is null ? Repo.Find() : Repo.GetPagified(filter));

			var items = new List<TDto>();

			foreach (var entry in entries)
			{
				items.Add(await Adapter.MapToDto(entry));
			}

			var dto = new PagedDto<TDto>
			{
				Items = items,
				TotalCount = await Repo.GetCount(filter)
			};

			return ServiceResult.Ok(dto);
		}
		catch (Exception e)
		{
			return CreateErrorResult(e);
		}
	}

	/// <inheritdoc />
	public virtual async Task<ServiceResult> Delete(TKey id)
	{
		try
		{
			await Repo.Delete(id);
			return ServiceResult.Ok();
		}
		catch (Exception e)
		{
			return CreateErrorResult(e);
		}
	}

	protected static ServiceResult CreateErrorResult(Exception exception)
	{
		var error = ServiceError.Unknown;
		var message = new StringBuilder(ErrorMessages.ContactIt);

		switch (exception)
		{
			case DbUpdateConcurrencyException:
				error = ServiceError.DatabaseConcurrency;
				ResetMessage(message, exception.Message);
				break;
			case KeyNotFoundException:
				error = ServiceError.NotFound;
				ResetMessage(message, exception.Message);
				break;
			case AppConflictException:
				error = ServiceError.DataConflict;
				ResetMessage(message, exception.Message);
				break;
			case ApplicationException:
				ResetMessage(message, exception.Message);
				break;
			default:
				if (exception.StackTrace?.Contains("sql", StringComparison.OrdinalIgnoreCase) ?? false)
				{
					message.Insert(0, $"{ErrorMessages.Database} ");
				}
				else
				{
					message.Insert(0, $"{ErrorMessages.Generic.Unknown} ");
				}
				break;
		}

		return ServiceResult.Failure(error, message.ToString());

		void ResetMessage(StringBuilder sb, string errorMessage)
		{
			sb.Clear();
			sb.Append(errorMessage);
		}
	}
}

public abstract class CrudService<TEntity, TDto, TRepo> : CrudService<TEntity, TDto, Guid, TRepo>
	where TEntity : EntityBase, new()
	where TDto : EntityBase
	where TRepo : IRepository<TEntity>
{
	/// <inheritdoc />
	protected CrudService(TRepo repo,
	                      IDtoAdapter<TEntity, TDto> adapter)
		: base(repo, adapter)
	{
	}
}