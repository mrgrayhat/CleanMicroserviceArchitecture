using System.Collections.Generic;

namespace STS.Application.Features.Employees.Queries.GetEmployeesList
{
    public class EmployeesListVm
    {
        public IList<EmployeeLookupDto> Employees { get; set; }
    }
}
 