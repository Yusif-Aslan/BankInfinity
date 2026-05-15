using BankInfinity.Api.DTOs;
using FluentValidation;

namespace BankInfinity.Api.Validators;

public class TransactionRequestValidator : AbstractValidator<TransactionRequest>
{
    public TransactionRequestValidator()
    {
        RuleFor(transaction => transaction.Amount)
            .GreaterThan(0).WithMessage("Transaction amount must be strictly greater than zero.");
    }
}