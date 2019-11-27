#region "Import Namespaces"
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
#endregion "Import Namespaces" 

#region "MSFT.RDMISaaS.API.Common"
namespace MSFT.WVDSaaS.API.Common
{

    #region "Configurations"
    public class Configurations
    {
        #region " Declaring variables "
        public string rdBrokerUrl = "";
        public string redirectUrl = "";
        public string applicationId = "";
        public string resourceUrl = "";
        public string managementResourceUrl = "";
        public string clientSecret = "";
        public string aadTokenUrl = "";
        public string applicationVersion = "";
        public string gitHubUrl = "";
        #endregion

        #region " Constructor "
        public Configurations()
        {
            rdBrokerUrl= System.Web.Configuration.WebConfigurationManager.AppSettings["RDBrokerUrl"];
            redirectUrl = System.Web.Configuration.WebConfigurationManager.AppSettings["RedirectURI"];
            applicationId = System.Web.Configuration.WebConfigurationManager.AppSettings["ApplicationId"];
            resourceUrl = System.Web.Configuration.WebConfigurationManager.AppSettings["ResourceUrl"];
            managementResourceUrl= System.Web.Configuration.WebConfigurationManager.AppSettings["ManagementResourceUrl"];
            clientSecret = System.Web.Configuration.WebConfigurationManager.AppSettings["ApplicationSecret"];
            aadTokenUrl = System.Web.Configuration.WebConfigurationManager.AppSettings["AADTokenURL"];
            applicationVersion = System.Web.Configuration.WebConfigurationManager.AppSettings["ApplicationVersion"];
            gitHubUrl= System.Web.Configuration.WebConfigurationManager.AppSettings["GitHubUrl"];
        }
        #endregion

    }
    #endregion  
}
#endregion "MSFT.RDMISaaS.API.Common" 
