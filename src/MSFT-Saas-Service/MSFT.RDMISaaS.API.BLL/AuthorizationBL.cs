using MSFT.RDMISaaS.API.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Net.Http.Headers;

namespace MSFT.RDMISaaS.API.BLL
{
    public class AuthorizationBL
    {

        //public List<RdMgmtRoleAssignment> GetRoleAssignments(string deploymentUrl,string accessToken,string upn)
        //{
        //    List<RdMgmtRoleAssignment> rdMgmtRoleAssignments = new List<RdMgmtRoleAssignment>();
        //    try
        //    {
        //        HttpResponseMessage response = CommonBL.InitializeHttpClient(deploymentUrl, accessToken).GetAsync("RdsManagement/V1/Rds.Authorization/roleAssignments?upn="+upn).Result;


        //        string strJson = response.Content.ReadAsStringAsync().Result;
        //        if (response.IsSuccessStatusCode)
        //        {
        //            //Deserialize the string to JSON object
        //            var jObj = (JArray)JsonConvert.DeserializeObject(strJson);
        //            if (jObj.Count > 0)
        //            {
        //                rdMgmtRoleAssignments = jObj.Select(item => new RdMgmtRoleAssignment
        //                {
        //                    roleAssignmentId = (string)item["roleAssignmentId"],
        //                    scope = (string)item["scope"],
        //                    displayName = (string)item["displayName"],
        //                    signInName = (string)item["signInName"],
        //                    roleDefinitionName = (string)item["roleDefinitionName"],
        //                    roleDefinitionId = (string)item["roleDefinitionId"],
        //                    objectId = (string)item["objectId"],
        //                    objectType = (string)item["objectType"]
        //                }).ToList();
        //            }

        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //    return rdMgmtRoleAssignments;
        //}

        public HttpResponseMessage GetRoleAssignments(string deploymentUrl, string accessToken, string upn)
        {
           // List<RdMgmtRoleAssignment> rdMgmtRoleAssignments = new List<RdMgmtRoleAssignment>();
            try
            {
                HttpResponseMessage response = CommonBL.InitializeHttpClient(deploymentUrl, accessToken).GetAsync("RdsManagement/V1/Rds.Authorization/roleAssignments?upn=" + upn).Result;
                return response;

                //string strJson = response.Content.ReadAsStringAsync().Result;
                //if (response.IsSuccessStatusCode)
                //{
                //    //Deserialize the string to JSON object
                //    var jObj = (JArray)JsonConvert.DeserializeObject(strJson);
                //    //if (jObj.Count > 0)
                //    //{
                //    //    rdMgmtRoleAssignments = jObj.Select(item => new RdMgmtRoleAssignment
                //    //    {
                //    //        roleAssignmentId = (string)item["roleAssignmentId"],
                //    //        scope = (string)item["scope"],
                //    //        displayName = (string)item["displayName"],
                //    //        signInName = (string)item["signInName"],
                //    //        roleDefinitionName = (string)item["roleDefinitionName"],
                //    //        roleDefinitionId = (string)item["roleDefinitionId"],
                //    //        objectId = (string)item["objectId"],
                //    //        objectType = (string)item["objectType"]
                //    //    }).ToList();
                //    //}
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
            //return rdMgmtRoleAssignments;
        }


        public List<RdMgmtRoleAssignment> GetRoleAssignmentsByUser(string deploymentUrl, string accessToken, string loginUserName)
        {
            List<RdMgmtRoleAssignment> rdMgmtRoleAssignments = new List<RdMgmtRoleAssignment>();
            try
            {
                HttpResponseMessage response = CommonBL.InitializeHttpClient(deploymentUrl, accessToken).GetAsync("RdsManagement/V1/Rds.Authorization/roleAssignments").Result;


                string strJson = response.Content.ReadAsStringAsync().Result;
                if (response.IsSuccessStatusCode)
                {
                    //Deserialize the string to JSON object
                    var jObj = (JArray)JsonConvert.DeserializeObject(strJson);
                    if (jObj.Count > 0 && jObj.Select(x => (string)x["signInName"] == loginUserName).Count() > 0)
                    {
                        
                        rdMgmtRoleAssignments = jObj.Select(item => new RdMgmtRoleAssignment
                        {
                            roleAssignmentId = (string)item["roleAssignmentId"],
                            scope = (string)item["scope"],
                            displayName = (string)item["displayName"],
                            signInName = (string)item["signInName"],
                            roleDefinitionName = (string)item["roleDefinitionName"],
                            roleDefinitionId = (string)item["roleDefinitionId"],
                            objectId = (string)item["objectId"],
                            objectType = (string)item["objectType"]
                        }).ToList();
                    }

                }

            }
            catch (Exception ex)
            {
                return null;
            }
            return rdMgmtRoleAssignments;
        }
    }
}
