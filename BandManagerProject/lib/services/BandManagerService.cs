using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Net;
using Newtonsoft.Json;
using BandManagerProject.lib.models;

namespace BandManagerProject.lib.services
{
    class BandManagerService
    {
        private static string _baseUrl = "http://bandmanager.dev/api";
        private static WebClient webClient = new WebClient();
        private static List<User> users;

        /**
         * Get app application users
         * API endpoint:    '/users'
         * Method:          'GET'
         */
        public static List<User> getAllUsers()
        {
            var response = webClient.DownloadString(_baseUrl + "/users");
            users = JsonConvert.DeserializeObject<List<User>>(response);
            return users;
        }

        /**
         * Get all pages for specific user. Returns empty if no pages found.
         * 
         * @param User user
         * @return List<Page>
         * 
         * API endpoint:    '/pages/{facebook_id}'
         * Params:          'facebook_id'
         * Method:          'GET'
         */
        public static List<Page> getUserPages(User user)
        {
            var response = webClient.DownloadString(_baseUrl + "/pages/" + user.facebook_id);
            List<Page> pages = JsonConvert.DeserializeObject<List<Page>>(response);
            return pages;

            // genre, likes, name, page_id, picture
        }

        /**
         * Send new user name to BandManager API and update value in database
         * Reqeust method is POST
         * @return string
         */
        public static string updateUserName(string newName, string facebook_id)
        {
            webClient.Headers["Content-Type"] = "application/x-www-form-urlencoded";

            var data = new NameValueCollection();

            data.Add("facebook_id", facebook_id);
            data.Add("new_name", newName);

            byte[] responseBytes = webClient.UploadValues(_baseUrl + "/user/name", "POST", data);
            string responseString = Encoding.UTF8.GetString(responseBytes);

            return responseString;
        }
    }
}
