using System;

namespace Sienar;

public interface IEntity<TKey> where TKey : IEquatable<TKey>
{
	TKey Id { get; set; }
}