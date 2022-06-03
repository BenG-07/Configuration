using DUAttributes.DataUnitType;

namespace ConfigurationManager.DataUnitComparison
{
    public class IsDataSourceUnit : IsDataUnit
    {
        public override bool Visit(DataSourceUnit dataSourceUnit) => true;
    }
}
