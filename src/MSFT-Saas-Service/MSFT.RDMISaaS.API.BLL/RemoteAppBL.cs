#region "Import Namespaces"
using MSFT.RDMISaaS.API.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
#endregion "Import Namespaces"

#region "MSFT.RDMISaaS.API.BLL"
namespace MSFT.RDMISaaS.API.BLL
{
    #region "RemoteAppBL"
    public class RemoteAppBL
    {
        RemoteAppResult appResult = new RemoteAppResult();
        //string tenantGroup = Constants.tenantGroupName;

        #region "Functions/Methods"
        /// <summary>
        /// Description - Gets a RemoteApp within a  Tenant, HostPool and AppGroup associated with the specified Rds context.
        /// </summary>
        /// <param name="deploymentUrl">RD Broker Url</param>
        /// <param name="accessToken">Access token</param>
        /// <param name="tenantName">Name of Tenant</param>
        /// <param name="hostPoolName">Name of Hostpool</param>
        /// <param name="appGroupName">Name of App Group</param>
        /// <param name="remoteAppName">Name of Remote App</param>
        /// <returns></returns>
        public HttpResponseMessage GetRemoteAppDetails(string tenantGroupName, string deploymentUrl, string accessToken, string tenantName, string hostPoolName, string appGroupName, string remoteAppName)
        {
            // RdMgmtRemoteApp rdMgmtRemoteApp = new RdMgmtRemoteApp();
            try
            {
                //call rest api to get all app groups -- july code bit
                HttpResponseMessage response = CommonBL.InitializeHttpClient(deploymentUrl, accessToken).GetAsync("/RdsManagement/V1/TenantGroups/" + tenantGroupName + "/Tenants/" + tenantName + "/HostPools/" + hostPoolName + "/AppGroups/" + appGroupName + "/RemoteApps/" + remoteAppName).Result;
                return response;
                //string strJson = response.Content.ReadAsStringAsync().Result;
                //if (response.IsSuccessStatusCode)
                //{
                //    //Deserialize the string to JSON object
                //    rdMgmtRemoteApp = JsonConvert.DeserializeObject<RdMgmtRemoteApp>(strJson);
                //}
            }
            catch
            {
                return null;
            }
            // return rdMgmtRemoteApp;
        }

