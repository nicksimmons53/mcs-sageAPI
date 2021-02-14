using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Xml.Linq;
using Newtonsoft.Json.Linq;
using SageAPI.Models;
using System.Configuration;

namespace SageAPI.Controllers
{
    public class PartClassController : ApiController
    {
        /*
        // GET: api/PartClass
        public IEnumerable<PartClass> Get()
        {

            return PartClass;
        }*/

        // GET: api/PartClass/10000
        public string Get(int id)
        {
            XElement getPartClassXML = XElement.Parse("<api:MBXML xmlns:api = 'http://sage100contractor.com/api'></api:MBXML>");

            XElement sessionElement = new XElement("MBXMLSessionRq",
                new XElement("Company", ConfigurationManager.AppSettings["Company"]),
                new XElement("User", "nicholas")
            );

            XElement messageElement = new XElement("MBXMLMsgsRq",
                new XAttribute("messageSetID", 1),
                new XAttribute("onError", "continueOnError"),
                new XElement("PartClassQryRq",
                    new XAttribute("requestID", 1),
                    new XElement("ObjectRef",
                        new XElement("ObjectID", id)
                    )
                )
            );

            getPartClassXML.Add(sessionElement);
            getPartClassXML.Add(messageElement);

            Sage.SMB.API.IMBXML sage = new Sage.SMB.API.IMBXML();
            API api = new API();

            apiSessionStartup(api, sage);

            string apiResponse = api.submit(sage, getPartClassXML.ToString());

            apiSessionEnd(api, sage);

            return apiResponse;
        }

        // POST: api/PartClass
        public HttpResponseMessage Post([FromBody]JObject newPartClass)
        {
            PartClass partClassInfo = newPartClass["info"].ToObject<PartClass>();

            // Create XML Request
            XElement createPartClassXML = XElement.Parse("<api:MBXML xmlns:api = 'http://sage100contractor.com/api'></api:MBXML>");

            XElement sessionElement = new XElement("MBXMLSessionRq",
                new XElement("Company", ConfigurationManager.AppSettings["Company"]),
                new XElement("User", "nicholas")
            );

            XElement messageElement = new XElement("MBXMLMsgsRq",
                new XAttribute("messageSetID", 1),
                new XAttribute("onError", "continueOnError"),
                new XElement("PartClassAddRq",
                    new XAttribute("requestID", 1),
                    new XElement("ObjectRef",
                        new XElement("ObjectID", partClassInfo.ObjectID)
                    ),
                    new XElement("Name", partClassInfo.Name),
                    new XElement("IndentLevel", partClassInfo.IndentLevel)
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

        // PUT: api/PartClass/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/PartClass/5
        public void Delete(int id)
        {
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
