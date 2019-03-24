using System.Collections.Generic;

namespace Domain.Base
{
    public abstract class Entity<TKey>
    {
        protected bool Equals(Entity<TKey> other)
        {
            return EqualityComparer<TKey>.Default.Equals(Id, other.Id);
        }

        public override int GetHashCode()
        {
            return EqualityComparer<TKey>.Default.GetHashCode(Id);
        }

        public TKey Id { get; set; }
        public bool Active { get; set; }

        public override bool Equals(object obj)
        {
            var other = obj as Entity<TKey>;

            if (ReferenceEquals(other, null))
                return false;

            if (ReferenceEquals(this, other))
                return true;

            return Id.Equals(other.Id);
        }

        public static bool operator ==(Entity<TKey> a, Entity<TKey> b)
        {
            if (ReferenceEquals(a, null) && ReferenceEquals(b, null))
                return true;

            if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
                return false;

            return a.Id.Equals(b);
        }

        public static bool operator !=(Entity<TKey> a, Entity<TKey> b)
        {
            return !(a == b);
        } 
    }
}
