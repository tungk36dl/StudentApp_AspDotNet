namespace StudentMngt.Domain.Exceptions;

public static class ModelException
{
    public class ModelNotValidException : BadRequestException
    {
        public ModelNotValidException(string message)
            : base(message)
        {

        }
        
    }
    
}
