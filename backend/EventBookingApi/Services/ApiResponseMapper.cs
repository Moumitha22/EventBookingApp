using EventBookingApi.Interfaces;
using EventBookingApi.Models.DTOs;

namespace EventBookingApi.Services
{
    public class ApiResponseMapper : IApiResponseMapper
    {
        public ApiResponse<T> MapToOkResponse<T>(string message, T data)
        {
            return new ApiResponse<T>
            {
                Success = true,
                StatusCode = 200,
                Message = message,
                Data = data
            };
        }

        public ApiResponse<object> MapToOkResponse(string message)
        {
            return new ApiResponse<object>
            {
                Success = true,
                StatusCode = 200,
                Message = message,
                Data = null
            };
        }

        public ApiResponse<T> MapToErrorResponse<T>(int statusCode, string message, Dictionary<string, string[]>? errors = null)
        {
            return new ApiResponse<T>
            {
                Success = false,
                StatusCode = statusCode,
                Message = message,
                Errors = errors
            };
        }
    }
}
