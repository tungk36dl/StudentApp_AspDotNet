using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentMngt.Domain.Exceptions
{
    public static class MajorException
    {
        public class MajorNotFoundException : NotFoundException
        {
            public MajorNotFoundException(Guid MajorId) : base($"The Major with the id {MajorId} was not found.")
            {

            }
        }
        public class CreateMajorException : BadRequestException
        {
            public CreateMajorException(String MajorName) : base($"Somthing when wrong when create Major {MajorName}")
            {

            }
        }

        public class UpdateMajorException : BadRequestException
        {
            public UpdateMajorException(Guid MajorId) : base($"Somthing when wrong when update Major with id {MajorId}")
            {

            }
        }

        public class DeleteMajorException : BadRequestException
        {
            public DeleteMajorException(Guid MajorId) : base($"Something when wrong when delete Major with id {MajorId}")
            {
            }
        }
    }
}
