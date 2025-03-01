﻿using System;
using System.Collections.Generic;

namespace LyphTEC.Repository;

public abstract class Entity : IEntity, IEquatable<Entity>, IEqualityComparer<Entity>
{
    protected Entity()
    {
        DateCreatedUtc = DateTime.UtcNow;
        DateUpdatedUtc = DateTime.UtcNow;
    }

    public virtual dynamic Id { get; set; }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    public override bool Equals(object obj)
    {
        if (obj is null)
        {
            return false;
        }

        if (ReferenceEquals(this, obj))
        {
            return true;
        }

        return obj.GetType() == typeof(Entity) && Equals((Entity)obj);
    }

    public bool Equals(Entity other)
    {
        if (other is null)
        {
            return false;
        }

        return ReferenceEquals(this, other) || other.Id.Equals(Id);
    }

    public virtual DateTime DateCreatedUtc { get; set; }
    public virtual DateTime DateUpdatedUtc { get; set; }

    public bool Equals(Entity x, Entity y)
    {
        return x.Equals(y);
    }

    public int GetHashCode(Entity obj)
    {
        if (obj.Id.GetType() == typeof(int))
        {
            return (int)obj.Id;
        }

        return obj.GetHashCode();
    }
}
