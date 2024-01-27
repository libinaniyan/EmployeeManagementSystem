using Dapper;
using Employee.Context;
using Employee.Models;
using System.Net;

namespace Employee.Repository
{
    public class EmployeeRepository: IEmployeeRepository
    {

        private readonly DapperContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public EmployeeRepository(DapperContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;

        }

        public async Task<IEnumerable<EmployeeDto>> GetEmployees()
        {
            var query = "SELECT * FROM Employees";

            using (var connection = _context.CreateConnection())
            {
                var employees = await connection.QueryAsync<EmployeeDto>(query);
                return employees.ToList();
            }
        }

        public async Task<EmployeeDto> GetEmployee(int id)

        {
            var query = "SELECT * FROM Employees WHERE EmpId = @id";

            using (var connection = _context.CreateConnection())
            {
                var emp = await connection.QuerySingleOrDefaultAsync<EmployeeDto>(query, new { id=id });

                return emp;
            }
        }
        public async Task<HttpStatusCode>AddEmployee(EmployeeDto employeeDto)
        {
            var query = @"INSERT INTO Employees (FirstName, LastName, PhoneNo, Email, Age, Position, Department, Qualification, Gender, Salary) 
                VALUES (@FirstName, @LastName, @PhoneNo, @Email, @Age, @Position, @Department, @Qualification, @Gender, @Salary)";

            using (var connection = _context.CreateConnection())
            {
                try
                {
                    await connection.ExecuteAsync(query, employeeDto);
                    return HttpStatusCode.OK;
                }
                catch
                {
                    return HttpStatusCode.BadRequest;
                }               
            }
        }
        public async Task<EmployeeDto>UpdateEmployee(int empId, EmployeeDto updatedEmployeeDto)
        {
            var query = @"UPDATE Employees
                  SET FirstName = @FirstName, 
                      LastName = @LastName, 
                      PhoneNo = @PhoneNo, 
                      Email = @Email, 
                      Age = @Age, 
                      Position = @Position, 
                      Department = @Department, 
                      Qualification = @Qualification, 
                      Gender = @Gender, 
                      Salary = @Salary
                  WHERE EmpId = @EmpId;";

            using (var connection = _context.CreateConnection())
            {
                    updatedEmployeeDto.EmpId = empId;
                    var updatedEmployee = await connection.QuerySingleOrDefaultAsync<EmployeeDto>(query, updatedEmployeeDto);                    
                    return (updatedEmployee);
            }
        }
        public async Task<HttpStatusCode> DeleteEmployee(int empId)
        {
            var query = @"DELETE FROM Employees WHERE EmpId = @EmpId;";
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    await connection.ExecuteAsync(query);
                    return HttpStatusCode.OK;
                }
            }
            catch
            {
                return HttpStatusCode.BadRequest;
            }
        }
    }
}
