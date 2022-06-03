using DUAttributes.DataUnitType;

namespace ConfigurationManager.DataUnitComparison
{
    public class IsDataProcessingUnit : IsDataUnit
    {
        public override bool Visit(DataProcessingUnit dataProcessingUnit) => true;
    }
}