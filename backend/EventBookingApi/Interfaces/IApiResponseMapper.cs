using EventBookingApi.Models.DTOs;

namespace EventBookingApi.Interfaces
{
    public interface IApiResponseMapper
    {
        ApiResponse<T> MapToOkResponse<T>(string message, T data);
        ApiResponse<object> MapToOkResponse(string message);
        ApiResponse<T> MapToErrorResponse<T>(int statusCode, string message, Dictionary<string, string[]>? errors = null);
    }
}
