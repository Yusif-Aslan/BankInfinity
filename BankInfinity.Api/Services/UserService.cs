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
        var existingUsers = await _userRepository.GetAllAsync();
        if (existingUsers.Any(u => u.Email.Equals(request.Email, StringComparison.OrdinalIgnoreCase)))
        {
            return Result<UserResponse>.Failure("A user with this email address already exists.");
        }

        var user = new User
        {
            FullName = request.FullName,
            Email = request.Email,
            PhoneNumber = request.PhoneNumber
        };

        await _userRepository.AddAsync(user);
        await _userRepository.SaveChangesAsync();

        var response = new UserResponse(user.Id, user.FullName, user.Email);
        
        return Result<UserResponse>.Success(response);
    }

    public async Task<Result<UserResponse>> GetUserAsync(int userId)
    {
        var user = (User)await _userRepository.GetByIdAsync(userId);
        
        if (user == null)
        {
            return Result<UserResponse>.Failure("User not found.");
        }

        var response = new UserResponse(user.Id, user.FullName, user.Email);
        
        return Result<UserResponse>.Success(response);
    }
}