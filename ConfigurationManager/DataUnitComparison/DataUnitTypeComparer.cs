using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DUAttributes.DataUnitType;

namespace ConfigurationManager.DataUnitComparison
{
    public class DataUnitTypeComparer : IDataUnitVisitor<bool>
    {
        DataUnit dataUnit;

        public DataUnitTypeComparer(DataUnit dataUnit)
        {
            this.dataUnit = dataUnit;
        }

        public bool Visit(DataSourceUnit dataSourceUnit) => dataUnit.Accept(new IsDataSourceUnit());

        public bool Visit(DataProcessingUnit dataProcessingUnit) => dataUnit.Accept(new IsDataProcessingUnit());

        public bool Visit(DataVisualizationUnit dataVisualizationUnit) => dataUnit.Accept(new IsDataVisualizationeUnit());
    }

}