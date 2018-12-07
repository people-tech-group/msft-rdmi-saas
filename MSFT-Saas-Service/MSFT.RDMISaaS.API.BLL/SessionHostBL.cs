#region "Import Namespaces"
using MSFT.RDMISaaS.API.Model;
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
namespace MSFT.RDMISaaS.API.BLL
{
    #region "SessionHostBL"
    public class SessionHostBL
    {
        SessionHostResult hostResult = new SessionHostResult();
        string tenantGroup = Constants.tenantGroupName;

        /// <summary>
        /// Description: Gets all Rds SessionHosts associated with the Tenant and Host Pool specified in the Rds context.
        /// </summary>
        /// <param name="deploymentUrl">RD Broker Url</param>
        /// <param name="accessToken">Access token</param>
        /// <param name="tenantName">Name of Tenant</param>
        /// <param name="hostPoolName">Name of hostpool</param>
        /// <param name="isSessionHostNameOnly">To Get only HostName</param>
        /// <returns></returns>
        public List<RdMgmtSessionHost> GetSessionhostList(string deploymentUrl, string accessToken, string tenantName, string hostPoolName, bool isSessionHostNameOnly,bool isAll, int pageSize, string sortField, bool isDescending, int initialSkip, string lastEntry)
        {
            List<RdMgmtSessionHost> rdMgmtSessionHosts = new List<RdMgmtSessionHost>();
            HttpResponseMessage response; 
           
            try
            {
                if (isAll == true)
                {
                    response = CommonBL.InitializeHttpClient(deploymentUrl, accessToken).GetAsync("/RdsManagement/V1/TenantGroups/" + tenantGroup + "/Tenants/" + tenantName + "/HostPools/" + hostPoolName + "/SessionHosts").Result;

                }
                else {

                    response = CommonBL.InitializeHttpClient(deploymentUrl, accessToken).GetAsync("/RdsManagement/V1/TenantGroups/" + tenantGroup + "/Tenants/" + tenantName + "/HostPools/" + hostPoolName + "/SessionHosts?PageSize=" + pageSize + "&LastEntry=" + lastEntry + "&SortField=" + sortField + "&IsDescending=" + isDescending + "&InitialSkip=" + initialSkip).Result;
                }
                //call rest api to get all Session hosts asscociated with selected  hostpool -- july code bit
                string strJson = response.Content.ReadAsStringAsync().Result;
                if (response.IsSuccessStatusCode)
                {
                    //Deserialize the string to JSON object
                    var jObj = (JArray)JsonConvert.DeserializeObject(strJson);
                    if (jObj.Count > 0)
                    {
                        if (isSessionHostNameOnly)
                        {
                            rdMgmtSessionHosts = jObj.Select(item => new RdMgmtSessionHost
                            {
                                tenantName = (string)item["tenantName"],
                                hostPoolName = (string)item["hostPoolName"],
                                sessionHostName = (string)item["sessionHostName"]
                            }).ToList();
                        }
                        else
                        {
                            rdMgmtSessionHosts = jObj.Select(item => new RdMgmtSessionHost
                            {
                                tenantName = (string)item["tenantName"],
                                hostPoolName = (string)item["hostPoolName"],
                                sessionHostName = (string)item["sessionHostName"],
                                allowNewSession = (bool)item["allowNewSession"],
                                sessions = (int)item["sessions"],
                                lastHeartBeat = (DateTime)item["lastHeartBeat"],
                                agentVersion = (string)item["agentVersion"]
                            }).ToList();
                        }
                    }
                }
            }
            catch 
            {
                return null;
            }
            return rdMgmtSessionHosts;
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
        public RdMgmtSessionHost GetSessionHostDetails(string deploymentUrl, string accessToken, string tenantName, string hostPoolName, string sessionHostName)
        {
            RdMgmtSessionHost rdMgmtSessionHost = new RdMgmtSessionHost();
            try
            {
                //call rest api to get session host details -- july code bit
                HttpResponseMessage response = CommonBL.InitializeHttpClient(deploymentUrl, accessToken).GetAsync("/RdsManagement/V1/TenantGroups/" + tenantGroup + "/Tenants/" + tenantName + "/HostPools/" + hostPoolName + "/SessionHosts/" + sessionHostName).Result;
                string strJson = response.Content.ReadAsStringAsync().Result;
                if (response.IsSuccessStatusCode)
                {
                    //Deserialize the string to JSON object
                    rdMgmtSessionHost = JsonConvert.DeserializeObject<RdMgmtSessionHost>(strJson);
                }

            }
            catch 
            {
                return null;
            }
            return rdMgmtSessionHost;
        }

        /// <summary>
        /// Description : Updates a Rds SessionHost associated with the Tenant and HostPool specified in the Rds context.
        /// </summary>
        /// <param name="deploymentUrl">RD Broker Url</param>
        /// <param name="accessToken">Access token</param>
        /// <param name="rdMgmtSessionHost">Sessio  Host Class</param>
        /// <returns></returns>
        public SessionHostResult UpdateSessionHost(string deploymentUrl, string accessToken, RdMgmtSessionHost rdMgmtSessionHost)
        {
            try
            {
                //assigning values to Session host class object
                SessionHostDTO sessionHostDTO = new SessionHostDTO();
                sessionHostDTO.tenantName = rdMgmtSessionHost.tenantName;
                sessionHostDTO.hostPoolName = rdMgmtSessionHost.hostPoolName;
                sessionHostDTO.sessionHostName = rdMgmtSessionHost.sessionHostName;
                sessionHostDTO.allowNewSession = rdMgmtSessionHost.allowNewSession;

                //call rest service to update Sesssion host -- july code bit
                var content = new StringContent(JsonConvert.SerializeObject(sessionHostDTO), Encoding.UTF8, "application/json");
                HttpResponseMessage response = CommonBL.PatchAsync(deploymentUrl, accessToken, "/RdsManagement/V1/TenantGroups/" + tenantGroup + "/Tenants/" + rdMgmtSessionHost.tenantName + "/HostPools/" + rdMgmtSessionHost.hostPoolName + "/SessionHosts/" + rdMgmtSessionHost.sessionHostName, content).Result;
                string strJson = response.Content.ReadAsStringAsync().Result;
                if (response.IsSuccessStatusCode)
                {
                    hostResult.isSuccess = true;
                    hostResult.message = "Session host '" + rdMgmtSessionHost.sessionHostName + "' has been updated successfully.";
                }
                else
                {
                    if (!string.IsNullOrEmpty(strJson))
                    {
                        hostResult.isSuccess = false;
                        hostResult.message = CommonBL.GetErrorMessage(strJson);
                    }
                    else
                    {
                        hostResult.isSuccess = false;
                        hostResult.message = "Updating session host '" + rdMgmtSessionHost.sessionHostName + "' is failed. Please try it again later.";
                    }
                }
            }
            catch (Exception ex)
            {
                hostResult.isSuccess = false;
                hostResult.message = "Session host '" + rdMgmtSessionHost.sessionHostName + "' has not been updated."+ex.Message.ToString()+" Please try it again later.";
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
        public SessionHostResult DeletesessionHost(string deploymentUrl, string accessToken, string tenantName, string hostPoolName, string sessionHostName)
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
                sessionHostResult.message = "Session host " + sessionHostName + " has not been deleted."+ex.Message.ToString()+" Please try it later again.";
            }
            return sessionHostResult;
        }
    }
    #endregion "SessionHostBL"
}
#endregion "MSFT.RDMISaaS.API.BLL"
