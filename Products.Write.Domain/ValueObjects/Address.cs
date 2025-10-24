using Products.Write.Domain.Base;

namespace Products.Write.Domain.ValueObjects
{
    // value objects are usually serialized and deserialized to go through message queues,
    // and being read-only stops the deserializer from assigning values, so you just leave them as private set

    public class Address : ValueObject
    {
        // To Persist value objects as owned entity types in EF Core 2.0 and later,
        // see: https://learn.microsoft.com/en-us/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/implement-value-objects
        public string? Street { get; private set; }
        public string? City { get; private set; }
        public string? State { get; private set; }
        public string? Country { get; private set; }
        public string? ZipCode { get; private set; }

        public Address() { }

        public Address(string street, string city, string state, string country, string zipcode)
        {
            Street = street;
            City = city;
            State = state;
            Country = country;
            ZipCode = zipcode;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            // Using a yield return statement to return each element one at a time
            yield return Street!;
            yield return City!;
            yield return State!;
            yield return Country!;
            yield return ZipCode!;
        }
    }
}

/*
Two instances of the Address type can be compared using all the following methods:

var one = new Address("1 Microsoft Way", "Redmond", "WA", "US", "98052");
var two = new Address("1 Microsoft Way", "Redmond", "WA", "US", "98052");

Console.WriteLine(EqualityComparer<Address>.Default.Equals(one, two)); // True
Console.WriteLine(object.Equals(one, two)); // True
Console.WriteLine(one.Equals(two)); // True
Console.WriteLine(one == two); // True

When all the values are the same, the comparisons are correctly evaluated as true. 
If you didn't choose to overload the == and != operators, then the last comparison of one == two would evaluate as false. 
*/