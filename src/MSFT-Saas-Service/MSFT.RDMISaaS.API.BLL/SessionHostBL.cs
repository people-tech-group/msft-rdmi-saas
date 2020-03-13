#region "Import Namespaces"
using MSFT.WVDSaaS.API.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
#endregion "Import Namespaces"

#region "MSFT.RDMISaaS.API.BLL"
namespace MSFT.WVDSaaS.API.BLL
{
    #region "SessionHostBL"
    public class SessionHostBL
    {
        JObject hostResult = new JObject();

        /// <summary>
        /// Description: Gets all Rds SessionHosts associated with the Tenant and Host Pool specified in the Rds context.
        /// </summary>
        /// <param name="deploymentUrl">RD Broker Url</param>
        /// <param name="accessToken">Access token</param>
        /// <param name="tenantName">Name of Tenant</param>
        /// <param name="hostPoolName">Name of hostpool</param>
        /// <param name="isSessionHostNameOnly">To Get only HostName</param>
        /// //old parameters --  bool isSessionHostNameOnly,bool isAll, int pageSize, string sortField, bool isDescending, int initialSkip, string lastEntry
        /// <returns></returns>
        public HttpResponseMessage GetWVDSessionhostList(string deploymentUrl, string accessToken, string tenantGroup, string tenantName, string hostPoolName)
        {
            try
            {
                HttpResponseMessage response = CommonBL.InitializeHttpClient(deploymentUrl, accessToken).GetAsync("/RdsManagement/V1/TenantGroups/" + tenantGroup + "/Tenants/" + tenantName + "/HostPools/" + hostPoolName + "/SessionHosts").Result;
                return response;

                //api call included pagination
                //    response = CommonBL.InitializeHttpClient(deploymentUrl, accessToken).GetAsync("/RdsManagement/V1/TenantGroups/" + tenantGroup + "/Tenants/" + tenantName + "/HostPools/" + hostPoolName + "/SessionHosts?PageSize=" + pageSize + "&LastEntry=" + lastEntry + "&SortField=" + sortField + "&IsDescending=" + isDescending + "&InitialSkip=" + initialSkip).Result;
            }
            catch
            {
                return null;
            }

        }
        public HttpResponseMessage GetAzureVMList(string deploymentUrl, string accessToken, string subscriptionId)
        {
            try
            {
                HttpResponseMessage response = CommonBL.InitializeHttpClient(deploymentUrl, accessToken).GetAsync("subscriptions/" + subscriptionId + "/providers/Microsoft.Compute/virtualMachines?api-version=2018-04-01").Result;
                return response;
            }
            catch
            {
                return null;
            }
        }
        public async Task<HttpResponseMessage> GetSessionhostList(string deploymentUrl, string wvdAccessToken, string tenantGroup, string tenantName, string hostPoolName, string azureDeployUrl = null, string azureAccessToken = null, string subscriptionId = null)
        {
            try
            {
                HttpResponseMessage WVDResponse = GetWVDSessionhostList(deploymentUrl, wvdAccessToken, tenantGroup, tenantName, hostPoolName);
                if (!string.IsNullOrEmpty(subscriptionId))
                {
                    if (WVDResponse.StatusCode == HttpStatusCode.OK)
                    {
                        var wvdData = WVDResponse.Content.ReadAsStringAsync().Result;
                        var arrWVD = (JArray)JsonConvert.DeserializeObject(wvdData);
                        var WVDresult = ((JArray)arrWVD).ToList();
                        var AzureResult = await GetAzureVMs(azureDeployUrl, azureAccessToken, subscriptionId);
                        if (AzureResult != null && AzureResult.Count > 0)
                        {
                            var finalVmList = AzureResult.ToList().Where(r => WVDresult.ToList().Any(d => r["vmName"].ToString() == d["sessionHostName"].ToString().Split('.')[0].ToString())).ToList();
                            finalVmList.ForEach(item =>
                            {
                                var element = WVDresult.ToList().FirstOrDefault(d => d["sessionHostName"].ToString().Split('.')[0].ToString() == item["vmName"].ToString());
                                item["sessionHostName"] = element["sessionHostName"];
                                item["tenantName"] = element["tenantName"];
                                item["tenantGroupName"] = element["tenantGroupName"];
                                item["hostPoolName"] = element["hostPoolName"];
                                item["allowNewSession"] = element["allowNewSession"];
                                item["sessions"] = element["sessions"];
                                item["lastHeartBeat"] = element["lastHeartBeat"];
                                item["agentVersion"] = element["agentVersion"];
                                item["assignedUser"] = element["assignedUser"];
                                item["osVersion"] = element["osVersion"];
                                item["sxSStackVersion"] = element["sxSStackVersion"];
                                item["status"] = element["status"];
                                item["updateState"] = element["updateState"];
                                item["lastUpdateTime"] = element["lastUpdateTime"];
                                item["updateErrorMessage"] = element["updateErrorMessage"];
                            });

                            if (WVDresult.Count > finalVmList.Count)
                            {
                                var wvdList = WVDresult.ToList().Select(item => item["sessionHostName"]).ToList();
                                var comparedList = finalVmList.ToList().Select(item => item["sessionHostName"]).ToList();
                                var filteredSessionHostList = wvdList.ToList().Except(comparedList.ToList());

                                if (subscriptionId != null && azureDeployUrl != null && azureAccessToken != null)
                                {

                                    var scalesetInfo = GetScaleSetInstances(azureDeployUrl, azureAccessToken, subscriptionId);
                                    var scalesetInstancesContant = scalesetInfo.Content.ReadAsStringAsync().Result;
                                    JObject scalesetInstancesjson = JObject.Parse(scalesetInstancesContant);
                                    var scalesetInstancesList = ((JArray)scalesetInstancesjson["value"]).ToList();

                                    foreach (var element in scalesetInstancesList)
                                    {
                                        var resGrpName = element["id"].ToString().Split('/')[4].ToLower();
                                        var scalesetName = element["name"].ToString();
                                        var AzureScaleSetresponse = GetScaleSetVms(azureDeployUrl, azureAccessToken, subscriptionId, resGrpName, scalesetName);
                                        var AzurescalesetVmsContant = AzureScaleSetresponse.Content.ReadAsStringAsync().Result;
                                        JObject AzurescalesetVmsjson = JObject.Parse(AzurescalesetVmsContant);
                                        var AzurescalesetVmsList = ((JArray)AzurescalesetVmsjson["value"]).ToList();
                                        var GettingAzureScalesetDetails = AzurescalesetVmsList.Select(item => new JObject()
                                        {
                                               new JProperty("subscriptionId",  item["id"]== null ? null: item["id"].ToString().Split('/')[2]),
                                               new JProperty("resourceGroupName",   item["id"]== null ? null:item["id"].ToString().Split('/')[4]),
                                               new JProperty("scaleSetName",   item["id"]== null ? null:item["id"].ToString().Split('/')[8]),
                                               new JProperty("instanceId",   item["id"]== null ? null:item["instanceId"]),
                                               new JProperty("vmName",   item["properties"]["osProfile"]["computerName"]== null ? null:item["properties"]["osProfile"]["computerName"])
                                        }).ToList();

                                        var matchedHostWithScaleSetVms = GettingAzureScalesetDetails.Where(r => filteredSessionHostList.ToList().Any(d => r["vmName"].ToString() == d.ToString().Split('.')[0].ToString())).ToList();
                                        var matchedvms = matchedHostWithScaleSetVms.Select(item => item["vmName"].ToString()).ToList();
                                        // var filteredList= filteredSessionHostList.SelectMany(item => item.ToString().Split('.')[0].ToString()).Select(item1=> item1.ToString()).ToList();
                                        var filteredList = new List<object>();
                                        foreach (var item in filteredSessionHostList)
                                        {
                                            filteredList.Add(item.ToString().Split('.')[0].ToString());
                                        }
                                        var notMatchedHostWithScaleSetVms = filteredList.Except(matchedvms.ToList());

                                        matchedHostWithScaleSetVms.ToList().ForEach(item =>
                                        {

                                            var filter = WVDresult.ToList().Where(x => x["sessionHostName"].ToString().Split('.')[0] == item["vmName"].ToString()).ToList()[0];

                                            JObject jObject = new JObject
                                                 {
                                             new JProperty("sessionHostName",  filter["sessionHostName"] ),
                                             new JProperty("tenantName",  filter["tenantName"] ),
                                             new JProperty("tenantGroupName",  filter["tenantGroupName"] ),
                                             new JProperty("hostPoolName",  filter["hostPoolName"] ),
                                             new JProperty("allowNewSession",  filter["allowNewSession"] ),
                                             new JProperty("sessions",  filter["sessions"] ),
                                             new JProperty("lastHeartBeat",  filter["lastHeartBeat"] ),
                                             new JProperty("agentVersion",  filter["agentVersion"] ),
                                             new JProperty("assignedUser",  filter["assignedUser"] ),
                                             new JProperty("osVersion",  filter["osVersion"] ),
                                             new JProperty("sxSStackVersion",  filter["sxSStackVersion"] ),
                                             new JProperty("status",  filter["status"] ),
                                             new JProperty("updateState",  filter["updateState"] ),
                                             new JProperty("lastUpdateTime",  filter["lastUpdateTime"] ),
                                             new JProperty("updateErrorMessage",  filter["updateErrorMessage"] ),
                                             new JProperty("resourceGroupName",  item["resourceGroupName"] ),
                                             new JProperty("scaleSetName",  item["scaleSetName"] ),
                                             new JProperty("instanceId",  item["instanceId"] ),
                                             new JProperty("vmName",  item["vmName"] ),
                                                 };
                                            finalVmList.Add(jObject);
                                        });

                                        notMatchedHostWithScaleSetVms.ToList().ForEach(vm =>
                                        {

                                            var filter = WVDresult.ToList().Where(x => x["sessionHostName"].ToString().Split('.')[0] == vm.ToString()).ToList()[0];

                                            JObject jObject = new JObject
                                                 {
                                             new JProperty("sessionHostName",  filter["sessionHostName"] ),
                                             new JProperty("tenantName",  filter["tenantName"] ),
                                             new JProperty("tenantGroupName",  filter["tenantGroupName"] ),
                                             new JProperty("hostPoolName",  filter["hostPoolName"] ),
                                             new JProperty("allowNewSession",  filter["allowNewSession"] ),
                                             new JProperty("sessions",  filter["sessions"] ),
                                             new JProperty("lastHeartBeat",  filter["lastHeartBeat"] ),
                                             new JProperty("agentVersion",  filter["agentVersion"] ),
                                             new JProperty("assignedUser",  filter["assignedUser"] ),
                                             new JProperty("osVersion",  filter["osVersion"] ),
                                             new JProperty("sxSStackVersion",  filter["sxSStackVersion"] ),
                                             new JProperty("status",  filter["status"] ),
                                             new JProperty("updateState",  filter["updateState"] ),
                                             new JProperty("lastUpdateTime",  filter["lastUpdateTime"] ),
                                             new JProperty("updateErrorMessage",  filter["updateErrorMessage"] )

                                                 };
                                            finalVmList.Add(jObject);
                                        });





                                    }

                                }
                            }
                            return new HttpResponseMessage()
                            {
                                StatusCode = HttpStatusCode.OK,
                                Content = new StringContent(JsonConvert.SerializeObject(finalVmList))
                            };
                        }
                        else
                        {
                            return WVDResponse;
                        }
                    }
                    else
                    {
                        return WVDResponse;
                    }
                }
                else
                {
                    return WVDResponse;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public HttpResponseMessage GetScaleSetInstances(string azureDeployUrl, string accessToken, string subscriptionId)
        {
            try
            {
                HttpResponseMessage response = CommonBL.InitializeHttpClient(azureDeployUrl, accessToken).GetAsync("subscriptions/" + subscriptionId + "/providers/Microsoft.Compute/virtualMachineScaleSets?api-version=2019-03-01").Result;
                return response;
            }
            catch
            {
                return null;
            }
        }

        public HttpResponseMessage GetScaleSetVms(string azureDeployUrl, string accessToken, string subscriptionId, string resourceGroupName, string scaleSetName)
        {
            try
            {
                HttpResponseMessage response = CommonBL.InitializeHttpClient(azureDeployUrl, accessToken).GetAsync("subscriptions/" + subscriptionId + "/resourceGroups/" + resourceGroupName + "/providers/Microsoft.Compute/virtualMachineScaleSets/" + scaleSetName + "/virtualMachines?api-version=2019-03-01").Result;
                return response;
            }
            catch
            {
                return null;
            }
        }
        public JObject RestartScaleSetVms(string azureDeployUrl, string accessToken, string subscriptionId, string resourceGroupName, string scaleSetName, JObject InstanceId, string sessionHostName)
        {
            try
            {
                var content = new StringContent(JsonConvert.SerializeObject(InstanceId), Encoding.UTF8, "application/json");

                HttpResponseMessage response = CommonBL.InitializeHttpClient(azureDeployUrl, accessToken).PostAsync("subscriptions/" + subscriptionId + "/resourceGroups/" + resourceGroupName + "/providers/Microsoft.Compute/virtualMachineScaleSets/" + scaleSetName + "/restart?api-version=2019-07-01", content).Result;
                string strJson = response.Content.ReadAsStringAsync().Result;
                if (response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.Accepted)
                {
                    hostResult.Add("isSuccess", true);
                    hostResult.Add("message", sessionHostName + " is restarted successfully");
                }
                else
                {
                    hostResult.Add("isSuccess", false);
                    hostResult.Add("message", CommonBL.GetErrorMessage(strJson));
                }

            }
            catch (Exception ex)
            {
                hostResult.Add("isSuccess", false);
                hostResult.Add("message", ex.Message.ToString());
            }

            return hostResult;
        }



        public async Task<List<JObject>> GetAzureVMs(string deploymentUrl, string accessToken, string subscriptionId)
        {
            string nextLink = "";
            HttpResponseMessage responseNext = null;
            try
            {
                HttpResponseMessage response = CommonBL.InitializeHttpClient(deploymentUrl, accessToken).GetAsync("subscriptions/" + subscriptionId + "/providers/Microsoft.Compute/virtualMachines?api-version=2018-04-01").Result;
                var azureData = response.Content.ReadAsStringAsync().Result;
                var arrAzure1 = (JObject)JsonConvert.DeserializeObject(azureData);
                var arrAzure = arrAzure1["value"];
                var finalVmList = new List<JObject>();
                var AzureResult = new List<JObject>();
                nextLink = arrAzure1["nextLink"] != null ? arrAzure1["nextLink"].ToString() : null;
                AzureResult = ((JArray)arrAzure).Select(item => new JObject()
                    {
                            new JProperty("vmName",  item["name"]),
                            new JProperty("subscriptionId",  item["id"]==null ? null: item["id"].ToString().Split('/')[2]),
                            new JProperty("resourceGroupName",   item["id"]==null ? null:item["id"].ToString().Split('/')[4]),
                    }).ToList();
                finalVmList.AddRange(AzureResult);
                while (!string.IsNullOrEmpty(nextLink))
                {
                    using (HttpClient client = new HttpClient())
                    {
                        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, nextLink);
                        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                        responseNext = await client.SendAsync(request); // clientr.SendAsync(requestr);
                        var azureData1 = responseNext.Content.ReadAsStringAsync().Result;
                        var arrAzureNext = (JObject)JsonConvert.DeserializeObject(azureData1);
                        var arrAzureData = arrAzureNext["value"];
                        nextLink = arrAzureNext["nextLink"] != null ? arrAzureNext["nextLink"].ToString() : null;
                        AzureResult = ((JArray)arrAzureData).Select(item => new JObject()
                        {
                               new JProperty("vmName",  item["name"]),
                               new JProperty("subscriptionId",  item["id"]==null ? null: item["id"].ToString().Split('/')[2]),
                               new JProperty("resourceGroupName",   item["id"]==null ? null:item["id"].ToString().Split('/')[4]),
                        }).ToList();
                        finalVmList.AddRange(AzureResult);
                    }
                }
                return finalVmList;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Description: Gets a Rds HostPool associated with the Tenant  specified in the Rds context.
        /// </summary>
        /// <param name="deploymentUrl">RD Broker Url</param>
        /// <param name="accessToken">Access Token</param>
        /// <param name="tenantName">Name of Tenant</param>
        /// <param name="hostPoolName">Name of Hostpool</param>
        /// <param name="sessionHostName">Name of Session Host</param>
        /// <returns></returns>
        public HttpResponseMessage GetSessionHostDetails(string deploymentUrl, string accessToken, string tenantGroup, string tenantName, string hostPoolName, string sessionHostName)
        {
            try
            {
                //call rest api to get session host details -- july code bit
                HttpResponseMessage response = CommonBL.InitializeHttpClient(deploymentUrl, accessToken).GetAsync("/RdsManagement/V1/TenantGroups/" + tenantGroup + "/Tenants/" + tenantName + "/HostPools/" + hostPoolName + "/SessionHosts/" + sessionHostName).Result;
                return response;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Description : Updates a Rds SessionHost associated with the Tenant and HostPool specified in the Rds context.
        /// </summary>
        /// <param name="deploymentUrl">RD Broker Url</param>
        /// <param name="accessToken">Access token</param>
        /// <param name="rdMgmtSessionHost">Sessio  Host Class</param>
        /// <returns></returns>
        public JObject UpdateSessionHost(string deploymentUrl, string accessToken, JObject rdMgmtSessionHost)
        {
            try
            {
                //call rest service to update Sesssion host -- july code bit
                var content = new StringContent(JsonConvert.SerializeObject(rdMgmtSessionHost), Encoding.UTF8, "application/json");
                HttpResponseMessage response = CommonBL.PatchAsync(deploymentUrl, accessToken, "/RdsManagement/V1/TenantGroups/" + rdMgmtSessionHost["tenantGroupName"].ToString() + "/Tenants/" + rdMgmtSessionHost["tenantName"].ToString() + "/HostPools/" + rdMgmtSessionHost["hostPoolName"].ToString() + "/SessionHosts/" + rdMgmtSessionHost["sessionHostName"].ToString(), content).Result;
                string strJson = response.Content.ReadAsStringAsync().Result;
                if (response.IsSuccessStatusCode)
                {
                    hostResult.Add("isSuccess", true);
                    hostResult.Add("message", "Session host '" + rdMgmtSessionHost["sessionHostName"].ToString() + "' has been updated successfully.");
                }
                else if ((int)response.StatusCode == 429)
                {
                    hostResult.Add("isSuccess", false);
                    hostResult.Add("message", strJson + " Please try again later.");
                }
                else
                {
                    if (!string.IsNullOrEmpty(strJson))
                    {
                        hostResult.Add("isSuccess", false);
                        hostResult.Add("message", CommonBL.GetErrorMessage(strJson));
                    }
                    else
                    {
                        hostResult.Add("isSuccess", false);
                        hostResult.Add("message", "Updating session host '" + rdMgmtSessionHost["sessionHostName"].ToString() + "' is failed. Please try it again later.");
                    }
                }
            }
            catch (Exception ex)
            {
                hostResult.Add("isSuccess", false);
                hostResult.Add("message", "Session host '" + rdMgmtSessionHost["sessionHostName"].ToString() + "' has not been updated." + ex.Message.ToString() + " Please try it again later.");
            }
            return hostResult;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="deploymentUrl"></param>
        /// <param name="accessToken"></param>
        /// <param name="rdMgmtSessionHost"></param>
        /// <returns></returns>
        public JObject ChangeDrainMode(string deploymentUrl, string accessToken, JObject rdMgmtSessionHost)
        {
            try
            {
                //call rest service to update Sesssion host -- july code bit
                var content = new StringContent(JsonConvert.SerializeObject(rdMgmtSessionHost), Encoding.UTF8, "application/json");
                HttpResponseMessage response = CommonBL.PatchAsync(deploymentUrl, accessToken, "/RdsManagement/V1/TenantGroups/" + rdMgmtSessionHost["tenantGroupName"].ToString() + "/Tenants/" + rdMgmtSessionHost["tenantName"].ToString() + "/HostPools/" + rdMgmtSessionHost["hostPoolName"].ToString() + "/SessionHosts/" + rdMgmtSessionHost["sessionHostName"].ToString(), content).Result;
                string strJson = response.Content.ReadAsStringAsync().Result;
                if (response.IsSuccessStatusCode)
                {
                    hostResult.Add("isSuccess", true);
                    hostResult.Add("message", "'Allow new Session' is set to " + rdMgmtSessionHost["allowNewSession"] + " for " + rdMgmtSessionHost["sessionHostName"].ToString() + "'.");
                }
                else if ((int)response.StatusCode == 429)
                {
                    hostResult.Add("isSuccess", false);
                    hostResult.Add("message", strJson + " Please try again later.");
                }
                else
                {
                    if (!string.IsNullOrEmpty(strJson))
                    {
                        hostResult.Add("isSuccess", false);
                        hostResult.Add("message", CommonBL.GetErrorMessage(strJson));
                    }
                    else
                    {
                        hostResult.Add("isSuccess", false);
                        hostResult.Add("message", "Failed to set 'Allow new Session' for '" + rdMgmtSessionHost["sessionHostName"].ToString() + "'. Please try it again later.");
                    }
                }
            }
            catch (Exception ex)
            {
                hostResult.Add("isSuccess", false);
                hostResult.Add("message", "Failed to set 'Allow new Session' for '" + rdMgmtSessionHost["sessionHostName"].ToString() + "'. " + ex.Message.ToString() + " Please try it again later.");
            }
            return hostResult;
        }



        /// <summary>
        /// Description : Removes a Rds SessionHost associated with the Tenant and HostPool specified in the Rds context.
        /// </summary>
        /// <param name="deploymenturl">RD Broker Url</param>
        /// <param name="accessToken"> Access Token</param>
        /// <param name="tenantName">Name of Tenant</param>
        /// <param name="hostPoolName">Name of Hostpool</param>
        /// <param name="sessionHostName">Name of Session Host</param>
        /// <returns></returns>
        public SessionHostResult DeletesessionHost(string deploymentUrl, string accessToken, string tenantGroup, string tenantName, string hostPoolName, string sessionHostName)
        {
            SessionHostResult sessionHostResult = new SessionHostResult();
            try
            {
                //call rest service to delete session host -- july code bit
                HttpResponseMessage response = CommonBL.InitializeHttpClient(deploymentUrl, accessToken).DeleteAsync("/RdsManagement/V1/TenantGroups/" + tenantGroup + "/Tenants/" + tenantName + "/HostPools/" + hostPoolName + "/SessionHosts/" + sessionHostName + "?Force=True").Result;
                string strJson = response.Content.ReadAsStringAsync().Result;
                if (response.IsSuccessStatusCode)
                {
                    sessionHostResult.isSuccess = true;
                    sessionHostResult.message = "Session host '" + sessionHostName + "' has been deleted successfully.";
                }
                else if ((int)response.StatusCode == 429)
                {
                    sessionHostResult.isSuccess = false;
                    sessionHostResult.message = strJson + " Please try again later.";
                }
                else
                {
                    if (!string.IsNullOrEmpty(strJson))
                    {
                        sessionHostResult.isSuccess = false;
                        sessionHostResult.message = CommonBL.GetErrorMessage(strJson);
                    }
                    else
                    {
                        sessionHostResult.isSuccess = false;
                        sessionHostResult.message = "Session host " + sessionHostName + " has not been deleted. Please try it later again.";
                    }
                }
            }
            catch (Exception ex)
            {
                sessionHostResult.isSuccess = false;
                sessionHostResult.message = "Session host " + sessionHostName + " has not been deleted." + ex.Message.ToString() + " Please try it later again.";
            }
            return sessionHostResult;
        }

        public JObject RestartHost(string deploymentUrl, string accessToken, string subscriptionId, string resourceGroupName, string sessionHostName)
        {
            try
            {
                JObject vmDetails = new JObject()
                {
                    new  JProperty("subscriptionId",subscriptionId),
                    new  JProperty("resourceGroupName",resourceGroupName),
                    new  JProperty("vmName",sessionHostName)
                };
                var content = new StringContent(JsonConvert.SerializeObject(vmDetails), Encoding.UTF8, "application/json");
                HttpResponseMessage response = CommonBL.InitializeHttpClient(deploymentUrl, accessToken).PostAsync("subscriptions/" + subscriptionId + "/resourceGroups/" + resourceGroupName + "/providers/Microsoft.Compute/virtualMachines/" + sessionHostName + "/restart?api-version=2018-06-01", content).Result;
                string strJson = response.Content.ReadAsStringAsync().Result;
                if (response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.Accepted)
                {
                    hostResult.Add("isSuccess", true);
                    hostResult.Add("message", sessionHostName + " is restarted successfully");
                }
                else
                {
                    hostResult.Add("isSuccess", false);
                    hostResult.Add("message", CommonBL.GetErrorMessage(strJson));
                }
            }
            catch (Exception ex)
            {
                hostResult.Add("isSuccess", false);
                hostResult.Add("message", ex.Message.ToString());
            }

            return hostResult;

        }
    }
    #endregion "SessionHostBL"
}
#endregion "MSFT.RDMISaaS.API.BLL"
