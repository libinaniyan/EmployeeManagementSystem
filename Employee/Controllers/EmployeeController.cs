using Employee.Models;
using Employee.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Employee.Controllers
{
    [Route("api/employees")]
    [ApiController]
    public class EmployeeController:ControllerBase
    {
        private readonly IEmployeeRepository _empRepo;
        private readonly ILogger<EmployeeController> _logger;

        public EmployeeController(IEmployeeRepository empRepo, ILogger<EmployeeController> logger)
        {
            _empRepo = empRepo;
            _logger = logger;

        }

        [HttpGet("GetEmployees", Name = "GetEmployees")]
        public async Task<IActionResult> GetEmployees()
        {
            try
            {
                var employees = await _empRepo.GetEmployees();
                return Ok(employees);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        
        [HttpGet("GetEmployee/{id}", Name = "EmployeeById")]
        public async Task<IActionResult> GetEmployee(int id)
        {
            try
            {
                var emp = await _empRepo.GetEmployee(id);
                if (emp == null)
                    return NotFound();

                return Ok(emp);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error! Employee Not Found.");
                return StatusCode(500, ex.Message);
            }
        }
        [HttpPost]
        public async Task<IActionResult> AddEmployee(EmployeeDto employeeDto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    EmployeeDto newEmployee = new EmployeeDto
                    {
                        FirstName = employeeDto.FirstName,
                        LastName = employeeDto.LastName,
                        PhoneNo = employeeDto.PhoneNo,
                        Email = employeeDto.Email,
                        Age = employeeDto.Age,
                        Position = employeeDto.Position,
                        Department = employeeDto.Department,
                        Qualification = employeeDto.Qualification,
                        Gender = employeeDto.Gender,
                        Salary = employeeDto.Salary
                    };

                    await _empRepo.AddEmployee(newEmployee);
                    return Ok("Employee added successfully");
                   
                }
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error! Failed to add employee.");
                return BadRequest($"Error adding employee: {ex.Message}");
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateEmployee(int empId , EmployeeDto updatedEmployeeDto)
        {
            try
            {   
                // Reusing the same for check the employee with given id is null or not.
                var emp = await _empRepo.GetEmployee(empId);
                if (emp != null)
                {
                    await _empRepo.UpdateEmployee(empId, updatedEmployeeDto);
                    var employee = await _empRepo.GetEmployee(emp.EmpId); // to display the updated values in response
                    return Ok(employee);
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error! Failed to update employee.");
                return BadRequest($"Error updating employee: {ex.Message}");
            }
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteEmployee(int empId)
        {
            try
            {   
                // Reusing the same for check the employee with given id is null or not.
                var emp = await _empRepo.GetEmployee(empId);  
                if (emp != null)
                {
                    await _empRepo.DeleteEmployee(empId);
                    return Ok("Employee deleted successfully");
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error! Failed to delete employee.");
                return BadRequest($"Error deleting employee: {ex.Message}");
            }
        }

    }
}
