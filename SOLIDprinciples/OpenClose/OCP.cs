using NUnit.Framework;

using System;
using System.Collections.Generic;
using System.Text;

namespace SOLIDprinciples.OpenClose
{
    public class OCP
    {
        // A software Artifact should be Open for Extension but Closed for Modification

        // Make component extensible without modify the component, 
        // so the Low level components changes don't affect Hight level components
        // The components that contains Business rulers should be more protected

        // If A should be protected from changes in B, B must depent on A.
        public class A { }
        public class B { public A a; }

        public abstract class BaseSalaryCalculator
        {
            protected DeveloperReport DeveloperReport { get; private set; }
            public BaseSalaryCalculator(DeveloperReport developerReport)
            {
                DeveloperReport = developerReport;
            }
            public abstract double CalculateSalary();
        }

        public class SeniorDevSalaryCalculator : BaseSalaryCalculator
        {
            public SeniorDevSalaryCalculator(DeveloperReport report)
                : base(report)
            {
            }
            public override double CalculateSalary() => DeveloperReport.HourlyRate * DeveloperReport.WorkingHours * 1.2;
        }

        public class SalaryCalculator
        {
            private readonly IEnumerable<BaseSalaryCalculator> _developerCalculation;
            public SalaryCalculator(IEnumerable<BaseSalaryCalculator> developerCalculation)
            {
                _developerCalculation = developerCalculation;
            }
            public double CalculateTotalSalaries()
            {
                double totalSalaries = 0D;
                foreach (var devCalc in _developerCalculation)
                {
                    totalSalaries += devCalc.CalculateSalary();
                }
                return totalSalaries;
            }
        }

        // Afther modifies
        public class JuniorDevSalaryCalculator : BaseSalaryCalculator
        {
            public JuniorDevSalaryCalculator(DeveloperReport developerReport)
                : base(developerReport)
            {
            }
            public override double CalculateSalary() => DeveloperReport.HourlyRate * DeveloperReport.WorkingHours;
        }

        [Test]
        public void Run()
        {
            var devCalculations = new List<BaseSalaryCalculator>
            {
                new SeniorDevSalaryCalculator(new DeveloperReport {Id = 1, Name = "Dev1", Level = "Senior developer", HourlyRate = 30.5, WorkingHours = 160 }),
                new JuniorDevSalaryCalculator(new DeveloperReport {Id = 2, Name = "Dev2", Level = "Junior developer", HourlyRate = 20, WorkingHours = 150 }),
                new SeniorDevSalaryCalculator(new DeveloperReport {Id = 3, Name = "Dev3", Level = "Senior developer", HourlyRate = 30.5, WorkingHours = 180 })
            };
            var calculator = new SalaryCalculator(devCalculations);
            Console.WriteLine($"Sum of all the developer salaries is {calculator.CalculateTotalSalaries()} dollars");        
        }
    }
}
