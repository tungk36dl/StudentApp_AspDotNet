using Microsoft.AspNetCore.Http;

namespace StudentMngt.Domain.Exceptions;

public abstract class NotFoundException : BaseException
{
    protected NotFoundException(string message)
        : base("Not Found", message)
    {
        StatusCode = StatusCodes.Status404NotFound;
    }
}
