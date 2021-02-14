using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Xml.Linq;
using System.Configuration;

namespace SageAPI.Controllers
{
    public class SQLController : ApiController
    {
        public HttpResponseMessage Get([FromBody]string query)
        {
            // Create XML Request
            XElement createPartClassXML = XElement.Parse("<api:MBXML xmlns:api = 'http://sage100contractor.com/api'></api:MBXML>");

            XElement sessionElement = new XElement("MBXMLSessionRq",
                new XElement("Company", ConfigurationManager.AppSettings["Company"]),
                new XElement("User", "nicholas")
            );

            XElement messageElement = new XElement("MBXMLMsgsRq",
                new XAttribute("messageSetID", 1),
                new XAttribute("onError", "continueOnError"),
                new XElement("SQLRunRq",
                    new XAttribute("requestID", 1),
                    new XElement("SQL", query)
                )
            );

            createPartClassXML.Add(sessionElement);
            createPartClassXML.Add(messageElement);

            // Create new API Objects for Sage interaction
            Sage.SMB.API.IMBXML sage = new Sage.SMB.API.IMBXML();
            API api = new API();

            // API Session Startup/Submit/End
            apiSessionStartup(api, sage);
            string apiResponse = api.submit(sage, createPartClassXML.ToString());
            apiSessionEnd(api, sage);

            return Request.CreateResponse(HttpStatusCode.Created, apiResponse);
        }

        static void apiSessionStartup(API api, Sage.SMB.API.IMBXML gobjMBAPI)
        {
            api.initializeAPI(gobjMBAPI);
            api.disableRequests(gobjMBAPI);
            api.setDataSource(gobjMBAPI);
            api.enableRequests(gobjMBAPI);
        }

        static Sage.SMB.API.IMBXML apiSessionEnd(API api, Sage.SMB.API.IMBXML gobjMBAPI)
        {
            // Cleanup Functions
            api.disableRequests(gobjMBAPI);
            api.deinitializeAPI(gobjMBAPI);

            return null;
        }
    }
}
