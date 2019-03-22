#region "Import Namespaces"
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using MSFT.RDMISaaS.API.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Web;
#endregion "Import Namespaces" 

#region "MSFT.RDMISaaS.API.BLL"
namespace MSFT.RDMISaaS.API.BLL
{
    #region "class-AppGroupBL"
    public class AppGroupBL
    {
        AppGroupResult groupResult = new AppGroupResult();
        // string tenantGroup = Constants.tenantGroupName;


        /// <summary>
        /// Description : Gets a user details from an AppGroup within  Tenant and HostPool associated with the specified Rds context.
        /// </summary>
        /// <param name="deploymentUrl"> RD Broker Url </param>
        /// <param name="accessToken">Access Token</param>
        /// <param name="userPrincipalName"> Login ID of AAD User</param>
        /// <param name="tenantName">Name of Tenant</param>
        /// <param name="hostPoolName"> Name of Hostpool</param>
        /// <param name="appGroupName">Name of App group</param>
        /// <returns></returns>
        public HttpResponseMessage GetUserDetails(string tenantGroupName, string deploymentUrl, string accessToken, string tenantName, string hostPoolName, string appGroupName, string userPrincipalName)
        {
            //RdMgmtUser rdMgmtUser = new RdMgmtUser();
            try
            {
                //call rest api to get app group user details -- july code bit
                HttpResponseMessage response = CommonBL.InitializeHttpClient(deploymentUrl, accessToken).GetAsync("/RdsManagement/V1/TenantGroups/" + tenantGroupName + "/Tenants/" + tenantName + "/HostPools/" + hostPoolName + "/AppGroups/" + appGroupName + "/AssignedUsers/" + userPrincipalName).Result;
                return response;
                //string strJson = response.Content.ReadAsStringAsync().Result;
                //if (response.IsSuccessStatusCode)
                //{
                //    //Deserialize the string to JSON object
                //    rdMgmtUser = JsonConvert.DeserializeObject<RdMgmtUser>(strJson);
                //}

            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Description-Gets a list of users from an AppGroup 
        /// </summary>
        /// <param name="deploymentUrl">RD Broker Url</param>
        /// <param name="accessToken"> Access Token</param>
        /// <param name="userPrincipalName"> Login ID of AAD User</param>
        /// <param name="tenantName">Name of Tenant</param>
        /// <param name="hostPoolName">Name of Hostpool</param>
        /// <param name="appGroupName">Name of App Group</param>
        /// //old parametewrs for pagination - bool isUserNameOnly,bool isAll, int pageSize, string sortField, bool isDescending, int initialSkip, string lastEntry
        /// <returns></returns>
        public HttpResponseMessage GetUsersList(string tenantGroupName, string deploymentUrl, string accessToken, string tenantName, string hostPoolName, string appGroupName)
        {
            // List<RdMgmtUser> rdMgmtUsers = new List<RdMgmtUser>();
            HttpResponseMessage response;
            try
            {
                response = CommonBL.InitializeHttpClient(deploymentUrl, accessToken).GetAsync("/RdsManagement/V1/TenantGroups/" + tenantGroupName + "/Tenants/" + tenantName + "/HostPools/" + hostPoolName + "/AppGroups/" + appGroupName + "/AssignedUsers").Result;
                return response;


                //folllowing api call is included pagination
                //response = CommonBL.InitializeHttpClient(deploymentUrl, accessToken).GetAsync("/RdsManagement/V1/TenantGroups/" + tenantGroupName + "/Tenants/" + tenantName + "/HostPools/" + hostPoolName + "/AppGroups/" + appGroupName + "/AssignedUsers?PageSize=" + pageSize + "&LastEntry=" + lastEntry + "&SortField=" + sortField + "&IsDescending=" + isDescending + "&InitialSkip=" + initialSkip).Result;


                ////call rest api to get list of app groups users --july code bit
                //string strJson = response.Content.ReadAsStringAsync().Result;
                //if (response.IsSuccessStatusCode)
                //{
                //    //Deserialize the string to JSON object
                //    var jObj = (JArray)JsonConvert.DeserializeObject(strJson);
                //    if (jObj.Count > 0)
                //    {
                //        if (isUserNameOnly)
                //        {
                //            rdMgmtUsers = jObj.Select(item => new RdMgmtUser
                //            {
                //                userPrincipalName = (string)item["userPrincipalName"]
                //            }).ToList();
                //        }
                //        else
                //        {
                //            rdMgmtUsers = jObj.Select(item => new RdMgmtUser
                //            {
                //                userPrincipalName = (string)item["userPrincipalName"],
                //                tenantName = (string)item["tenantName"],
                //                hostPoolName = (string)item["hostPoolName"],
                //                appGroupName = (string)item["appGroupName"]
                //            }).ToList();
                //        }
                //    }

                //}
            }
            catch
            {
                return null;
            }
            // return rdMgmtUsers;
        }

        /// <summary>
        /// Description-Gets an AppGroupDetails within a Tenant and Hostpool
        /// </summary>
        /// <param name="deploymentUrl">Rd Broker Url</param>
        /// <param name="accessToken"> Access Token</param>
        /// <param name="tenantName">Name of Tenant</param>
        /// <param name="hostPoolName">Name of Hostpool</param>
        /// <param name="appGroupName">Name of App group</param>
        /// <returns></returns>
        public HttpResponseMessage GetAppGroupDetails(string tenantGroupName, string deploymentUrl, string accessToken, string tenantName, string hostPoolName, string appGroupName)
        {
            RdMgmtAppGroup rdMgmtAppGroup = new RdMgmtAppGroup();
            try
            {
                //call rest api to get  app groups details -- july code bit
                HttpResponseMessage response = CommonBL.InitializeHttpClient(deploymentUrl, accessToken).GetAsync("/RdsManagement/V1/TenantGroups/" + tenantGroupName + "/Tenants/" + tenantName + "/HostPools/" + hostPoolName + "/AppGroups/" + appGroupName).Result;
                return response;
                //string strJson = response.Content.ReadAsStringAsync().Result;
                //if (response.IsSuccessStatusCode)
                //{
                //    ////Deserialize the string to JSON object

                //    var jObj = (JObject)JsonConvert.DeserializeObject(strJson);
                //    return jObj;

                //    //rdMgmtAppGroup = JsonConvert.DeserializeObject<RdMgmtAppGroup>(strJson);

                //    //if (rdMgmtAppGroup.resourceType == "0")
                //    //{
                //    //    rdMgmtAppGroup.resourceType = "Remote App Group";
                //    //}
                //    //else if (rdMgmtAppGroup.resourceType == "1")
                //    //{
                //    //    rdMgmtAppGroup.resourceType = "Desktop App Group";
                //    //}

                //    ////get list of Apps associated with this app group
                //    //RemoteAppBL remoteAppBL = new RemoteAppBL();
                //    //List<RdMgmtRemoteApp> rdMgmtRemoteApps = remoteAppBL.GetRemoteAppList(tenantGroupName,deploymentUrl, accessToken, tenantName, hostPoolName, rdMgmtAppGroup.appGroupName, true,true, 0, "", false, 0, "");
                //    //rdMgmtAppGroup.noOfApps = rdMgmtRemoteApps.Count;

                //    ////get list of Users associated with this app group
                //    //List<RdMgmtUser> rdMgmtUsers = GetUsersList(tenantGroupName,deploymentUrl, accessToken, tenantName, hostPoolName, rdMgmtAppGroup.appGroupName, true,true, 0, "", false, 0, "");
                //    //rdMgmtAppGroup.noOfusers = rdMgmtUsers.Count;
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
            //return rdMgmtAppGroup;
        }

        /// <summary>
        /// Description - Gets a list of AppGroups within a Tenant and Hostpool 
        /// </summary>
        /// <param name="deploymentUrl">RD Broker Url</param>
        /// <param name="accessToken">Access token</param>
        /// <param name="tenantName">Name of Tenant</param>
        /// <param name="hostPoolName">Name of Hostpool</param>
        /// <param name="isAppGroupNameOnly">used to get Only App Group Name</param>
        /// // old parameters -- , bool isAppGroupNameOnly,bool isAll, int pageSize, string sortField, bool isDescending, int initialSkip, string lastEntry
        /// <returns></returns>
        public HttpResponseMessage GetAppGroupsList(string tenantGroupName, string deploymentUrl, string accessToken, string tenantName, string hostPoolName)
        {
            try
            {
                HttpResponseMessage response = CommonBL.InitializeHttpClient(deploymentUrl, accessToken).GetAsync("/RdsManagement/V1/TenantGroups/" + tenantGroupName + "/Tenants/" + tenantName + "/HostPools/" + hostPoolName + "/AppGroups").Result;
                return response;

                //folllowing api call is included pagination
                // response = CommonBL.InitializeHttpClient(deploymentUrl, accessToken).GetAsync("/RdsManagement/V1/TenantGroups/" + tenantGroupName + "/Tenants/" + tenantName + "/HostPools/" + hostPoolName + "/AppGroups?PageSize=" + pageSize + "&LastEntry=" + lastEntry + "&SortField=" + sortField + "&IsDescending=" + isDescending + "&InitialSkip=" + initialSkip).Result;


                //call rest api to get list of app groups -- july code bit
                // string strJson = response.Content.ReadAsStringAsync().Result;
                // if (response.IsSuccessStatusCode)
                // {
                ////Deserialize the string to JSON object
                // var jObj = (JArray)JsonConvert.DeserializeObject(strJson);
                //return jObj;
                //if (jObj.Count > 0)
                //{

                //    if (isAppGroupNameOnly)
                //    {
                //        rdMgmtAppGroups = jObj.Select(item => new RdMgmtAppGroup
                //        {
                //            appGroupName = (string)item["appGroupName"]
                //        }).ToList();
                //    }
                //    else
                //    {
                //        rdMgmtAppGroups = jObj.Select(item => new RdMgmtAppGroup
                //        {
                //            tenantName = (string)item["tenantName"],
                //            hostPoolName = (string)item["hostPoolName"],
                //            appGroupName = (string)item["appGroupName"],
                //            description = (string)item["description"],
                //            friendlyName = (string)item["friendlyName"],
                //            resourceType = item["resourceType"].ToString() == "0" ? "Remote App Group" : "Desktop App Group"
                //        }).ToList();
                //    }
                //}
                //}
                //else
                //{
                //    return null;
                //}

                //if (rdMgmtAppGroups.Count > 0)
                //{
                //    for (int i = 0; i < rdMgmtAppGroups.Count; i++)
                //    {
                //        //get list of Apps associated with App group
                //        RemoteAppBL remoteAppBL = new RemoteAppBL();
                //        List<RdMgmtRemoteApp> rdMgmtRemoteApps = remoteAppBL.GetRemoteAppList(tenantGroupName,deploymentUrl, accessToken, tenantName, hostPoolName, rdMgmtAppGroups[i].appGroupName, true,true, 0, "", false, 0, "");
                //        rdMgmtAppGroups[i].noOfApps = rdMgmtRemoteApps.Count;

                //        //get list of Users associsted with app group
                //        List<RdMgmtUser> rdMgmtUsers = GetUsersList(tenantGroupName,deploymentUrl, accessToken, tenantName, hostPoolName, rdMgmtAppGroups[i].appGroupName, true,true, 0, "", false, 0, "");
                //        rdMgmtAppGroups[i].noOfusers = rdMgmtUsers.Count;
                //    }
                //}
            }
            catch
            {
                return null;
            }
            // return rdMgmtAppGroups;
        }

        /// <summary>
        /// Description-Gets a list of StartMenuApps within a Teanat and Hostpool. 
        /// </summary>
        /// <param name="deploymentUrl">RD Broker Url</param>
        /// <param name="accessToken"> Access Token</param>
        /// <param name="tenantName"> Name of Tenant</param>
        /// <param name="hostPoolName">Name of Hostpool</param>
        /// <param name="appGroupName">Name of App group</param>
        /// old parameters --  int pageSize, string sortField, bool isDescending, int initialSkip, string lastEntry
        /// <returns></returns>
        public HttpResponseMessage GetStartMenuAppsList(string tenantGroupName, string deploymentUrl, string accessToken, string tenantName, string hostPoolName, string appGroupName)
        {
            List<RdMgmtStartMenuApp> rdMgmtStartMenuApps = new List<RdMgmtStartMenuApp>();
            try
            {
                //call rest api to get all startmenu apps in app group -- july code bit
                HttpResponseMessage response = CommonBL.InitializeHttpClient(deploymentUrl, accessToken).GetAsync("/RdsManagement/V1/TenantGroups/" + tenantGroupName + "/Tenants/" + tenantName + "/HostPools/" + hostPoolName + "/AppGroups/" + appGroupName + "/StartMenuApps").Result;

                //folllowing api call is included pagination
                //HttpResponseMessage response = CommonBL.InitializeHttpClient(deploymentUrl, accessToken).GetAsync("/RdsManagement/V1/TenantGroups/" + tenantGroupName + "/Tenants/" + tenantName + "/HostPools/" + hostPoolName + "/AppGroups/" + appGroupName + "/StartMenuApps?PageSize=" + pageSize + "&LastEntry=" + lastEntry + "&SortField=" + sortField + "&IsDescending=" + isDescending + "&InitialSkip=" + initialSkip).Result;
                return response;
                //string strJson = response.Content.ReadAsStringAsync().Result;
                //if (response.IsSuccessStatusCode)
                //{
                //    //Deserialize the string to JSON object
                //    var jObj = (JArray)JsonConvert.DeserializeObject(strJson);
                //    if (jObj.Count > 0)
                //    {
                //        rdMgmtStartMenuApps = jObj.Select(item => new RdMgmtStartMenuApp
                //        {
                //            hostPoolName = (string)item["hostPoolName"],
                //            tenantName = (string)item["tenantName"],
                //            appGroupName = (string)item["appGroupName"],
                //            appAlias = (string)item["appAlias"],
                //            friendlyName = (string)item["friendlyName"],
                //            filePath = (string)item["filePath"],
                //            commandLineArguments = (string)item["commandLineArguments"],
                //            iconPath = (string)item["iconPath"],
                //            iconIndex = (int)item["iconIndex"]
                //        }).ToList();
                //    }
                //}
            }
            catch
            {
                return null;
            }
            //  return rdMgmtStartMenuApps;
        }

        /// <summary>
        /// Description : Creates an AppGroup within Tenant, and HostPool associated with the specified Rds context.
        /// </summary>
        /// <param name="deploymentUrl">RD Broker Url</param>
        /// <param name="accessToken"> Access Token</param>
        /// <param name="rdMgmtAppGroup"> App Group Class </param>
        /// <returns></returns>
        public AppGroupResult CreateAppGroup(string deploymentUrl, string accessToken, RdMgmtAppGroup rdMgmtAppGroup)
        {
            try
            {
                AppGroupDTO appGroupDTO = new AppGroupDTO();
                appGroupDTO.tenantGroupName = rdMgmtAppGroup.tenantGroupName;
                appGroupDTO.appGroupName = rdMgmtAppGroup.appGroupName;
                appGroupDTO.description = rdMgmtAppGroup.description;
                appGroupDTO.friendlyName = rdMgmtAppGroup.friendlyName;
                appGroupDTO.hostPoolName = rdMgmtAppGroup.hostPoolName;
                appGroupDTO.resourceType = Convert.ToInt32(rdMgmtAppGroup.resourceType);
                appGroupDTO.tenantName = rdMgmtAppGroup.tenantName;


                //call rest service to create app group -- july code bit
                var content = new StringContent(JsonConvert.SerializeObject(appGroupDTO), Encoding.UTF8, "application/json");
                HttpResponseMessage response = CommonBL.InitializeHttpClient(deploymentUrl, accessToken).PostAsync("/RdsManagement/V1/TenantGroups/" + rdMgmtAppGroup.tenantGroupName + "/Tenants/" + appGroupDTO.tenantName + "/HostPools/" + appGroupDTO.hostPoolName + "/AppGroups/" + appGroupDTO.appGroupName, content).Result;
                string strJson = response.Content.ReadAsStringAsync().Result;
                if (response.IsSuccessStatusCode)
                {
                    groupResult.isSuccess = true;
                    groupResult.message = "App group '" + appGroupDTO.appGroupName + "' has been created successfully.";
                }
                else if ((int)response.StatusCode == 429)
                {
                    groupResult.isSuccess = false;
                    groupResult.message = strJson + " Please try again later.";
                }
                else
                {

                    if (!string.IsNullOrEmpty(strJson))
                    {
                        groupResult.isSuccess = false;
                        groupResult.message = CommonBL.GetErrorMessage(strJson);
                    }
                    else
                    {
                        groupResult.isSuccess = false;
                        groupResult.message = "AppGroup '" + rdMgmtAppGroup.appGroupName + "' has not been created. Please try it later again.";
                    }
                }
            }
            catch (Exception ex)
            {
                groupResult.isSuccess = false;
                groupResult.message = "AppGroup '" + rdMgmtAppGroup.appGroupName + "' has not been created." + ex.Message.ToString() + " Please try it later again.";
            }
            return groupResult;
        }

        /// <summary>
        /// Description : Updates an AppGroup within a Tenant, and HostPool associated with the specified Rds context.
        /// </summary>
        /// <param name="deploymentUrl">RD Broker Url</param>
        /// <param name="accessToken">Access Token</param>
        /// <param name="rdMgmtAppGroup">App Group Class </param>
        /// <returns></returns>
        public AppGroupResult UpdateAppGroup(string deploymentUrl, string accessToken, RdMgmtAppGroup rdMgmtAppGroup)
        {
            try
            {
                AppGroupDTO appGroupDTO = new AppGroupDTO();
                appGroupDTO.tenantGroupName = rdMgmtAppGroup.tenantGroupName;
                appGroupDTO.appGroupName = rdMgmtAppGroup.appGroupName;
                appGroupDTO.description = rdMgmtAppGroup.description;
                appGroupDTO.friendlyName = rdMgmtAppGroup.friendlyName;
                appGroupDTO.hostPoolName = rdMgmtAppGroup.hostPoolName;
                appGroupDTO.resourceType = Convert.ToInt32(rdMgmtAppGroup.resourceType);
                appGroupDTO.tenantName = rdMgmtAppGroup.tenantName;

                //call rest service to update app group details 
                var content = new StringContent(JsonConvert.SerializeObject(appGroupDTO), Encoding.UTF8, "application/json");
                HttpResponseMessage response = CommonBL.PatchAsync(deploymentUrl, accessToken, "/RdsManagement/V1/TenantGroups/" + rdMgmtAppGroup.tenantGroupName + "/Tenants/" + appGroupDTO.tenantName + "/HostPools/" + appGroupDTO.hostPoolName + "/AppGroups/" + appGroupDTO.appGroupName, content).Result;
                string strJson = response.Content.ReadAsStringAsync().Result;
                if (response.IsSuccessStatusCode)
                {
                    //Deserialize the string to JSON object
                    groupResult.isSuccess = true;
                    groupResult.message = "App group '" + appGroupDTO.appGroupName + "' has been updated successfully.";
                }
                else if ((int)response.StatusCode == 429)
                {
                    groupResult.isSuccess = false;
                    groupResult.message = strJson + " Please try again later.";
                }
                else
                {
                    if (!string.IsNullOrEmpty(strJson))
                    {
                        groupResult.isSuccess = false;
                        groupResult.message = CommonBL.GetErrorMessage(strJson);
                    }
                    else
                    {
                        groupResult.isSuccess = false;
                        groupResult.message = "AppGroup '" + rdMgmtAppGroup.appGroupName + "' has not been updated. Please try it later again.";
                    }
                }
            }
            catch (Exception ex)
            {
                groupResult.isSuccess = false;
                groupResult.message = "AppGroup '" + rdMgmtAppGroup.appGroupName + "' has not been updated." + ex.Message.ToString() + " Please try it later again.";
            }
            return groupResult;
        }

        /// <summary>
        /// Description : Removes an user from an AppGroup within a Tenant, HostPool and AppGroup associated with the specified Rds context
        /// </summary>
        /// <param name="deploymentURL">Rd Broker Url</param>
        /// <param name="accessToken"> Access token</param>
        /// <param name="tenantName">Name of Tenant</param>
        /// <param name="hostPoolName">Name of Hostpool </param>
        /// <param name="appGroupName">Name of App Group</param>
        /// <param name="appGroupUser">Login ID of AAD User</param>
        /// <returns></returns>
        public AppGroupResult DeleteAppGroupUser(string tenantGroupName, string deploymentUrl, string accessToken, string tenantName, string hostPoolName, string appGroupName, string appGroupUser)
        {
            try
            {
                //call rest service to remove user from App group
                HttpResponseMessage response = CommonBL.InitializeHttpClient(deploymentUrl, accessToken).DeleteAsync("/RdsManagement/V1/TenantGroups/" + tenantGroupName + "/Tenants/" + tenantName + "/HostPools/" + hostPoolName + "/AppGroups/" + appGroupName + "/AssignedUsers/" + appGroupUser).Result;
                string strJson = response.Content.ReadAsStringAsync().Result;
                if (response.IsSuccessStatusCode)
                {
                    groupResult.isSuccess = true;
                    groupResult.message = "User '" + appGroupUser + "' has been removed from app group " + appGroupName + " successfully.";
                }
                else if ((int)response.StatusCode == 429)
                {
                    groupResult.isSuccess = false;
                    groupResult.message = strJson + " Please try again later.";
                }
                else
                {
                    if (!string.IsNullOrEmpty(strJson))
                    {
                        groupResult.isSuccess = false;
                        groupResult.message = CommonBL.GetErrorMessage(strJson);
                    }
                    else
                    {
                        groupResult.isSuccess = false;
                        groupResult.message = "User '" + appGroupUser + "' has not been removed. Please try it later again.";
                    }
                }
            }
            catch (Exception ex)
            {
                groupResult.isSuccess = false;
                groupResult.message = "User '" + appGroupUser + "' has not been removed." + ex.Message.ToString() + " and try again later.";
            }
            return groupResult;
        }

        /// <summary>
        /// Description : Adds a user to an AppGroup within Tenant and HostPool associated with the specified Rds context.
        /// </summary>
        /// <param name="deploymentUrl">RD Broker Url</param>
        /// <param name="accessToken"> Access Token</param>
        /// <param name="rdMgmtUser"> App Group user claass</param>
        /// <returns></returns>
        public AppGroupResult CreateAppGroupUser(string deploymentUrl, string accessToken, RdMgmtUser rdMgmtUser)
        {
            try
            {
                AppGroupUserDTO appGroupUserDTO = new AppGroupUserDTO();
                appGroupUserDTO.appGroupName = rdMgmtUser.appGroupName;
                appGroupUserDTO.appGroupUser = rdMgmtUser.appGroupUser;
                appGroupUserDTO.hostPoolName = rdMgmtUser.hostPoolName;
                appGroupUserDTO.tenantName = rdMgmtUser.tenantName;
                appGroupUserDTO.userPrincipalName = rdMgmtUser.userPrincipalName;
                appGroupUserDTO.tenantGroupName = rdMgmtUser.tenantGroupName;


                //call rest service to add user to app group - july code bit
                var content = new StringContent(JsonConvert.SerializeObject(appGroupUserDTO), Encoding.UTF8, "application/json");
                HttpResponseMessage response = CommonBL.InitializeHttpClient(deploymentUrl, accessToken).PostAsync("/RdsManagement/V1/TenantGroups/" + rdMgmtUser.tenantGroupName + "/Tenants/" + appGroupUserDTO.tenantName + "/HostPools/" + appGroupUserDTO.hostPoolName + "/AppGroups/" + appGroupUserDTO.appGroupName + "/AssignedUsers/" + appGroupUserDTO.userPrincipalName, content).Result;
                string strJson = response.Content.ReadAsStringAsync().Result;
                if (response.IsSuccessStatusCode)
                {
                    groupResult.isSuccess = true;
                    groupResult.message = "User '" + rdMgmtUser.userPrincipalName + "' has been added to app group " + rdMgmtUser.appGroupName + " successfully.";
                }
                else if ((int)response.StatusCode == 429)
                {
                    groupResult.isSuccess = false;
                    groupResult.message = strJson + " Please try again later.";
                }
                else
                {
                    if (!string.IsNullOrEmpty(strJson))
                    {
                        groupResult.isSuccess = false;
                        groupResult.message = CommonBL.GetErrorMessage(strJson);
                    }
                    else
                    {
                        groupResult.isSuccess = false;
                        groupResult.message = "User '" + rdMgmtUser.userPrincipalName + "' has not been added to app group. Please try it later again. ";
                    }
                }
            }
            catch (Exception ex)
            {
                groupResult.isSuccess = false;
                groupResult.message = "User '" + rdMgmtUser.userPrincipalName + "' has not been added to app group " + ex.Message.ToString() + " Please try again later.";
            }
            return groupResult;
        }

        /// <summary>
        /// Description Description : Removes an user from an AppGroup within a Tenant, HostPool and AppGroup associated with the specified Rds context
        /// </summary>
        /// <param name="deploymentURL">RD Broker Url</param>
        /// <param name="accessToken">Access Token</param>
        /// <param name="tenantName">Name of tenant</param>
        /// <param name="hostpoolName">Name of Hostpool</param>
        /// <param name="appGroupName">Name of AppGroup</param>
        /// <returns></returns>
        public AppGroupResult DeleteAppGroup(string tenantGroupName, string deploymentURL, string accessToken, string tenantName, string hostpoolName, string appGroupName)
        {
            try
            {
                //call rest service to delete app group - july code bit
                HttpResponseMessage response = CommonBL.InitializeHttpClient(deploymentURL, accessToken).DeleteAsync("/RdsManagement/V1/TenantGroups/" + tenantGroupName + "/Tenants/" + tenantName + "/HostPools/" + hostpoolName + "/AppGroups/" + appGroupName).Result;
                string strJson = response.Content.ReadAsStringAsync().Result;
                if (response.IsSuccessStatusCode)
                {
                    groupResult.isSuccess = true;
                    groupResult.message = "App group '" + appGroupName + "' has been deleted successfully.";
                }
                else if ((int)response.StatusCode == 429)
                {
                    groupResult.isSuccess = false;
                    groupResult.message = strJson + " Please try again later.";
                }
                else
                {
                    if (!string.IsNullOrEmpty(strJson))
                    {
                        groupResult.isSuccess = false;
                        groupResult.message = CommonBL.GetErrorMessage(strJson);
                    }
                    else
                    {
                        groupResult.isSuccess = false;
                        groupResult.message = "AppGroup '" + appGroupName + "' has not been deleted . Please try it again later.";
                    }
                }
            }
            catch (Exception ex)
            {
                groupResult.isSuccess = false;
                groupResult.message = "AppGroup '" + appGroupName + "' has not been deleted." + ex.Message.ToString() + "Please try it again later.";
            }
            return groupResult;
        }
    }
    #endregion  "Class - AppGroupBL"
}
#endregion "MSFT.RDMISaaS.API.BLL" 