using CA_2025.Domain.Employees;
using FluentValidation;
using GenericRepository;
using Mapster;
using MediatR;
using TS.Result;

namespace CA_2025.Application.Employees;

public sealed record EmployeeCreateCommand(
    string FirstName,
    string LastName,
    DateOnly BirthOfDate,
    decimal Salary,
    PersonelInformation PersonelInformation,
    Address? Address) : IRequest<Result<string>>;


public sealed class EmployeeCreateCommandValidator : AbstractValidator<EmployeeCreateCommand>
{
    public EmployeeCreateCommandValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty().WithMessage("Ad alanı boş geçilemez").MinimumLength(3).WithMessage("Ad alanı minimum 3 karakter olmalıdır.");
        RuleFor(x => x.LastName).NotEmpty().WithMessage("Soyad alanı boş geçilemez").MinimumLength(3).WithMessage("Soyad alanı minimum 3 karakter olmalıdır."); ;
        RuleFor(x => x.PersonelInformation.TCNo).NotEmpty().WithMessage("TC Kimlik Numarası alanı boş geçilemez").MinimumLength(11).MaximumLength(11).WithMessage("TC Kimlik numarası 11 karakter olmalıdır");

        RuleFor(x => x.BirthOfDate).NotEmpty().WithMessage("Doğum tarihi alanı boş geçilemez");
        RuleFor(x => x.Salary).NotEmpty().WithMessage("Maaş alanı boş geçilemez");
        RuleFor(x => x.PersonelInformation).NotNull().WithMessage("Personel bilgileri alanı boş geçilemez");
       
    }
}

internal sealed class EmployeeCreateCommandHandler(IEmployeeRepository employeeRepository, IUnitOfWork unitOfWork) : IRequestHandler<EmployeeCreateCommand, Result<string>>
{
    public async Task<Result<string>> Handle(EmployeeCreateCommand request, CancellationToken cancellationToken)
    {
        var isEmployeeExist = await employeeRepository.AnyAsync(x => x.PersonelInformation.TCNo == request.PersonelInformation.TCNo, cancellationToken);

        if (isEmployeeExist)
        {
            return Result<string>.Failure("Bu TC Kimlik Numarası ile kayıtlı bir personel bulunmaktadır");
        }

        Employee employee = request.Adapt<Employee>();

        employeeRepository.Add(employee);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return "Personel kaydı başarıyla tamamlandı";
    }
}
