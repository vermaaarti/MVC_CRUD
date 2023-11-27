using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using MVC_Assignment3.Models;
using Newtonsoft.Json;
using System.Data;
using System.Text.Json.Serialization;

namespace MVC_Assignment3.Controllers
{
    public class EmployeeController : Controller
    {

        private readonly string connectionString;
        public EmployeeController(IConfiguration config)
        {
            connectionString = config.GetConnectionString("DefaultConnection");
        }

        // creating dataTable
        public DataTable EmployeeData()

        {
            DataTable returnDataTable = new DataTable();

            using (SqlConnection connection = new SqlConnection(connectionString))

            {

                connection.Open();

                SqlCommand cmd = new SqlCommand();

                cmd.Connection = connection;

                cmd.CommandType = CommandType.StoredProcedure;

                cmd.CommandText = "DepartmentEmployeeSalaryDetail"; // Use the name of your stored procedure

                SqlDataAdapter dataAdp = new SqlDataAdapter(cmd);

                dataAdp.Fill(returnDataTable);

                connection.Close();
                    
            }

            return returnDataTable;
        }


        //  dataTable to object conversion
        public List<EmployeeList> ConvertTableIntoModel(DataTable returnDataTable)
        {
            List<EmployeeList> objectList = new List<EmployeeList>();

            foreach (DataRow dr in returnDataTable.Rows)
            {
                EmployeeList newObj = new EmployeeList();
             
                newObj.EmployeeId = Convert.ToInt32(dr["EmployeeId"]);  // Beware of the possible conversion errors due to type mismatches
                newObj.EmployeeName = dr["EmployeeName"].ToString();
                newObj.DepartmentId = Convert.ToInt32(dr["DepartmentId"]);
                newObj.SalaryInformatonId = Convert.ToInt32(dr["SalaryInformatonId"]);
                newObj.DepartmentName = dr["DepartmentName"].ToString();
                newObj.AmountPerYear = Convert.ToInt32(dr["AmountPerYear"]);
                 newObj.IsDeleted = 0;

                objectList.Add(newObj);
            }
            return objectList;
        }


        // converting from object to JSON
        public IActionResult JSONEmployeeData()
        {
            var Data = ConvertTableIntoModel(EmployeeData());
            return Ok(Data);
        }

        // fn to send the updated array to the database after changing the amount 
        public DataTable UpdatedRawData(List<EmployeeList> employeeList)

        {
          
           var returnDataTable = new DataTable();
            var JSONData = JsonConvert.SerializeObject(employeeList);
            using (SqlConnection connection = new SqlConnection(connectionString))

            {
                connection.Open();

                SqlCommand cmd = new SqlCommand();

                cmd.Connection = connection;

                cmd.CommandType = CommandType.StoredProcedure;

                cmd.CommandText = "SaveEmployeeAarti"; // Use the name of your stored procedure

                cmd.Parameters.Add(new SqlParameter("@JsonEmployee", JSONData));

                SqlDataAdapter dataAdp = new SqlDataAdapter(cmd);

                dataAdp.Fill(returnDataTable);

                connection.Close();

            }
            return returnDataTable;


        }

        
        [HttpPost]
        public IActionResult UpdatedData(List<EmployeeList> employeeList)
        {
            var data = UpdatedRawData(employeeList);
            if (data != null)
            {
                return Ok();
            }
            return BadRequest();
        }


        //  Delete Employees
        public DataTable DeleteEmployee(List<EmployeeList> employeeList)

