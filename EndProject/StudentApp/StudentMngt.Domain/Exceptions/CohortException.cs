using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentMngt.Domain.Exceptions
{
    public static class CohortException
    {
        public class CohortNotFoundException : NotFoundException
        {
            public CohortNotFoundException(Guid CohortId) : base($"The Cohort with the id {CohortId} was not found.")
            {

            }
        }
        public class CreateCohortException : BadRequestException
        {
            public CreateCohortException(String CohortName) : base($"Somthing when wrong when create Cohort {CohortName}")
            {

            }
        }

        public class UpdateCohortException : BadRequestException
        {
            public UpdateCohortException(Guid CohortId) : base($"Somthing when wrong when update Cohort with id {CohortId}")
            {

            }
        }

        public class DeleteCohortException : BadRequestException
        {
            public DeleteCohortException(Guid CohortId) : base($"Something when wrong when delete Cohort with id {CohortId}")
            {
            }
        }

        public class HandleCohortException : BadRequestException
        {
            public HandleCohortException(string message)
                : base(message) { }
        }
    }
}
