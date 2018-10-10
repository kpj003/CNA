using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Configuration;
using System.Linq;
using System.Text;

namespace Data.Utilities
{
    public static class Settings
    {
        #region AppSetting Key Constants

        #region Internal Constants
        
        private const string IMAGE_HTML_TAG = "<img src=\"/images/{0}\" alt=\"{1}\" title=\"{1}\" style=\"float:none; vertical-align:middle; margin-right:2px;\" />";
 
        #endregion

        #region Public Constants
           
        public const string APPSETTING_NEW_USER_EMAIL_SUBJECT = "WelcomeNewUser";
        public const string APPSETTING_FORGOT_USERNAME_EMAIL_SUBJECT = "ForgotUserName";
        public const string APPSETTING_FORGOT_PASSWORD_EMAIL_SUBJECT = "ForgotPassword";
        public const string APPSETTING_FORGOT_PASSWORD_EMAIL_BODYFILE = "ForgotPasswordMessageFile";

              
        #endregion

        #endregion

        #region Public Properties

      
        public static string AARKConnectionString
        {
            get { return WebConfigurationManager.ConnectionStrings["AARKConnectionString"].ConnectionString; }
        }

        public static string ContactEmail
        {
            get { return WebConfigurationManager.AppSettings["ContactEmail"]; }
        }

        public static string LocalHostEmail
        {
            get { return WebConfigurationManager.AppSettings["LocalHostEmail"]; }
        }

      
         #endregion

        #region Utilities to help returning the values

        public static string GetIconImageTagString(string ImageName, string AltText)
        {
            return string.Format(IMAGE_HTML_TAG, ImageName, AltText);
        }

      
        /// <summary>
        /// Creates an absolute from a passed-in relative link that contains a preceding slash (/)
        /// </summary>
        /// <param name="RelativeUrl">A string URL value with a preceding slash (/) character</param>
        /// <returns>An string value representing an absolute URL with the host used for a web request</returns>
        public static string MakeAbsoluteLink(string RelativeUrl)
        {
            return (HttpContext.Current.Request.IsSecureConnection) ? "https://" : "http://" + HttpContext.Current.Request.Url.Authority + RelativeUrl;
        }

        /// <summary>
        /// Returns a string appSetting value from the Web.Config
        /// </summary>
        /// <param name="AppSettingName">string</param>
        /// <remarks>This function will return an empty string if the setting key does not exist in Web.config</remarks>
        /// <returns>a string representing the value of the appSetting key</returns>
        public static string GetStringAppSetting(string AppSettingName)
        {
            string _appSetting = string.Empty;
            try { _appSetting = WebConfigurationManager.AppSettings[AppSettingName]; }
            catch { }

            return _appSetting;
        }

        /// <summary>
        /// Returns a boolean value representing a string appSetting value from the Web.Config
        /// </summary>
        /// <param name="AppSettingName">The appSetting Key you wish to retrieve</param>
        /// <remarks>This function will return a FALSE boolean value if the setting key does not exist in Web.config or an error occurs</remarks>
        /// <returns>A boolean representation of the string value in Web.config</returns>
        public static bool GetBooleanAppSetting(string AppSettingName)
        {
            string _stringAppSetting = GetStringAppSetting(AppSettingName);
            bool _boolAppSetting = false;
            try
            {
                _boolAppSetting = bool.Parse(_stringAppSetting);
            }
            catch { }

            return _boolAppSetting;
        }
        #endregion

    
    }
}
