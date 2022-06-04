﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUAttributes.DataUnitType
{
    public class DataVisualizationUnit : DataUnit
    {
        public override void Accept(IDataUnitVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override T Accept<T>(IDataUnitVisitor<T> visitor)
        {
            return visitor.Visit(this);
        }
    }
}