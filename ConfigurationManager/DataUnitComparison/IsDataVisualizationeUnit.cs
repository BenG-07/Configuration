using DUAttributes.DataUnitType;

namespace ConfigurationManager.DataUnitComparison
{
    public class IsDataVisualizationeUnit : IsDataUnit
    {
        public override bool Visit(DataVisualizationUnit dataVisualizationUnit) => true;
    }
}