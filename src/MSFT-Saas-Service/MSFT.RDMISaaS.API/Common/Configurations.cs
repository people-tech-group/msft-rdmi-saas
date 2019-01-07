#region "Import Namespaces"
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
#endregion "Import Namespaces" 

#region "MSFT.RDMISaaS.API.Common"
namespace MSFT.RDMISaaS.API.Common
{

    #region "Configurations"
    public class Configurations
    {
        #region " Declaring variables "
        public string rdBrokerUrl = "";
        public string redirectUrl = "";
        public string applicationId = "";
        public string resourceUrl = "";
        #endregion

        #region " Constructor "
        public Configurations()
        {
            rdBrokerUrl= System.Web.Configuration.WebConfigurationManager.AppSettings["RDBrokerUrl"];
            redirectUrl = System.Web.Configuration.WebConfigurationManager.AppSettings["RedirectURI"];
            applicationId = System.Web.Configuration.WebConfigurationManager.AppSettings["ApplicationId"];
            resourceUrl = System.Web.Configuration.WebConfigurationManager.AppSettings["ResourceUrl"];
        }
        #endregion

    }
    #endregion  
}
#endregion "MSFT.RDMISaaS.API.Common" 
