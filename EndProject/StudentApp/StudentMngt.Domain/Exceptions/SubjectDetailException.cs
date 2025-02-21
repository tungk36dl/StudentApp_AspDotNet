using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentMngt.Domain.Exceptions
{
    public static class SubjectDetailException
    {
        public class SubjectDetailNotFoundException : NotFoundException
        {
            public SubjectDetailNotFoundException(Guid subjectDetailId) : base($"The subject with the id {subjectDetailId} was not found.")
            {

            }
        }
        public class CreatesubjectDetailException : BadRequestException
        {
            public CreatesubjectDetailException(String subjectDetailName) : base($"Somthing when wrong when create subjectDetail {subjectDetailName}")
            {

            }
        }

        public class UpdatesubjectDetailException : BadRequestException
        {
            public UpdatesubjectDetailException(Guid subjectDetailId) : base($"Somthing when wrong when update subjectDetail with id {subjectDetailId}")
            {

            }
        }

        public class DeletesubjectDetailException : BadRequestException
        {
            public DeletesubjectDetailException(Guid subjectDetailId) : base($"Something when wrong when delete subjectDetail with id {subjectDetailId}")
            {
            }
        }
        public class HandleSubjectDetailException : BadRequestException
        {
            public HandleSubjectDetailException(string message)
                : base(message) { }
        }
    }
}
