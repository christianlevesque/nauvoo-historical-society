#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Identity;
using Core;
using Microsoft.EntityFrameworkCore;

namespace Repositories.Identity;

[ExcludeFromCodeCoverage]
[Index(nameof(UserName), IsUnique = true)]
[Index(nameof(Email), IsUnique = true)]
public class ApplicationUser : IdentityUser, IEntity<string>
{
	/// <summary>
    /// Gets or sets the primary key for this user.
    /// </summary>
    [PersonalData]
	[MaxLength(36)]
    public override string Id { get; set; }

    /// <summary>
    /// Gets or sets the user name for this user.
    /// </summary>
    [ProtectedPersonalData]
    [MaxLength(32)]
    [Required]
    public override string UserName { get; set; }

    /// <summary>
    /// Gets or sets the normalized user name for this user.
    /// </summary>
    [ProtectedPersonalData]
    [MaxLength(32)]
    public override string NormalizedUserName { get; set; }

    /// <summary>
    /// Gets or sets the email address for this user.
    /// </summary>
    [ProtectedPersonalData]
    [MaxLength(100)]
    [Required]
    public override string Email { get; set; }

    /// <summary>
    /// Gets or sets the normalized email address for this user.
    /// </summary>
    [ProtectedPersonalData]
    [MaxLength(100)]
    public override string NormalizedEmail { get; set; }

    /// <summary>
    /// Gets or sets a salted and hashed representation of the password for this user.
    /// </summary>
    [MaxLength(100)]
    public override string PasswordHash { get; set; }

    /// <summary>
    /// A random value that must change whenever a users credentials change (password changed, login removed)
    /// </summary>
    [MaxLength(36)]
    public override string SecurityStamp { get; set; }

    /// <summary>
    /// A random value that must change whenever a user is persisted to the store
    /// </summary>
    [MaxLength(36)]
    public override string ConcurrencyStamp { get; set; } = Guid.NewGuid().ToString();

    /// <summary>
    /// Gets or sets a telephone number for the user.
    /// </summary>
    [ProtectedPersonalData]
    [MaxLength(20)]
    public override string PhoneNumber { get; set; }

	[PersonalData]
	[MaxLength(100)]
	public virtual string PendingEmail { get; set; }
}
