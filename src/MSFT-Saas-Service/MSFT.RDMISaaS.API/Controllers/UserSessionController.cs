#region "Import Namespaces"
using MSFT.RDMISaaS.API.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MSFT.RDMISaaS.API.BLL;
using System.Web.Http.Cors;
using MSFT.RDMISaaS.API.Common;
#endregion "Import Namespaces"

#region "MSFT.RDMISaaS.API.Controllers"
namespace MSFT.RDMISaaS.API.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]

    #region "UserSessionController"
    public class UserSessionController : ApiController
    {
        #region "Class level declaration"
        UserSessionBL userSessionBL = new UserSessionBL();
        Common.Common common = new Common.Common();
        Common.Configurations configurations = new Common.Configurations();
        string deploymentUrl = "";
        string invalidToken = Constants.invalidToken.ToString().ToLower();
        string invalidCode = Constants.invalidCode.ToString().ToLower();
        #endregion

        #region "Functions/Methods"
        /// <summary>
        /// Description - get list of User sessions
        /// </summary>
        /// <param name="tenantName">name of tenant</param>
        /// <param name="hostPoolName">Name of Hostpool</param>
        /// <param name="refresh_token">Refresh Token to get access token</param>
        /// <returns></returns>
        public List<RdMgmtUserSession> GetListOfUserSessioons( string tenantGroupName, string tenantName, string hostPoolName, string refresh_token, int pageSize, string sortField, bool isDescending = false, int initialSkip = 0, string lastEntry = null)
        {
            //get deployment url
            deploymentUrl = configurations.rdBrokerUrl;
            List<RdMgmtUserSession> rdMgmtUserSessions = new List<RdMgmtUserSession>();
            try
            {
                if (!string.IsNullOrEmpty(refresh_token))
                {
                    string accessToken = "";
                    //get token value
                    accessToken = common.GetTokenValue(refresh_token);
                    if (!string.IsNullOrEmpty(accessToken) && accessToken.ToString().ToLower() != invalidToken && accessToken.ToString().ToLower() != invalidCode)
                    {
                        rdMgmtUserSessions = userSessionBL.GetListOfUserSessioons(deploymentUrl, accessToken, tenantGroupName, tenantName, hostPoolName,false, pageSize, sortField, isDescending, initialSkip, lastEntry);
                    }
                    else
                    {
                        RdMgmtUserSession rdMgmtUserSession = new RdMgmtUserSession();
                        rdMgmtUserSession.code = Constants.invalidToken;
                        rdMgmtUserSessions.Add(rdMgmtUserSession);
                    }
                }
            }
            catch 
            {
                return null;
            }
            return rdMgmtUserSessions;
        }
        #endregion

    }
    #endregion

}
#endregion
