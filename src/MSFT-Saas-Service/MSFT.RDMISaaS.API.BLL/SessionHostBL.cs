#region "Import Namespaces"
using MSFT.WVDSaaS.API.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
#endregion "Import Namespaces"

#region "MSFT.RDMISaaS.API.BLL"
namespace MSFT.WVDSaaS.API.BLL
{
    #region "SessionHostBL"
    public class SessionHostBL
    {
        JObject hostResult = new JObject();

        /// <summary>
        /// Description: Gets all Rds SessionHosts associated with the Tenant and Host Pool specified in the Rds context.
        /// </summary>
        /// <param name="deploymentUrl">RD Broker Url</param>
        /// <param name="accessToken">Access token</param>
        /// <param name="tenantName">Name of Tenant</param>
        /// <param name="hostPoolName">Name of hostpool</param>
        /// <param name="isSessionHostNameOnly">To Get only HostName</param>
        /// //old parameters --  bool isSessionHostNameOnly,bool isAll, int pageSize, string sortField, bool isDescending, int initialSkip, string lastEntry
        /// <returns></returns>
        public HttpResponseMessage GetSessionhostList(string deploymentUrl, string accessToken, string tenantGroup, string tenantName, string hostPoolName)
        {
            try
            {
                HttpResponseMessage response = CommonBL.InitializeHttpClient(deploymentUrl, accessToken).GetAsync("/RdsManagement/V1/TenantGroups/" + tenantGroup + "/Tenants/" + tenantName + "/HostPools/" + hostPoolName + "/SessionHosts").Result;
                return response;

                //api call included pagination
                //    response = CommonBL.InitializeHttpClient(deploymentUrl, accessToken).GetAsync("/RdsManagement/V1/TenantGroups/" + tenantGroup + "/Tenants/" + tenantName + "/HostPools/" + hostPoolName + "/SessionHosts?PageSize=" + pageSize + "&LastEntry=" + lastEntry + "&SortField=" + sortField + "&IsDescending=" + isDescending + "&InitialSkip=" + initialSkip).Result;
            }
            catch
            {
                return null;
            }

        }

