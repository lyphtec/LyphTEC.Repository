using System;

namespace LyphTEC.Repository;

public interface IAggregateRoot : IEntity
{
    DateTime DateCreatedUtc { get; set; }
    DateTime DateUpdatedUtc { get; set; }
}
