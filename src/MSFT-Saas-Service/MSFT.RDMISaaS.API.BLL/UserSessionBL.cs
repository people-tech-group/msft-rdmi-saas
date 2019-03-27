#region "Import Namespaces"
using MSFT.WVDSaaS.API.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
#endregion "Import Namespaces"

#region "MSFT.RDMISaaS.API.BLL"
namespace MSFT.WVDSaaS.API.BLL
{
    #region "UserSessionBL"
    public class UserSessionBL
    {
        // string tenantGroup = Constants.tenantGroupName;

        #region "Functions/Methods"
        /// <summary>
        /// Description - get list of user session
        /// </summary>
        /// <param name="deploymentUrl"></param>
        /// <param name="accessToken"></param>
        /// <param name="tenantName"></param>
        /// <param name="hostPoolName"></param>
        /// old parameters-- , bool isAll, int pageSize, string sortField, bool isDescending, int initialSkip, string lastEntry
        /// <returns></returns>
        public HttpResponseMessage GetListOfUserSessioons(string deploymentUrl, string accessToken, string tenantGroup, string tenantName, string hostPoolName)
        {
            try
            {
                HttpResponseMessage response = CommonBL.InitializeHttpClient(deploymentUrl, accessToken).GetAsync("/RdsManagement/V1/TenantGroups/" + tenantGroup + "/Tenants/" + tenantName + "/HostPools/" + hostPoolName + "/Sessions").Result;
                return response;
                //api call included pagination
                //response = CommonBL.InitializeHttpClient(deploymentUrl, accessToken).GetAsync("/RdsManagement/V1/TenantGroups/" + tenantGroup + "/Tenants/" + tenantName + "/HostPools/" + hostPoolName + "/Sessions?PageSize=" + pageSize + "&LastEntry=" + lastEntry + "&SortField=" + sortField + "&IsDescending=" + isDescending + "&InitialSkip=" + initialSkip).Result;
            }
            catch
            {
                return null;
            }
        }
        #endregion

    }
    #endregion "MSFT.RDMISaaS.API.BLL"

}
#endregion
