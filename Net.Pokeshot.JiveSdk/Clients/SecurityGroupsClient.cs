using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Web;

using Net.Pokeshot.JiveSdk.Models;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Net.Pokeshot.JiveSdk.Clients
{
    /// <summary>
    /// 
    /// </summary>
    public class SecurityGroupsClient : JiveClient
    {
        string securityGroupsUrl { get { return JiveCommunityUrl + "/api/core/v3/securityGroups"; } }
        public SecurityGroupsClient(string communityUrl, NetworkCredential credentials) : base(communityUrl, credentials) { }


        /// <summary>
        /// Return a SecurityGroup whose name matches the specified name.
        /// </summary>
        /// <param name="groupname">Name of the security group to be returned</param>
        /// <param name="fields">Field names to be returned (default is all)</param>
        /// <returns>a Person object representing the requested user</returns>
        public Group GetSecurityGroupByName(string groupname, List<string> fields = null)
        {
            string url = securityGroupsUrl + "/name/" + groupname;
            if (fields != null && fields.Count > 0)
            {
                url += "?fields=";
                foreach (var field in fields)
                {
                    url += field + ",";
                }
                // remove last comma
                url = url.Remove(url.Length - 1);
            }

            string json;
            try
            {
                json = GetAbsolute(url);
            }
            catch (HttpException e)
            {
                Console.WriteLine(e.Message);
                switch (e.GetHttpCode())
                {
                    case 400:
                        throw new HttpException(e.WebEventCode, "Specified username is malformed", e);
                    case 403:
                        throw new HttpException(e.WebEventCode, "The requesting user is not authorized to retrieve security groups", e);
                    case 404:
                        throw new HttpException(e.WebEventCode, "If the name does not match a valid security group", e);
                    default:
                        throw;
                }
            }

            JObject results = JObject.Parse(json);

            return results.ToObject<Group>();
        }

        /// <summary>
        /// Add the specified people as regular members of the specified security group.
        /// </summary>
        /// <param name="securityGroupID">ID of the security group to which regular members should be added</param>
        /// <param name="new_members">URIs of the people to be added as regular members</param>
        /// <param name="fields">Fields to be returned (default is @all)</param>
        /// <returns>Category object representing the newly created category</returns>
        public void CreateMembers(int securityGroupID, string[] new_members, List<string> fields = null)
        {
            //construct the url for the HTTP request based on the user specifications
            string url = securityGroupsUrl + "/" + securityGroupID.ToString() + "/members";

            if (fields != null && fields.Count > 0)
            {
                url += "&fields=";
                foreach (var field in fields)
                {
                    url += field + ",";
                }
                // remove last comma
                url = url.Remove(url.Length - 1);
            }

            //convert the Person object to JSON format and post via HTTP
            string json = JsonConvert.SerializeObject(new_members, new JsonSerializerSettings { DefaultValueHandling = DefaultValueHandling.Ignore, Formatting = Formatting.Indented });
            string result;
            try
            {
                result = PostAbsolute(url, json);
            }
            catch (HttpException e)
            {
                switch (e.GetHttpCode())
                {
                    case 400:
                        throw new HttpException(e.WebEventCode, "An input field is malformed", e);
                    case 403:
                        throw new HttpException(e.WebEventCode, "You are not allowed to add regular members to a security group", e);
                    case 404:
                        throw new HttpException(e.WebEventCode, "The specified security group, or one of the specified members, is not found", e);
                    case 409:
                        throw new HttpException(e.WebEventCode, "The new entity would conflict with system restrictions (such as being both an admin and a regular member)", e);
                    default:
                        throw;
                }
            }
        }
    }
}
