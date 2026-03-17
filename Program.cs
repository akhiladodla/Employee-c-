 using System;
using System.Collections.Generic;
using System.Linq;

namespace EmployeeApp
{
    class Program
    {
        static readonly List<Employee> employees = new();

        static void Main()
        {
            while (true)
            {
                ShowMenu();
                Console.Write("Choose an option: ");
                string choice = Console.ReadLine() ?? "";

                try
                {
                    switch (choice)
                    {
                        case "1": AddFullTime(); break;
                        case "2": AddContract(); break;
                        case "3": ViewAll(); break;
                        case "4": SearchByDepartment(); break;
                        case "5": SortBySalary(); break;
                        case "6": DeleteEmployee(); break;
                        case "7": return;

                        // Bonus
                        case "8": ShowHighestPaid(); break;
                        case "9": ShowAverageSalary(); break;
                        case "10": GroupByDepartment(); break;

                        default:
                            Console.WriteLine("Invalid option. Please choose from the menu.");
                            break;
                    }
                }
                catch (InvalidOperationException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                catch (KeyNotFoundException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Unexpected error: {ex.Message}");
                }

                Console.WriteLine();
            }
        }

        static void ShowMenu()
        {
            Console.WriteLine("======================================");
            Console.WriteLine(" Employee Manager");
            Console.WriteLine("======================================");
            Console.WriteLine("1. Add Full-Time Employee");
            Console.WriteLine("2. Add Contract Employee");
            Console.WriteLine("3. View All Employees");
            Console.WriteLine("4. Search by Department");
            Console.WriteLine("5. Sort by Salary (High → Low)");
            Console.WriteLine("6. Delete Employee (by Id)");
            Console.WriteLine("7. Exit");
            Console.WriteLine("----- Bonus -----");
            Console.WriteLine("8. Show Highest Paid Employee");
            Console.WriteLine("9. Show Average Salary");
            Console.WriteLine("10. Group by Department");
            Console.WriteLine("======================================");
        }

        // ---------- Add Employees ----------
        static void AddFullTime()
        {
            Console.WriteLine("Add Full-Time Employee");
            int id = ReadInt("Id: ");
            string name = ReadRequired("Name: ");
            string dept = ReadRequired("Department: ");
            decimal baseSalary = ReadDecimal("Base Salary: ");
            decimal bonus = ReadDecimal("Bonus: ");

            var emp = new FullTimeEmployee
            {
                Id = id,
                Name = name,
                Department = dept,
                BaseSalary = baseSalary,
                Bonus = bonus
            };

            AddOrThrowIfDuplicateId(emp);
            Console.WriteLine("Full-time employee added successfully.");
        }

        static void AddContract()
        {
            Console.WriteLine("Add Contract Employee");
            int id = ReadInt("Id: ");
            string name = ReadRequired("Name: ");
            string dept = ReadRequired("Department: ");
            decimal hours = ReadDecimal("Hours Worked: ");
            decimal rate = ReadDecimal("Hourly Rate: ");

            var emp = new ContractEmployee
            {
                Id = id,
                Name = name,
                Department = dept,
                HoursWorked = hours,
                HourlyRate = rate,
                BaseSalary = 0
            };

            AddOrThrowIfDuplicateId(emp);
            Console.WriteLine("Contract employee added successfully.");
        }

        static void AddOrThrowIfDuplicateId(Employee emp)
        {
            if (employees.Any(e => e.Id == emp.Id))
                throw new InvalidOperationException("An employee with this Id already exists.");
            employees.Add(emp);
        }

        // ---------- View / Search / Sort / Delete ----------
        static void ViewAll()
        {
            EnsureNotEmpty();
            PrintEmployees(employees);
        }

        static void SearchByDepartment()
        {
            EnsureNotEmpty();
            string dept = ReadRequired("Enter department to search: ");
            var match = employees
                .Where(e => string.Equals(e.Department, dept, StringComparison.OrdinalIgnoreCase))
                .ToList();

            if (match.Count == 0)
            {
                Console.WriteLine("No employees found in that department.");
                return;
            }

            PrintEmployees(match);
        }

        static void SortBySalary()
        {
            EnsureNotEmpty();
            var sorted = employees
                .OrderByDescending(e => e.CalculateSalary())
                .ToList();

            PrintEmployees(sorted);
        }

        static void DeleteEmployee()
        {
            EnsureNotEmpty();
            int id = ReadInt("Enter Employee Id to delete: ");
            var emp = employees.FirstOrDefault(e => e.Id == id);
            if (emp == null)
                throw new KeyNotFoundException("Employee not found.");

            employees.Remove(emp);
            Console.WriteLine("Employee deleted.");
        }

        // ---------- Bonus ----------
        static void ShowHighestPaid()
        {
            EnsureNotEmpty();
            var top = employees
                .OrderByDescending(e => e.CalculateSalary())
                .First();

            Console.WriteLine($"Highest Paid: {top.Name} [{top.EmployeeType}] | {top.CalculateSalary():C} | Dept: {top.Department} | Id: {top.Id}");
        }

        static void ShowAverageSalary()
        {
            EnsureNotEmpty();
            decimal avg = employees.Average(e => e.CalculateSalary());
            Console.WriteLine($"Average Salary: {avg:C}");
        }

        static void GroupByDepartment()
        {
            EnsureNotEmpty();
            var groups = employees.GroupBy(e => e.Department);

            foreach (var g in groups)
            {
                Console.WriteLine($"\nDepartment: {g.Key} (Total: {g.Count()})");
                foreach (var e in g.OrderByDescending(x => x.CalculateSalary()))
                {
                    Console.WriteLine($"  - [{e.EmployeeType}] {e.Name} | Id: {e.Id} | Salary: {e.CalculateSalary():C}");
                }
            }
        }

        // ---------- Helpers ----------
        static void PrintEmployees(IEnumerable<Employee> list)
        {
            Console.WriteLine();
            Console.WriteLine($"{"Id",-4} {"Name",-20} {"Department",-12} {"Type",-10} {"Salary",10}");
            Console.WriteLine(new string('-', 65));
            foreach (var e in list)
                Console.WriteLine(e.ToString());
        }

        static void EnsureNotEmpty()
        {
            if (employees.Count == 0)
                throw new InvalidOperationException("The employee list is empty. Add employees first.");
        }

        static int ReadInt(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string? s = Console.ReadLine();
                if (int.TryParse(s, out int value)) return value;
                Console.WriteLine("Please enter a valid integer.");
            }
        }

        static decimal ReadDecimal(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string? s = Console.ReadLine();
                if (decimal.TryParse(s, out decimal value)) return value;
                Console.WriteLine("Please enter a valid number.");
            }
        }

        static string ReadRequired(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string? s = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(s)) return s.Trim();
                Console.WriteLine("This value is required.");
            }
        }
    }
}