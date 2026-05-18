using BankInfinity.Api.DTOs;
using BankInfinity.Api.Models;

namespace BankInfinity.Api.Interfaces;

public interface IAuthService
{
    Task<Result<string>> LoginAsync(LoginRequest request);
}