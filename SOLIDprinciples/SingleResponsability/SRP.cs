using NUnit.Framework;

using System;
using System.Collections.Generic;
using System.Text;

namespace SOLIDprinciples.SingleResponsability
{
    public class SRP
    {
        // A Module should be responsible to one, and only one, actor.

        public interface Employee // Responsible to rappresent data
        {
            public int id { get; set; }
        }

        public interface PayCalculator // Responsible for calculate pays
        {
            public float CalculatePay(Employee employee); 
        }

        public interface HourReporter // Responsible for report hours
        {
            public float ReportHours(Employee employee);
        }

        public interface EmployeeSaver // Responsible for save to a DB
        {
            public float SaveEmployee(Employee employee);
        }

        public class EmployeeFacade // Responsible to supply an interface to the client
        {
            private readonly Employee _employee;

            private PayCalculator _payCalculator;
            private HourReporter _hourReporter;
            private EmployeeSaver _employeeSaver;

            public EmployeeFacade(Employee employee)
            {
                _employee = employee;
            }

            public float CalculatePay() { return _payCalculator.CalculatePay(_employee); }
            public void ReportHours() { _hourReporter.ReportHours(_employee); }
            public void Save() { _employeeSaver.SaveEmployee(_employee); }

        }
    }
}
