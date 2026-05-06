using BankInfinity.Api.DTOs;
using BankInfinity.Api.Interfaces;
using BankInfinity.Api.Models;

namespace BankInfinity.Api.Services;

public class UserService : IUserService
{
    private readonly IRepository<User> _userRepository;

    public UserService(IRepository<User> userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Result<UserResponse>> CreateUserAsync(CreateUserRequest request)
    {
        var user = new User
        {
            FullName = request.FullName,
            Email = request.Email,
            PhoneNumber = request.PhoneNumber
        };

        await _userRepository.AddAsync(user);
        await _userRepository.SaveChangesAsync();

        return Result<UserResponse>.Success(new UserResponse(user.Id, user.FullName, user.Email));
    }

    public async Task<Result<UserResponse>> GetUserAsync(int userId)
    {
        var user = await _userRepository.GetByIdAsync(userId);
            
        if (user == null)
        {
            return Result<UserResponse>.Failure("User not found.");
        }

        return Result<UserResponse>.Success(new UserResponse(user.Id, user.FullName, user.Email));
    }
}