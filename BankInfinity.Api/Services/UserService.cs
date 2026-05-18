using BankInfinity.Api.DTOs;
using BankInfinity.Api.Interfaces;
using BankInfinity.Api.Models;

namespace BankInfinity.Api.Services;

public class UserService : IUserService
{
    private readonly IRepository<User> _userRepository;
    private readonly IEmailService _emailService;

    public UserService(IRepository<User> userRepository, IEmailService emailService)
    {
        _userRepository = userRepository;
        _emailService = emailService;
    }
public async Task<Result<bool>> VerifyEmailAsync(VerifyEmailRequest request)
{
    var users = await _userRepository.GetAllAsync();
    var user = users.FirstOrDefault(u => u.Email.Equals(request.Email, StringComparison.OrdinalIgnoreCase));

    if (user == null)
    {
        return Result<bool>.Failure("User not found.");
    }

    if (user.Status != UserStatus.PendingEmailVerification)
    {
        return Result<bool>.Failure("Email is already verified or account is in an invalid state.");
    }

    if (user.EmailVerificationCode != request.Code)
    {
        return Result<bool>.Failure("Invalid verification code.");
    }

    if (user.VerificationCodeExpiry < DateTime.UtcNow)
    {
        return Result<bool>.Failure("Verification code has expired. Please request a new one.");
    }
    
    user.Status = UserStatus.PendingKYC; 
    user.EmailVerificationCode = null;
    user.VerificationCodeExpiry = null;

    await _userRepository.UpdateAsync(user);
    await _userRepository.SaveChangesAsync();

    return Result<bool>.Success(true);
}
    public async Task<Result<UserResponse>> CreateUserAsync(CreateUserRequest request)
    {
        var existingUsers = await _userRepository.GetAllAsync();
        if (existingUsers.Any(u => u.Email.Equals(request.Email, StringComparison.OrdinalIgnoreCase)))
        {
            return Result<UserResponse>.Failure("A user with this email address already exists.");
        }
        
        var verificationCode = new Random().Next(100000, 999999).ToString();

        var user = new User
        {
            FullName = request.FullName,
            Email = request.Email,
            PhoneNumber = request.PhoneNumber,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            Status = UserStatus.PendingEmailVerification,
            EmailVerificationCode = verificationCode,
            VerificationCodeExpiry = DateTime.UtcNow.AddMinutes(15) 
        };

        await _userRepository.AddAsync(user);
        await _userRepository.SaveChangesAsync();
        
        var emailBody = $@"
            <h2>Welcome to BankInfinity!</h2>
            <p>Your email verification code is: <strong>{verificationCode}</strong></p>
            <p>This code will expire in 15 minutes.</p>";
            
        await _emailService.SendEmailAsync(user.Email, "Verify your BankInfinity Account", emailBody);

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