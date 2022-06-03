using DUAttributes.DataUnitType;

namespace ConfigurationManager.DataUnitComparison
{
    public abstract class IsDataUnit : IDataUnitVisitor<bool>
    {
        public virtual bool Visit(DataSourceUnit dataSourceUnit) => false;

        public virtual bool Visit(DataProcessingUnit dataProcessingUnit) => false;

        public virtual bool Visit(DataVisualizationUnit dataVisualizationUnit) => false;
    }
}
