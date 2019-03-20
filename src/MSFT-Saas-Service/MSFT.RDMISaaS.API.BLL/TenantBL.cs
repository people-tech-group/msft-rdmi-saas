#region "Import Namespaces"
using MSFT.RDMISaaS.API.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Net.Http.Headers;
#endregion "Import Namespaces"

#region "MSFT.RDMISaaS.API.BLL"
namespace MSFT.RDMISaaS.API.BLL
{
    public class TenantBL
    {
        TenantResult tenantResult = new TenantResult();
      // string tenantGroup = Constants.tenantGroupName;

        /// <summary>
        /// Description-Gets a specific Rds tenant.
        /// </summary>
        /// <param name="deploymentUrl">RD Broker url</param>
        /// <param name="accessToken">Access Token</param>
        /// <param name="tenantName">Anme of Tenant</param>
        /// <returns></returns>
        public JObject GetTenantDetails(string tenantGroupName, string deploymentUrl, string accessToken , string tenantName)
        {
            //RdMgmtTenant rdMgmtTenant = new RdMgmtTenant();
            try
            {
                //call rest api to get tenant details -- july code bit
                HttpResponseMessage response = CommonBL.InitializeHttpClient(deploymentUrl, accessToken).GetAsync("/RdsManagement/V1/TenantGroups/"+ tenantGroupName + "/Tenants/" + tenantName).Result;
                string strJson = response.Content.ReadAsStringAsync().Result;
                if (response.IsSuccessStatusCode)
                {
                    var jObj = (JObject)JsonConvert.DeserializeObject(strJson);
                    return jObj;
                   // rdMgmtTenant = JsonConvert.DeserializeObject<RdMgmtTenant>(strJson);
                }
                else
                {
                    return null;
                }
                
                //if(!string.IsNullOrEmpty(rdMgmtTenant.tenantName))
                //{
                //    //count hostpools associated with tenant
                //    HostPoolBL hostPoolBL = new HostPoolBL();
                //   // List<RdMgmtHostPool> hostPoolDataDTOs = hostPoolBL.GetHostPoolList(tenantGroupName,deploymentUrl, accessToken, rdMgmtTenant.tenantName,true,true,0,"",false,0,"");
                //   JArray hostPoolDataDTOs = hostPoolBL.GetHostPoolList(tenantGroupName,deploymentUrl, accessToken, rdMgmtTenant.tenantName,true,true,0,"",false,0,"");
                //    rdMgmtTenant.noOfHostpool = hostPoolDataDTOs.Count;

                //   //if(hostPoolDataDTOs.Count > 0)
                //   // {
                //   //     for (int i = 0; i < hostPoolDataDTOs.Count; i++)
                //   //     {
                //   //         rdMgmtTenant.noOfActivehosts = rdMgmtTenant.noOfActivehosts+ hostPoolDataDTOs[i].noOfActivehosts;
                //   //         rdMgmtTenant.noOfAppgroups = rdMgmtTenant.noOfAppgroups + hostPoolDataDTOs[i].noOfAppgroups;
                //   //         rdMgmtTenant.noOfSessions = rdMgmtTenant.noOfSessions + hostPoolDataDTOs[i].noOfSessions;
                //   //         rdMgmtTenant.noOfUsers = rdMgmtTenant.noOfUsers + hostPoolDataDTOs[i].noOfUsers;
                //   //     }
                //   // }
                //}
            }
            catch 
            {
                return null;
            }
           //return rdMgmtTenant;
        }
      
