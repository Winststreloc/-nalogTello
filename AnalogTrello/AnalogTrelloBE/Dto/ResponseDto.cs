namespace AnalogTrelloBE.Dto;

public class ResponseDto<T>
{
    public ResponseDto(T result)
    {
        Result = result;
        IsSuccess = true;
    }

    public ResponseDto(string errorMessage)
    {
        ErrorMessages = errorMessage;
        IsSuccess = false;
    }
    public bool IsSuccess { get; set; }
    public T Result { get; set; }
    public string ErrorMessages { get; set; }

    public static ResponseDto<T> Success(T result) => new(result);
    public static ResponseDto<T> Failed(string errorMessage) => new(errorMessage);
}