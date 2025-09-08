namespace TaskManager.Api.Models
{
    public sealed class ApiResponse<T>
    {
        public int StatusCode { get; init; }
        public string Message { get; init; } = string.Empty;
        public T? Data { get; init; }
        public string TraceId { get; init; } = string.Empty;
        public DateTime Timestamp { get; init; }

        public static ApiResponse<T> Success(T data, string message = "Success", int statusCode = 200, string? traceId = null)
            => new()
            {
                StatusCode = statusCode,
                Message = message,
                Data = data,
                TraceId = traceId ?? Guid.NewGuid().ToString(),
                Timestamp = DateTime.UtcNow
            };

        public static ApiResponse<T> Fail(string message, int statusCode, string? traceId = null)
            => new()
            {
                StatusCode = statusCode,
                Message = message,
                Data = default,
                TraceId = traceId ?? Guid.NewGuid().ToString(),
                Timestamp = DateTime.UtcNow
            };
    }
}
