using Seedwork.DomainDriven.Core;

namespace Wanderland.Flight.Domain
{
    public class City:ValueObject
    {
        public City(int id)
        {
            Id = id;
        }

        public int Id { get; private set; }
        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Id;
        }
    }
}