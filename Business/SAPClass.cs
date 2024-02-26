using Aula06_Exercicio_.Models;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Aula06_Exercicio_.Business
{
    public class SAPClass
    {
        private String _slAddress;
        private String _server;

        public SAPClass(string SLAddress, String Server)
        {
            _slAddress = SLAddress;
            _server = Server;
        }

        public SAPClass()
        {

        }

        public String SAPLogin(LoginModel login)
        {
            try
            {
                var client = new RestClient(_slAddress);
                var request = new RestRequest("/Login", Method.POST);
                request.AddHeader("Content-Type", "application/json");

                var body = Newtonsoft.Json.JsonConvert.SerializeObject(login);
                request.AddParameter("application/json", body, ParameterType.RequestBody);

                ServicePointManager.ServerCertificateValidationCallback += new System.Net.Security.RemoteCertificateValidationCallback(ValidateServerCertificate);

                RestSharp.IRestResponse response = client.Execute(request);

                var B1Session = response.Cookies.FirstOrDefault().Value;

                return B1Session;
            }
            catch
            {
                throw;
            }
        }

        public OrderModel InserirPedido(OrderModel order, String SessionID)
        {
            try
            {
                var client = new RestClient(_slAddress);
                var request = new RestRequest("/Orders", Method.POST);
                request.AddHeader("Content-Type", "application/json");

                var body = Newtonsoft.Json.JsonConvert.SerializeObject(order);
                request.AddParameter("application/json", body, ParameterType.RequestBody);

                CookieContainer cookiecon = new CookieContainer();
                cookiecon.Add(new Cookie("B1SESSION", SessionID, "/b1s/v1", "hana10b"));

                client.CookieContainer = cookiecon;

                ServicePointManager.ServerCertificateValidationCallback += new System.Net.Security.RemoteCertificateValidationCallback(ValidateServerCertificate);

                RestSharp.IRestResponse response = client.Execute(request);

                OrderModel pedidoRetorno = new OrderModel();
                if (response.StatusCode == HttpStatusCode.Created)
                {
                    pedidoRetorno = Newtonsoft.Json.JsonConvert.DeserializeObject<OrderModel>(response.Content);
                }
                else
                {
                    dynamic ret = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(response.Content);
                    var message = ret.error.message.value.ToString();

                    throw new Exception(message);
                }

                return pedidoRetorno;
            }
            catch
            {
                throw;
            }
        }

        public String ConnectToDi(LoginModel login)
        {
            CommonClass.oCompany = new SAPbobsCOM.Company();

            try
            {
                /*Como conectar com versão HANA 2.0 SAP 10*/
                CommonClass.oCompany.SLDServer = _server + ":40000";
                CommonClass.oCompany.Server = "NDB@" + _server + ":30013";
                CommonClass.oCompany.CompanyDB = login.CompanyDB;
                CommonClass.oCompany.UserName = login.UserName;
                CommonClass.oCompany.Password = login.Password;

                CommonClass.oCompany.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_HANADB;
                CommonClass.oCompany.language = SAPbobsCOM.BoSuppLangs.ln_Portuguese_Br;


                Int32 lRet = CommonClass.oCompany.Connect();
                if (lRet != 0)
                    throw new Exception(CommonClass.oCompany.GetLastErrorDescription());

                return "on-line";
            }
            catch
            {
                return "off-line";
                throw;
            }
        }

        public void InsertItem(String ItemCode, String ItemName)
        {
            SAPbobsCOM.Items oItem = null;

            try
            {
                oItem = CommonClass.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oItems);

                oItem.ItemCode = ItemCode;
                oItem.ItemName = ItemName;


                Int32 lRet = oItem.Add();
                if (lRet != 0)
                    throw new Exception(CommonClass.oCompany.GetLastErrorDescription());
            }
            catch
            {
                throw;
            }
            finally
            {
                if (oItem != null)
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(oItem);
            }
        }

        public void InsertPN(String PNCode, String PNName)
        {
            SAPbobsCOM.BusinessPartners oBP = null;

            try
            {
                oBP = CommonClass.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oBusinessPartners);

                oBP.CardCode = PNCode;
                oBP.CardName = PNName;
                oBP.CardType = SAPbobsCOM.BoCardTypes.cCustomer;
                
                Int32 lRet = oBP.Add();
                if (lRet != 0)
                    throw new Exception(CommonClass.oCompany.GetLastErrorDescription());
            }
            catch
            {
                throw;
            }
            finally
            {
                if (oBP != null)
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(oBP);
            }
        }

        public static bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }
    }
}
