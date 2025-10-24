using Products.Write.Domain.Base;
using System.ComponentModel;

namespace Products.Write.Domain.Enumerations
{
    public class Status : Enumeration
    {
        public static Status Active = new Status(1, nameof(Active));        // .ToLowerInvariant());
        public static Status InActive = new Status(2, nameof(InActive));    // .ToLowerInvariant());
        public static Status Obsolete = new Status(3, nameof(Obsolete));    // .ToLowerInvariant());

        public Status(int id, string name) : base(id, name)
        {
        }

        public static IEnumerable<Status> List() => new[] { Active, InActive, Obsolete };

        public static Status FromName(string name)
        {
            var state = List().SingleOrDefault(s => string.Equals(s.Name, name, StringComparison.CurrentCultureIgnoreCase));

            if (state == null)
            {
                throw new InvalidEnumArgumentException($"Possible values for Category: {string.Join(",", List().Select(s => s.Name))}");
                // return Status.InActive;
            }

            return state;
        }

        public static Status From(int id)
        {
            var state = List().SingleOrDefault(s => s.Id == id);

            if (state == null)
            {
                throw new InvalidEnumArgumentException($"Possible values for Category: {string.Join(",", List().Select(s => s.Name))}");
                // return Status.InActive;
            }

            return state;
        }
    }
}
