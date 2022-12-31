using System;
using Sienar;
using Microsoft.EntityFrameworkCore.Migrations;
using Sienar.Infrastructure;

#nullable disable

namespace Services.Migrations
{
    public partial class SeedDatabase : Migration
    {
	    // TODO: add values for actual application
	    private string _adminUsername = Environment.GetEnvironmentVariable("ADMIN_USERNAME");
	    private string _adminEmail = Environment.GetEnvironmentVariable("ADMIN_EMAIL");
	    private string _adminPasswordHash = Environment.GetEnvironmentVariable("ADMIN_PASSWORD_HASH");
	    private string _adminUserId = Environment.GetEnvironmentVariable("ADMIN_USER_ID");
	    private string _adminRoleId = Environment.GetEnvironmentVariable("ADMIN_ROLE_ID");

        protected override void Up(MigrationBuilder migrationBuilder)
        {
	        // Add states
            State[] states =
	        {
		        new State
		        {
			        Name = "Alabama",
			        Abbreviation = "AL"
		        },
		        new State
		        {
			        Name = "Alaska",
			        Abbreviation = "AK"
		        },
		        new State
		        {
			        Name = "Arkansas",
			        Abbreviation = "AR"
		        },
		        new State
		        {
			        Name = "Arizona",
			        Abbreviation = "AZ"
		        },
		        new State
		        {
			        Name = "Calfornia",
			        Abbreviation = "CA"
		        },
		        new State
		        {
			        Name = "Colorado",
			        Abbreviation = "CO"
		        },
		        new State
		        {
			        Name = "Connecticut",
			        Abbreviation = "CT"
		        },
		        new State
		        {
			        Name = "Delaware",
			        Abbreviation = "DE"
		        },
		        new State
		        {
			        Name = "Florida",
			        Abbreviation = "FL"
		        },
		        new State
		        {
			        Name = "Georgia",
			        Abbreviation = "GA"
		        },
		        new State
		        {
			        Name = "Hawaii",
			        Abbreviation = "HI"
		        },
		        new State
		        {
			        Name = "Idaho",
			        Abbreviation = "ID"
		        },
		        new State
		        {
			        Name = "Illinois",
			        Abbreviation = "IL"
		        },
		        new State
		        {
			        Name = "Indiana",
			        Abbreviation = "IN"
		        },
		        new State
		        {
			        Name = "Iowa",
			        Abbreviation = "IA"
		        },
		        new State
		        {
			        Name = "Kansas",
			        Abbreviation = "KS"
		        },
		        new State
		        {
			        Name = "Kentucky",
			        Abbreviation = "KY"
		        },
		        new State
		        {
			        Name = "Louisiana",
			        Abbreviation = "LA"
		        },
		        new State
		        {
			        Name = "Maine",
			        Abbreviation = "ME"
		        },
		        new State
		        {
			        Name = "Maryland",
			        Abbreviation = "MD"
		        },
		        new State
		        {
			        Name = "Massachusetts",
			        Abbreviation = "MA"
		        },
		        new State
		        {
			        Name = "Michigan",
			        Abbreviation = "MI"
		        },
		        new State
		        {
			        Name = "Minnesota",
			        Abbreviation = "MN"
		        },
		        new State
		        {
			        Name = "Mississippi",
			        Abbreviation = "MS"
		        },
		        new State
		        {
			        Name = "Missouri",
			        Abbreviation = "MO"
		        },
		        new State
		        {
			        Name = "Montana",
			        Abbreviation = "MT"
		        },
		        new State
		        {
			        Name = "Nebraska",
			        Abbreviation = "NE"
		        },
		        new State
		        {
			        Name = "Nevada",
			        Abbreviation = "NV"
		        },
		        new State
		        {
			        Name = "New Hampshire",
			        Abbreviation = "NH"
		        },
		        new State
		        {
			        Name = "New Jersey",
			        Abbreviation = "NJ"
		        },
		        new State
		        {
			        Name = "New Mexico",
			        Abbreviation = "NM"
		        },
		        new State
		        {
			        Name = "New York",
			        Abbreviation = "NY"
		        },
		        new State
		        {
			        Name = "North Carolina",
			        Abbreviation = "NC"
		        },
		        new State
		        {
			        Name = "North Dakota",
			        Abbreviation = "ND"
		        },
		        new State
		        {
			        Name = "Ohio",
			        Abbreviation = "OH"
		        },
		        new State
		        {
			        Name = "Oklahoma",
			        Abbreviation = "OK"
		        },
		        new State
		        {
			        Name = "Oregon",
			        Abbreviation = "OR"
		        },
		        new State
		        {
			        Name = "Pennsylvania",
			        Abbreviation = "PA"
		        },
		        new State
		        {
			        Name = "Rhode Island",
			        Abbreviation = "RI"
		        },
		        new State
		        {
			        Name = "South Carolina",
			        Abbreviation = "SC"
		        },
		        new State
		        {
			        Name = "South Dakota",
			        Abbreviation = "SD"
		        },
		        new State
		        {
			        Name = "Tennessee",
			        Abbreviation = "TN"
		        },
		        new State
		        {
			        Name = "Texas",
			        Abbreviation = "TX"
		        },
		        new State
		        {
			        Name = "Utah",
			        Abbreviation = "UT"
		        },
		        new State
		        {
			        Name = "Vermont",
			        Abbreviation = "VT"
		        },
		        new State
		        {
			        Name = "Virginia",
			        Abbreviation = "VA"
		        },
		        new State
		        {
			        Name = "Washington",
			        Abbreviation = "WA"
		        },
		        new State
		        {
			        Name = "West Virginia",
			        Abbreviation = "WV"
		        },
		        new State
		        {
			        Name = "Wisconsin",
			        Abbreviation = "WI"
		        },
		        new State
		        {
			        Name = "Wyoming",
			        Abbreviation = "WY"
		        },
		        new State
		        {
			        Name = "Washington, DC",
			        Abbreviation = "DC"
		        },
	        };

	        foreach (var state in states)
	        {
		        migrationBuilder.Sql($"INSERT INTO [dbo].[States] ([Id], [Name], [Abbreviation], [ConcurrencyStamp]) VALUES (newid(), '{state.Name}', '{state.Abbreviation}', newid())");
	        }

	        // Add admin user
	        migrationBuilder.Sql($"INSERT INTO [dbo].[AspNetUsers] ([Id], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [EmailConfirmed], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnabled], [AccessFailedCount]) VALUES ('{_adminUserId}', '{_adminUsername}', '{_adminUsername.ToUpper()}', '{_adminEmail}', '{_adminEmail.ToUpper()}', '{_adminPasswordHash}', newid(), newid(), 1, 0, 0, 1, 0)");

	        // Add admin role and add admin user to admin role
	        migrationBuilder.Sql($"INSERT INTO [dbo].[AspNetRoles] ([Id], [Name], [NormalizedName], [ConcurrencyStamp]) VALUES ('{_adminRoleId}', '{Roles.Admin}', '{Roles.Admin.ToUpper()}', newid())");
	        migrationBuilder.Sql($"INSERT INTO [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES ('{_adminUserId}', '{_adminRoleId}')");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
	        // Delete states
	        migrationBuilder.Sql("DELETE FROM [dbo].[States] WHERE TRUE");

	        // Delete admin UserRole
	        migrationBuilder.Sql($"DELETE FROM [dbo].[AspNetUserRoles] WHERE [RoleId] = '{_adminRoleId}' AND [UserId] = '{_adminUserId}'");

	        // Delete admin user and role
	        migrationBuilder.Sql($"DELETE FROM [dbo].[AspNetUsers] WHERE [Id] = '{_adminUserId}'");
	        migrationBuilder.Sql($"DELETE FROM [dbo].[AspNetRoles] WHERE [Id] = '{_adminRoleId}'");
        }
    }
}
