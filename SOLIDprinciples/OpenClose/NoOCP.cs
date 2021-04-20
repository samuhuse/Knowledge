using NUnit.Framework;

using System;
using System.Collections.Generic;
using System.Text;

namespace SOLIDprinciples.OpenClose
{
    public class NoOCP
    {
        public class SalaryCalculator
        {
            private readonly IEnumerable<DeveloperReport> _developerReports;
            public SalaryCalculator(List<DeveloperReport> developerReports)
            {
                _developerReports = developerReports;
            }

            // Before modifies
            //public double CalculateTotalSalaries()
            //{
            //    double totalSalaries = 0D;
            //    foreach (var devReport in _developerReports)
            //    {
            //        totalSalaries += devReport.HourlyRate * devReport.WorkingHours;
            //    }
            //    return totalSalaries;
            //}

            // Afther modifies
            public double CalculateTotalSalaries()
            {
                double totalSalaries = 0D;
                foreach (var devReport in _developerReports)
                {
                    if (devReport.Level == "Senior developer")
                    {
                        totalSalaries += devReport.HourlyRate * devReport.WorkingHours * 1.2;
                    }
                    else
                    {
                        totalSalaries += devReport.HourlyRate * devReport.WorkingHours;
                    }
                }
                return totalSalaries;
            }
        }

        [Test]
        public void Run()
        {
            var devReports = new List<DeveloperReport>
            {
                new DeveloperReport {Id = 1, Name = "Dev1", Level = "Senior developer", HourlyRate  = 30.5, WorkingHours = 160 },
                new DeveloperReport {Id = 2, Name = "Dev2", Level = "Junior developer", HourlyRate  = 20, WorkingHours = 150 },
                new DeveloperReport {Id = 3, Name = "Dev3", Level = "Senior developer", HourlyRate  = 30.5, WorkingHours = 180 }
            };
            var calculator = new SalaryCalculator(devReports);
            Console.WriteLine($"Sum of all the developer salaries is {calculator.CalculateTotalSalaries()} dollars");
        }    
    }
}
