using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

//using System;  
//using System.Linq;  
using System.Configuration;  
using System.Collections.Specialized;  
using System.Configuration.Provider;  
using System.Data;  
using System.Data.SqlClient;  
using System.Security.Cryptography;  
using System.Text;  
using System.Web.Configuration;  
using System.Web.Security;
using Data.Utilities;

namespace AArk.Code_Files
{
    public class CustomMembershipProvider : MembershipProvider
    {
        public override bool EnablePasswordRetrieval => throw new NotImplementedException();

        public override bool EnablePasswordReset => true;

        public override bool RequiresQuestionAndAnswer => false;

        public override string ApplicationName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override int MaxInvalidPasswordAttempts => throw new NotImplementedException();

        public override int PasswordAttemptWindow => throw new NotImplementedException();

        public override bool RequiresUniqueEmail => throw new NotImplementedException();

        public override MembershipPasswordFormat PasswordFormat => throw new NotImplementedException();

        public override int MinRequiredPasswordLength => throw new NotImplementedException();

        public override int MinRequiredNonAlphanumericCharacters => throw new NotImplementedException();

        public override string PasswordStrengthRegularExpression => throw new NotImplementedException();

        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            throw new NotImplementedException();
        }

        public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
        {
            throw new NotImplementedException();
        }

        public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override int GetNumberOfUsersOnline()
        {
            throw new NotImplementedException();
        }

        public override string GetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            throw new NotImplementedException();
        }

        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            DataTable dt = DataProxy.GetUserInfoUsername(username);
            if(dt.Rows.Count == 0)
            {
                return new MembershipUser(
                    providerName: "CustomMembershipProvider",
                    name: "",
                    providerUserKey: null,
                    email: "",
                    passwordQuestion: "",
                    comment: "",
                    isApproved: false,
                    isLockedOut: true,
                    creationDate: DateTime.UtcNow,
                    lastLoginDate: DateTime.UtcNow,
                    lastActivityDate: DateTime.UtcNow,
                    lastPasswordChangedDate: DateTime.UtcNow,
                    lastLockoutDate: DateTime.UtcNow);
            }
            else
            {
                var dr = dt.Rows[0];
                var createDate = dr["CreateDate"] == null ? DateTime.UtcNow : Convert.ToDateTime(dr["CreateDate"]);
                return new MembershipUser(
                    providerName: "MyMembershipProvider",
                    name: dr["UserName"] as string,
                    providerUserKey: null,
                    email: dr["Email"] as string,
                    passwordQuestion: "",
                    comment: "",
                    isApproved: true,
                    isLockedOut: false,
                    creationDate: createDate,
                    lastLoginDate: DateTime.UtcNow,
                    lastActivityDate: DateTime.UtcNow,
                    lastPasswordChangedDate: DateTime.UtcNow,
                    lastLockoutDate: DateTime.UtcNow);
            }
        }

        public override string GetUserNameByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public override string ResetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        public override bool UnlockUser(string userName)
        {
            throw new NotImplementedException();
        }

        public override void UpdateUser(MembershipUser user)
        {
            throw new NotImplementedException();
        }

        public override bool ValidateUser(string username, string password)
        {
            var dt = DataProxy.GetUserInfo(username, password);
            return dt.Rows.Count > 0;
        }
    }
}