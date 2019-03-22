#region " Import Namespaces"
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

    #region "Class - AppGroupController"
    public class AppGroupController : ApiController
    {
        #region "Class level declaration"
        AppGroupBL appGroupBL = new AppGroupBL();
        AppGroupResult groupResult = new AppGroupResult();
        Common.Common common = new Common.Common();
        Common.Configurations configurations = new Common.Configurations();
        string deploymentUrl = "";
        string invalidToken = Constants.invalidToken.ToString().ToLower();
        string invalidCode = Constants.invalidCode.ToString().ToLower();
        #endregion

        /// <summary>
        /// Description : Gets a user details from an AppGroup within Tenant and HostPool associated with the specified Rds context.
        /// </summary>
        /// <param name="userPrincipalName">  Login Id of AAD user  </param>
        /// <param name="tenantName"> Name of Tenant</param>
        /// <param name="hostPoolName"> Name of Hostpool </param>
        /// <param name="appGroupName">Name of App Group</param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetUserDetails(string tenantGroupName, string userPrincipalName, string tenantName, string hostPoolName, string appGroupName, string refresh_token)
        {
            //get deployment url
            deploymentUrl = configurations.rdBrokerUrl;
            // RdMgmtUser rdMgmtUser = new RdMgmtUser();
            try
            {
                if (!string.IsNullOrEmpty(refresh_token))
                {
                    string accessToken = "";
                    //get token value
                    accessToken = common.GetTokenValue(refresh_token);

                    if (!string.IsNullOrEmpty(accessToken) && accessToken.ToString().ToLower() != invalidToken && accessToken.ToString().ToLower() != invalidCode)
                    {
                        return appGroupBL.GetUserDetails(tenantGroupName, deploymentUrl, accessToken, tenantName, hostPoolName, appGroupName, userPrincipalName);
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.OK,
                           new JObject() { "code", Constants.invalidToken }
                        );
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
            // return rdMgmtUser;
        }

        /// <summary>
        /// Description: Gets a list of users from an AppGroup within a  Tenant, HostPool and AppGroup associated with the specified Rds context
        /// </summary>
        /// <param name="tenantName"> Name of Tenant</param>
        /// <param name="hostPoolName"> Name of hostpool</param>
        /// <param name="appGroupName"> name of app group</param>
        /// // --old parameters for pagination - , int pageSize, string sortField, bool isDescending = false, int initialSkip = 0, string lastEntry = null
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetUsersList(string tenantGroupName, string tenantName, string hostPoolName, string appGroupName, string refresh_token)
        {
            //get deployment url
            deploymentUrl = configurations.rdBrokerUrl;
            //List<RdMgmtUser> rdMgmtUsers = new List<RdMgmtUser>();
            try
            {
                if (!string.IsNullOrEmpty(refresh_token))
                {
                    string accessToken = "";
                    //get token value
                    accessToken = common.GetTokenValue(refresh_token);
                    if (!string.IsNullOrEmpty(accessToken) && accessToken.ToString().ToLower() != invalidToken && accessToken.ToString().ToLower() != invalidCode)
                    {
                        return appGroupBL.GetUsersList(tenantGroupName, deploymentUrl, accessToken, tenantName, hostPoolName, appGroupName);
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new JArray()
                        {
                           new JObject() {"code",  Constants.invalidToken}
                        });
                    }
                }
                else
                { return null; }
            }
            catch
            {
                return null;
            }
            // return rdMgmtUsers;
        }

        /// <summary>
        /// Description : Gets an AppGroup details within a  Tenant, and HostPool associated with the specified Rds context.
        /// </summary>
        /// <param name="tenantName">Name of Tenant</param>
        /// <param name="hostPoolName">Name of hostpool</param>
        /// <param name="appGroupName">Name of app group</param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetAppGroupDetails(string tenantGroupName, string tenantName, string hostPoolName, string appGroupName, string refresh_token)
        {
            //get deployment url
            deploymentUrl = configurations.rdBrokerUrl;
            RdMgmtAppGroup rdMgmtAppGroup = new RdMgmtAppGroup();
            try
            {
                if (!string.IsNullOrEmpty(refresh_token) && !string.IsNullOrEmpty(tenantName))
                {
                    string accessToken = "";
                    //get token value
                    accessToken = common.GetTokenValue(refresh_token);
                    if (!string.IsNullOrEmpty(accessToken) && accessToken.ToString().ToLower() != invalidToken && accessToken.ToString().ToLower() != invalidCode)
                    {
                        return appGroupBL.GetAppGroupDetails(tenantGroupName, deploymentUrl, accessToken, tenantName, hostPoolName, appGroupName);
                        // rdMgmtAppGroup = appGroupBL.GetAppGroupDetails(tenantGroupName,deploymentUrl, accessToken, tenantName, hostPoolName, appGroupName);
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.OK,
                           new JObject() { "code", Constants.invalidToken }
                        );
                        //  rdMgmtAppGroup.code = Constants.invalidToken;
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
            //return rdMgmtAppGroup;
        }

        /// <summary>
        /// Description : Gets a list of AppGroups within  Tenant, and HostPool associated with the specified Rds context
        /// </summary>
        /// <param name="tenantName">Name of Tenant</param>
        /// <param name="hostPoolName"> Name of Hostpool</param>
        /// //old parameters -- int pageSize, string sortField, bool isDescending = false, int initialSkip = 0, string lastEntry = null
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetAppGroupsList(string tenantGroupName, string tenantName, string hostPoolName, string refresh_token)
        {
            //get deployment url
            deploymentUrl = configurations.rdBrokerUrl;
            // List<RdMgmtAppGroup> rdMgmtAppGroups = new List<RdMgmtAppGroup>();
            try
            {
                if (!string.IsNullOrEmpty(refresh_token))
                {
                    string accessToken = "";
                    //get token value
                    accessToken = common.GetTokenValue(refresh_token);
                    if (!string.IsNullOrEmpty(accessToken) && accessToken.ToString().ToLower() != invalidToken && accessToken.ToString().ToLower() != invalidCode)
                    {
                        return appGroupBL.GetAppGroupsList(tenantGroupName, deploymentUrl, accessToken, tenantName, hostPoolName);
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new JArray() { new JObject() { { "code", Constants.invalidToken } } });
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
            //return rdMgmtAppGroups;
        }

        /// <summary>
        /// Description : Gets a list of StartMenuApps within  Tenant, and HostPool associated with the specified Rds context.
        /// </summary>
        /// <param name="tenantName"> Name of Tenant</param>
        /// <param name="appGroupName">Name of app group</param>
        /// <param name="hostPoolName">Name of host pool</param>
        /// old parameters for pagination --  int pageSize, string sortField, bool isDescending = false, int initialSkip = 0, string lastEntry = null
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetStartMenuAppsList(string tenantGroupName, string tenantName, string appGroupName, string hostPoolName, string refresh_token)
        {
            //get deployment url
            deploymentUrl = configurations.rdBrokerUrl;
            List<RdMgmtStartMenuApp> rdMgmtStartMenuApps = new List<RdMgmtStartMenuApp>();
            try
            {
                if (!string.IsNullOrEmpty(refresh_token))
                {
                    string accessToken = "";
                    //get token value
                    accessToken = common.GetTokenValue(refresh_token);
                    if (!string.IsNullOrEmpty(accessToken) && accessToken.ToString().ToLower() != invalidToken && accessToken.ToString().ToLower() != invalidCode)
                    {
                        return appGroupBL.GetStartMenuAppsList(tenantGroupName, deploymentUrl, accessToken, tenantName, hostPoolName, appGroupName);
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new JArray() { new JObject() { { "code", Constants.invalidToken } } });

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
        /// Description : Creates an AppGroup within Tenant, and HostPool associated with the specified Rds context.
        /// </summary>
        /// <param name="rdMgmtAppGroup">Remote app group class </param>
        /// <returns></returns>
        public IHttpActionResult Post([FromBody] RdMgmtAppGroup rdMgmtAppGroup)
        {
            //get deployment url
            deploymentUrl = configurations.rdBrokerUrl;
            try
            {
                if (rdMgmtAppGroup != null)
                {
                    if (!string.IsNullOrEmpty(rdMgmtAppGroup.refresh_token))
                    {
                        string accessToken = "";
                        //get token value
                        accessToken = common.GetTokenValue(rdMgmtAppGroup.refresh_token);
                        if (!string.IsNullOrEmpty(accessToken) && accessToken.ToString().ToLower() != invalidToken && accessToken.ToString().ToLower() != invalidCode)
                        {
                            groupResult = appGroupBL.CreateAppGroup(deploymentUrl, accessToken, rdMgmtAppGroup);
                        }
                        else
                        {
                            groupResult.isSuccess = false;
                            groupResult.message = Constants.invalidToken;
                        }
                    }
                    else
                    {
                        groupResult.isSuccess = false;
                        groupResult.message = Constants.invalidDataMessage;
                    }
                }
                else
                {
                    groupResult.isSuccess = false;
                    groupResult.message = Constants.invalidDataMessage;
                }

            }
            catch (Exception ex)
            {
                groupResult.isSuccess = false;
                groupResult.message = "AppGroup '" + rdMgmtAppGroup.appGroupName + "' has not been created." + ex.Message.ToString() + " Please try again later.";
            }
            return Ok(groupResult);
        }

        /// <summary>
        /// Description : Updates an AppGroup within a Tenant, and HostPool associated with the specified Rds context.
        /// </summary>
        /// <param name="rdMgmtAppGroup"> Remote app group class </param>
        /// <returns></returns>
        public IHttpActionResult Put([FromBody] RdMgmtAppGroup rdMgmtAppGroup)
        {
            //get deployment url
            deploymentUrl = configurations.rdBrokerUrl;
            try
            {
                if (rdMgmtAppGroup != null)
                {
                    if (!string.IsNullOrEmpty(rdMgmtAppGroup.refresh_token))
                    {
                        string token = "";
                        //get token value
                        token = common.GetTokenValue(rdMgmtAppGroup.refresh_token);
                        if (!string.IsNullOrEmpty(token) && token.ToString().ToLower() != invalidToken && token.ToString().ToLower() != invalidCode)
                        {
                            groupResult = appGroupBL.UpdateAppGroup(deploymentUrl, token, rdMgmtAppGroup);
                        }
                        else
                        {
                            groupResult.isSuccess = false;
                            groupResult.message = Constants.invalidToken;
                        }
                    }
                    else
                    {
                        groupResult.isSuccess = false;
                        groupResult.message = Constants.invalidDataMessage;
                    }
                }
                else
                {
                    groupResult.isSuccess = false;
                    groupResult.message = Constants.invalidDataMessage;
                }

            }
            catch (Exception ex)
            {
                groupResult.isSuccess = false;
                groupResult.message = "AppGroup '" + rdMgmtAppGroup.appGroupName + "' has not been updated ." + ex.Message.ToString() + " Please try again later.";
            }
            return Ok(groupResult);
        }

        /// <summary>
        /// Description : Deletes an AppGroup within Tenant, and HostPool associated with the specified Rds context.
        /// </summary>
        /// <param name="tenantName"> Name of Tenant</param>
        /// <param name="hostpoolName">Name of hostpool</param>
        /// <param name="appgroupName">Name of App group</param>
        /// <returns></returns>
        public IHttpActionResult Delete([FromUri] string tenantGroupName, string tenantName, string hostpoolName, string appgroupName, string refresh_token)
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
                        groupResult = appGroupBL.DeleteAppGroup(tenantGroupName, deploymentUrl, accessToken, tenantName, hostpoolName, appgroupName);
                    }
                    else
                    {
                        groupResult.isSuccess = false;
                        groupResult.message = Constants.invalidToken;
                    }
                }
                else
                {
                    groupResult.isSuccess = false;
                    groupResult.message = Constants.invalidDataMessage;
                }
            }
            catch (Exception ex)
            {
                groupResult.isSuccess = false;
                groupResult.message = "App group '" + appgroupName + "' has not been deleted." + ex.Message.ToString() + " Please try again later."; ;
            }
            return Ok(groupResult);
        }

        /// <summary>
        /// Description : Adds a user to an AppGroup within Tenant and HostPool associated with the specified Rds context.
        /// </summary>
        /// <param name="appGroupUser"> Object of App group user class </param>
        /// <returns></returns>
        public IHttpActionResult PostUsers([FromBody] RdMgmtUser rdMgmtUser)
        {
            //get deployment url
            deploymentUrl = configurations.rdBrokerUrl;
            try
            {
                if (rdMgmtUser != null)
                {
                    if (!string.IsNullOrEmpty(rdMgmtUser.refresh_token))
                    {
                        string accessToken = "";
                        //get token value
                        accessToken = common.GetTokenValue(rdMgmtUser.refresh_token);
                        if (!string.IsNullOrEmpty(accessToken) && accessToken.ToString().ToLower() != invalidToken && accessToken.ToString().ToLower() != invalidCode)
                        {
                            groupResult = appGroupBL.CreateAppGroupUser(deploymentUrl, accessToken, rdMgmtUser);
                        }
                        else
                        {
                            groupResult.isSuccess = false;
                            groupResult.message = Constants.invalidToken;
                        }
                    }
                    else
                    {
                        groupResult.isSuccess = false;
                        groupResult.message = Constants.invalidDataMessage;
                    }
                }
                else
                {
                    groupResult.isSuccess = false;
                    groupResult.message = Constants.invalidDataMessage;
                }

            }
            catch (Exception ex)
            {
                groupResult.isSuccess = false;
                groupResult.message = " User '" + rdMgmtUser.userPrincipalName + "' has not been added." + ex.Message.ToString() + " and try again later."; ;
            }
            return Ok(groupResult);
        }

        /// <summary>
        /// Description : Removes an user from an AppGroup within a Tenant, HostPool and AppGroup associated with the specified Rds context
        /// </summary>
        /// <param name="tenantName"></param>
        /// <param name="hostPoolName"></param>
        /// <param name="appGroupName"></param>
        /// <param name="appGroupUser"></param>
        /// <returns></returns>
        public IHttpActionResult DeleteAssignedUser([FromUri]string tenantGroupName, string tenantName, string hostPoolName, string appGroupName, string appGroupUser, string refresh_token)
        {
            //get deployment url
            deploymentUrl = configurations.rdBrokerUrl;
            try
            {
                if (!string.IsNullOrEmpty(refresh_token))
                {
                    string token = "";
                    //get token value
                    token = common.GetTokenValue(refresh_token);
                    if (!string.IsNullOrEmpty(token) && token.ToString().ToLower() != invalidToken && token.ToString().ToLower() != invalidCode)
                    {
                        groupResult = appGroupBL.DeleteAppGroupUser(tenantGroupName, deploymentUrl, token, tenantName, hostPoolName, appGroupName, appGroupUser);
                    }
                    else
                    {
                        groupResult.isSuccess = false;
                        groupResult.message = Constants.invalidToken;
                    }
                }
            }
            catch (Exception ex)
            {
                groupResult.isSuccess = false;
                groupResult.message = "User '" + appGroupUser + "' has not been removed." + ex.Message.ToString() + " and try again later."; ;
            }
            return Ok(groupResult);
        }
    }
    #endregion  "Class - AppGroupController"
}
#endregion "MSFT.RDMISaaS.API.Controllers" 