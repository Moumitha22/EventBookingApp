using AutoMapper;
using EventBookingApi.Exceptions;
using EventBookingApi.Interfaces;
using EventBookingApi.Models;
using EventBookingApi.Models.DTOs;

namespace EventBookingApi.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUserService _userService;
        private readonly IEncryptionService _encryptionService;
        private readonly ITokenService _tokenService;
        private readonly IRefreshTokenService _refreshTokenService;
        private readonly IMapper _mapper;

        public AuthenticationService(
            IUserService userService,
            IEncryptionService encryptionService,
            ITokenService tokenService,
            IRefreshTokenService refreshTokenService,
            IMapper mapper)
        {
            _userService = userService;
            _encryptionService = encryptionService;
            _tokenService = tokenService;
            _refreshTokenService = refreshTokenService;
            _mapper = mapper;
        }

        public async Task<string> RegisterAsync(UserRegisterRequestDto registerRequest)
        {
            var existingUser = await _userService.GetUserByEmailAsync(registerRequest.Email);
            if (existingUser != null)
            {
                throw new BadRequestException("User with this email already exists.");
            }

            var hashedPassword = _encryptionService.HashPassword(registerRequest.Password);

            var newUser = _mapper.Map<User>(registerRequest);
            newUser.Password = hashedPassword;

            await _userService.CreateUserAsync(newUser);

            return "User registered successfully";
        }

        public async Task<UserLoginResponseDto> LoginAsync(UserLoginRequestDto loginRequest)
        {
            var user = await _userService.GetUserByEmailAsync(loginRequest.Email.Trim());
            if (user == null || !_encryptionService.VerifyPassword(loginRequest.Password, user.Password))
            {
                throw new UnauthorizedException("Invalid email or password.");
            }

            var accessToken = await _tokenService.GenerateAccessTokenAsync(user);
            var refreshToken = await _refreshTokenService.GenerateRefreshTokenAsync(user.Id);

            var userDto = _mapper.Map<UserLoginResponseDto>(user);
            userDto.AccessToken = accessToken;
            userDto.RefreshToken = refreshToken;

            return userDto;
        }

        public async Task LogoutAsync(string refreshToken)
        {
            await _refreshTokenService.RevokeRefreshTokenAsync(refreshToken);
        }

        public async Task<UserLoginResponseDto> RefreshTokenAsync(string refreshToken)
        {
            var isValid = await _refreshTokenService.IsRefreshTokenValidAsync(refreshToken);
            if (!isValid)
            {
                throw new BadRequestException("Invalid refresh token.");
            }

            var user = await _refreshTokenService.GetUserByRefreshTokenAsync(refreshToken);
            if (user == null)
            {
                throw new NotFoundException("User not found for this refresh token.");
            }

            var accessToken = await _tokenService.GenerateAccessTokenAsync(user);
            var newRefreshToken = await _refreshTokenService.GenerateRefreshTokenAsync(user.Id);

            await _refreshTokenService.RevokeAndReplaceAsync(refreshToken, newRefreshToken);

            var userDto = _mapper.Map<UserLoginResponseDto>(user);
            userDto.AccessToken = accessToken;
            userDto.RefreshToken = newRefreshToken;

            return userDto;
        }
    }
}
