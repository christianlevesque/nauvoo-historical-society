#nullable disable

using System.ComponentModel.DataAnnotations;

namespace Core.Infrastructure;

public class StateDto : EntityBase
{
	[Required]
	[MaxLength(20, ErrorMessage = "The state name must be no longer than 20 characters long")]
	public string Name { get; set; }

	[Required]
	[MaxLength(2, ErrorMessage = "The state abbreviation must be exactly 2 characters long")]
	[MinLength(2, ErrorMessage = "The state abbreviation must be exactly 2 characters long")]
	public string Abbreviation { get; set; }
}