using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentMngt.Domain.Exceptions
{
    public static class ScoreException
    {
        public class ScoreNotFoundException : NotFoundException
        {
            public ScoreNotFoundException(Guid scoreId) : base($"The score with the id {scoreId} was not found.")
            {

            }
        }
        public class CreateScoreException : BadRequestException
        {
            public CreateScoreException(Guid userId) : base($"Somthing when wrong when create score, userId: {userId}")
            {

            }
        }

        public class UpdateScoreException : BadRequestException
        {
            public UpdateScoreException(Guid scoreId) : base($"Somthing when wrong when update score with id {scoreId}")
            {

            }
        }

        public class DeleteScoreException : BadRequestException
        {
            public DeleteScoreException(Guid scoreId) : base($"Something when wrong when delete score with id {scoreId}")
            {
            }
        }

        public class CreateListScoreException : BadRequestException
        {
            public CreateListScoreException() : base($"Something when wrong when create list score")
            {
            }
        }

        public class HandleScoreException : BadRequestException
        {
            public HandleScoreException(string message)
                : base(message) { }
        }
    }
}