        /// <summary>
        /// Description - Create a RemoteApp within a  Tenant, HostPool and AppGroup associated with the specified Rds context.
        /// </summary>
        /// <param name="deploymentUrl">RD Broker Url</param>
        /// <param name="accessToken">Access Token</param>
        /// <param name="rdMgmtRemoteApp">Remote App class</param>
        /// <returns></returns>
        public RemoteAppResult CreateRemoteApp(string deploymentUrl, string accessToken, RdMgmtRemoteApp rdMgmtRemoteApp)
        {
            try
            {
                RemoteAppDTO remoteAppDTO = new RemoteAppDTO();
                remoteAppDTO.tenantName = rdMgmtRemoteApp.tenantName;
                remoteAppDTO.friendlyName = rdMgmtRemoteApp.friendlyName;
                remoteAppDTO.description = rdMgmtRemoteApp.description;
                remoteAppDTO.appGroupName = rdMgmtRemoteApp.appGroupName;
                remoteAppDTO.hostPoolName = rdMgmtRemoteApp.hostPoolName;
                remoteAppDTO.remoteAppName = rdMgmtRemoteApp.remoteAppName;
                remoteAppDTO.filePath = rdMgmtRemoteApp.filePath;
                remoteAppDTO.commandLineSetting = rdMgmtRemoteApp.commandLineSetting;
                remoteAppDTO.iconIndex = rdMgmtRemoteApp.iconIndex;
                remoteAppDTO.requiredCommandLine = rdMgmtRemoteApp.requiredCommandLine;
                remoteAppDTO.showInWebFeed = rdMgmtRemoteApp.showInWebFeed;
                remoteAppDTO.appAlias = rdMgmtRemoteApp.appAlias;
                remoteAppDTO.tenantGroupName = rdMgmtRemoteApp.tenantGroupName;
                //call rest api to publish remote appgroup app -- july code bit
                var content = new StringContent(JsonConvert.SerializeObject(remoteAppDTO), Encoding.UTF8, "application/json");
                HttpResponseMessage response = CommonBL.InitializeHttpClient(deploymentUrl, accessToken).PostAsync("/RdsManagement/V1/TenantGroups/" + rdMgmtRemoteApp.tenantGroupName + "/Tenants/" + rdMgmtRemoteApp.tenantName + "/HostPools/" + rdMgmtRemoteApp.hostPoolName + "/AppGroups/" + rdMgmtRemoteApp.appGroupName + "/RemoteApps/" + rdMgmtRemoteApp.remoteAppName, content).Result;
                string strJson = response.Content.ReadAsStringAsync().Result;
                if (response.IsSuccessStatusCode)
                {
                    if (response.StatusCode.ToString().ToLower() == "created")
                    {
                        appResult.isSuccess = true;
                        appResult.message = "Remote app '" + rdMgmtRemoteApp.remoteAppName + "' has been published successfully.";
                    }
                }
                else if ((int)response.StatusCode == 429)
                {
                    appResult.isSuccess = false;
                    appResult.message = strJson + " Please try again later.";
                }
                else
                {
                    if (!string.IsNullOrEmpty(strJson))
                    {
                        appResult.isSuccess = false;
                        appResult.message = CommonBL.GetErrorMessage(strJson);
                    }
                    else
                    {
                        appResult.isSuccess = false;
                        appResult.message = "Remote app '" + rdMgmtRemoteApp.remoteAppName + "' has not been published. Please try it later again.";
                    }
                }
            }
            catch (Exception ex)
            {
                appResult.isSuccess = false;
                appResult.message = "Remote app '" + rdMgmtRemoteApp.remoteAppName + "' has not been published." + ex.Message.ToString() + " Please try it later again.";
            }
            return appResult;
        }


