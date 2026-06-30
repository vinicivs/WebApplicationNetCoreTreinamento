using WebApplicationMinimalAPINetCoreTreinamento.Application.DTOs;
using FluentValidation;

namespace WebApplicationMinimalAPINetCoreTreinamento.Application.Validators
{
    public class CreateCepDtoValidator : AbstractValidator<CreateCepDto>
    {
        public CreateCepDtoValidator()
        {
            RuleFor(x => x.Codigo)
                .NotEmpty().WithMessage("O código do CEP é obrigatório")
                .Length(8).WithMessage("O CEP deve ter 8 dígitos")
                .Matches(@"^\d{8}$").WithMessage("O CEP deve conter apenas números");

            RuleFor(x => x.Logradouro)
                .NotEmpty().WithMessage("O logradouro é obrigatório")
                .MaximumLength(200).WithMessage("O logradouro deve ter no máximo 200 caracteres");

            RuleFor(x => x.Bairro)
                .NotEmpty().WithMessage("O bairro é obrigatório")
                .MaximumLength(100).WithMessage("O bairro deve ter no máximo 100 caracteres");

            RuleFor(x => x.Cidade)
                .NotEmpty().WithMessage("A cidade é obrigatória")
                .MaximumLength(100).WithMessage("A cidade deve ter no máximo 100 caracteres");

            RuleFor(x => x.Uf)
                .NotEmpty().WithMessage("O estado é obrigatório")
                .Length(2).WithMessage("O estado deve ter 2 caracteres")
                .Must(BeAValidState).WithMessage("Estado inválido");

            //RuleFor(x => x.Pais)
            //    .NotEmpty().WithMessage("O país é obrigatório")
            //    .MaximumLength(50).WithMessage("O país deve ter no máximo 50 caracteres");
        }

        private bool BeAValidState(string estado)
        {
            var validStates = new[] { "AC", "AL", "AP", "AM", "BA", "CE", "DF", "ES", "GO", "MA",
                                  "MT", "MS", "MG", "PA", "PB", "PR", "PE", "PI", "RJ", "RN",
                                  "RS", "RO", "RR", "SC", "SP", "SE", "TO" };
            return validStates.Contains(estado.ToUpper());
        }
    }
    public class UpdateCepDtoValidator : AbstractValidator<UpdateCepDto>
    {
        public UpdateCepDtoValidator()
        {
            RuleFor(x => x.Logradouro)
                .NotEmpty().WithMessage("O logradouro é obrigatório")
                .MaximumLength(200).WithMessage("O logradouro deve ter no máximo 200 caracteres");

            RuleFor(x => x.Bairro)
                .NotEmpty().WithMessage("O bairro é obrigatório")
                .MaximumLength(100).WithMessage("O bairro deve ter no máximo 100 caracteres");

            RuleFor(x => x.Cidade)
                .NotEmpty().WithMessage("A cidade é obrigatória")
                .MaximumLength(100).WithMessage("A cidade deve ter no máximo 100 caracteres");

            RuleFor(x => x.Uf)
                .NotEmpty().WithMessage("O estado é obrigatório")
                .Length(2).WithMessage("O estado deve ter 2 caracteres")
                .Must(BeAValidState).WithMessage("Estado inválido");

            //RuleFor(x => x.Pais)
            //    .NotEmpty().WithMessage("O país é obrigatório")
            //    .MaximumLength(50).WithMessage("O país deve ter no máximo 50 caracteres");
        }

        private bool BeAValidState(string estado)
        {
            var validStates = new[] { "AC", "AL", "AP", "AM", "BA", "CE", "DF", "ES", "GO", "MA",
                                  "MT", "MS", "MG", "PA", "PB", "PR", "PE", "PI", "RJ", "RN",
                                  "RS", "RO", "RR", "SC", "SP", "SE", "TO" };
            return validStates.Contains(estado.ToUpper());
        }
    }
}
