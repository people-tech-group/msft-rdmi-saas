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
using MSFT.RDMISaaS.API.Common;
#endregion "Import Namespaces"

#region "MSFT.RDMISaaS.API.Controllers"
namespace MSFT.RDMISaaS.API.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    #region "TenantController"
    public class TenantController : ApiController
    {
        #region "Class level Declaration"
        TenantBL tenantBL = new TenantBL();
        TenantResult tenantResult = new TenantResult();
        Common.Common common = new Common.Common();
        Common.Configurations configurations = new Common.Configurations();
        string deploymentUrl = "";
        string invalidToken = Constants.invalidToken.ToString().ToLower();
        string invalidCode = Constants.invalidCode.ToString().ToLower();
        #endregion

        /// <summary>
        /// Description - Gets a specific Rds tenant
        /// </summary>
        /// <param name="tenantName">Name of tenant</param>
        /// <param name="refresh_token">Refresh token to get access token</param>
        /// <returns></returns>
        public RdMgmtTenant GetTenantDetails(string tenantGroupName,string tenantName, string refresh_token)
        {
            //get deployment url
            deploymentUrl = configurations.rdBrokerUrl;
            RdMgmtTenant rdMgmtTenant = new RdMgmtTenant();
            try
            {
                if (!string.IsNullOrEmpty(refresh_token))
                {
                    string accessToken = "";
                    //get token value
                    accessToken = common.GetTokenValue(refresh_token);
                    if (!string.IsNullOrEmpty(accessToken) && accessToken.ToString().ToLower() != invalidToken && accessToken.ToString().ToLower() != invalidCode)
                    {
                        rdMgmtTenant = tenantBL.GetTenantDetails(tenantGroupName,deploymentUrl, accessToken,tenantName);
                    }
                    else
                    {
                        rdMgmtTenant.code = Constants.invalidToken;
                    }
                }
            }
            catch 
            {
                return null;
            }
            return rdMgmtTenant;
        }

        /// <summary>
        /// Description - Gets a list of Rds tenants.
        /// </summary>
        /// <param name="refresh_token">effsh token to get access token</param>
        /// <returns></returns>
        public Tenants GetTenantList(string tenantGroupName, string refresh_token, int pageSize, string sortField, bool isDescending = false, int initialSkip = 0, string lastEntry = "")
        {
            //get deployment url
            deploymentUrl = configurations.rdBrokerUrl;
            Tenants tenants = new Tenants();
            try
            {
                if (!string.IsNullOrEmpty(refresh_token))
                {
                    string accessToken = "";
                    //get token value
                    accessToken = common.GetTokenValue(refresh_token);
                    if (!string.IsNullOrEmpty(accessToken) && accessToken.ToString().ToLower() != invalidToken && accessToken.ToString().ToLower() != invalidCode)
                    {
                        tenants = tenantBL.GetTenantList(tenantGroupName,deploymentUrl, accessToken, pageSize, sortField, isDescending, initialSkip, lastEntry);
                    }
                    else
                    {
                        List<RdMgmtTenant> rdMgmtTenants = new List<RdMgmtTenant>();
                        RdMgmtTenant rdMgmtTenant = new RdMgmtTenant();
                        rdMgmtTenant.code = Constants.invalidToken;
                        rdMgmtTenants.Add(rdMgmtTenant);
                        tenants.rdMgmtTenants = rdMgmtTenants;
                    }
                }
            }
            catch
            {
                return null;
            }
            return tenants;
        }

        /// <summary>
        /// Description : Creates a new Rds Tenant in the Rds context.
        /// </summary>
        /// <param name="rdMgmtTenant">Tenant class</param>
        /// <returns></returns>
        public IHttpActionResult Post([FromBody] string tenantGroupName, RdMgmtTenant rdMgmtTenant)
        {
            //get deployment url
            deploymentUrl = configurations.rdBrokerUrl;
            try
            {
                if (rdMgmtTenant != null)
                {
                    if (!string.IsNullOrEmpty(rdMgmtTenant.refresh_token))
                    {
                        string accessToken = "";
                        //get token value
                        accessToken = common.GetTokenValue(rdMgmtTenant.refresh_token);
                        if (!string.IsNullOrEmpty(accessToken) && accessToken.ToString().ToLower() != invalidToken && accessToken.ToString().ToLower() != invalidCode)
                        {
                            tenantResult = tenantBL.CreateTenant(tenantGroupName,deploymentUrl, accessToken, rdMgmtTenant);
                        }
                        else
                        {
                            tenantResult.isSuccess = false;
                            tenantResult.message = Constants.invalidToken;
                        }
                    }
                }
                else
                {
                    tenantResult.isSuccess = false;
                    tenantResult.message = Constants.invalidDataMessage;
                }

            }
            catch (Exception ex)
            {
                tenantResult.isSuccess = false;
                tenantResult.message = "Tenant '"+ rdMgmtTenant.tenantName + "' has not been created."+ex.Message.ToString()+ " Please try again later.";
            }
            return Ok(tenantResult);
        }

        /// <summary>
        /// Description : Updates properties for an existing Rds tenant
        /// </summary>
        /// <param name="rdMgmtTenant"></param>
        /// <returns></returns>
        public IHttpActionResult Put([FromBody] string tenantGroupName, RdMgmtTenant rdMgmtTenant)
        {
            //get deployment url
            deploymentUrl = configurations.rdBrokerUrl;
            try
            {
                if (rdMgmtTenant != null)
                {
                    if (!string.IsNullOrEmpty(rdMgmtTenant.refresh_token) && !string.IsNullOrEmpty(rdMgmtTenant.tenantName))
                    {
                        string accessToken = "";
                        //get token value
                        accessToken = common.GetTokenValue(rdMgmtTenant.refresh_token);
                        if (!string.IsNullOrEmpty(accessToken) && accessToken.ToString().ToLower() != invalidToken && accessToken.ToString().ToLower() != invalidCode)
                        {
                            tenantResult = tenantBL.UpdateTenant(tenantGroupName,deploymentUrl, accessToken, rdMgmtTenant);
                        }
                        else
                        {
                            tenantResult.isSuccess = false;
                            tenantResult.message = Constants.invalidToken;
                        }
                    }
                    else
                    {
                        tenantResult.isSuccess = false;
                        tenantResult.message = Constants.invalidDataMessage;
                    }
                }
                else
                {
                    tenantResult.isSuccess = false;
                    tenantResult.message = Constants.invalidDataMessage;
                }

            }
            catch (Exception ex)
            {
                tenantResult.isSuccess = false;
                tenantResult.message = "Tenant '"+ rdMgmtTenant.tenantName + "' has not been updated."+ex.Message.ToString()+"Please try again later.";
            }
            return Ok(tenantResult);
        }

        /// <summary>
        /// Description : Deletes a specific Rds tenant.
        /// </summary>
        /// <param name="tenantName"></param>
        /// <param name="refresh_token">Refrsh token to get access token</param>
        /// <returns></returns>
        public IHttpActionResult Delete([FromUri] string tenantGroupName,string tenantName, string refresh_token)
        {
            //get deployment url
            deploymentUrl = configurations.rdBrokerUrl;
            try
            {
                if (!string.IsNullOrEmpty(refresh_token) && !string.IsNullOrEmpty(tenantName))
                {
                    string accessToken = "";
                    //get token value
                    accessToken = common.GetTokenValue(refresh_token);
                    if (!string.IsNullOrEmpty(accessToken) && accessToken.ToString().ToLower() != invalidToken && accessToken.ToString().ToLower() != invalidCode)
                    {
                        tenantResult = tenantBL.DeleteTenant(tenantGroupName,deploymentUrl, accessToken, tenantName);
                    }
                    else
                    {
                        tenantResult.isSuccess = false;
                        tenantResult.message = Constants.invalidToken;
                    }
                }
                else
                {
                    tenantResult.isSuccess = false;
                    tenantResult.message = Constants.invalidDataMessage;
                }
            }
            catch (Exception ex)
            {
                tenantResult.isSuccess = false;
                tenantResult.message = "Tenant "+ tenantName + " has not been deleted."+ ex.Message.ToString() + " and try again later."; ;
            }
            return Ok(tenantResult);
        }
        public List<RdMgmtTenant> GetAllTenants(string tenantGroupName,string refresh_token)
        {
            //get deployment url
            deploymentUrl = configurations.rdBrokerUrl;
            List<RdMgmtTenant> rdMgmtTenants = new List<RdMgmtTenant>();

            try
            {
                if (!string.IsNullOrEmpty(refresh_token))
                {
                    string accessToken = "";
                    //get token value
                    accessToken = common.GetTokenValue(refresh_token);
                    if (!string.IsNullOrEmpty(accessToken) && accessToken.ToString().ToLower() != invalidToken && accessToken.ToString().ToLower() != invalidCode)
                    {
                        rdMgmtTenants = tenantBL.GetAllTenantList(tenantGroupName,deploymentUrl, accessToken);
                    }
                    else
                    {
                        RdMgmtTenant rdMgmtTenant = new RdMgmtTenant();
                        rdMgmtTenant.code = Constants.invalidToken;
                        rdMgmtTenants.Add(rdMgmtTenant);
                    }
                }
            }
            catch
            {
                return null;
            }
            return rdMgmtTenants;
        }
    }

    #endregion

}
#endregion
