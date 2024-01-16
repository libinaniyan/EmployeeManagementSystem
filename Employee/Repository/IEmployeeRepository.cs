using Employee.Dto;
using Employee.Models;
using System.Net;

namespace Employee.Repository
{
    public interface IEmployeeRepository
    {
        public Task<IEnumerable<EmployeeDto>> GetEmployees();
        public Task<EmployeeDto> GetEmployee(int id);
        public Task<HttpStatusCode> AddEmployee(EmployeeDto employee);
        public Task<EmployeeDto> UpdateEmployee(int empId, EmployeeDto updatedEmployeeDto);
        public Task<HttpStatusCode> DeleteEmployee(int empId);
    }
}
