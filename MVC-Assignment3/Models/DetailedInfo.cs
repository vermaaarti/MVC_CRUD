using MVC_Assignment3.Models;


namespace MVCAssignment2.Models
{
    public class DetailedInfo
    {

        public DetailedInfo(EmployeeInfo employeeInfo, SalaryInfo salaryInfo)
        {
            EmployeeInfo = employeeInfo;
            SalaryInfo = salaryInfo;
        }

        public EmployeeInfo EmployeeInfo { get; set; }

        public SalaryInfo SalaryInfo { get; set; }
    }
}