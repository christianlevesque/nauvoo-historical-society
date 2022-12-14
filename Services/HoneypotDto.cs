using System;

namespace Services;

/// <summary>
/// Represents a DTO that has honeypot capabilities
/// </summary>
/// <typeparam name="TKey">The type of the entity's primary key</typeparam>
public class HoneypotDto<TKey> : EntityBase<TKey>
	where TKey : struct, IEquatable<TKey>
{
	/// <summary>
	/// Used to detect spambot submissions
	/// </summary>
	public string? SecretKeyField { get; set; }

	/// <summary>
	/// Used to determine how long a form took an agent to complete
	/// </summary>
	public TimeSpan TimeToComplete { get; set; }

	/// <summary>
	/// Used to determine whether the honeypot captured a spambot or not
	/// </summary>
	public bool IsSpambot => !string.IsNullOrEmpty(SecretKeyField);
}

/// <summary>
/// Represents a DTO that has honeypot capabilities, using a <see cref="Guid"/> primary key
/// </summary>
public class HoneypotDto : HoneypotDto<Guid> {}