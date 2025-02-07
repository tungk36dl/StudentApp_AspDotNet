using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentMngt.Domain.Exceptions
{
    public static class ClassesException
    {
        public class ClassesNotFoundException : NotFoundException
        {
            public ClassesNotFoundException(Guid ClassesId) : base($"The Classes with the id {ClassesId} was not found.")
            {

            }
        }
        public class CreateClassesException : BadRequestException
        {
            public CreateClassesException(String ClassesName) : base($"Somthing when wrong when create Classes {ClassesName}")
            {

            }
        }

        public class UpdateClassesException : BadRequestException
        {
            public UpdateClassesException(Guid ClassesId) : base($"Somthing when wrong when update Classes with id {ClassesId}")
            {

            }
        }

        public class DeleteClassesException : BadRequestException
        {
            public DeleteClassesException(Guid ClassesId) : base($"Something when wrong when delete Classes with id {ClassesId}")
            {
            }
        }
    }
}
