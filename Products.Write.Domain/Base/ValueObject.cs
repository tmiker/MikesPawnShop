namespace Products.Write.Domain.Base
{
    public abstract class ValueObject
    {
        // To Persist value objects as owned entity types in EF Core 2.0 and later,
        // see: https://learn.microsoft.com/en-us/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/implement-value-objects

        protected static bool EqualOperator(ValueObject left, ValueObject right)
        {
            if (ReferenceEquals(left, null) ^ ReferenceEquals(right, null))
            {
                return false;
            }
            return ReferenceEquals(left, right) || left!.Equals(right!);
        }

        protected static bool NotEqualOperator(ValueObject left, ValueObject right)
        {
            return !EqualOperator(left, right);
        }

        protected abstract IEnumerable<object> GetEqualityComponents();

        public override bool Equals(object? obj)
        {
            if (obj == null || obj.GetType() != GetType())
            {
                return false;
            }

            var other = (ValueObject)obj;

            return GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
        }

        public override int GetHashCode()
        {
            return GetEqualityComponents()
                .Select(x => x != null ? x.GetHashCode() : 0)
                .Aggregate((x, y) => x ^ y);
        }

        //// If desired to overload the == and != operators by making comparisons delegate to the Equals override: 
        //public static bool operator ==(ValueObject one, ValueObject two)
        //{
        //    return EqualOperator(one, two);
        //}

        //public static bool operator !=(ValueObject one, ValueObject two)
        //{
        //    return NotEqualOperator(one, two);
        //}
    }
}
