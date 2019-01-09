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
        /// <returns></returns>
        public List<RdMgmtUserSession> GetListOfUserSessioons(string deploymentUrl, string accessToken, string tenantGroup, string tenantName, string hostPoolName, bool isAll, int pageSize, string sortField, bool isDescending, int initialSkip, string lastEntry)
        {
            List<RdMgmtUserSession> rdMgmtUserSessions = new List<RdMgmtUserSession>();
            HttpResponseMessage response;
            try
            {
                if(isAll == true)
                {
                    response = CommonBL.InitializeHttpClient(deploymentUrl, accessToken).GetAsync("/RdsManagement/V1/TenantGroups/" + tenantGroup + "/Tenants/" + tenantName + "/HostPools/" + hostPoolName + "/Sessions").Result;

                }
                else
                {
                    response = CommonBL.InitializeHttpClient(deploymentUrl, accessToken).GetAsync("/RdsManagement/V1/TenantGroups/" + tenantGroup + "/Tenants/" + tenantName + "/HostPools/" + hostPoolName + "/Sessions?PageSize=" + pageSize + "&LastEntry=" + lastEntry + "&SortField=" + sortField + "&IsDescending=" + isDescending + "&InitialSkip=" + initialSkip).Result;

                }
                //call rest api to get all user sessions -- july code bit
                string strJson = response.Content.ReadAsStringAsync().Result;
                if (response.IsSuccessStatusCode)
                {
                    //Deserialize the string to JSON object
                    var jObj = (JArray)JsonConvert.DeserializeObject(strJson);
                    if(jObj.Count>0)
                    {
                        rdMgmtUserSessions = jObj.Select(item => new RdMgmtUserSession
                        {
                            tenantName = (string)item["tenantName"],
                            hostPoolName = (string)item["hostPoolName"],
                            sessionHostName = (string)item["sessionHostName"],
                            userPrincipalName = (string)item["userPrincipalName"],
                            sessionId = (int)item["sessionId"],
                            applicationType = (string)item["applicationType"]
                        }).ToList();
                    }
                }
            }
            catch 
            {
                return null;
            }

            return rdMgmtUserSessions;
        }
        #endregion

    }
    #endregion "MSFT.RDMISaaS.API.BLL"

}
#endregion
