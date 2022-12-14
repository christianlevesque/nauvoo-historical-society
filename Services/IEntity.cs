using System;

namespace Services;

public interface IEntity<TKey> where TKey : IEquatable<TKey>
{
	TKey Id { get; set; }
}