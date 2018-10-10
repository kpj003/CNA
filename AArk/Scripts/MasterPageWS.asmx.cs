using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Web.Script.Services;
using Data.Utilities;

namespace AArk.Scripts
{
    /// <summary>
    /// Summary description for MasterPageWS
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class MasterPageWS : System.Web.Services.WebService
    {

        [WebMethod(EnableSession = true)]

        public void CallFromMasterJS(int assessID, bool archive)
        {
            
            if (archive)
                DataProxy.ArchiveAssessment(assessID);
            else
                DataProxy.UnarchiveAssessment(assessID);
        }
    }
}
