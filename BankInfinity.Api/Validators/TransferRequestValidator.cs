using BankInfinity.Api.DTOs;
using FluentValidation;

namespace BankInfinity.Api.Validators;

public class TransferRequestValidator : AbstractValidator<TransferRequest>
{
    public TransferRequestValidator()
    {
        RuleFor(x => x.SenderAccountId)
            .GreaterThan(0).WithMessage("Sender Account ID is invalid.");

        RuleFor(x => x.ReceiverAccountId)
            .GreaterThan(0).WithMessage("Receiver Account ID is invalid.");

        RuleFor(x => x.SenderAccountId)
            .NotEqual(x => x.ReceiverAccountId).WithMessage("Sender and receiver accounts cannot be the same.");

        RuleFor(x => x.Amount)
            .GreaterThan(0).WithMessage("Transfer amount must be strictly greater than zero.");
    }
}