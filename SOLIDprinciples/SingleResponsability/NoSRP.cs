using NUnit.Framework;

using System;
using System.Collections.Generic;
using System.Text;

namespace SOLIDprinciples.SingleResponsability
{
    public class NoSRP
    {
        public interface Employee
        {
            public int id { get; set; }

            // The object is responsible for these Functions
            // up to 3 Actors could need to modify Employee
            public float CalculatePay();
            public void ReportHours();
            public void Save();
            
        }
    }
}
