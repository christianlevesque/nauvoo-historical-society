#nullable disable

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Sienar.Identity;
using Sienar.Infrastructure;

namespace Sienar;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole, string>
{
	public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
	{
	}

	public DbSet<State> States { get; set; }
}
