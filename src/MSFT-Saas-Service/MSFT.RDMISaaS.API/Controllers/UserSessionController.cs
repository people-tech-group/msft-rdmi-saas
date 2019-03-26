#region "Import Namespaces"
using MSFT.WVDSaaS.API.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MSFT.WVDSaaS.API.BLL;
using System.Web.Http.Cors;
using MSFT.WVDSaaS.API.Common;
using Newtonsoft.Json.Linq;
#endregion "Import Namespaces"

#region "MSFT.RDMISaaS.API.Controllers"
namespace MSFT.WVDSaaS.API.Controllers
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
        /// old parameters for pagination --   int pageSize, string sortField, bool isDescending = false, int initialSkip = 0, string lastEntry = null
        /// <returns></returns>
        public HttpResponseMessage GetListOfUserSessions(string tenantGroupName, string tenantName, string hostPoolName, string refresh_token)
        {
            //get deployment url
            deploymentUrl = configurations.rdBrokerUrl;
            //List<RdMgmtUserSession> rdMgmtUserSessions = new List<RdMgmtUserSession>();
            try
            {
                if (!string.IsNullOrEmpty(refresh_token))
                {
                    string accessToken = "";
                    //get token value
                    accessToken = common.GetTokenValue(refresh_token);
                    if (!string.IsNullOrEmpty(accessToken) && accessToken.ToString().ToLower() != invalidToken && accessToken.ToString().ToLower() != invalidCode)
                    {
                        return userSessionBL.GetListOfUserSessioons(deploymentUrl, accessToken, tenantGroupName, tenantName, hostPoolName);
                    }
                    else
                    {

                        return Request.CreateResponse(HttpStatusCode.OK, new JArray() { new JObject() { { "code", Constants.invalidToken } } });
                        //RdMgmtUserSession rdMgmtUserSession = new RdMgmtUserSession();
                        //rdMgmtUserSession.code = Constants.invalidToken;
                        //rdMgmtUserSessions.Add(rdMgmtUserSession);
                    }
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }
            // return rdMgmtUserSessions;
        }
        #endregion

    }
    #endregion

}
#endregion
