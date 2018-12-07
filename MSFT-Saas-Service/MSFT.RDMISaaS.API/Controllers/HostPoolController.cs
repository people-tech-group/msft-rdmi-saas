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

    #region "HostPoolController"
    public class HostPoolController : ApiController
    {
        #region "Class level declaration"
        HostPoolBL hostPoolBL = new HostPoolBL();
        HostPoolResult poolResult = new HostPoolResult();
        Common.Common common = new Common.Common();
        Common.Configurations configurations= new Common.Configurations();
        string deploymentUrl = "";
        string invalidToken = Constants.invalidToken.ToString().ToLower();
        string invalidCode = Constants.invalidCode.ToString().ToLower();
        #endregion

        #region "Functions/Methods"

        /// <summary>
        /// Description - Gets a Rds HostPool associated with the Tenant  specified in the Rds context.
        /// </summary>
        /// <param name="tenantName">Name of Tenant</param>
        /// <param name="hostPoolName">Name of Hostpool</param>
        /// <param name="refresh_token">Refresh token to get access token</param>
        /// <returns></returns>
        public RdMgmtHostPool GetHostPoolDetails(string tenantGroupName,string tenantName, string hostPoolName, string refresh_token)
        {

            //get deployment url
            deploymentUrl = configurations.rdBrokerUrl;
            RdMgmtHostPool rdMgmtHostPool = new RdMgmtHostPool();
            try
            {
                if (!string.IsNullOrEmpty(refresh_token))
                {
                    string accessToken = "";
                    //get token value
                    accessToken = common.GetTokenValue(refresh_token);
                    if (!string.IsNullOrEmpty(accessToken) && accessToken.ToString().ToLower() != invalidToken && accessToken.ToString().ToLower() != invalidCode)
                    {
                        rdMgmtHostPool = hostPoolBL.GetHostPoolDetails(tenantGroupName,deploymentUrl, accessToken, tenantName, hostPoolName);
                    }
                    else
                    {
                        rdMgmtHostPool.code = Constants.invalidToken;
                    }
                }
            }
            catch 
            {
                return null;
            }
            return rdMgmtHostPool;
        }

        /// <summary>
        /// Description: Creates a new Rds HostPool within a Tenant specified in the Rds context. 
        /// </summary>
        /// <param name="rdMgmthostpool"> Hostpool Class </param>
        /// <returns></returns>
        public IHttpActionResult Post([FromBody] string tenantGroupName, RdMgmtHostPool rdMgmthostpool)
        {
            //get deployment url
            deploymentUrl = configurations.rdBrokerUrl;
            try
            {
                if (rdMgmthostpool != null)
                {
                    if (!string.IsNullOrEmpty(rdMgmthostpool.refresh_token))
                    {
                        string accessToken = "";
                        //get token value
                        accessToken = common.GetTokenValue(rdMgmthostpool.refresh_token);
                        if (!string.IsNullOrEmpty(accessToken) && accessToken.ToString().ToLower() != invalidToken && accessToken.ToString().ToLower() != invalidCode)
                        {
                            poolResult = hostPoolBL.CreateHostPool(tenantGroupName, deploymentUrl, accessToken, rdMgmthostpool);
                        }
                        else
                        {
                            poolResult.isSuccess = false;
                            poolResult.message = Constants.invalidToken;
                        }
                    }
                }
                else
                {
                    poolResult.isSuccess = false;
                    poolResult.message = Constants.invalidDataMessage;
                }
            }
            catch (Exception ex)
            {
                poolResult.isSuccess = false;
                poolResult.message = "Hostpool '"+ rdMgmthostpool.hostPoolName + "' has not been created." + ex.Message.ToString() + "Please try again later";
            }
            return Ok(poolResult);
        }

        /// <summary>
        /// Description: Removes a Rds HostPool associated with the Tenant specified in the Rds context.
        /// </summary>
        /// <param name="tenantName">Name of Tenant</param>
        /// <param name="hostPoolName">Name of hostpool</param>
        /// <param name="refresh_token">Refresh token to get access token</param>
        /// <returns></returns>
        public IHttpActionResult Delete([FromUri] string tenantGroupName, string tenantName, string hostPoolName, string refresh_token)
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
                        poolResult = hostPoolBL.DeleteHostPool(tenantGroupName, deploymentUrl, accessToken, tenantName, hostPoolName);
                    }
                    else
                    {
                        poolResult.isSuccess = false;
                        poolResult.message = Constants.invalidToken;
                    }
                }
                else
                {
                    poolResult.isSuccess = false;
                    poolResult.message = Constants.invalidDataMessage;
                }
            }
            catch (Exception ex)
            {
                poolResult.isSuccess = false;
                poolResult.message = "Hostpool '"+ hostPoolName + "' has not been deleted."+ex.Message.ToString()+" Please try again later.";
            }
            return Ok(poolResult);
        }

        /// <summary>
        /// Description : Gets a list of Rds HostPools associated with the Tenant specified in the Rds context.
        /// </summary>
        /// <param name="tenantName">Name of Tenant</param>
        /// <param name="refresh_token">Refresh token to get access token</param>
        /// <returns></returns>
        public List<RdMgmtHostPool> GetHostPoolList(string tenantGroupName, string tenantName, string refresh_token, int pageSize, string sortField, bool isDescending = false, int initialSkip = 0, string lastEntry = null)
        {
            //get deployment url
            deploymentUrl = configurations.rdBrokerUrl;
            List<RdMgmtHostPool> lsthostpool = new List<RdMgmtHostPool>();
            try
            {
                if (!string.IsNullOrEmpty(refresh_token))
                {
                    string accessToken = "";
                    //get token value
                    accessToken = common.GetTokenValue(refresh_token);
                    if (!string.IsNullOrEmpty(accessToken) && accessToken.ToString().ToLower() != invalidToken && accessToken.ToString().ToLower() != invalidCode)
                    {
                        lsthostpool = hostPoolBL.GetHostPoolList(tenantGroupName, deploymentUrl, accessToken, tenantName, false,false,  pageSize,  sortField,  isDescending ,  initialSkip,  lastEntry);
                    }
                    else
                    {
                        RdMgmtHostPool rdMgmtHostPool = new RdMgmtHostPool();
                        rdMgmtHostPool.code = Constants.invalidToken;
                        lsthostpool.Add(rdMgmtHostPool);
                    }
                }
            }
            catch 
            {
                return null;
            }
            return lsthostpool;
        }

        /// <summary>
        /// Description : Updates a Rds HostPools associated with the Tenant specified in the Rds context.
        /// </summary>
        /// <param name="rdMgmthostpool">Hostpool class </param>
        /// <returns></returns>
        public IHttpActionResult Put([FromBody] string tenantGroupName, RdMgmtHostPool rdMgmthostpool)
        {
            //get deployment url
            deploymentUrl = configurations.rdBrokerUrl;
            try
            {
                if (rdMgmthostpool != null)
                {
                    if (!string.IsNullOrEmpty(rdMgmthostpool.refresh_token))
                    {
                        string token = "";
                        //get token value
                        token = common.GetTokenValue(rdMgmthostpool.refresh_token);
                        if (!string.IsNullOrEmpty(token) && token.ToString().ToLower() != invalidToken && token.ToString().ToLower() != invalidCode)
                        {
                            poolResult = hostPoolBL.UpdateHostPool(tenantGroupName, deploymentUrl, token, rdMgmthostpool);
                        }
                        else
                        {
                            poolResult.isSuccess = false;
                            poolResult.message = Constants.invalidToken;
                        }
                    }
                    else
                    {
                        poolResult.isSuccess = false;
                        poolResult.message = Constants.invalidDataMessage;
                    }
                }
                else
                {
                    poolResult.isSuccess = false;
                    poolResult.message = Constants.invalidDataMessage;
                }

            }
            catch (Exception ex)
            {
                poolResult.isSuccess = false;
                poolResult.message = "Hostpool '"+ rdMgmthostpool .hostPoolName+ "' has not been updated."+ex.Message.ToString()+" Please try again later.";
            }
            return Ok(poolResult);
        }
        #endregion

    }
    #endregion

}
#endregion
