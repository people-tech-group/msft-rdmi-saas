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
using Newtonsoft.Json.Linq;
#endregion "Import Namespaces"

#region "MSFT.RDMISaaS.API.Controllers"
namespace MSFT.RDMISaaS.API.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]

    #region "SessionHostController"
    public class SessionHostController : ApiController
    {
        #region "Class level Declaration"
        SessionHostBL sessionHostBL = new SessionHostBL();
        SessionHostResult hostResult = new SessionHostResult();
        Common.Common common = new Common.Common();
        Common.Configurations configurations = new Common.Configurations();
        string deploymentUrl = "";
        string invalidToken = Constants.invalidToken.ToString().ToLower();
        string invalidCode = Constants.invalidCode.ToString().ToLower();

        #endregion

        #region "Functions/Methods"
        /// <summary>
        /// Description: Gets all Rds SessionHosts associated with the Tenant and Host Pool specified in the Rds context.
        /// </summary>
        /// <param name="tenantName">Name of Tenant</param>
        /// <param name="hostPoolName">Name of Hostpool</param>
        /// <param name="refresh_token">Refresh token to get access token</param>
        /// <returns></returns>
        public HttpResponseMessage GetSessionhostList(string tenantGroupName, string tenantName, string hostPoolName, string refresh_token, int pageSize, string sortField, bool isDescending = false, int initialSkip = 0, string lastEntry = null)
        {
            //get deployment url
            deploymentUrl = configurations.rdBrokerUrl;
           // List<RdMgmtSessionHost> rdMgmtSessionHosts = new List<RdMgmtSessionHost>();
            try
            {
                if (!string.IsNullOrEmpty(refresh_token))
                {
                    string accessToken = "";
                    //get token value
                    accessToken = common.GetTokenValue(refresh_token);
                    if (!string.IsNullOrEmpty(accessToken) && accessToken.ToString().ToLower() != invalidToken && accessToken.ToString().ToLower() != invalidCode)
                    {
                       return sessionHostBL.GetSessionhostList(deploymentUrl, accessToken, tenantGroupName, tenantName, hostPoolName, false,false, pageSize, sortField, isDescending, initialSkip, lastEntry);
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new JArray() { new JObject() { { "code", Constants.invalidToken } } });
                    }
                }
                else
                { return null; }
            }
            catch 
            {
                return null;
            }
          
        }

        /// <summary>
        /// Description : Gets a Rds HostPool associated with the Tenant specified in the Rds context.
        /// </summary>
        /// <param name="tenantName">Name of Tenant</param>
        /// <param name="hostPoolName">Name of hostpool</param>
        /// <param name="sessionHostName">Name of Session Host</param>
        /// <param name="refresh_token">Refresh token to get access token</param>
        /// <returns></returns>
        public HttpResponseMessage GetSessionHostDetails(string tenantGroupName,string  tenantName, string hostPoolName, string sessionHostName, string refresh_token)
        {
            //get deployment url
            deploymentUrl = configurations.rdBrokerUrl;
          //  RdMgmtSessionHost rdMgmtSessionHost = new RdMgmtSessionHost();
            try
            {
                if (!string.IsNullOrEmpty(refresh_token))
                {
                    string accessToken = "";
                    //get token value
                    accessToken = common.GetTokenValue(refresh_token);
                    if (!string.IsNullOrEmpty(accessToken) && accessToken.ToString().ToLower() != invalidToken && accessToken.ToString().ToLower() != invalidCode)
                    {
                        return sessionHostBL.GetSessionHostDetails(deploymentUrl, accessToken, tenantGroupName, tenantName, hostPoolName, sessionHostName);
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new JObject() { { "code", Constants.invalidToken } } );
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
        }

        /// <summary>
        /// Description : Updates a Rds SessionHost associated with the Tenant and HostPool specified in the Rds context.
        /// </summary>
        /// <param name="rdMgmtSessionHost"> Session  Host class</param>
        /// <returns></returns>
        public IHttpActionResult Put(RdMgmtSessionHost rdMgmtSessionHost)
        {
            //get deployment url
            deploymentUrl = configurations.rdBrokerUrl;
            try
            {
                if (rdMgmtSessionHost != null)
                {
                    if (!string.IsNullOrEmpty(rdMgmtSessionHost.refresh_token))
                    {
                        string accessToken = "";
                        //get token value
                        accessToken = common.GetTokenValue(rdMgmtSessionHost.refresh_token);
                        if (!string.IsNullOrEmpty(accessToken) && accessToken.ToString().ToLower() != invalidToken && accessToken.ToString().ToLower() != invalidCode)
                        {
                            hostResult = sessionHostBL.UpdateSessionHost(deploymentUrl, accessToken,   rdMgmtSessionHost);
                        }
                        else
                        {
                            hostResult.isSuccess = false;
                            hostResult.message = Constants.invalidToken;
                        }
                    }
                }
                else
                {
                    hostResult.isSuccess = false;
                    hostResult.message = Constants.invalidDataMessage;
                }
            }
            catch (Exception ex)
            {
                hostResult.isSuccess = false;
                hostResult.message = "Host '"+ rdMgmtSessionHost.sessionHostName + "' has not been updated."+ex.Message.ToString()+" Please try again later.";
            }
            return Ok(hostResult);
        }

        /// <summary>
        /// Description : Removes a Rds SessionHost associated with the Tenant and HostPool specified in the Rds context.
        /// </summary>
        /// <param name="tenantName">Name of Tenant</param>
        /// <param name="hostPoolName">Name of hostpool</param>
        /// <param name="sessionHostName">Name of Session Host</param>
        /// <param name="refresh_token">refresh token to get access token</param>
        /// <returns></returns>
        public IHttpActionResult DeleteSessionHost(string tenantGroup,string tenantName, string hostPoolName, string sessionHostName, string refresh_token)
        {
            //get deployment url
            deploymentUrl = configurations.rdBrokerUrl;
            SessionHostResult sessionHostResult = new SessionHostResult();
            try
            {
                if (!string.IsNullOrEmpty(refresh_token))
                {
                    string accessToken = "";
                    //get token value
                    accessToken = common.GetTokenValue(refresh_token);
                    if (!string.IsNullOrEmpty(accessToken) && accessToken.ToString().ToLower() != invalidToken && accessToken.ToString().ToLower() != invalidCode)
                    {
                        sessionHostResult = sessionHostBL.DeletesessionHost(deploymentUrl, accessToken,tenantGroup, tenantName, hostPoolName, sessionHostName);
                    }
                    else
                    {
                        sessionHostResult.message = Constants.invalidToken;
                        sessionHostResult.isSuccess = false;
                    }
                }
            }
            catch (Exception ex)
            {
                sessionHostResult.message = "Session Host '"+ sessionHostName + "' has not neen deleted."+ex.Message.ToString()+" Please try it later again.";
                sessionHostResult.isSuccess = false;
            }
            return Ok(sessionHostResult);
        }

        #endregion

    }
    #endregion

}
#endregion
