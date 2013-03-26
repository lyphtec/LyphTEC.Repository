using System;

namespace LyphTEC.Repository
{
    public abstract class Entity : IEntity, IEquatable<Entity>
    {
        protected Entity()
        {
            DateCreatedUtc = DateTime.UtcNow;
            DateUpdatedUtc = DateTime.UtcNow;
        }

        public virtual object Id { get; set; }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
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
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            return ReferenceEquals(this, other) || other.Id.Equals(Id);
        }

        public virtual DateTime DateCreatedUtc { get; set; }
        public virtual DateTime DateUpdatedUtc { get; set; }

    }
}
