namespace MVC_Assignment3.Models
{
       public class EmployeeList
    {
        public int EmployeeId { get; set; }
        public int DepartmentId { get; set; }
        public int SalaryInformatonId { get; set; }



        public string? EmployeeName { get; set; }
        public string? DepartmentName { get; set; }
        public int IsDeleted { get; set; }
        public int IsModified { get; set; }

        public double AmountPerYear { get; set; }
    }
    public enum Dept
    {
        HR,
        IT,
        Finance

    }
}
