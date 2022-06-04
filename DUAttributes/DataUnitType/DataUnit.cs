﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUAttributes.DataUnitType
{
    public abstract class DataUnit : IDataUnitVisitable
    {
        public abstract void Accept(IDataUnitVisitor visitor);

        public abstract T Accept<T>(IDataUnitVisitor<T> visitor);

        public override string ToString() => this.GetType().Name;
    }
}