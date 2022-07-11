using System.Text.Json.Serialization;

namespace Guide_Project.SharedLibrary.DTOs;

public class Response<T> where T : class
{
    public T? Data { get; private set; }
    public int Status { get; private set; }
    [JsonIgnore]
    public bool IsSuccess { get; private set; }

    public ErrorDto? Error { get; private set; }

    public static Response<T> SuccessWithData(T data, int status)
    {
        return new Response<T> { Data = data, Status = status, IsSuccess = true };
    }
    public static Response<T> Success(int status)
    {
        return new Response<T> { Data = default, Status = status, IsSuccess = true };
    }
    public static Response<T> Fail(ErrorDto errorDto, int status)
    {
        return new Response<T> { Error = errorDto, Status = status, IsSuccess = false };
    }
    public static Response<T> FailWithMessage(string errorMessage, int status,bool isShowAble)
    {
        var errorDto = new ErrorDto(errorMessage, isShowAble);
        return new Response<T> { Error = errorDto, Status = status, IsSuccess = false };
    }
}