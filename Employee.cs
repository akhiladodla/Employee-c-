using System;
namespace EmployeeApp
{
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Department { get; set; } = "";
        public decimal BaseSalary { get; set; }  // Used by FullTime; keep 0 for Contract

        public virtual decimal CalculateSalary() => BaseSalary;
        public virtual string EmployeeType => "Employee";

        public override string ToString()
        {
            return $"{Id,-4} {Name,-20} {Department,-12} {EmployeeType,-10} {CalculateSalary(),10:C}";
        }
    }
}