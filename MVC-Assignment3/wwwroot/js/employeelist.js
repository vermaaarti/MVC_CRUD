let globalArray = [];
$(document).ready(function () {

      
    $.ajax({
        type: 'GET',
        url: '/Employee/JSONEmployeeData',
        dataType: 'json',
        success: function (data) {
          
            intilizeDataTable(data);
            globalArray = data;
            console.log(globalArray);
        },
        error: function (errorThrown, textStatus, xhr) {
            console.log('Error in Operation', error);
        }
    });

   

    


})

function intilizeDataTable(data) {
   new DataTable( '#dataTable',{
        data: data,
        columns: [

            
            {
                "data": "",
                "render": function (data, type, row) {
                  return `<input  class="form-check-input" type="checkbox" value="" id="EmpId" onchange=changeisDeleted(event,${row.employeeId})>` ;
                },

            }, 

            {
               "data": "employeeName",
                "render": function (data, type, row) {
                    return '<a href="/Employee/EmployeeDetailView/?id=' + row.employeeId + '">' + data + '</a>';
                },
               
            },

          {
              data: 'departmentName', render: function (data, type, row) {
                  let color = 'black';
                  if (data == 'IT') {
                      color = 'yellow';
                  }
                  if (data == 'HR') {
                      color = 'red';
                  }
                  if (data == 'Finance') {
                      color = 'green';
                  }
                  return '<span style="color:' + color + '">' + data + '</span>';
              } },

             {
                "data": "amountPerYear",
                 "render": function (data, type,row) {
                     return `<input type="text" onchange=updateSalary(event,${row.employeeId}) 
                    class="form-control"  value=${data} id="amount" >`;
                }

            },
           

        ],
        lengthChange: false,
        searching: false,
        info: false,
        paging: false
   });

}

function updateSalary(event, id) {


    const newSalary = event.target.value;

    globalArray.map(employee => {
        if (employee.employeeId == id) {
            employee.amountPerYear = newSalary;
            employee.isModified = 1;
        }
    })
    console.log(globalArray);
   
}

function changeisDeleted(event, id) {
    if (event.target.checked) {
        globalArray.map(data => {
            if (data.employeeId == id) {
                data.isDeleted = 1;
            }
        })
    }
}

function SaveEmployee(event) {

    event.preventDefault();

    $.ajax({
        type: 'POST',
        url: '/Employee/UpdatedData',
        data: { employeeList: globalArray },
        success: function (data) {

            console.log(data);
        },
        error: function (errorThrown, textStatus, xhr) {
            console.log('Error in Operation');
        }
    });
}


 function DeleteEmployee(event) {

    event.preventDefault();
     const dleArray = globalArray.filter(emp => emp.isDeleted == 1);
     $.ajax({
         type: 'POST',
         url: '/Employee/DeleteEmployeeData',
         data: { employeeList: dleArray } ,
        success: function (data) {

            console.log(data);
        },
        error: function (errorThrown, textStatus, xhr) {
            console.log('Error in Operation');
        }
    });
}

function AddNewEmployee(event) {

    event.preventDefault();
    window.location.href = "/Employee/EmployeeDetailView/-1";

}







   











