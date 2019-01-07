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
#endregion "Import Namespaces"

#region "MSFT.RDMISaaS.API.Controllers"
namespace MSFT.RDMISaaS.API.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]

    #region "RemoteAppController"
    public class RemoteAppController : ApiController
    {
        #region "Class level declaration"
        RemoteAppBL remoteAppBL = new RemoteAppBL();
        RemoteAppResult remoteAppResult = new RemoteAppResult();
        List<RemoteAppResult> lstremoteAppResult = new List<RemoteAppResult>();
        Common.Common common = new Common.Common();
        Common.Configurations configurations = new Common.Configurations();
        string deploymentUrl = "";
        string invalidToken = Constants.invalidToken.ToString().ToLower();
        string invalidCode = Constants.invalidCode.ToString().ToLower();
        #endregion

        #region "Functions/Methods"
        /// <summary>
        /// Decription : Gets a RemoteApp within a Tenant, HostPool and AppGroup associated with the specified Rds context.
        /// </summary>
        /// <param name="tenantName">Name of tenant</param>
        /// <param name="hostPoolName">Name of Hostpool</param>
        /// <param name="appGroupName">Name of App Group</param>
        /// <param name="remoteAppName">Name of Remote App</param>
        /// <param name="refresh_token">Refresh token to get access token</param>
        /// <returns></returns>
        public RdMgmtRemoteApp GetRemoteAppDetails(string tenantGroupName, string tenantName, string hostPoolName, string appGroupName, string remoteAppName, string refresh_token)
        {
            //get deployment url
            deploymentUrl = configurations.rdBrokerUrl;
            RdMgmtRemoteApp remoteapp = new RdMgmtRemoteApp();
            try
            {
                if (!string.IsNullOrEmpty(refresh_token))
                {
                    string accessToken = "";
                    //get token value
                    accessToken = common.GetTokenValue(refresh_token);
                    if (!string.IsNullOrEmpty(accessToken) && accessToken.ToString().ToLower() != invalidToken && accessToken.ToString().ToLower() != invalidCode)
                    {
                        remoteapp = remoteAppBL.GetRemoteAppDetails(tenantGroupName,deploymentUrl, accessToken, tenantName, hostPoolName, appGroupName, remoteAppName);
                    }
                    else
                    {
                        remoteapp.code = Constants.invalidToken;
                    }
                }
            }
            catch
            {
                return null;
            }
            return remoteapp;
        }

        /// <summary>
        /// Description : Creates a RemoteApp within a Tenant, HostPool and AppGroup associated with the specified Rds context.
        /// </summary>
        /// <param name="rdMgmtRemoteApp">Remote App class</param>
        /// <returns></returns>
        public IHttpActionResult Post([FromBody] RdMgmtRemoteApp rdMgmtRemoteApp)
        {
            //get deployment url
            deploymentUrl = configurations.rdBrokerUrl;
            try
            {
                if (rdMgmtRemoteApp != null)
                {
                    if (!string.IsNullOrEmpty(rdMgmtRemoteApp.refresh_token))
                    {
                        string accessToken = "";
                        //get token value
                        accessToken = common.GetTokenValue(rdMgmtRemoteApp.refresh_token);
                        if (!string.IsNullOrEmpty(accessToken) && accessToken.ToString().ToLower() != invalidToken && accessToken.ToString().ToLower() != invalidCode)
                        {
                            remoteAppResult = remoteAppBL.CreateRemoteApp(deploymentUrl, accessToken, rdMgmtRemoteApp);
                        }
                        else
                        {
                            remoteAppResult.isSuccess = false;
                            remoteAppResult.message = Constants.invalidToken;
                        }
                    }
                }
                else
                {
                    remoteAppResult.isSuccess = false;
                    remoteAppResult.message = Constants.invalidDataMessage;
                }
            }
            catch (Exception ex)
            {
                remoteAppResult.isSuccess = false;
                remoteAppResult.message = "Remote App '"+ rdMgmtRemoteApp .remoteAppName+ "' has not been published."+ ex.Message.ToString()+" Pleasse try again later.";
            }
            return Ok(remoteAppResult);
        }

        /// <summary>
        /// Descriptio - punlish multiple aps to app group
        /// </summary>
        /// <param name="rdMgmtRemoteApps"></param>
        /// <param name="refresh_token">Refresh token to get Access Token</param>
        /// <returns></returns>
        public IHttpActionResult PostApps([FromBody] List<RdMgmtRemoteApp> rdMgmtRemoteApps, string refresh_token)
        {
            //get deployment url
            deploymentUrl = configurations.rdBrokerUrl;
            try
            {
                if (rdMgmtRemoteApps != null && rdMgmtRemoteApps.Count > 0)
                {
                    if (!string.IsNullOrEmpty(refresh_token))
                    {
                        string accessToken = "";
                        //get token value
                        accessToken = common.GetTokenValue(refresh_token);
                        if (!string.IsNullOrEmpty(accessToken) && accessToken.ToString().ToLower() != invalidToken && accessToken.ToString().ToLower() != invalidCode)
                        {
                            for (int i = 0; i < rdMgmtRemoteApps.Count; i++)
                            {
                                remoteAppResult = remoteAppBL.CreateRemoteApp(deploymentUrl, accessToken, rdMgmtRemoteApps[i]);
                                lstremoteAppResult.Add(remoteAppResult);
                            }
                        }
                        else
                        {
                            remoteAppResult.isSuccess = false;
                            remoteAppResult.message = Constants.invalidToken;
                            lstremoteAppResult.Add(remoteAppResult);
                        }
                    }
                }
                else
                {
                    remoteAppResult.isSuccess = false;
                    remoteAppResult.message = Constants.invalidDataMessage;
                    lstremoteAppResult.Add(remoteAppResult);
                }
            }
            catch (Exception ex)
            {
                remoteAppResult.isSuccess = false;
                remoteAppResult.message = "Remote apps has not been published."+ex.Message.ToString()+" Please try again later.";
                lstremoteAppResult.Add(remoteAppResult);
            }
            return Ok(remoteAppResult);
        }

        /// <summary>
        /// Description : Removes a RemoteApp within a Tenant, HostPool and AppGroup associated with the specified Rds context.
        /// </summary>
        /// <param name="tenantName">Name of tenant</param>
        /// <param name="hostPoolName">Name of Hostpool</param>
        /// <param name="appGroupName">Name of App Group</param>
        /// <param name="remoteAppName">Name of Remote App</param>
        /// <param name="refresh_token">Refresh token to get Access Token</param>
        /// <returns></returns>
        public IHttpActionResult Delete([FromUri] string tenantGroupName, string tenantName, string hostPoolName, string appGroupName, string remoteAppName, string refresh_token)
        {
            //get deployment url
            deploymentUrl = configurations.rdBrokerUrl;
            try
            {
                if (!string.IsNullOrEmpty(refresh_token))
                {
                    string accessToken = "";
                    //get token value
                    accessToken = common.GetTokenValue(refresh_token);
                    if (!string.IsNullOrEmpty(accessToken) && accessToken.ToString().ToLower() != invalidToken && accessToken.ToString().ToLower() != invalidCode)
                    {
                        remoteAppResult = remoteAppBL.DeleteRemoteApp(tenantGroupName, deploymentUrl, accessToken, tenantName, hostPoolName, appGroupName, remoteAppName);
                    }
                    else
                    {
                        remoteAppResult.isSuccess = false;
                        remoteAppResult.message = Constants.invalidToken;
                    }
                }
            }
            catch (Exception ex)
            {
                remoteAppResult.isSuccess = false;
                remoteAppResult.message = "Remote App '"+ remoteAppName + "' has not been removed."+ex.Message.ToString()+" Please try again later.";
            }
            return Ok(remoteAppResult);
        }

        /// <summary>
        /// Description : Gets a list of RemoteApps within a Tenant, HostPool and AppGroup associated with the specified Rds context.
        /// </summary>
        /// <param name="tenantName">Name of Tenant</param>
        /// <param name="hostPoolName">Name of Hostpool</param>
        /// <param name="appGroupName">Name of App Group</param>
        /// <param name="remoteAppName">Name of Remote App</param>
        /// <param name="refresh_token">Refresh token to get access token</param>
        /// <returns></returns>
        public List<RdMgmtRemoteApp> GetRemoteAppList(string tenantGroupName, string tenantName, string hostPoolName, string appGroupName, string refresh_token, int pageSize, string sortField, bool isDescending = false, int initialSkip = 0, string lastEntry = null)
        {
            //get deployment url
            deploymentUrl = configurations.rdBrokerUrl;
            List<RdMgmtRemoteApp> rdMgmtRemoteApps = new List<RdMgmtRemoteApp>();
            try
            {
                if (!string.IsNullOrEmpty(refresh_token))
                {
                    string accessToken = "";
                    //get token value
                    accessToken = common.GetTokenValue(refresh_token);
                    if (!string.IsNullOrEmpty(accessToken) && accessToken.ToString().ToLower() != invalidToken && accessToken.ToString().ToLower() != invalidCode)
                    {
                        rdMgmtRemoteApps = remoteAppBL.GetRemoteAppList(tenantGroupName, deploymentUrl, accessToken, tenantName, hostPoolName, appGroupName, false,false, pageSize, sortField, isDescending, initialSkip, lastEntry);
                    }
                    else
                    {
                        RdMgmtRemoteApp rdMgmtRemoteApp = new RdMgmtRemoteApp();
                        rdMgmtRemoteApp.code = Constants.invalidToken;
                        rdMgmtRemoteApps.Add(rdMgmtRemoteApp);
                    }
                }
            }
            catch 
            {
                return null;
            }
            return rdMgmtRemoteApps;
        }
        #endregion

    }
    #endregion

}
#endregion
