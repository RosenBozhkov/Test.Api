using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Business.Interfaces.v1;
using Business.Models.v1;
using Common.Exceptions;
using Common.Resources;
using inacs.v8.nuget.DevAttributes;
using inacs.v8.nuget.Core.Models;
using Microsoft.Extensions.Logging;
using Persistence.Entities.v1;
using Persistence.Interfaces.v1;
using System.Security.Cryptography;
using System.Linq;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;

namespace Business.Implementations.v1;

/// <summary>
/// User service
/// </summary>
public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IValidatorService _validatorService;
    private readonly IMapper _mapper;
    private readonly RequestState _requestState;
    private readonly ILogger<UserService> _logger;
    private readonly IConfiguration _configuration;

    /// <summary>
    /// Constructor with DI
    /// </summary>
    /// <param name="userRepository"></param> 
    /// <param name="validatorService"></param> 
    /// <param name="mapper"></param>
    /// <param name="requestState"></param>
    /// <param name="logger"></param>
    /// <param name="configuration"></param>
    public UserService(IUserRepository userRepository, IValidatorService validatorService, IMapper mapper,
        RequestState requestState, ILogger<UserService> logger, IConfiguration configuration)
    {
        _userRepository = userRepository;
        _validatorService = validatorService;
        _mapper = mapper;
        _requestState = requestState;
        _logger = logger;
        _configuration = configuration;
    }

    /// <summary>
    /// Get user by id
    /// </summary>
    /// <param name="id"></param>
    /// <exception cref="NotFoundException"></exception>
    public async Task<UserResponse> GetByIdAsync(Guid id)
    {
        User user = await _userRepository.GetByIdAsync(id)
                  ?? throw new NotFoundException(Messages.ResourceNotFound);

        var result = _mapper.Map<UserResponse>(user);
        return result;
    }

    /// <summary>
    /// Registers a user
    /// </summary>
    /// <param name="model"></param>
    public async Task<UserResponse> RegisterAsync(UserRequest model)
    {
        await _userRepository.ValidateUsernameNotExist(model.Username);

        CreatePasswordHash(model.Password, out byte[] passwordHash, out byte[] passwordSalt);

        User userToRegister = new() { Username = model.Username, PasswordHash = passwordHash, PasswordSalt = passwordSalt };
        _userRepository.Add(userToRegister);
        await _userRepository.SaveChangesAsync();

        return _mapper.Map<UserResponse>(userToRegister);
    }

    /// <summary>
    /// Logs the user in
    /// </summary>
    /// <param name="model"></param>
    public async Task<string> LoginAsync(UserRequest model)
    {
        User user = await GetValidUserAsync(model.Username);

        if (!VerifyPasswordHash(model.Password, user.PasswordHash, user.PasswordSalt))
        {
            return ("Wrong password.");
        }

        string token = CreateToken(user);

        return token;
    }

    private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        using (var hmac = new HMACSHA512())
        {
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        }
    }

    private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
    {
        using (var hmac = new HMACSHA512(passwordSalt))
        {
            var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            return computedHash.SequenceEqual(passwordHash);
        }
    }

    private async Task<User> GetValidUserAsync(string name)
    {
        User user = await _userRepository.GetByNameAsync(name) ?? throw new NotFoundException("User does not exist");

        return user;
    }

    private string CreateToken(User user)
    {
        List<Claim> claims = new()
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, "Admin")
            };

        SymmetricSecurityKey key = new(System.Text.Encoding.UTF8.GetBytes(
            _configuration.GetSection("JwtParameters:SecretKey").Value));

        SigningCredentials creds = new(key, SecurityAlgorithms.HmacSha512Signature);

        JwtSecurityToken token = new(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds);

        string jwt = new JwtSecurityTokenHandler().WriteToken(token);

        return jwt;
    }
}
