using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Xml.Linq;
using SageAPI.Models;
using Newtonsoft.Json.Linq;
using System.Configuration;

namespace SageAPI.Controllers
{
    public class PartController : ApiController
    {
        // GET: api/Part
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Part/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Part
        public HttpResponseMessage Post([FromBody]JObject newParts)
        {
            Part[] parts = newParts["parts"].ToObject<Part[]>();

            // Create XML Request
            XElement createPartXML = XElement.Parse("<api:MBXML xmlns:api = 'http://sage100contractor.com/api'></api:MBXML>");

            XElement sessionElement = new XElement("MBXMLSessionRq",
                new XElement("Company", ConfigurationManager.AppSettings["Company"]),
                new XElement("User", "nicholas")
            );

            XElement messageElement = new XElement("MBXMLMsgsRq",
                new XAttribute("messageSetID", "1"),
                new XAttribute("onError", "continueOnError")
            );

            int requestIDCount = 1;
            int objectID = 0; 
            if (parts[0].ObjectID == 0 || parts[0].ObjectID == null)
            {
                objectID = getLastObjectID() + 1;
            }

            foreach (Part part in parts)
            {
                XElement partElement = new XElement("PartAddRq",
                    new XAttribute("requestID", requestIDCount),
                    new XElement("ObjectRef",
                        new XElement("ObjectID", objectID)
                    ),
                    new XElement("Desc", part.Desc),
                    new XElement("Unit", part.Unit),
                    new XElement("BinNumber", part.BinNumber),
                    new XElement("AlphaPart", part.AlphaPart),
                    new XElement("MSDSNumber", part.MSDSNumber),
                    new XElement("Manufacturer", part.Manufacturer),
                    new XElement("ManufacturerPartNumber", part.ManufacturerPartNumber),
                    new XElement("UserDefined1", part.UserDefined1),
                    new XElement("UserDefined2", part.UserDefined2),
                    new XElement("CostCodeRef",
                        new XElement("ObjectID", part.CostCodeRef)
                    ),
                    new XElement("CostTypeRef", 
                        new XElement("ObjectID", part.CostTypeRef)
                    ),
                    new XElement("TaskRef", 
                        new XElement("ObjectID", part.TaskRef)
                    ),
                    new XElement("PartClassRef", 
                        new XElement("ObjectID", part.PartClassRef)
                    ),
                    new XElement("InventoryLocationRef", 
                        new XElement("ObjectID", part.InventoryLocationRef)
                    ),
                    new XElement("PriceLastUpdatedDate", part.PriceLastUpdatedDate),
                    new XElement("ReorderQuantity", part.ReorderQuantity),
                    new XElement("MinimumOrderQuantity", part.MinimumOrderQuantity),
                    new XElement("PackagedQuantity", part.PackagedQuantity),
                    new XElement("ShippingWeight", part.ShippingWeight),
                    new XElement("DefaultCost", part.DefaultCost),
                    new XElement("LaborUnitQuantity", part.LaborUnitQuantity),
                    new XElement("BillingAmount", part.BillingAmount),
                    new XElement("IsStockPart", part.IsStockPart),
                    new XElement("IsSerialized", part.IsSerialized),
                    new XElement("BillingMarkupRate", part.BillingMarkupRate),
                    new XElement("PartRef", 
                        new XElement("ObjectID", part.PartRef)
                    ),
                    new XElement("Memo", part.Memo),
                    new XElement("IsServiceEquipment", part.IsServiceEquipment),
                    new XElement("OEMWarrantyDuration", part.OEMWarrantyDuration),
                    new XElement("InventoryRequired", part.InventoryRequired)
                );

                messageElement.Add(partElement);
                requestIDCount += 1;
                objectID += 1;
            }

            createPartXML.Add(sessionElement);
            createPartXML.Add(messageElement);

            // Create new API Objects for Sage interaction
            Sage.SMB.API.IMBXML sage = new Sage.SMB.API.IMBXML();
            API api = new API();

            // API Session Startup/Submit/End
            apiSessionStartup(api, sage);
            string apiResponse = api.submit(sage, createPartXML.ToString());
            apiSessionEnd(api, sage);

            return Request.CreateResponse(HttpStatusCode.Created, apiResponse);
        }

        // PUT: api/Part/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Part/5
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

        static int getLastObjectID( )
        {
            // Create XML Request
            XElement getLastObjectIDXML = XElement.Parse("<api:MBXML xmlns:api = 'http://sage100contractor.com/api'></api:MBXML>");

            XElement sessionElement = new XElement("MBXMLSessionRq",
                new XElement("Company", ConfigurationManager.AppSettings["Company"]),
                new XElement("User", "nicholas")
            );

            XElement messageElement = new XElement("MBXMLMsgsRq",
                new XAttribute("messageSetID", 1),
                new XAttribute("onError", "continueOnError"),
                new XElement("SQLRunRq",
                    new XAttribute("requestID", 1),
                    new XElement("SQL", "SELECT * FROM Part")
                )
            );

            getLastObjectIDXML.Add(sessionElement);
            getLastObjectIDXML.Add(messageElement);

            // Create new API Objects for Sage interaction
            Sage.SMB.API.IMBXML sage = new Sage.SMB.API.IMBXML();
            API api = new API();

            // API Session Startup/Submit/End
            apiSessionStartup(api, sage);
            string apiResponse = api.submit(sage, getLastObjectIDXML.ToString());
            apiSessionEnd(api, sage);

            XDocument xmlResponse = XDocument.Parse(apiResponse);
            int objectID = Int32.Parse(xmlResponse.Descendants().Last().Attribute("ObjectID").Value);

            return objectID;
        } 
    }
}
