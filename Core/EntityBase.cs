using System;

namespace Core;

/// <summary>
/// A base class containing the fields required by all entities in the app with a customizable primary key type
/// </summary>
/// <typeparam name="TKey">The type of the primary key</typeparam>
public abstract class EntityBase<TKey>
	where TKey : struct, IEquatable<TKey>
{
	/// <summary>
	/// Represents the primary key of the entity
	/// </summary>
	public TKey Id { get; set; }

	/// <summary>
	/// A unique value on the entity that ensures the entity is not modified concurrently
	/// </summary>
	public Guid ConcurrencyStamp { get; set; }
}

/// <summary>
/// A base class containing the fields required by all entities in the app with a GUID primary key
/// </summary>
public abstract class EntityBase : EntityBase<Guid> {}