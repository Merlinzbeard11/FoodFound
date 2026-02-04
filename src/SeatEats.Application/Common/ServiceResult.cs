namespace SeatEats.Application.Common;

public class ServiceResult<T>
{
    public bool IsSuccess { get; private set; }
    public T? Value { get; private set; }
    public string? Error { get; private set; }
    public string? ErrorCode { get; private set; }

    private ServiceResult() { }

    public static ServiceResult<T> Success(T value) => new()
    {
        IsSuccess = true,
        Value = value
    };

    public static ServiceResult<T> Failure(string error, string? errorCode = null) => new()
    {
        IsSuccess = false,
        Error = error,
        ErrorCode = errorCode
    };
}