        /// <summary>
        /// Description - Gets a list of RemoteApps within a TenantGroup, Tenant, HostPool and AppGroup associated with the specified Rds context.
        /// </summary>
        /// <param name="deploymentUrl">RD Broker url</param>
        /// <param name="accessToken">Access Token</param>
        /// <param name="tenantName">Name of Tenant</param>
        /// <param name="hostPoolName">Name of Hostpool</param>
        /// <param name="appGroupName">Name of App Group</param>
        /// <param name="remoteAppName">Name of Remote App</param>
        /// <param name="isRemoteAppNameOnly">To get Remote app Name only</param>
        /// //old parameters --  bool isRemoteAppNameOnly,bool isAll, int pageSize, string sortField, bool isDescending, int initialSkip, string lastEntry
        /// <returns></returns>
        public HttpResponseMessage GetRemoteAppList(string tenantGroupName, string deploymentUrl, string accessToken, string tenantName, string hostPoolName, string appGroupName)
        {
            // List<RdMgmtRemoteApp> rdMgmtRemoteApps = new List<RdMgmtRemoteApp>();
            try
            {
                HttpResponseMessage response = CommonBL.InitializeHttpClient(deploymentUrl, accessToken).GetAsync("/RdsManagement/V1/TenantGroups/" + tenantGroupName + "/Tenants/" + tenantName + "/HostPools/" + hostPoolName + "/AppGroups/" + appGroupName + "/RemoteApps").Result;
                return response;

                //api call included pagination 
                // response = CommonBL.InitializeHttpClient(deploymentUrl, accessToken).GetAsync("/RdsManagement/V1/TenantGroups/" + tenantGroupName + "/Tenants/" + tenantName + "/HostPools/" + hostPoolName + "/AppGroups/" + appGroupName + "/RemoteApps?PageSize=" + pageSize + "&LastEntry=" + lastEntry + "&SortField=" + sortField + "&IsDescending=" + isDescending + "&InitialSkip=" + initialSkip).Result;


                ////call rest api to get  remote app list  -- july code bit
                //string strJson = response.Content.ReadAsStringAsync().Result;
                //if (response.IsSuccessStatusCode)
                //{
                //    //Deserialize the string to JSON object
                //    var jObj = (JArray)JsonConvert.DeserializeObject(strJson);
                //    if (jObj.Count > 0)
                //    {
                //        if (isRemoteAppNameOnly)
                //        {
                //            rdMgmtRemoteApps = jObj.Select(item => new RdMgmtRemoteApp
                //            {
                //                remoteAppName = (string)item["remoteAppName"]
                //            }).ToList();
                //        }
                //        else
                //        {
                //            rdMgmtRemoteApps = jObj.Select(item => new RdMgmtRemoteApp
                //            {
                //                tenantName = (string)item["tenantName"],
                //                remoteAppName = (string)item["remoteAppName"],
                //                appAlias = (string)item["appAlias"],
                //                hostPoolName = (string)item["hostPoolName"],
                //                appGroupName = (string)item["appGroupName"],
                //                description = (string)item["tenantName"],
                //                filePath = (string)item["filePath"],
                //                friendlyName = (string)item["friendlyName"],
                //                commandLineSetting = (string)item["commandLineSetting"],
                //                iconIndex = (int)item["iconIndex"],
                //                iconPath = (string)item["iconPath"],
                //                requiredCommandLine = (string)item["requiredCommandLine"],
                //                showInWebFeed = (bool)item["showInWebFeed"]
                //            }).ToList();
                //        }
                //    }
                //}
            }
            catch
            {
                return null;
            }
            // return rdMgmtRemoteApps;
        }
        /// <summary>
        /// Description : Remove remote app from associated app group
        /// </summary>
        /// <param name="deploymentUrl"></param>
        /// <param name="accessToken"></param>
        /// <param name="tenantName"></param>
        /// <param name="hostPoolName"></param>
        /// <param name="appGroupName"></param>
        /// <param name="remoteAppName"></param>
        /// <returns></returns>
        public RemoteAppResult DeleteRemoteApp(string tenantGroupName, string deploymentUrl, string accessToken, string tenantName, string hostPoolName, string appGroupName, string remoteAppName)
        {
            try
            {

                //call rest api to remove remote app -- july code bit
                HttpResponseMessage response = CommonBL.InitializeHttpClient(deploymentUrl, accessToken).DeleteAsync("/RdsManagement/V1/TenantGroups/" + tenantGroupName + "/Tenants/" + tenantName + "/HostPools/" + hostPoolName + "/AppGroups/" + appGroupName + "/RemoteApps/" + remoteAppName).Result;
                string strJson = response.Content.ReadAsStringAsync().Result;
                if (response.IsSuccessStatusCode)
                {
                    appResult.isSuccess = true;
                    appResult.message = "Remote app '" + remoteAppName + "' has been removed from app group '" + appGroupName + "' successfully.";
                }
                else if ((int)response.StatusCode == 429)
                {
                    appResult.isSuccess = false;
                    appResult.message = strJson + " Please try again later.";
                }
                else
                {
                    if (!string.IsNullOrEmpty(strJson))
                    {
                        appResult.isSuccess = false;
                        appResult.message = CommonBL.GetErrorMessage(strJson);
                    }
                    else
                    {
                        appResult.isSuccess = false;
                        appResult.message = "Remote app '" + remoteAppName + "' has not been removed from app group '" + appGroupName + "'. Please try again later.";
                    }
                }
            }
            catch (Exception ex)
            {
                appResult.isSuccess = false;
                appResult.message = "Remote app '" + remoteAppName + "' has not been removed from app group '" + appGroupName + "'." + ex.Message.ToString() + " Please try again later.";
            }
            return appResult;
        }
        #endregion

    }
    #endregion

}
#endregion

