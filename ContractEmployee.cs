using System;
namespace EmployeeApp
{
    public class ContractEmployee : Employee
    {
        public decimal HoursWorked { get; set; }
        public decimal HourlyRate { get; set; }
        public override decimal CalculateSalary() => HoursWorked * HourlyRate;
        public override string EmployeeType => "Contract";
    }
}