using BankInfinity.Api.DTOs;
using BankInfinity.Api.Models;

namespace BankInfinity.Api.Interfaces;

public interface IUserService
{
    Task<Result<UserResponse>> CreateUserAsync(CreateUserRequest request);
    Task<Result<UserResponse>> GetUserAsync(int userId);
    Task<Result<bool>> VerifyEmailAsync(VerifyEmailRequest request);
}