        {

            var returnDataTable = new DataTable();
            var JSONData = JsonConvert.SerializeObject(employeeList);
            using (SqlConnection connection = new SqlConnection(connectionString))

            {
                connection.Open();

                SqlCommand cmd = new SqlCommand();

                cmd.Connection = connection;

                cmd.CommandType = CommandType.StoredProcedure;

                cmd.CommandText = "DeleteEmployeeData"; // Use the name of your stored procedure

                cmd.Parameters.Add(new SqlParameter("@EmployeeArray", JSONData));

                SqlDataAdapter dataAdp = new SqlDataAdapter(cmd);

                dataAdp.Fill(returnDataTable);

                connection.Close();

            }
            return returnDataTable;


        }
        [HttpPost]
        public IActionResult DeleteEmployeeData(List<EmployeeList> employeeList)
        {
            var res = DeleteEmployee(employeeList);
            if (res == null)
                return BadRequest(res);

            return Ok();
        }

        // add new employees

     
        public IActionResult EmployeeDetailView( int id)
        {
            if(id != -1)
            {
                var data = ConvertTableIntoModel(EmployeeData());
               var  filteredData = data.FirstOrDefault(row=>row.EmployeeId==id);
                return View(filteredData);

            }
           var emptyData = new EmployeeList();
            return View(emptyData);

        }

        public IActionResult AddToTable(EmployeeList empList)
        {
            if(empList.EmployeeId == 0)
            {
                AddNewEmployee(empList);
            }
            else
            {
                UpdateSingleEmployee(empList);
            }
            return RedirectToAction("EmployeeList");
        }
      

        public DataTable AddNewEmployee(EmployeeList employeeList)

        {

            var returnDataTable = new DataTable();
            var JSONData = JsonConvert.SerializeObject(employeeList);
            using (SqlConnection connection = new SqlConnection(connectionString))

            {
                connection.Open();

                SqlCommand cmd = new SqlCommand();

                cmd.Connection = connection;

                cmd.CommandType = CommandType.StoredProcedure;

                cmd.CommandText = "InsertNewEmployees"; // Use the name of your stored procedure

                cmd.Parameters.Add(new SqlParameter("@EmpJson", JSONData));

                SqlDataAdapter dataAdp = new SqlDataAdapter(cmd);

                dataAdp.Fill(returnDataTable);

                connection.Close();

            }
            return returnDataTable;


        }





        //   method to update employees existing record
           public DataTable EditEmployee(EmployeeList employeeList)

           {

               var returnDataTable = new DataTable();
               var JSONData = JsonConvert.SerializeObject(employeeList);
               using (SqlConnection connection = new SqlConnection(connectionString))

               {
                   connection.Open();

                   SqlCommand cmd = new SqlCommand();

                   cmd.Connection = connection;

                   cmd.CommandType = CommandType.StoredProcedure;

                   cmd.CommandText = "EditEmployeeData"; // Use the name of your stored procedure

                   cmd.Parameters.Add(new SqlParameter("@JsonEmp", JSONData));

                   SqlDataAdapter dataAdp = new SqlDataAdapter(cmd);

                   dataAdp.Fill(returnDataTable);

                   connection.Close();

               }
               return returnDataTable;


           }
        // EmployeeUpdatedData

        public DataTable UpdateSingleEmployee(EmployeeList emplist)
        {
            var returnDataTable = new DataTable();
            var JsonData = JsonConvert.SerializeObject(emplist);
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "EmployeeUpdatedData";
                cmd.Parameters.Add(new SqlParameter("@EmployeeJson", JsonData));
                SqlDataAdapter dataAdp = new SqlDataAdapter(cmd);
                dataAdp.Fill(returnDataTable);
                connection.Close();
            }
            return returnDataTable;
        }





           [HttpPost]
           public IActionResult AddEmployee(EmployeeList employeeList)
           {

               if (employeeList.EmployeeId == 0)
               {
                   AddNewEmployee(employeeList);
                }
               else
               {
                UpdateSingleEmployee(employeeList);
               }
               return RedirectToAction("EmployeeList");
           }

        [HttpPost]
        public IActionResult EmployeeList(EmployeeList emplist)
        {
            return View("EmployeeDetailView");
        }


        // update Employee information on Edit Employee Info


        public IActionResult EmployeeList()
        {

             return View();
        }


    }
}
