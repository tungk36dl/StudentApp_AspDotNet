using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentMngt.Domain.Exceptions
{
    public static class SubjectException
    {
        public class SubjectNotFoundException : NotFoundException
        {
            public SubjectNotFoundException(Guid subjectId) : base($"The subject with the id {subjectId} was not found.")
            {

            }
        }
        public class CreateSubjectException : BadRequestException
        {
            public CreateSubjectException(String subjectName) : base($"Somthing when wrong when create subject {subjectName}")
            {

            }
        }

        public class UpdateSubjectException : BadRequestException
        {
            public UpdateSubjectException(Guid subjectId) : base($"Somthing when wrong when update subject with id {subjectId}")
            {

            }
        }

        public class DeleteSubjectException : BadRequestException
        {
            public DeleteSubjectException(Guid subjectId) : base($"Something when wrong when delete subject with id {subjectId}")
            {
            }
        }
        public class HandleSubjectException : BadRequestException
        {
            public HandleSubjectException(string message)
                : base(message) { }
        }
    }
}
