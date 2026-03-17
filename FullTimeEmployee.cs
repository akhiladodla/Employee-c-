using System;
namespace EmployeeApp
{
    public class FullTimeEmployee : Employee
    {
        public decimal Bonus { get; set; }
        public override decimal CalculateSalary() => BaseSalary + Bonus;
        public override string EmployeeType => "FullTime";
    }
}