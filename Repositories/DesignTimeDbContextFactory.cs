using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Repositories;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
	public ApplicationDbContext CreateDbContext(string[] args)
	{
		var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
		var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__Default");
		if (string.IsNullOrEmpty(connectionString))
		{
			throw new NullReferenceException("Database connection string cannot be null.");
		}

		builder.UseSqlServer(connectionString);

		return new ApplicationDbContext(builder.Options);
	}
}