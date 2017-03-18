﻿using System;
using System.Web.Mvc;

using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Mvc;
using Google.Apis.Drive.v3;
using Google.Apis.Util.Store;

using System.IO;

namespace NetFrameSiteOnDrive
{
    public class AppFlowMetadata : FlowMetadata
    {
        public const string ClinetIdString = "759434407900-a8qlujijs5l9evsv3ioblbv5d7q88jbv.apps.googleusercontent.com";
        private static readonly Lazy<string> s_lazySecret = new Lazy<string>(
            () => {
                string localFile = System.Web.Hosting.HostingEnvironment.MapPath("~/secret.txt");
                using (var sr = new StreamReader(localFile))
                {
                    return sr.ReadLine().Trim();
                }
            });

        private static string s_clientSecretString => s_lazySecret.Value;

        private static readonly IAuthorizationCodeFlow flow =
            new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer
            {
                ClientSecrets = new ClientSecrets
                {
                    ClientId = ClinetIdString,
                    ClientSecret = s_clientSecretString
                },
                Scopes = new[] { DriveService.Scope.Drive },
                DataStore = new FileDataStore("Drive.Api.Auth.Store")
            });

        public override string GetUserId(Controller controller)
        {
            // In this sample we use the session to store the user identifiers.
            // That's not the best practice, because you should have a logic to identify
            // a user. You might want to use "OpenID Connect".
            // You can read more about the protocol in the following link:
            // https://developers.google.com/accounts/docs/OAuth2Login.
            var user = controller.Session["user"];
            if (user == null)
            {
                user = Guid.NewGuid();
                controller.Session["user"] = user;
            }
            return user.ToString();

        }

        public override IAuthorizationCodeFlow Flow
        {
            get { return flow; }
        }
    }
}