#region "Import Namespaces"
using MSFT.RDMISaaS.API.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.IdentityModel.Tokens.Jwt;
using MSFT.RDMISaaS.API.BLL;

#endregion "Import Namespaces"

#region "MSFT.RDMISaaS.API.Common"
namespace MSFT.RDMISaaS.API.Common

{

    #region "Common"
    public class Common
    {
        #region "Class level declaration"

        Configurations configurations = new Configurations();
        AuthorizationBL authorizationBL = new AuthorizationBL();
        #endregion



        #region "Functions/Methods"


        /// <summary>
        /// Description - Get access token from code
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public string GetAccessToken(string code)
        {
            var responseString = "";
            try
            {
                string OAUTH_2_0_TOKEN_ENDPOINT = "https://login.microsoftonline.com/common/oauth2/token";// //https://login.windows.net/common/oauth2/token
                string client_ID = configurations.applicationId;
                var request = (HttpWebRequest)WebRequest.Create(OAUTH_2_0_TOKEN_ENDPOINT);
                var postData = "redirect_uri=" + HttpUtility.UrlEncode(configurations.redirectUrl);
                postData += "&grant_type=authorization_code";
                postData += "&resource=" + HttpUtility.UrlEncode(configurations.resourceUrl);
                postData += "&client_id=" + client_ID;
                postData += "&code=" + code + "";
                var data = Encoding.ASCII.GetBytes(postData);
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = data.Length;
                using (var stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
                var response = (HttpWebResponse)request.GetResponse();
                responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
            }
            catch (Exception ex)
            {
                return Constants.invalidCode.ToString();
            }
            return responseString.ToString();
        }

        /// <summary>
        /// Description - Get access token using refresh token
        /// </summary>
        /// <param name="refreshToken"></param>
        /// <returns></returns>
        public string GetAccessTokenByRefreshToken(string refreshToken)
        {
            var responseString = "";
            try
            {
                string OAUTH_2_0_TOKEN_ENDPOINT = "https://login.windows.net/common/oauth2/token";
                string client_ID = configurations.applicationId;
                var request = (HttpWebRequest)WebRequest.Create(OAUTH_2_0_TOKEN_ENDPOINT);
                var postData = "redirect_uri=" + configurations.redirectUrl;
                postData += "&grant_type=refresh_token";
                postData += "&resource=" + configurations.resourceUrl;
                postData += "&client_id=" + client_ID;
                postData += "&refresh_token=" + refreshToken + "";
                var data = Encoding.ASCII.GetBytes(postData);
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = data.Length;
                using (var stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
                var response = (HttpWebResponse)request.GetResponse();
                responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
            }
            catch
            {
                return Constants.invalidCode.ToString();
            }
            return responseString.ToString();
        }
        /// <summary>
        /// Description : Get login details from code
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public Login Login(string Code)
        {


            string token = GetAccessToken(Code);
            Login loginDetails = JsonConvert.DeserializeObject<Login>(token);
            var handler = new JwtSecurityTokenHandler();
            var tokenS = handler.ReadToken(loginDetails.Access_Token) as JwtSecurityToken;
            loginDetails.UserName = tokenS.Claims.First(claim => claim.Type.Equals("name")).Value.ToString();
            loginDetails.Email = tokenS.Claims.First(claim => claim.Type.Equals("unique_name")).Value;
            loginDetails.Code = "";

            //get rele definition
            //if (loginDetails != null && loginDetails.Access_Token != null)
            //{
            //    string deploymentUrl = configurations.rdBrokerUrl;
            //    List<RdMgmtRoleAssignment> rdMgmtRoleAssignments = new List<RdMgmtRoleAssignment>();
            //    List<string> list = new List<string>();

            //    rdMgmtRoleAssignments = authorizationBL.GetRoleAssignments(deploymentUrl, loginDetails.Access_Token, loginDetails.Email.ToString());
            //    for (int i = 0; i < rdMgmtRoleAssignments.Count; i++)
            //    {
            //        if (rdMgmtRoleAssignments[i].signInName != null && rdMgmtRoleAssignments[i].signInName.ToString().ToLower() == loginDetails.Email.ToString().ToLower())
            //        {
            //loginDetails.RoleAssignment = rdMgmtRoleAssignments[i];
            //            if (loginDetails.RoleAssignment.scope.Split('/').Length > 1)
            //            {
            //                list.Add(loginDetails.RoleAssignment.scope.Split('/')[1].ToString());
            //            }
            //            else
            //            {
            //                list.Add(Constants.tenantGroupName);
            //            }
            //            //string TenantGroupName = "";
            //            //if (loginDetails.RoleAssignment.scope.Split('/').Length > 1)
            //            //{
            //            //    TenantGroupName = loginDetails.RoleAssignment.scope.Split('/')[1].ToString();
            //            //}
            //            //else
            //            //{
            //            //    TenantGroupName = Constants.tenantGroupName;
            //            //}
            //            //loginDetails.TenantGroupName = TenantGroupName;
            //            // break;
            //        }
            //    }
            //    loginDetails.TenantGroups = list.ToArray();

            //}

            ////new way to get role assignment

            if (loginDetails != null && loginDetails.Access_Token != null)
            {
                string deploymentUrl = configurations.rdBrokerUrl;
                List<string> list = new List<string>();
                HttpResponseMessage httpResponse = authorizationBL.GetRoleAssignments(deploymentUrl, loginDetails.Access_Token, loginDetails.Email.ToString());
                string strJson = httpResponse.Content.ReadAsStringAsync().Result;

                if (httpResponse.IsSuccessStatusCode)
                {
                    var rdMgmtRoleAssignments = (JArray)JsonConvert.DeserializeObject(strJson);
                    for (int i = 0; i < rdMgmtRoleAssignments.Count; i++)
                    {
                        loginDetails.RoleAssignment = new JObject() { { "roleDefinitionName", rdMgmtRoleAssignments[i]["roleDefinitionName"].ToString() }, { "scope", rdMgmtRoleAssignments[i]["scope"].ToString() } };
                        if (rdMgmtRoleAssignments[i]["signInName"] != null && rdMgmtRoleAssignments[i]["signInName"].ToString().ToLower() == loginDetails.Email.ToString().ToLower())
                        {
                            if (rdMgmtRoleAssignments[i]["scope"].ToString().Split('/').Length > 1)
                            {
                                list.Add(rdMgmtRoleAssignments[i]["scope"].ToString().Split('/')[1].ToString());
                            }
                            else
                            {
                                list.Add(Constants.tenantGroupName);
                            }
                        }
                    }
                    loginDetails.TenantGroups = list.ToArray();
                    //return loginDetails;
                }
                else if ((int)httpResponse.StatusCode == 429)
                {
                    loginDetails.Error = new JObject() { { "StatusCode", httpResponse.StatusCode.ToString() }, { "Message", strJson } };
                }
            }
            else
            {
                return null;
            }
            return loginDetails;
        }

        /// <summary>
        ///  Description : Get login details from code
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public string GetRefreshTokenValue(string code)
        {
            string refresh_token = "";
            string token = GetAccessToken(code);
            if (!string.IsNullOrEmpty(token))
            {
                if (token.ToString().ToLower() == Constants.invalidCode.ToString().ToLower())
                {
                    refresh_token = Constants.invalidCode;
                }
                else
                {
                    TokenDetails tokenDetails = Newtonsoft.Json.JsonConvert.DeserializeObject<TokenDetails>(token);
                    if (tokenDetails != null)
                    {
                        refresh_token = tokenDetails.refresh_token;
                    }
                    else
                    {
                        refresh_token = Constants.invalidToken;
                    }
                }
            }
            else
            {
                refresh_token = Constants.invalidToken;
            }
            return refresh_token;
        }

        /// <summary>
        /// Description - Get Access token value
        /// </summary>
        /// <param name="refreshToken"></param>
        /// <returns></returns>
        public string GetTokenValue(string refreshToken)
        {
            string access_token = "";
            string token = GetAccessTokenByRefreshToken(refreshToken);
            TokenDetails tokenDetails = new TokenDetails();
            if (!string.IsNullOrEmpty(token))
            {
                if (token.ToString().ToLower() == Constants.invalidCode.ToString().ToLower())
                {
                    tokenDetails.access_token = Constants.invalidCode;
                }
                else
                {
                    tokenDetails = Newtonsoft.Json.JsonConvert.DeserializeObject<TokenDetails>(token);
                    if (tokenDetails != null)
                    {
                        access_token = tokenDetails.access_token;
                    }
                    else
                    {
                        access_token = Constants.invalidToken;
                    }
                }
            }
            else
            {
                access_token = Constants.invalidToken;
            }
            return access_token;
        }

        #endregion

    }
    #endregion "Common"
}
#endregion "MSFT.RDMISaaS.API.Common"
