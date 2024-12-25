using Microsoft.AspNetCore.Http;

namespace StudentMngt.Domain.Exceptions;

public abstract class BadRequestException : BaseException
{
    protected BadRequestException(string message)
        : base("Bad Request", message)
    {
        StatusCode = StatusCodes.Status400BadRequest;
    }
}
