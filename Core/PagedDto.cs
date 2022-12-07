using System;
using System.Collections.Generic;

namespace Core;

public class PagedDto<TModel>
{
	public IEnumerable<TModel> Items { get; set; } = Array.Empty<TModel>();
	public int TotalCount { get; set; }
}