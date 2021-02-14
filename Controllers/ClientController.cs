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
    [Authorize]
    public class ClientController : ApiController
    {
        // GET: api/Client
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Client/137
        public string Get(int id)
        {
            // Create XML Request
            XElement getClientXML = XElement.Parse("<api:MBXML xmlns:api = 'http://sage100contractor.com/api'></api:MBXML>");

            XElement sessionElement = new XElement("MBXMLSessionRq",
                new XElement("Company", ConfigurationManager.AppSettings["Company"]),
                new XElement("User", "nicholas")
            );

            XElement messageElement = new XElement("MBXMLMsgsRq",
                new XAttribute("messageSetID", 1),
                new XAttribute("onError", "continueOnError"),
                new XElement("ClientQryRq",
                    new XAttribute("requestID", 1),
                    new XElement("ObjectRef",
                        new XElement("ObjectID", id)
                    )
                )
            );

            getClientXML.Add(sessionElement);
            getClientXML.Add(messageElement);

            // Create new API Objects for Sage interaction
            Sage.SMB.API.IMBXML sage = new Sage.SMB.API.IMBXML();
            API api = new API();

            // API Session Startup/Submit/End
            apiSessionStartup(api, sage);
            string response = api.submit(sage, getClientXML.ToString( ));
            apiSessionEnd(api, sage);

            // Return string
            return response;
        }

        // POST: api/Client
        // In body {info: {}, contacts: [{}]}
        public HttpResponseMessage Post([FromBody]JObject newClient)
        {
            Client clientInfo = newClient["info"].ToObject<Client>();
            ClientContact[] clientContacts = newClient["contacts"].ToObject<ClientContact[]>();

            // Create XML Request
            XElement getClientXML = XElement.Parse("<api:MBXML xmlns:api = 'http://sage100contractor.com/api'></api:MBXML>");

            XElement sessionElement = new XElement("MBXMLSessionRq",
                new XElement("Company", ConfigurationManager.AppSettings["Company"]),
                new XElement("User", "nicholas")
            );

            // Client Info
            XElement messageElement = new XElement("MBXMLMsgsRq",
                new XAttribute("messageSetID", "1"),
                new XAttribute("onError", "continueOnError"),
                new XElement("ClientAddNextRq",
                    new XAttribute("requestID", 1),
                    new XElement("ShortName", clientInfo.ShortName),
                    new XElement("Name", clientInfo.Name),
                    new XElement("Greeting", clientInfo.Greeting),
                    new XElement("Addr1", clientInfo.Addr1),
                    new XElement("Addr2", clientInfo.Addr2),
                    new XElement("City", clientInfo.City),
                    new XElement("State", clientInfo.State),
                    new XElement("PostalCode", clientInfo.PostalCode),
                    new XElement("BillingAddr1", clientInfo.BillingAddr1),
                    new XElement("BillingAddr2", clientInfo.BillingAddr2),
                    new XElement("BillingCity", clientInfo.BillingCity),
                    new XElement("BillingState", clientInfo.BillingState),
                    new XElement("BillingPostalCode", clientInfo.BillingPostalCode),
                    new XElement("ShippingAddr1", clientInfo.ShippingAddr1),
                    new XElement("ShippingAddr2", clientInfo.ShippingAddr2),
                    new XElement("ShippingCity", clientInfo.ShippingCity),
                    new XElement("ShippingState", clientInfo.ShippingState),
                    new XElement("ShippingPostalCode", clientInfo.ShippingPostalCode),
                    new XElement("UserDefined1", clientInfo.UserDefined1),
                    new XElement("UserDefined2", clientInfo.UserDefined2),
                    new XElement("UserDefined3", clientInfo.UserDefined3),
                    new XElement("UserDefined4", clientInfo.UserDefined4),
                    new XElement("UserDefined5", clientInfo.UserDefined5),
                    new XElement("UserDefined6Logical", clientInfo.UserDefined6Logical),
                    new XElement("UserDefined7Logical", clientInfo.UserDefined7Logical),
                    new XElement("UserDefined8Logical", clientInfo.UserDefined8Logical),
                    new XElement("UserDefined9Logical", clientInfo.UserDefined9Logical),
                    new XElement("ReceivedDate", clientInfo.ReceivedDate),
                    new XElement("SalespersonRef",
                        new XElement("ObjectID", clientInfo.SalespersonRef)
                    ),
                    new XElement("ManagerRef",
                        new XElement("ObjectID", clientInfo.ManagerRef)
                    ),
                    new XElement("SalesTaxDistrictRef",
                        new XElement("ObjectID", clientInfo.SalesTaxDistrictRef)
                    ),
                    new XElement("ContactedDate", clientInfo.ContactedDate),
                    new XElement("CallbackDate", clientInfo.CallbackDate),
                    new XElement("MailPiece", clientInfo.MailpieceDate),
                    new XElement("MailedDate", clientInfo.MailedDate),
                    new XElement("PurchasedDate", clientInfo.PurchasedDate),
                    new XElement("ReferenceDate", clientInfo.ReferenceDate),
                    new XElement("ProductRef",
                        new XElement("ObjectID", clientInfo.ProductRef)
                    ),
                    new XElement("DiscountTerm", clientInfo.DiscountTerm),
                    new XElement("EarlyPaymentDiscountRate", clientInfo.EarlyPaymentDiscountRate),
                    new XElement("DueTerm", clientInfo.DueTerm),
                    new XElement("FinanceRate", clientInfo.FinanceRate),
                    new XElement("SourceRef",
                        new XElement("ObjectID", clientInfo.SourceRef)
                    ),
                    new XElement("ClientTypeRef",
                        new XElement("ObjectID", clientInfo.ClientTypeRef)
                    ),
                    new XElement("ClientStatusRef",
                        new XElement("ObjectID", clientInfo.ClientStatusRef)
                    ),
                    new XElement("MailListRef",
                        new XElement("ObjectID", clientInfo.MailListRef)
                    ),
                    new XElement("AreaRef",
                        new XElement("ObjectID", clientInfo.AreaRef)
                    ),
                    new XElement("SizeRef",
                        new XElement("ObjectID", clientInfo.SizeRef)
                    ),
                    new XElement("ContractRef",
                        new XElement("ObjectID", clientInfo.ContractRef)
                    ),
                    new XElement("ExpirationDate", clientInfo.ExpirationDate),
                    new XElement("DiscountRate", clientInfo.DiscountRate),
                    new XElement("MapLocation", clientInfo.MapLocation),
                    new XElement("CrossStreet", clientInfo.CrossStreet),
                    new XElement("PartBillingBasis", clientInfo.PartBillingBasis),
                    new XElement("PurchaseOrderNumber", clientInfo.PurchaseOrderNumber),
                    new XElement("TaxExemptionNumber", clientInfo.TaxExemptionNumber),
                    new XElement("CreditCardNumber", clientInfo.CreditCardNumber),
                    new XElement("CreditCardExpirationDate", clientInfo.CreditCardExpirationDate),
                    new XElement("CreditCardCarhdholder", clientInfo.CreditCardCardholder),
                    new XElement("CreditCardType", clientInfo.CreditCardType),
                    new XElement("Memo", clientInfo.Memo),
                    new XElement("StatementEmail", clientInfo.StatementEmail)
                )
            );

            // Client Contact Info
            foreach (ClientContact contact in clientContacts)
            {
                XElement contactTree = new XElement("ClientContactAdd",
                    new XElement("ContactName", contact.ContactName),
                    new XElement("JobTitle", contact.JobTitle),
                    new XElement("Phone", contact.Phone),
                    new XElement("Email", contact.Email)
                );

                messageElement.Descendants("ClientAddNextRq").First().Add(contactTree);
            }

            getClientXML.Add(sessionElement);
            getClientXML.Add(messageElement);

            // Create new API Objects for Sage interaction
            Sage.SMB.API.IMBXML sage = new Sage.SMB.API.IMBXML();
            API api = new API();

            // API Session Startup/Submit/End
            apiSessionStartup(api, sage);
            string apiResponse = api.submit(sage, getClientXML.ToString());
            apiSessionEnd(api, sage);

            return Request.CreateResponse(HttpStatusCode.Created, apiResponse);
        }

        // PUT: api/Client/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Client/5
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
