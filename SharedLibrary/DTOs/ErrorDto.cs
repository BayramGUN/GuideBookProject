namespace Guide_Project.SharedLibrary.DTOs;

public class ErrorDto
{
    public List<String> Errors { get; private set; } = new List<string>();
    public bool IsShowAble { get; private set; }
    public ErrorDto(string error, bool isShowAble)
    {
        Errors.Add(error);
        IsShowAble = isShowAble;
    }
    public ErrorDto(List<string> errors, bool isShowAble)
    {
        Errors = errors;
        IsShowAble = isShowAble;
    }
}
