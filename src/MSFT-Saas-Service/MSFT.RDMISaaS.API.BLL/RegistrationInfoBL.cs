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
    #region "RegistrationInfoBL"
    public class RegistrationInfoBL
    {
        RegistrationInfoResult infoResult = new RegistrationInfoResult();
       // string tenantGroup = Constants.tenantGroupName;

        /// <summary>
        /// Description - Exports a Rds RegistrationInfo associated with the TenantGroup, Tenant and HostPool specified in the Rds context.
        /// </summary>
        /// <param name="deploymentUrl"></param>
        /// <param name="accessToken"></param>
        /// <param name="tenantName"></param>
        /// <param name="hostPoolName"></param>
        /// <returns></returns>
        public HttpResponseMessage GetRegistrationInfo(string tenantGroupName,string deploymentUrl, string accessToken, string tenantName, string hostPoolName)
        {
            //RdMgmtRegistrationInfo rdMgmtRegistrationInfo = new RdMgmtRegistrationInfo();
            try
            {
                //call rest api to get RegistrationInfo -- july code bit
                HttpResponseMessage response = CommonBL.InitializeHttpClient(deploymentUrl, accessToken).GetAsync("/RdsManagement/V1/TenantGroups/" + tenantGroupName + "/Tenants/" + tenantName + "/HostPools/" + hostPoolName + "/RegistrationInfos/actions/export").Result;
                return response;
                //string strJson = response.Content.ReadAsStringAsync().Result;
                //if (response.IsSuccessStatusCode)
                //{
                //    //Deserialize the string to JSON object
                //    rdMgmtRegistrationInfo = JsonConvert.DeserializeObject<RdMgmtRegistrationInfo>(strJson);
                //}

            }
            catch 
            {
                return null;
            }
            //return rdMgmtRegistrationInfo;
        }

        /// <summary>
        /// Description : Generate registration key to create host
        /// </summary>
        /// <param name="deploymentUrl"></param>
        /// <param name="accessToken"></param>
        /// <param name="rdMgmtRegistrationInfo"></param>
        /// <returns></returns>
        public RegistrationInfoResult CreateRegistrationInfo(string deploymentUrl, string accessToken, RdMgmtRegistrationInfo rdMgmtRegistrationInfo)
        {
            try
            {
                RegistrationInfoDTO registrationInfoDTO = new RegistrationInfoDTO();
                registrationInfoDTO.tenantName = rdMgmtRegistrationInfo.tenantName;
                registrationInfoDTO.hostPoolName = rdMgmtRegistrationInfo.hostPoolName;
                registrationInfoDTO.expirationTime = rdMgmtRegistrationInfo.expirationUtc;
                registrationInfoDTO.tenantGroupName = rdMgmtRegistrationInfo.tenantGroupName;

                //call rest api to generate registration key -- july code bit
                var content = new StringContent(JsonConvert.SerializeObject(registrationInfoDTO), Encoding.UTF8, "application/json");
                HttpResponseMessage response = CommonBL.InitializeHttpClient(deploymentUrl, accessToken).PostAsync("/RdsManagement/V1/TenantGroups/" + rdMgmtRegistrationInfo.tenantGroupName + "/Tenants/" + rdMgmtRegistrationInfo.tenantName + "/HostPools/" + rdMgmtRegistrationInfo.hostPoolName + "/RegistrationInfos/", content).Result;

                string strJson = response.Content.ReadAsStringAsync().Result;
                if (response.IsSuccessStatusCode)
                {
                    if (response.StatusCode.ToString().ToLower() == "created")
                    {
                        infoResult.isSuccess = true;
                        infoResult.message = "Registration Key has been generated for hostpool '" + rdMgmtRegistrationInfo.hostPoolName + "' successfully.";
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(strJson))
                    {
                        infoResult.isSuccess = false;
                        infoResult.message = CommonBL.GetErrorMessage(strJson);
                    }
                    else
                    {
                        infoResult.isSuccess = false;
                        infoResult.message = "Registration Key has not been generated. Please try again later.";
                    }
                }
            }
            catch (Exception ex)
            {
                infoResult.isSuccess = false;
                infoResult.message = "Registration Key has not been generated."+ex.Message.ToString()+" Please try it later again.";
            }
            return infoResult;
        }

        /// <summary>
        /// Description : Removes a Rds Registration key associated with the Tenant and HostPool specified in the Rds context
        /// </summary>
        /// <param name="deploymentUrl">RD broker Url</param>
        /// <param name="accessToken">Access Token</param>
        /// <param name="tenantName">Name of Tenant</param>
        /// <param name="hostPoolName">Name of hostpool</param>
        /// <returns></returns>
        public RegistrationInfoResult DeleteRegistrationInfo(string tenantGroupName,string deploymentUrl, string accessToken, string tenantName, string hostPoolName)
        {
            try
            {

                //call rest api to delete registration key  -- july code bit
                HttpResponseMessage response = CommonBL.InitializeHttpClient(deploymentUrl, accessToken).DeleteAsync("/RdsManagement/V1/TenantGroups/" + tenantGroupName + "/Tenants/" + tenantName + "/HostPools/" + hostPoolName + "/RegistrationInfos/").Result;
                string strJson = response.Content.ReadAsStringAsync().Result;
                if (response.IsSuccessStatusCode)
                {
                    infoResult.isSuccess = true;
                    infoResult.message = "Registration Key has been deleted successully.";
                }
                else
                {
                    if (!string.IsNullOrEmpty(strJson))
                    {
                        infoResult.isSuccess = false;
                        infoResult.message = CommonBL.GetErrorMessage(strJson);
                    }
                    else
                    {
                        infoResult.isSuccess = false;
                        infoResult.message = "Registration Key has not been deleted. Please try it later again.";
                    }
                }
            }
            catch (Exception ex)
            {
                infoResult.isSuccess = false;
                infoResult.message = "Registration Key has been deleted."+ex.Message.ToString()+" Please try again later.";
            }
            return infoResult;
        }
    }
    #endregion

}
#endregion

