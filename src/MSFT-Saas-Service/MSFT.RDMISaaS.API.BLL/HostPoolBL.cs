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
    #region "HostPoolBL"
    public class HostPoolBL
    {
        HostPoolResult poolResult = new HostPoolResult();
       // string tenantGroup = Constants.tenantGroupName;

        /// <summary>
        /// Description - Gets a Rds HostPool associated with the Tenant  specified in the Rds context.
        /// </summary>
        /// <param name="deploymentUrl">RD Broker url</param>
        /// <param name="accessToken">Access Token</param>
        /// <param name="tenantName">Name of Tenant</param>
        /// <param name="hostPoolName">name of Hostpool</param>
        /// <returns></returns>
        public HttpResponseMessage GetHostPoolDetails(string tenantGroupName,string deploymentUrl, string accessToken, string tenantName, string hostPoolName)
        {
           // RdMgmtHostPool rdMgmtHostPool = new RdMgmtHostPool();
            try
            {
                //call rest api to get host pool details -- july code bit
                HttpResponseMessage response = CommonBL.InitializeHttpClient(deploymentUrl, accessToken).GetAsync("/RdsManagement/V1/TenantGroups/"+ tenantGroupName + "/Tenants/" + tenantName + "/HostPools/" + hostPoolName).Result;
                return response;
                //string strJson = response.Content.ReadAsStringAsync().Result;
                //if (response.IsSuccessStatusCode)
                //{
                //    var jObj = (JObject)JsonConvert.DeserializeObject(strJson);
                //    return jObj;
                //    //rdMgmtHostPool = JsonConvert.DeserializeObject<RdMgmtHostPool>(strJson);
                //}
                //else
                //{
                //    return null;
                //}

                //if(!string.IsNullOrEmpty(rdMgmtHostPool.hostPoolName))
                //{
                //    AppGroupBL appGroupBL = new AppGroupBL();
                //    List<RdMgmtAppGroup> appGroupDTOs = appGroupBL.GetAppGroupsList(tenantGroupName,deploymentUrl, accessToken, rdMgmtHostPool.tenantName, rdMgmtHostPool.hostPoolName,true,true, 0, "", false, 0, "");
                //    rdMgmtHostPool.noOfAppgroups = rdMgmtHostPool.noOfAppgroups + appGroupDTOs.Count;

                //    //list app goup users associated with hostpool
                //    if(appGroupDTOs.Count>0)
                //    {
                //        for (int j = 0; j < appGroupDTOs.Count; j++)
                //        {
                //            List<RdMgmtUser> rdMgmtUsers = appGroupBL.GetUsersList(tenantGroupName,deploymentUrl, accessToken, rdMgmtHostPool.tenantName, rdMgmtHostPool.hostPoolName, appGroupDTOs[j].appGroupName,true,true, 0, "", false, 0, "");
                //            rdMgmtHostPool.noOfUsers = rdMgmtHostPool.noOfUsers + rdMgmtUsers.Count;
                //        }
                //    }
                    
                //    //list of active hosts associated with Host pool 
                //    SessionHostBL sessionHostBL = new SessionHostBL();
                //    List<RdMgmtSessionHost> rdMgmtSessionHosts = sessionHostBL.GetSessionhostList(deploymentUrl, accessToken,tenantGroupName, tenantName, rdMgmtHostPool.hostPoolName,true,true, 0, "", false, 0, "");
                //    rdMgmtHostPool.noOfActivehosts = rdMgmtHostPool.noOfActivehosts + rdMgmtSessionHosts.Count;

                //    // sessions associated with hostpool
                //    if(rdMgmtSessionHosts.Count>0)
                //    {
                //        List<RdMgmtUserSession> lstSesssions = new List<RdMgmtUserSession>();
                //        UserSessionBL userSessionBL = new UserSessionBL();
                //        lstSesssions = userSessionBL.GetListOfUserSessioons(deploymentUrl, accessToken, tenantGroupName, tenantName, hostPoolName, true, 0, "", false, 0, "");
                //        rdMgmtHostPool.noOfSessions = lstSesssions.Count;
                //    }
                //}
            }
            catch 
            {
                return null;
            }
          //  return rdMgmtHostPool;
        }

        /// <summary>
        /// Description - Gets a list of Rds HostPools associated with the Tenant specified in the Rds context.
        /// </summary>
        /// <param name="deploymentUrl">RD Broker Url</param>
        /// <param name="accessToken">Access token</param>
        /// <param name="tenantName">Name Of Tenant</param>
        /// <param name="isHostpoolNameOnly">Get only hostpool name </param>
        /// <returns></returns>


        public HttpResponseMessage GetHostPoolList(string tenantGroupName, string deploymentUrl, string accessToken, string tenantName, bool isHostpoolNameOnly, bool isAll, int pageSize, string sortField, bool isDescending, int initialSkip, string lastEntry)
        {
            try
            {
                HttpResponseMessage response;
                if (isAll == true)
                {
                    response = CommonBL.InitializeHttpClient(deploymentUrl, accessToken).GetAsync("/RdsManagement/V1/TenantGroups/" + tenantGroupName + "/Tenants/" + tenantName + "/HostPools").Result;
                }
                else
                {
                    response = CommonBL.InitializeHttpClient(deploymentUrl, accessToken).GetAsync("/RdsManagement/V1/TenantGroups/" + tenantGroupName + "/Tenants/" + tenantName + "/HostPools?PageSize=" + pageSize + "&LastEntry=" + lastEntry + "&SortField=" + sortField + "&IsDescending=" + isDescending + "&InitialSkip=" + initialSkip).Result;
                }
                return response;
                //call rest api to get host pool list -- july code bit
                //string strJson = response.Content.ReadAsStringAsync().Result;
                //if (response.IsSuccessStatusCode)
                //{
                //    //Deserialize the string to JSON object
                //    var jObj = (JArray)JsonConvert.DeserializeObject(strJson);
                //    return jObj;
                //}
                //else
                //{
                //    return null;
                //}
            }
            catch
            {
                return null;
            }
        }

        //public List<RdMgmtHostPool> GetHostPoolList_old(string tenantGroupName,string deploymentUrl, string accessToken, string tenantName,bool isHostpoolNameOnly, bool isAll, int pageSize, string sortField, bool isDescending, int initialSkip, string lastEntry)
        //{
        //    List<RdMgmtHostPool> rdMgmtHostPools = new List<RdMgmtHostPool>();
        //    try
        //    {
        //        HttpResponseMessage response;
        //        if (isAll == true)
        //        {
        //            response = CommonBL.InitializeHttpClient(deploymentUrl, accessToken).GetAsync("/RdsManagement/V1/TenantGroups/" + tenantGroupName + "/Tenants/" + tenantName + "/HostPools").Result;
        //        }
        //        else
        //        {
        //            response = CommonBL.InitializeHttpClient(deploymentUrl, accessToken).GetAsync("/RdsManagement/V1/TenantGroups/" + tenantGroupName + "/Tenants/" + tenantName + "/HostPools?PageSize=" + pageSize + "&LastEntry=" + lastEntry + "&SortField=" + sortField + "&IsDescending=" + isDescending + "&InitialSkip=" + initialSkip).Result;

        //        }

        //        //call rest api to get host pool list -- july code bit
        //        string strJson = response.Content.ReadAsStringAsync().Result;
        //        if (response.IsSuccessStatusCode)
        //        {
        //            //Deserialize the string to JSON object
        //            var jObj = (JArray)JsonConvert.DeserializeObject(strJson);
        //            if(jObj.Count>0)
        //            {
        //                if (isHostpoolNameOnly)
        //                {
        //                    rdMgmtHostPools = jObj.Select(item => new RdMgmtHostPool
        //                    {
        //                        hostPoolName = (string)item["hostPoolName"]
        //                    }).ToList();
        //                }
        //                else
        //                {
        //                    rdMgmtHostPools = jObj.Select(item => new RdMgmtHostPool
        //                    {
        //                        tenantName = (string)item["tenantName"],
        //                        hostPoolName = (string)item["hostPoolName"],
        //                        friendlyName = (string)item["friendlyName"],
        //                        description = (string)item["description"],
        //                        persistent = item["persistent"] != null ? Convert.ToBoolean(item["persistent"].ToString()) : false,
        //                        diskPath = (string)item["diskPath"],
        //                        enableUserProfileDisk = item["enableUserProfileDisk"] != null ? Convert.ToBoolean(item["enableUserProfileDisk"].ToString()) : false,
        //                       // excludeFolderPath = (string)item["excludeFolderPath"],
        //                       // includeFilePath = item["includeFilePath"],
        //                        //includeFolderPath = item["includeFolderPath"],
        //                        //customRdpProperty = item["customRdpProperty"],
        //                        maxSessionLimit = (string)item["maxSessionLimit"],
        //                        useReverseConnect = item["useReverseConnect"] != null ? item["useReverseConnect"].ToString() : "false",
        //                    }).ToList();
        //                }
        //            }

        //            if(rdMgmtHostPools.Count>0)
        //            {
        //                //get count of Activehosts, Appgroups, Sessions, users for selected hostpool
        //                for (int i = 0; i < rdMgmtHostPools.Count; i++)
        //                {
        //                    RdMgmtHostPool rdMgmtHostPool = GetHostPoolDetails(tenantGroupName,deploymentUrl, accessToken, tenantName, rdMgmtHostPools[i].hostPoolName);
        //                    rdMgmtHostPools[i].noOfActivehosts = rdMgmtHostPool.noOfActivehosts;
        //                    rdMgmtHostPools[i].noOfAppgroups = rdMgmtHostPool.noOfAppgroups;
        //                    rdMgmtHostPools[i].noOfSessions = rdMgmtHostPool.noOfSessions;
        //                    rdMgmtHostPools[i].noOfUsers = rdMgmtHostPool.noOfUsers;
        //                }
        //            }
        //        }
        //    }
        //    catch 
        //    {
        //        return null;
        //    }
        //    return rdMgmtHostPools;
        //}
        /// <summary>
        /// Description- Creates a new Rds HostPool within a Tenant specified in the Rds context. 
        /// </summary>
        /// <param name="deploymentUrl">RD Broker Url</param>
        /// <param name="accessToken">Access Token</param>
        /// <param name="rdMgmtHostPool">Hostpool class</param>
        /// <returns></returns>
        public HostPoolResult CreateHostPool(string deploymentUrl, string accessToken, RdMgmtHostPool rdMgmtHostPool)
        {
            try
            {
                HostPoolDataDTO hostpoolDataDTO = new HostPoolDataDTO();
                hostpoolDataDTO.tenantGroupName = rdMgmtHostPool.tenantGroupName;
                hostpoolDataDTO.tenantName = rdMgmtHostPool.tenantName;
                hostpoolDataDTO.hostpoolName = rdMgmtHostPool.hostPoolName;
                hostpoolDataDTO.friendlyName = rdMgmtHostPool.friendlyName;
                hostpoolDataDTO.description = rdMgmtHostPool.description;
                if (rdMgmtHostPool.persistent == false)
                    rdMgmtHostPool.loadBalancerType = rdMgmtHostPool.loadBalancerTypeName == "BreadthFirst" ? Convert.ToInt32(Enums.loadBalancer.BreadthFirst) : Convert.ToInt32(Enums.loadBalancer.DepthFirst);
                else
                    rdMgmtHostPool.loadBalancerType = Convert.ToInt32(Enums.loadBalancer.Persistent);

                //call rest api to create host pool -- july code bit
                var content = new StringContent(JsonConvert.SerializeObject(hostpoolDataDTO), Encoding.UTF8, "application/json");
                HttpResponseMessage response = CommonBL.InitializeHttpClient(deploymentUrl, accessToken).PostAsync("/RdsManagement/V1/TenantGroups/"+ rdMgmtHostPool.tenantGroupName + "/Tenants/" + rdMgmtHostPool.tenantName + "/HostPools/" + rdMgmtHostPool.hostPoolName, content).Result;
                string strJson = response.Content.ReadAsStringAsync().Result;
                if (response.IsSuccessStatusCode)
                {
                    if (response.StatusCode.ToString().ToLower() == "created")
                    {
                        poolResult.isSuccess = true;
                        poolResult.message = "Hostpool '"+ rdMgmtHostPool.hostPoolName + "' has been created successfully.";
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(strJson))
                    {
                        poolResult.isSuccess = false;
                        poolResult.message = CommonBL.GetErrorMessage(strJson);
                    }
                    else {
                        poolResult.isSuccess = false;
                        poolResult.message = "Hostpool '"+ rdMgmtHostPool .hostPoolName+ "' has not been created. Please try again later. ";
                    }
                }
            }
            catch (Exception ex)
            {
                poolResult.isSuccess = false;
                poolResult.message = "Hostpool '" + rdMgmtHostPool.hostPoolName + "' has not been created."+ex.Message.ToString()+" Please try again later. ";
            }
            return poolResult;
        }

        /// <summary>
        /// Description : Update hostpool details associated with tenant
        /// </summary>
        /// <param name="deploymenturl">RD Broker Url</param>
        /// <param name="accessToken"> Access Token</param>
        /// <param name="rdMgmtHostPool">Hostpool Class</param>
        /// <returns></returns>
        public HostPoolResult UpdateHostPool(string deploymentUrl, string accessToken, RdMgmtHostPool rdMgmtHostPool)
        {
            try
            {
                //assigning values to Host pool class
                HostPoolDataDTO hostpoolDataDTO = new HostPoolDataDTO();
                hostpoolDataDTO.tenantName = rdMgmtHostPool.tenantName;
                hostpoolDataDTO.hostpoolName = rdMgmtHostPool.hostPoolName;
                hostpoolDataDTO.tenantGroupName = rdMgmtHostPool.tenantGroupName;
                if ((!string.IsNullOrEmpty(rdMgmtHostPool.diskPath)) && rdMgmtHostPool.enableUserProfileDisk == true)
                {
                    hostpoolDataDTO.diskPath = rdMgmtHostPool.diskPath;
                    hostpoolDataDTO.enableUserProfileDisk = rdMgmtHostPool.enableUserProfileDisk.ToString();
                }
                else
                {

                    hostpoolDataDTO.friendlyName = rdMgmtHostPool.friendlyName;
                    hostpoolDataDTO.description = rdMgmtHostPool.description;
                }

                //call rest api to update hostpool -- july code bit
                var content = new StringContent(JsonConvert.SerializeObject(hostpoolDataDTO), Encoding.UTF8, "application/json");
                HttpResponseMessage response = CommonBL.PatchAsync(deploymentUrl, accessToken, "/RdsManagement/V1/TenantGroups/" + rdMgmtHostPool.tenantGroupName + "/Tenants/" + hostpoolDataDTO.tenantName + "/HostPools/" + hostpoolDataDTO.hostpoolName, content).Result;
                string strJson = response.Content.ReadAsStringAsync().Result;
                if (response.IsSuccessStatusCode)
                {
                    poolResult.isSuccess = true;
                    poolResult.message = "Hostpool '"+ rdMgmtHostPool.hostPoolName+ "' has been updated successfully.";
                }
                else
                {
                    if (!string.IsNullOrEmpty(strJson))
                    {
                        poolResult.isSuccess = false;
                        poolResult.message = CommonBL.GetErrorMessage(strJson);
                    }
                    else
                    {
                        poolResult.isSuccess = false;
                        poolResult.message = "Hostpool '"+ rdMgmtHostPool.hostPoolName + "' has not been updated. Please try it later again.";
                    }
                }
            }
            catch (Exception ex)
            {
                poolResult.isSuccess = false;
                poolResult.message = "Hostpool '" + rdMgmtHostPool.hostPoolName + "' has not been updated."+ex.Message.ToString()+" Please try it later again.";
            }
            return poolResult;
        }
        /// <summary>
        /// Description-Delete HostPool from associated tenant
        /// </summary>
        /// <param name="deploymentUrl">RD Broker Url</param>
        /// <param name="accessToken">Access token</param>
        /// <param name="tenantName">Name of Tenant</param>
        /// <param name="hostPoolName">Name of Hostpool</param>
        /// <returns></returns>
        public HostPoolResult DeleteHostPool(string tenantGroupName,string deploymentUrl, string accessToken, string tenantName,string hostPoolName)
        {
            try
            {
                //call rest api to delete hostpool  -- july code bit
                HttpResponseMessage response = CommonBL.InitializeHttpClient(deploymentUrl, accessToken).DeleteAsync("/RdsManagement/V1/TenantGroups/" + tenantGroupName +  "/Tenants/" + tenantName + "/HostPools/" + hostPoolName).Result;
                string strJson = response.Content.ReadAsStringAsync().Result;
                if (response.IsSuccessStatusCode)
                {
                    poolResult.isSuccess = true;
                    poolResult.message = "Hostpool '"+ hostPoolName + "' has been deleted successfully.";
                }
                else
                {
                    if (!string.IsNullOrEmpty(strJson))
                    {
                        poolResult.isSuccess = false;
                        poolResult.message = CommonBL.GetErrorMessage(strJson);
                    }
                    else
                    {
                        poolResult.isSuccess = false;
                        poolResult.message = "Hostpool '"+ hostPoolName + "' has not been deleted. Please try again later.";
                    }
                }
            }
            catch (Exception ex)
            {
                poolResult.isSuccess = false;
                poolResult.message = "Hostpool '" + hostPoolName + "' has not been deleted."+ex.Message.ToString()+" Please try again later.";
            }
            return poolResult;
        }
    }
    #endregion

}
#endregion
