using CA_2025.Domain.Employees;
using CA_2025.Infrastructure.Context;
using GenericRepository;

namespace CA_2025.Infrastructure.Repositories;

internal sealed class EmployeeRepository(ApplicationDbContext context) : Repository<Employee, ApplicationDbContext>(context), IEmployeeRepository
{
}
