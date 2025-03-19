using CA_2025.Domain.Abstraction;
using CA_2025.Domain.Employees;
using MediatR;

namespace CA_2025.Application.Employees;

public sealed record EmployeeGetAllQuery() : IRequest<IQueryable<EmployeeGetAllQueryResponse>>;

public sealed class EmployeeGetAllQueryResponse : EntityDto
{
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public DateOnly BirthOfDate { get; set; }
    public decimal Salary { get; set; }
    public string TCNo { get; set; } = default!;
}

internal sealed class EmployeeGetAllQueryHandler(IEmployeeRepository employeeRepository) : IRequestHandler<EmployeeGetAllQuery, IQueryable<EmployeeGetAllQueryResponse>>
{
    public Task<IQueryable<EmployeeGetAllQueryResponse>> Handle(EmployeeGetAllQuery request, CancellationToken cancellationToken)
    {
        var response = employeeRepository.GetAll().Select(s => new EmployeeGetAllQueryResponse
        {
            Id = s.Id,
            BirthOfDate = s.BirthOfDate,
            CreateAt = s.CreateAt,
            DeleteAt = s.DeleteAt,
            FirstName = s.FirstName,
            IsDeleted = s.IsDeleted,
            LastName = s.LastName,
            Salary = s.Salary,
            TCNo = s.PersonelInformation.TCNo,
            UpdateAt = s.UpdateAt
        }).AsQueryable();
        return Task.FromResult(response);
    }
}