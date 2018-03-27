using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Trek
{
    internal class SurveyResult
    {
        public ICollection<string> Bikes { get; }

        public SurveyResult()
        {
            Bikes = new Collection<string>();
        }

        public override int GetHashCode()
        {
            const int startValue = 17;
            const int multiplier = 59;

            return Bikes.Aggregate(startValue, (current, value) => (current * multiplier) + value.GetHashCode());
        }
    }
}
