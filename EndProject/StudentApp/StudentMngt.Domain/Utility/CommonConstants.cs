using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentMngt.Domain.Utility
{
    public static class CommonConstants
    {
        public class Header
        {
            public const string CurrentUser = "CurrentUser";
        }
        public class Permissions
        {
            public const string USER_PERMISSION = "USER_PERMISSION";
            public const string ADD_USER_PERMISSION = "ADD_USER_PERMISSION";
            public const string UPDATE_USER_PERMISSION = "UPDATE_USER_PERMISSION";
            public const string DELETE_USER_PERMISSION = "DELETE_USER_PERMISSION";
            public const string VIEW_USER_PERMISSION = "VIEW_USER_PERMISSION";

            public const string ROLE_PERMISSION = "ROLE_PERMISSION";
            public const string ADD_ROLE_PERMISSION = "ADD_ROLE_PERMISSION";
            public const string UPDATE_ROLE_PERMISSION = "UPDATE_ROLE_PERMISSION";
            public const string DELETE_ROLE_PERMISSION = "DELETE_ROLE_PERMISSION";
            public const string VIEW_ROLE_PERMISSION = "VIEW_ROLE_PERMISSION";

            public const string SUBJECT_PERMISSION = "SUBJECT_PERMISSION";
            public const string ADD_SUBJECT_PERMISSION = "ADD_SUBJECT_PERMISSION";
            public const string UPDATE_SUBJECT_PERMISSION = "UPDATE_SUBJECT_PERMISSION";
            public const string DELETE_SUBJECT_PERMISSION = "DELETE_SUBJECT_PERMISSION";
            public const string VIEW_SUBJECT_PERMISSION = "VIEW_SUBJECT_PERMISSION";
            

            public const string IMAGE_PERMISSION = "IMAGE_PERMISSION";
            public const string ADD_IMAGE_PERMISSION = "ADD_IMAGE_PERMISSION";
            public const string UPDATE_IMAGE_PERMISSION = "UPDATE_IMAGE_PERMISSION";
            public const string DELETE_IMAGE_PERMISSION = "DELETE_IMAGE_PERMISSION";
            public const string VIEW_IMAGE_PERMISSION = "VIEW_IMAGE_PERMISSION";

        }
    }
}
