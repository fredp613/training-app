﻿
using System;
using System.Configuration;
using System.ServiceModel;

// These namespaces are found in the Microsoft.Crm.Sdk.Proxy.dll assembly
// located in the SDK\bin folder of the SDK download.
using Microsoft.Crm.Sdk.Messages;

// These namespaces are found in the Microsoft.Xrm.Sdk.dll assembly
// located in the SDK\bin folder of the SDK download.
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Tooling.Connector;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Training.crm
{
    public class CrmServiceProvider
    {

        private IOrganizationService _orgService;
        public CrmServiceClient conn;
        public static IConfigurationRoot Configuration { get; set; }

        public CrmServiceProvider()
        {
            // Connect to the CRM web service using a connection string.
            //CrmServiceClient conn = new Xrm.Tooling.Connector.CrmServiceClient(connectionString);
            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("secrets.json");

            Configuration = builder.Build();

            var credentials = new System.Net.NetworkCredential(Configuration["crmcreds:username"], Configuration["crmcreds:password"], Configuration["crmcreds:domain"]);

            conn = new CrmServiceClient(credentials,
                "workspace-justice-local-dev.crm.egcs.fmt-tgf.com",
                "443",
                "workspace-justice-local-dev", useUniqueInstance: true, useSsl: true);
            _orgService = (IOrganizationService)conn.OrganizationWebProxyClient != null ? (IOrganizationService)conn.OrganizationWebProxyClient : (IOrganizationService)conn.OrganizationServiceProxy;

           
            // _orgService = (IOrganizationService)conn.OrganizationWebProxyClient != null ? (IOrganizationService)conn.OrganizationWebProxyClient : (IOrganizationService)conn.OrganizationServiceProxy;

            //Create any entity records this sample requires.
        }

        public IOrganizationService GetService()
        {
            IOrganizationService service = _orgService;
            return service;
        }

        public string GetTestUser()
        {
            //// Obtain information about the logged on user from the web service.
            Guid userid = ((WhoAmIResponse)_orgService.Execute(new WhoAmIRequest())).UserId;
           // Entity account = _orgService.Retrieve("Account", Guid.NewGuid(), new ColumnSet(new String[] { "accountid", "accountname" }));
            Entity systemUser = _orgService.Retrieve("systemuser", userid,
                new ColumnSet(new string[] { "firstname", "lastname" }));
            var user = String.Format("Logged on user is {0} {1}.", systemUser.GetAttributeValue<string>("firstname"), systemUser.GetAttributeValue<string>("lastname"));
            //return conn.IsReady.ToString();
            return user;
            // Cast the proxy client to the IOrganizationService interface.
        }
    }
}
