using System;
using System.Collections.Generic;

namespace Services;

public class PagedDto<TModel>
{
	public IEnumerable<TModel> Items { get; set; } = Array.Empty<TModel>();
	public int TotalCount { get; set; }
}