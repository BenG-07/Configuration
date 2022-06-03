namespace DUAttributes.DataUnitType
{
    public interface IDataUnitVisitor
    {
        void Visit(DataSourceUnit dataSourceUnit);
        void Visit(DataProcessingUnit dataProcessingUnit);
        void Visit(DataVisualizationUnit dataVisualizationUnit);
    }

    public interface IDataUnitVisitor<T>
    {
        T Visit(DataSourceUnit dataSourceUnit);
        T Visit(DataProcessingUnit dataProcessingUnit);
        T Visit(DataVisualizationUnit dataVisualizationUnit);
    }
}
