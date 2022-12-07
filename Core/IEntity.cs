using System;

namespace Core;

public interface IEntity<TKey> where TKey : IEquatable<TKey>
{
	TKey Id { get; set; }
}