        /// <summary>
        /// Description: Gets a Rds HostPool associated with the Tenant  specified in the Rds context.
        /// </summary>
        /// <param name="deploymentUrl">RD Broker Url</param>
        /// <param name="accessToken">Access Token</param>
        /// <param name="tenantName">Name of Tenant</param>
        /// <param name="hostPoolName">Name of Hostpool</param>
        /// <param name="sessionHostName">Name of Session Host</param>
        /// <returns></returns>
        public HttpResponseMessage GetSessionHostDetails(string deploymentUrl, string accessToken, string tenantGroup, string tenantName, string hostPoolName, string sessionHostName)
        {
            try
            {
                //call rest api to get session host details -- july code bit
                HttpResponseMessage response = CommonBL.InitializeHttpClient(deploymentUrl, accessToken).GetAsync("/RdsManagement/V1/TenantGroups/" + tenantGroup + "/Tenants/" + tenantName + "/HostPools/" + hostPoolName + "/SessionHosts/" + sessionHostName).Result;
                return response;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Description : Updates a Rds SessionHost associated with the Tenant and HostPool specified in the Rds context.
        /// </summary>
        /// <param name="deploymentUrl">RD Broker Url</param>
        /// <param name="accessToken">Access token</param>
        /// <param name="rdMgmtSessionHost">Sessio  Host Class</param>
        /// <returns></returns>
        public JObject UpdateSessionHost(string deploymentUrl, string accessToken, JObject rdMgmtSessionHost)
        {
            try
            {
                //call rest service to update Sesssion host -- july code bit
                var content = new StringContent(JsonConvert.SerializeObject(rdMgmtSessionHost), Encoding.UTF8, "application/json");
                HttpResponseMessage response = CommonBL.PatchAsync(deploymentUrl, accessToken, "/RdsManagement/V1/TenantGroups/" + rdMgmtSessionHost["tenantGroupName"].ToString() + "/Tenants/" + rdMgmtSessionHost["tenantName"].ToString() + "/HostPools/" + rdMgmtSessionHost["hostPoolName"].ToString() + "/SessionHosts/" + rdMgmtSessionHost["sessionHostName"].ToString(), content).Result;
                string strJson = response.Content.ReadAsStringAsync().Result;
                if (response.IsSuccessStatusCode)
                {
                    hostResult.Add("isSuccess", true);
                    hostResult.Add("message", "Session host '" + rdMgmtSessionHost["sessionHostName"].ToString() + "' has been updated successfully.");
                }
                else if ((int)response.StatusCode == 429)
                {
                    hostResult.Add("isSuccess", false);
                    hostResult.Add("message", strJson + " Please try again later.");
                }
                else
                {
                    if (!string.IsNullOrEmpty(strJson))
                    {
                        hostResult.Add("isSuccess", false);
                        hostResult.Add("message", CommonBL.GetErrorMessage(strJson));
                    }
                    else
                    {
                        hostResult.Add("isSuccess", false);
                        hostResult.Add("message", "Updating session host '" + rdMgmtSessionHost["sessionHostName"].ToString() + "' is failed. Please try it again later.");
                    }
                }
            }
            catch (Exception ex)
            {
                hostResult.Add("isSuccess", false);
                hostResult.Add("message", "Session host '" + rdMgmtSessionHost["sessionHostName"].ToString() + "' has not been updated." + ex.Message.ToString() + " Please try it again later.");
            }
            return hostResult;
        }

        /// <summary>
        /// Description : Removes a Rds SessionHost associated with the Tenant and HostPool specified in the Rds context.
        /// </summary>
        /// <param name="deploymenturl">RD Broker Url</param>
        /// <param name="accessToken"> Access Token</param>
        /// <param name="tenantName">Name of Tenant</param>
        /// <param name="hostPoolName">Name of Hostpool</param>
        /// <param name="sessionHostName">Name of Session Host</param>
        /// <returns></returns>
        public SessionHostResult DeletesessionHost(string deploymentUrl, string accessToken, string tenantGroup, string tenantName, string hostPoolName, string sessionHostName)
        {
            SessionHostResult sessionHostResult = new SessionHostResult();
            try
            {
                //call rest service to delete session host -- july code bit
                HttpResponseMessage response = CommonBL.InitializeHttpClient(deploymentUrl, accessToken).DeleteAsync("/RdsManagement/V1/TenantGroups/" + tenantGroup + "/Tenants/" + tenantName + "/HostPools/" + hostPoolName + "/SessionHosts/" + sessionHostName + "/Force").Result;
                string strJson = response.Content.ReadAsStringAsync().Result;
                if (response.IsSuccessStatusCode)
                {
                    sessionHostResult.isSuccess = true;
                    sessionHostResult.message = "Session host '" + sessionHostName + "' has been deleted successfully.";
                }
                else if ((int)response.StatusCode == 429)
                {
                    sessionHostResult.isSuccess = false;
                    sessionHostResult.message = strJson + " Please try again later.";
                }
                else
                {
                    if (!string.IsNullOrEmpty(strJson))
                    {
                        sessionHostResult.isSuccess = false;
                        sessionHostResult.message = CommonBL.GetErrorMessage(strJson);
                    }
                    else
                    {
                        sessionHostResult.isSuccess = false;
                        sessionHostResult.message = "Session host " + sessionHostName + " has not been deleted. Please try it later again.";
                    }
                }
            }
            catch (Exception ex)
            {
                sessionHostResult.isSuccess = false;
                sessionHostResult.message = "Session host " + sessionHostName + " has not been deleted." + ex.Message.ToString() + " Please try it later again.";
            }
            return sessionHostResult;
        }
    }
    #endregion "SessionHostBL"
}
#endregion "MSFT.RDMISaaS.API.BLL"
