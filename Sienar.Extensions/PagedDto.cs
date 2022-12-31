using System;
using System.Collections.Generic;

namespace Sienar;

public class PagedDto<TModel>
{
	public IEnumerable<TModel> Items { get; set; } = Array.Empty<TModel>();
	public int TotalCount { get; set; }
}