        /// <summary>
        /// Description-Gets a list of Rds tenants.
        /// </summary>
        /// <param name="deploymentUrl">RD Broker Url</param>
        /// <param name="accessToken"> Access token</param>
        /// <returns></returns>
        public JArray GetTenantList(string tenantGroupName,string deploymenturl, string accessToken,int pageSize, string sortField, bool isDescending, int initialSkip, string lastEntry)
        {
           // Tenants tenants = new Tenants();
           // List<RdMgmtTenant> lstTenants = new List<RdMgmtTenant>();
            
            try
            {
                //call rest api to get all tenants -- sept code bit
                HttpResponseMessage response = CommonBL.InitializeHttpClient(deploymenturl, accessToken).GetAsync("/RdsManagement/V1/TenantGroups/" + tenantGroupName + "/Tenants").Result;

                string strJson = response.Content.ReadAsStringAsync().Result;
                if (response.IsSuccessStatusCode)
                {
                    //Deserialize the string to JSON object
                    var jObj = (JArray)JsonConvert.DeserializeObject(strJson);
                    return jObj;
                    //if(jObj.Count>0)
                    //{
                    //    lstTenants = jObj.Select(item => new RdMgmtTenant
                    //    {
                    //        id = (string)item["id"],
                    //        tenantGroupName = (string)item["tenantGroupName"],
                    //        aadTenantId = (string)item["aadTenantId"],
                    //        tenantName = (string)item["tenantName"],
                    //        description = (string)item["description"],
                    //        friendlyName = (string)item["friendlyName"],
                    //        ssoAdfsAuthority = (string)item["ssoAdfsAuthority"],
                    //        ssoClientId = (string)item["ssoClientId"],
                    //        ssoClientSecret = (string)item["ssoClientId"],
                    //        azureSubscriptionId=(string)item["azureSubscriptionId"]
                    //    }).ToList();
                    //}
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                //lstTenants= null;
                return null;
            }
           // tenants.rdMgmtTenants = lstTenants;
            //return tenants;
        }

        //public Tenants GetTenantList_old(string tenantGroupName, string deploymenturl, string accessToken, int pageSize, string sortField, bool isDescending, int initialSkip, string lastEntry)
        //{
        //    Tenants tenants = new Tenants();
        //    List<RdMgmtTenant> lstTenants = new List<RdMgmtTenant>();

        //    try
        //    {
        //        // get all tennat list --count - septcode bit
        //        HttpResponseMessage responseTenants = CommonBL.InitializeHttpClient(deploymenturl, accessToken).GetAsync("/RdsManagement/V1/TenantGroups/" + tenantGroupName + "/Tenants").Result;
        //        if (responseTenants.IsSuccessStatusCode)
        //        {
        //            string responseData = responseTenants.Content.ReadAsStringAsync().Result;

        //            var jObj = (JArray)JsonConvert.DeserializeObject(responseData);
        //            tenants.count = jObj.Count;
        //        }

        //        //call rest api to get all tenants -- sept code bit
        //        HttpResponseMessage response = CommonBL.InitializeHttpClient(deploymenturl, accessToken).GetAsync("/RdsManagement/V1/TenantGroups/" + tenantGroupName + "/Tenants").Result;

        //        //HttpResponseMessage response = CommonBL.InitializeHttpClient(deploymenturl, accessToken).GetAsync("/RdsManagement/V1/TenantGroups/" + tenantGroupName + "/Tenants?PageSize="+ pageSize + "&LastEntry="+ lastEntry + "&SortField="+ sortField + "&IsDescending="+ isDescending + "&InitialSkip="+ initialSkip).Result;
        //        var headers = response.Headers;

        //        if (headers.Contains("next"))
        //        {
        //            string defaultPhrase = "/RdsManagement/V1/TenantGroups/Default Tenant Group/Tenants";
        //            string nextstring = headers.GetValues("next").First().ToString();
        //            nextstring = nextstring.Substring(defaultPhrase.Length);
        //            //nextstring.QueryString["LastEntry"]
        //            tenants.lastEntry = nextstring;

        //        }


        //        string strJson = response.Content.ReadAsStringAsync().Result;
        //        if (response.IsSuccessStatusCode)
        //        {
        //            //Deserialize the string to JSON object
        //            var jObj = (JArray)JsonConvert.DeserializeObject(strJson);
        //            if (jObj.Count > 0)
        //            {
        //                lstTenants = jObj.Select(item => new RdMgmtTenant
        //                {
        //                    id = (string)item["id"],
        //                    tenantGroupName = (string)item["tenantGroupName"],
        //                    aadTenantId = (string)item["aadTenantId"],
        //                    tenantName = (string)item["tenantName"],
        //                    description = (string)item["description"],
        //                    friendlyName = (string)item["friendlyName"],
        //                    ssoAdfsAuthority = (string)item["ssoAdfsAuthority"],
        //                    ssoClientId = (string)item["ssoClientId"],
        //                    ssoClientSecret = (string)item["ssoClientId"]
        //                }).ToList();
        //            }

        //            if (lstTenants.Count > 0)
        //            {
        //                for (int i = 0; i < lstTenants.Count; i++)
        //                {
        //                    //get list of host pool
        //                    HostPoolBL hostPoolBL = new HostPoolBL();
        //                    List<RdMgmtHostPool> rdMgmtHostPools = hostPoolBL.GetHostPoolList(tenantGroupName, deploymenturl, accessToken, lstTenants[i].tenantName, true, true, 0, "", false, 0, "");
        //                    lstTenants[i].noOfHostpool = rdMgmtHostPools.Count;

        //                    if (rdMgmtHostPools.Count > 0)
        //                    {
        //                        for (int j = 0; j < rdMgmtHostPools.Count; j++)
        //                        {
        //                            lstTenants[i].noOfActivehosts = lstTenants[i].noOfActivehosts + rdMgmtHostPools[j].noOfActivehosts;
        //                            lstTenants[i].noOfAppgroups = lstTenants[i].noOfAppgroups + rdMgmtHostPools[j].noOfAppgroups;
        //                            lstTenants[i].noOfSessions = lstTenants[i].noOfSessions + rdMgmtHostPools[j].noOfSessions;
        //                            lstTenants[i].noOfUsers = lstTenants[i].noOfUsers + rdMgmtHostPools[j].noOfUsers;
        //                        }
        //                    }
        //                }

        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        lstTenants = null;
        //    }
        //    tenants.rdMgmtTenants = lstTenants;
        //    return tenants;
        //}

        /// <summary>
        /// Description :  create a RDs tenant
        /// </summary>
        /// <param name="deploymenturl">RD Broker Url</param>
        /// <param name="accessToken">Aaccess Token</param>
        /// <param name="rdMgmtTenant">Tenant Class</param>
        /// <returns></returns>
        public TenantResult CreateTenant(string deploymenturl, string accessToken, RdMgmtTenant rdMgmtTenant)
        {
            try
            {
                TenantDataDTO tenantDataDTO = new TenantDataDTO();
                tenantDataDTO.tenantName = rdMgmtTenant.tenantName;
                tenantDataDTO.friendlyName = rdMgmtTenant.friendlyName;
                tenantDataDTO.description = rdMgmtTenant.description;               
                tenantDataDTO.aadTenantId = rdMgmtTenant.aadTenantId;
                tenantDataDTO.id=rdMgmtTenant.id;
                tenantDataDTO.tenantGroupName = rdMgmtTenant.tenantGroupName;

               


                //call rest api to create tenant-- july code bit
                var content = new StringContent(JsonConvert.SerializeObject(tenantDataDTO), Encoding.UTF8, "application/json");
                HttpResponseMessage response = CommonBL.InitializeHttpClient(deploymenturl, accessToken).PostAsync("/RdsManagement/V1/TenantGroups/"+ rdMgmtTenant.tenantGroupName + "/Tenants/" + rdMgmtTenant.tenantName,content).Result;
                string strJson = response.Content.ReadAsStringAsync().Result;
                if(response.IsSuccessStatusCode)
                {
                    if(response.StatusCode.ToString().ToLower()=="created" || response.StatusCode.ToString().ToLower()=="ok")
                    {
                        tenantResult.isSuccess = true;
                        tenantResult.message = "Tenant '"+tenantDataDTO.tenantName+ "' has been created successfully.";
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(strJson))
                    {
                        tenantResult.message = CommonBL.GetErrorMessage(strJson);
                        tenantResult.isSuccess = false;
                    }
                    else
                    {
                        tenantResult.isSuccess = false;
                        tenantResult.message = "Tenant '"+rdMgmtTenant.tenantName+"' has not been created. Please try it again later.";
                    }
                }
            }
            catch (Exception ex)
            {
                tenantResult.isSuccess = false;
                tenantResult.message = "Tenant '" + rdMgmtTenant.tenantName + "' has not been created."+ex.Message.ToString()+" and please try again later.";
            }
            return tenantResult;
        }

        /// <summary>
        /// Description : Update tenant information
        /// </summary>
        /// <param name="deploymenturl">RD Broker Url</param>
        /// <param name="accessToken">Access Token</param>
        /// <param name="rdMgmtTenant">Tenant Class</param>
        /// <returns></returns>
        public TenantResult UpdateTenant(string deploymenturl, string accessToken, RdMgmtTenant rdMgmtTenant)
        {
            try
            {
                TenantDataDTO tenantDataDTO = new TenantDataDTO();
                tenantDataDTO.tenantName = rdMgmtTenant.tenantName;
                tenantDataDTO.friendlyName = rdMgmtTenant.friendlyName;
                tenantDataDTO.description = rdMgmtTenant.description;
                tenantDataDTO.id = rdMgmtTenant.id;
                tenantDataDTO.aadTenantId = rdMgmtTenant.aadTenantId;
                tenantDataDTO.ssoAdfsAuthority = rdMgmtTenant.ssoAdfsAuthority;
                tenantDataDTO.ssoClientId = rdMgmtTenant.ssoClientId;
                tenantDataDTO.ssoClientSecret = rdMgmtTenant.ssoClientSecret;
                tenantDataDTO.tenantGroupName = rdMgmtTenant.tenantGroupName;


                //call rest api to update tenant -- july code bit
                var content = new StringContent(JsonConvert.SerializeObject(tenantDataDTO), Encoding.UTF8, "application/json");
                HttpResponseMessage response = CommonBL.PatchAsync(deploymenturl, accessToken, "/RdsManagement/V1/TenantGroups/"+ rdMgmtTenant.tenantGroupName + "/Tenants/" + tenantDataDTO.tenantName, content).Result;
                string strJson = response.Content.ReadAsStringAsync().Result;
                if (response.IsSuccessStatusCode)
                {
                    tenantResult.isSuccess = true;
                    tenantResult.message = "Tenant '"+ rdMgmtTenant.tenantName + "' has been updated successfully.";
                }
                else
                {
                    if (!string.IsNullOrEmpty(strJson))
                    {
                        tenantResult.isSuccess = false;
                        tenantResult.message = CommonBL.GetErrorMessage(strJson);
                    }
                    else
                    {
                        tenantResult.isSuccess = false;
                        tenantResult.message = "Tenant '" + rdMgmtTenant.tenantName + "' has not been updated. Please try again later.";
                    }
                }
            }
            catch (Exception ex)
            {
                tenantResult.isSuccess = false;
                tenantResult.message = "Tenant " + rdMgmtTenant.tenantName + " has not been updated."+ex.Message.ToString()+" Please try again later.";
            }
            return tenantResult;
        }

        /// <summary>
        /// Description: Delete Tenant
        /// </summary>
        /// <param name="deploymenturl">RD Broker Url</param>
        /// <param name="accessToken">access Token</param>
        /// <param name="tenantName">tenantName</param>
        /// <returns></returns>
        public TenantResult DeleteTenant(string tenantGroupName,string deploymenturl, string accessToken, string tenantName)
        {
            try
            {
               
                //call rest api to delete tenant -- july code bit
                HttpResponseMessage response = CommonBL.InitializeHttpClient(deploymenturl, accessToken).DeleteAsync("/RdsManagement/V1/TenantGroups/"+ tenantGroupName + "/Tenants/" + tenantName).Result;
                string strJson = response.Content.ReadAsStringAsync().Result;
                if (response.IsSuccessStatusCode)
                {
                    tenantResult.isSuccess = true;
                    tenantResult.message = "Tenant '"+ tenantName + "' has been deleted successfully.";
                }
                else
                {
                    if (!string.IsNullOrEmpty(strJson))
                    {
                        tenantResult.isSuccess = false;
                        tenantResult.message = CommonBL.GetErrorMessage(strJson);
                    }
                    else
                    {
                        tenantResult.isSuccess = false;
                        tenantResult.message = "Tenant '" + tenantName + "' has not been deleted. Please try again later.";
                    }
                }
            }
            catch (Exception ex)
            {
                tenantResult.isSuccess = false;
                tenantResult.message = "Tenant '" + tenantName + "' has not been deleted."+ex.Message.ToString()+ " and try again later.";
            }
            return tenantResult;
        }

        public JArray GetAllTenantList(string tenantGroupName, string deploymenturl, string accessToken)
        {
            //List<RdMgmtTenant> lstTenants = new List<RdMgmtTenant>();
            try
            {
                //call rest api to get all tenants -- july code bit
                HttpResponseMessage response = CommonBL.InitializeHttpClient(deploymenturl, accessToken).GetAsync("/RdsManagement/V1/TenantGroups/" + tenantGroupName + "/Tenants").Result;
                string strJson = response.Content.ReadAsStringAsync().Result;
                if (response.IsSuccessStatusCode)
                {
                    //Deserialize the string to JSON object
                    var jObj = (JArray)JsonConvert.DeserializeObject(strJson);
                    return jObj;
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

        //public List<RdMgmtTenant> GetAllTenantList_old(string tenantGroupName,string deploymenturl, string accessToken)
        //{
        //    List<RdMgmtTenant> lstTenants = new List<RdMgmtTenant>();
        //    try
        //    {
        //        //call rest api to get all tenants -- july code bit
        //        HttpResponseMessage response = CommonBL.InitializeHttpClient(deploymenturl, accessToken).GetAsync("/RdsManagement/V1/TenantGroups/" + tenantGroupName + "/Tenants").Result;
        //        string strJson = response.Content.ReadAsStringAsync().Result;
        //        if (response.IsSuccessStatusCode)
        //        {
        //            //Deserialize the string to JSON object
        //            var jObj = (JArray)JsonConvert.DeserializeObject(strJson);
        //            if (jObj.Count > 0)
        //            {
        //                lstTenants = jObj.Select(item => new RdMgmtTenant
        //                {
        //                    id = (string)item["id"],
        //                    tenantGroupName = (string)item["tenantGroupName"],
        //                    aadTenantId = (string)item["aadTenantId"],
        //                    tenantName = (string)item["tenantName"],
        //                    description = (string)item["description"],
        //                    friendlyName = (string)item["friendlyName"],
        //                    ssoAdfsAuthority = (string)item["ssoAdfsAuthority"],
        //                    ssoClientId = (string)item["ssoClientId"],
        //                    ssoClientSecret = (string)item["ssoClientId"],
        //                    azureSubscriptionId = (string)item["azureSubscriptionId"]
        //                }).ToList();
        //            }
        //        }
        //    }
        //    catch
        //    {
        //        return null;
        //    }
        //    return lstTenants;
        //}
    }
}
#endregion "MSFT.RDMISaaS.API.BLL"

