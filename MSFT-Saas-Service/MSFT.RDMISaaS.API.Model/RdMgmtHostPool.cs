#region "Import Namespaces"
using System;
using System.Collections.Generic;
using System.Text;
#endregion "Import Namespaces"

#region "MSFT.RDMISaaS.API.Model"
namespace MSFT.RDMISaaS.API.Model
{
    #region "Class - RdMgmtHostPool"
    public class RdMgmtHostPool
    {
        public string tenantGroupName { get; set; }
        public string tenantName { get; set; }
        public string hostPoolName { get; set; }
        public string friendlyName { get; set; }
        public string description { get; set; }
       // public string persistent { get; set; }
        public string diskPath { get; set; }
        public Nullable<bool> enableUserProfileDisk { get; set; }
        public string excludeFolderPath { get; set; }
        public string excludeFilePath { get; set; }
        public string includeFilePath { get; set; }
        public string includeFolderPath { get; set; }
        public string customRdpProperty { get; set; }
        public int  maxSessionLimit { get; set; }
        public string useReverseConnect { get; set; }
        public string persistent { get; set; }
        public string autoAssignUser { get; set; }
        public string grantAdministrativePrivilege { get; set; }
        public int noOfActivehosts { get; set; }
        public int noOfAppgroups { get; set; }
        public int noOfUsers { get; set; }
        public int noOfSessions { get; set; }
        public string code { get; set; }
        public string refresh_token { get; set; }
    }
    #endregion  "Class - RdMgmtHostPool"


    #region "Class - HostPoolResult"
    public class HostPoolResult
    {
        public bool isSuccess { get; set; }
        public string message { get; set; }
    }
    #endregion  "Class - HostPoolResult"


    #region "Class - HostPoolDataDTO"
    public class HostPoolDataDTO
    {
        public string tenantGroupName { get; set; }
        public string aadHostPoolId { get; set; }
        public string hostpoolName { get; set; }
        public string tenantName { get; set; }
        public string friendlyName { get; set; }
        public string description { get; set; }
        public string persistent { get; set; }
        public string autoAssignUser { get; set; }
        public string grantAdministrativePrivilege { get; set; }
        public int maxSessionLimit { get; set; }
        public string enableUserProfileDisk { get; set; }
        public string diskPath { get; set; }

        public string maxUserProfileDiskSizeGb { get; set; }
        public string excludeFolderPath { get; set; }
        public string excludeFilePath { get; set; }
        public string includeFilePath { get; set; }
        public string includeFolderPath { get; set; }
        public string customRdpProperty { get; set; }
        public string useReverseConnect { get; set; }



    }
    #endregion  "Class - RdMgmtHostPool"
}
#endregion "MSFT.RDMISaaS.API.Model"  
