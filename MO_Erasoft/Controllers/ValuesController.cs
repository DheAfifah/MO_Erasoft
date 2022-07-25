//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net;
using System.Net.Http;
using System.Web.Http;

using EntityFramework.Extensions;
using Hangfire.RecurringJobAdmin;
using MO_Erasoft.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace MO_Erasoft.Controllers
{
    public class ValuesController : ApiController
    {
        // GET api/values
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }

        public ERAMODbContext ERAMODbContext { get; set; }
        public ERADbContext ERADbContext { get; set; }

        public AKUN_ERA_MODbContext AKUN_ERA_MODbContext { get; set; }


        public SIDbContext SIDbContext { get; set; }

        public ARDbContext ARDbContext { get; set; }

        public ValuesController()
        {
            ERAMODbContext = new ERAMODbContext("");
            ERADbContext = new ERADbContext("");
            SIDbContext = new SIDbContext("");
            ARDbContext = new ARDbContext("");
            AKUN_ERA_MODbContext = new AKUN_ERA_MODbContext("");
        }

        //[RecurringJob("4 * * * *", "SE Asia Standard Time", "default", RecurringJobId = "TestCallApi")]
        public async Task<string> TestCallApi()
        {
            string ret = "";
            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            //ServicePointManager.SecurityProtocol = (SecurityProtocolType)12288;

            string urll = "https://www.masteronline.co.id/api/refreshstokmp";

            HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create(urll);
            myReq.Method = "POST";
            myReq.Headers.Add("X-API-KEY", "REFRESHSTOKMP_M@STERONLINE4P1K3Y");
            myReq.Headers.Add("DBPATHERA", "ERASOFT_720302");
            myReq.Headers.Add("USERNAME", "Wilson Djiauw");
            //myReq.Headers.Add("DBPATHERA", "ERASOFT_240079");
            //myReq.Headers.Add("USERNAME", "Milad Fauzi");
            myReq.Accept = "application/x-www-form-urlencoded";
            myReq.ContentType = "application/json";
            myReq.ContentLength = 0;

            string responseFromServer = "";

            try
            {
                using (WebResponse response = await myReq.GetResponseAsync())
                {

                    using (Stream stream = response.GetResponseStream())
                    {
                        StreamReader reader = new StreamReader(stream);
                        responseFromServer = reader.ReadToEnd();
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return ret;
        }

        public class JsonData_StokOP
        {
            public string batch { get; set; }
            public string noStok { get; set; }
            public string email { get; set; }
            public string token { get; set; }
            public bool isAccurate { get; set; }
            public string DatabasePathErasoft { get; set; }
            public string dbSourceEra { get; set; }
        }
        public class JsonApi
        {
            public int code { get; set; }
            public string message { get; set; }
            public List<data> data { get; set; }
        }

        public class data
        {
            public string no_bukti { get; set; }
            public string id_header_kirim { get; set; }
            public string id_header_terima { get; set; }
            public string id_detail_kirim { get; set; }
            public string id_detail_terima { get; set; }
        }

        public class SIF01
        {
            public string CUST { get; set; }

            public string KODE { get; set; }
        }

        public class dataEra
        {
            public dataErasoft data { get; set; }
        }
        public class dataErasoft
        {
            public SIT01A Header { get; set; }
            public List<SIT01B_MO> listDetail { get; set; }
            public ARF01 Cust { get; set; }
            public ARF01C Pembeli { get; set; }
        }


        [System.Web.Http.Route("api/sales-invoice/update-header")]
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public async Task<IHttpActionResult> ProsesUpdateHeader([FromBody]dataEra data)
        {

            try
            {
                //var data = new dataEra();

                //var data = JsonConvert.DeserializeObject(data2, typeof(dataEra)) as dataEra;

                JsonApi result;
                string apiKey = "";
                string dbPathEra = "";
                string userName = "";

                var re = Request;
                var headers = re.Headers;

                //data.token = "";

                //if (headers.Contains("X-API-KEY"))
                //{
                //    apiKey = headers.GetValues("X-API-KEY").First();
                //}

                //if (apiKey != "UPDATESTOKMP_M@STERONLINE4P1K3Y")
                //{
                //    result = new JsonApi()
                //    {
                //        code = 401,
                //        message = "Wrong API KEY!",
                //        data = null
                //    };

                //    return Json(result);
                //}

                if (headers.Contains("DBPATHERA"))
                {
                    dbPathEra = headers.GetValues("DBPATHERA").First();
                }
                else
                {
                    result = new JsonApi()
                    {
                        code = 401,
                        message = "DBPATHERA can not be empty!",
                        data = null
                    };

                    return Json(result);
                }

                if (headers.Contains("USERNAME"))
                {
                    userName = headers.GetValues("USERNAME").First();
                }
                else
                {
                    result = new JsonApi()
                    {
                        code = 401,
                        message = "USERNAME can not be empty!",
                        data = null
                    };

                    return Json(result);
                }

                if (data == null)
                {
                    result = new JsonApi()
                    {
                        code = 401,
                        message = "faktur can not be empty!",
                        data = null
                    };

                    return Json(result);
                }

                result = new JsonApi();

                try
                {
                    //var tesinsert = ERAMODbContext.Database.ExecuteSqlCommand("INSERT INTO [temp_api_tokped] (response, cust, tglinput) VALUES ('TES', 'ERASOFT', DATEADD(HOUR, +7, GETUTCDATE()))");
                    //cek dataErasoft ada isinya atau tidak 
                    var tesinsert = data.data.Header.NO_BUKTI;
                    if (!string.IsNullOrEmpty(tesinsert))
                    {
                        //cek sit01a 
                        //disesuaikan dengan sit01a punya erasoft 
                        //cek sit01b 
                        //cek arf01 //detail marketplace 
                        //cek arf01c //pembeli
                        //dimasukkan ke table erasoft 
                        //MO butuh balikan recnum erasoft 

                        //var A = JsonConvert.SerializeObject(data);
                        //var listDetail = data.data.listDetail.ToList();

                        var AKUN_Object = AKUN_ERA_MODbContext.Database.SqlQuery<AKUN>("SELECT * FROM AKUN NOLOCK where Email = '"+ userName + "'").FirstOrDefault();

                        if (AKUN_Object != null)
                        {
                            var CS = AKUN_Object.APIConnectionString;
                            var server = CS.Substring(CS.LastIndexOf("Data Source=") + 8);

                            int index = CS.IndexOf(';');
                            if (index > 0)
                            {
                                server = CS.Substring(0, index);
                            }

                            server = server.Substring(12);

                            var DatabaseName = CS.Substring(CS.LastIndexOf("=") + 1);

                            var APIDbContextA = new APIDbContext(server, DatabaseName);

                            var header = data.data.Header;
                            SIDbContext = new SIDbContext("");

                            //string Query = "update SIT01A set Tgl = '"+ header.TGL+"'";
                            //SIDbContext.Database.ExecuteSqlCommand("update SIT01A set ");

                            var DbContext = SIDbContext.SIT01A.FirstOrDefault(a => a.NO_BUKTI == header.NO_BUKTI);

                            //var APIDbContextA = new APIDbContext(server, DatabaseName);

                            //var header = data.data.Header;

                            var RecordMarketplace = APIDbContextA.Database.SqlQuery<MARKETPLACE>("select * from Marketplace NOLOCK where Marketplace = '" + header.CUST + "' ").FirstOrDefault();

                            var KodeCustomerErasoft = RecordMarketplace.Customer;

                            ARDbContext = new ARDbContext(server, DatabaseName);

                            var ARF01 = ARDbContext.Database.SqlQuery<ARF01>("select * from ARF01 NOLOCK where Cust = '" + KodeCustomerErasoft + "' ").FirstOrDefault();

                            var NamaCustomerErasoft = ARF01.NAMA;

                            //DbContext.TGL = header.TGL;
                            //DbContext.STATUS = header.STATUS;
                            //DbContext.ST_POSTING = header.ST_POSTING;
                            //DbContext.TGL_KIRIM = header.TGL_KIRIM;
                            //DbContext.NO_REF = header.NO_REF;
                            //DbContext.NO_SO = header.NO_SO;
                            //DbContext.CUST = header.CUST;
                            //DbContext.NAMA_CUST = header.NAMA_CUST;
                            //DbContext.KODE_ALAMAT = header.KODE_ALAMAT;
                            //DbContext.NO_KENDARAAN = header.NO_KENDARAAN;

                            if (DbContext != null)
                            {
                                //DbContext.NILAI_DISC = header.NILAI_DISC;
                                //DbContext.PPN = header.PPN;
                                //DbContext.NILAI_PPN = Math.Ceiling((double)DbContext.PPN * ((double)DbContext.BRUTO - (double)DbContext.NILAI_DISC) / 100);
                                //DbContext.MATERAI = header.MATERAI;
                                ////DbContext.TGL = DateTime.ParseExact(header.TGL, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                                //DbContext.TGL = header.TGL;
                                //DbContext.CUST = header.CUST;
                                //DbContext.TERM = header.TERM;
                                //DbContext.PEMESAN = header.PEMESAN;
                                //DbContext.NAMAPEMESAN = header.NAMAPEMESAN;
                                ////DbContext.TGL_JT_TEMPO = DateTime.ParseExact(header.TGL_JT_TEMPO, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                                //DbContext.TGL_JT_TEMPO = header.TGL_JT_TEMPO;
                                //DbContext.NETTO = DbContext.BRUTO + DbContext.NILAI_PPN + DbContext.MATERAI - DbContext.NILAI_DISC;
                                //DbContext.PENGIRIM = header.PENGIRIM;
                                //DbContext.NAMAPENGIRIM = header.NAMAPENGIRIM;
                                //DbContext.NO_REF = header.NO_REF;



                                DbContext.TGL = header.TGL;
                                DbContext.STATUS = header.STATUS;
                                DbContext.ST_POSTING = header.ST_POSTING;
                                DbContext.TGL_KIRIM = header.TGL_KIRIM;
                                DbContext.NO_REF = header.NO_REF;
                                DbContext.NO_SO = header.NO_SO;
                                DbContext.CUST = KodeCustomerErasoft;
                                DbContext.NAMA_CUST = NamaCustomerErasoft;
                                DbContext.KODE_ALAMAT = header.KODE_ALAMAT;
                                DbContext.NO_KENDARAAN = header.NO_KENDARAAN;
                                DbContext.SOPIR = header.SOPIR;
                                DbContext.KET = header.KET;
                                DbContext.VLT = header.VLT;
                                DbContext.TUKAR = header.TUKAR;
                                DbContext.TUKAR_PPN = header.TUKAR_PPN;
                                DbContext.KODE_SALES = header.KODE_SALES;
                                DbContext.KODE_WIL = header.KODE_WIL;
                                DbContext.NO_F_PAJAK = header.NO_F_PAJAK;
                                DbContext.TGL_F_PAJAK = header.TGL_F_PAJAK;
                                DbContext.KODE_PROYEK = header.KODE_PROYEK;
                                //DbContext.BRUTO = detail.Sum(a => a.HARGA);
                                DbContext.DISCOUNT = header.DISCOUNT;
                                DbContext.NILAI_DISC = header.NILAI_DISC;
                                DbContext.PPN = header.PPN;
                                //DbContext.NILAI_PPN = NilaiPPN;
                                //DbContext.PPN_BM = header.PPN_BM;
                                DbContext.NILAI_PPNBM = header.NILAI_PPNBM;
                                DbContext.MATERAI = header.MATERAI;
                                //DbContext.NETTO = NETTO;
                                DbContext.USERNAME = header.USERNAME;
                                DbContext.TGLINPUT = header.TGLINPUT;
                                DbContext.NO_MK = header.NO_MK;
                                DbContext.JENIS_RETUR = header.JENIS_RETUR;
                                DbContext.PRINT_COUNT = header.PRINT_COUNT;
                                DbContext.RETUR_PENUH = header.RETUR_PENUH;
                                DbContext.JTRAN = header.JTRAN;
                                DbContext.AL3 = header.AL3;
                                DbContext.AL2 = header.AL2;
                                DbContext.AL1 = header.AL1;
                                DbContext.AL = header.AL;
                                DbContext.U_MUKA = header.U_MUKA;
                                DbContext.TERM = header.TERM;
                                DbContext.PPN_ditangguhkan = header.PPN_ditangguhkan;
                                DbContext.PPN_Bln_Lapor = header.PPN_Bln_Lapor;
                                DbContext.PPN_Thn_Lapor = header.PPN_Thn_Lapor;
                                DbContext.JENIS = header.JENIS;
                                DbContext.CUST_QQ = header.CUST_QQ;
                                DbContext.NAMA_CUST_QQ = header.NAMA_CUST_QQ;
                                DbContext.TGL_JT_TEMPO = header.TGL_JT_TEMPO;
                                DbContext.KIRIM_PENUH = header.KIRIM_PENUH;
                                DbContext.NO_FAKTUR_PPN_AR = header.NO_FAKTUR_PPN_AR;
                                DbContext.U_MUKA_FA = header.U_MUKA_FA;
                                DbContext.NO_FAKTUR_LAMA = header.NO_FAKTUR_LAMA;
                                DbContext.BATAL = header.BATAL;
                                DbContext.SJ_ADA_FAKTUR = header.SJ_ADA_FAKTUR;
                                DbContext.STATUS_LOADING = header.STATUS_LOADING;
                                DbContext.NO_FA_OUTLET = header.NO_FA_OUTLET;
                                DbContext.NO_LPB = header.NO_LPB;
                                DbContext.NO_PO_CUST = header.NO_PO_CUST;
                                DbContext.GROUP_LIMIT = header.GROUP_LIMIT;
                                DbContext.KODE_ANGKUTAN = header.KODE_ANGKUTAN;
                                DbContext.JENIS_MOBIL = header.JENIS_MOBIL;
                                DbContext.NILAI_ANGKUTAN = header.NILAI_ANGKUTAN;
                                DbContext.PENGIRIM = header.PENGIRIM;
                                //DbContext.NAMA_PENGIRIM = header.NAMA_PENGIRIM;
                                DbContext.ZONA = header.ZONA;
                                DbContext.JAMKIRIM = header.JAMKIRIM;
                                DbContext.UCAPAN = header.UCAPAN;
                                DbContext.N_UCAPAN = header.N_UCAPAN;
                                DbContext.PEMESAN = header.PEMESAN;
                                //DbContext.NAMA_PEMESAN = header.NAMA_PEMESAN;
                                DbContext.KOMISI = header.KOMISI;
                                DbContext.N_KOMISI = header.N_KOMISI;
                                DbContext.JML_VOUCHER = header.JML_VOUCHER;
                                DbContext.NO_SERI_VOUCHER = header.NO_SERI_VOUCHER;
                                DbContext.N_VOUCHER = header.N_VOUCHER;
                                DbContext.APPROVAL = header.APPROVAL;
                                DbContext.TOTAL_TITIPAN = header.TOTAL_TITIPAN;
                                DbContext.SUPP = header.SUPP;
                                DbContext.TGL_POSTING = header.TGL_POSTING;
                                DbContext.USERNAME_POSTING = header.USERNAME_POSTING;
                                DbContext.USERNAME_APPROVAL = header.USERNAME_APPROVAL;
                                //DbContext.RecNum = header.RecNum;

                                SIDbContext.SaveChanges();

                                var listDetail_Remaining = SIDbContext.Database.SqlQuery<SIT01B>("select * from SIT01B NOLOCK where NO_BUKTI = '" + header.NO_BUKTI + "' ").ToList();

                                var NilaiPPN = Math.Ceiling((double)header.PPN * ((double)listDetail_Remaining.Sum(a => a.HARGA) - (double)header.NILAI_DISC) / 100);

                                var NETTO = listDetail_Remaining.Sum(a => a.HARGA) + header.NILAI_PPN + header.MATERAI - header.NILAI_DISC;

                                SIDbContext.Database.ExecuteSqlCommand("update SIT01A set Bruto = '" + listDetail_Remaining.Sum(a => a.HARGA) + "', NETTO = '" + NETTO + "', NILAI_PPN = '" + NilaiPPN + "' where NO_BUKTI = '" + header.NO_BUKTI + "' ");

                                result.code = 200;
                                result.message = "Success";
                                result.data = null;
                                //result.recSit01a = "1";
                            }
                            else
                            {
                                result.code = 400;
                                result.message = "Error, nomor bukti " + header.NO_BUKTI + " tidak ditemukan di Erasoft";
                                result.data = null;
                            }


                        }
                        else
                        {
                            result.code = 400;
                            result.message = "Akun email tidak ditemukan.";
                            result.data = null;
                        }
                    }
                    else
                    {
                        result.code = 400;
                        result.message = "Error";
                        result.data = null;
                    }
                    //                    var EDB = new DatabaseSQL(dbPathEra);
                    //                    string EDBConnID = EDB.GetConnectionString("ConnId");
                    //                    var sqlStorage = new SqlServerStorage(EDBConnID);

                    //                    var Jobclient = new BackgroundJobClient(sqlStorage);

                    //#if (DEBUG || Debug_AWS)
                    //                    Task.Run(() => new PartnerApiControllerJob().prosesStokOpname(data.batch, data.noStok, data.email, data.token, data.isAccurate, data.DatabasePathErasoft, data.dbSourceEra)).Wait();
                    //#else
                    //                    Jobclient.Enqueue<PartnerApiControllerJob>(x => x.prosesStokOpname(data.batch, data.noStok, data.email, data.token, data.isAccurate, data.DatabasePathErasoft, data.dbSourceEra));
                    //#endif

                    
                }
                catch (Exception ex)
                {
                    result.code = 401;
                    result.message = "Error API. Please check Support Masteronline";
                    result.data = null;
                }

                return Json(result);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [System.Web.Http.Route("api/sales-invoice/insert")]
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public async Task<IHttpActionResult> ProsesInsertFaktur([FromBody] dataEra data)
        {

            try
            {
                //var data = new dataEra();

                //var data = JsonConvert.DeserializeObject(data2, typeof(dataEra)) as dataEra;

                JsonApi result;
                string apiKey = "";
                string dbPathEra = "";
                string userName = "";

                var re = Request;
                var headers = re.Headers;

                //data.token = "";

                //if (headers.Contains("X-API-KEY"))
                //{
                //    apiKey = headers.GetValues("X-API-KEY").First();
                //}

                //if (apiKey != "UPDATESTOKMP_M@STERONLINE4P1K3Y")
                //{
                //    result = new JsonApi()
                //    {
                //        code = 401,
                //        message = "Wrong API KEY!",
                //        data = null
                //    };

                //    return Json(result);
                //}

                if (headers.Contains("DBPATHERA"))
                {
                    dbPathEra = headers.GetValues("DBPATHERA").First();
                }
                else
                {
                    result = new JsonApi()
                    {
                        code = 401,
                        message = "DBPATHERA can not be empty!",
                        data = null
                    };

                    return Json(result);
                }

                if (headers.Contains("USERNAME"))
                {
                    userName = headers.GetValues("USERNAME").First();
                }
                else
                {
                    result = new JsonApi()
                    {
                        code = 401,
                        message = "USERNAME can not be empty!",
                        data = null
                    };

                    return Json(result);
                }

                if (data == null)
                {
                    result = new JsonApi()
                    {
                        code = 401,
                        message = "faktur can not be empty!",
                        data = null
                    };

                    return Json(result);
                }

                result = new JsonApi();

                try
                {
                    //var tesinsert = ERAMODbContext.Database.ExecuteSqlCommand("INSERT INTO [temp_api_tokped] (response, cust, tglinput) VALUES ('TES', 'ERASOFT', DATEADD(HOUR, +7, GETUTCDATE()))");
                    //cek dataErasoft ada isinya atau tidak 
                    var tesinsert = data.data.Header.NO_BUKTI;
                    if (!string.IsNullOrEmpty(tesinsert))
                    {
                        //cek sit01a 
                        //disesuaikan dengan sit01a punya erasoft 
                        //cek sit01b 
                        //cek arf01 //detail marketplace 
                        //cek arf01c //pembeli
                        //dimasukkan ke table erasoft 
                        //MO butuh balikan recnum erasoft 

                        //var A = JsonConvert.SerializeObject(data);
                        //var listDetail = data.data.listDetail.ToList();


                        var AKUN_Object = AKUN_ERA_MODbContext.Database.SqlQuery<AKUN>("SELECT * FROM AKUN NOLOCK where Email = '" + userName + "'").FirstOrDefault();

                        if (AKUN_Object != null)
                        {



                            var CS = AKUN_Object.APIConnectionString;
                            var server = CS.Substring(CS.LastIndexOf("Data Source=") + 8);

                            int index = CS.IndexOf(';');
                            if (index > 0)
                            {
                                server = CS.Substring(0, index);
                            }

                            server = server.Substring(12);

                            var DatabaseName = CS.Substring(CS.LastIndexOf("=") + 1);

                            var APIDbContextA = new APIDbContext(server, DatabaseName);

                            var header = data.data.Header;

                            var RecordMarketplace = APIDbContextA.Database.SqlQuery<MARKETPLACE>("select * from Marketplace NOLOCK where Marketplace = '" + header.CUST + "' ").FirstOrDefault();

                            if (RecordMarketplace != null)
                            {


                                var KodeCustomerErasoft = RecordMarketplace.Customer;

                                ARDbContext = new ARDbContext(server, DatabaseName);

                                var ARF01 = ARDbContext.Database.SqlQuery<ARF01>("select * from ARF01 NOLOCK where Cust = '"+ KodeCustomerErasoft +"' ").FirstOrDefault();

                                var NamaCustomerErasoft = ARF01.NAMA;

                                SIDbContext = new SIDbContext(server, DatabaseName);

                                //string Query = "update SIT01A set Tgl = '"+ header.TGL+"'";
                                //SIDbContext.Database.ExecuteSqlCommand("update SIT01A set ");

                                var DbContext = SIDbContext.SIT01A.FirstOrDefault(a => a.NO_BUKTI == header.NO_BUKTI);

                                if (DbContext == null)
                                {
                                    var SIT01A = new SIT01A();

                                    SIT01A.JENIS_FORM = header.JENIS_FORM;
                                    SIT01A.NO_BUKTI = header.NO_BUKTI;
                                    SIT01A.TGL = header.TGL;
                                    SIT01A.STATUS = header.STATUS;
                                    SIT01A.ST_POSTING = header.ST_POSTING;
                                    SIT01A.TGL_KIRIM = header.TGL_KIRIM;
                                    SIT01A.NO_REF = header.NO_REF;
                                    SIT01A.NO_SO = header.NO_SO;
                                    SIT01A.CUST = KodeCustomerErasoft;
                                    SIT01A.NAMA_CUST = NamaCustomerErasoft;
                                    SIT01A.KODE_ALAMAT = header.KODE_ALAMAT;
                                    SIT01A.NO_KENDARAAN = header.NO_KENDARAAN;
                                    SIT01A.SOPIR = header.SOPIR;
                                    SIT01A.KET = header.KET;
                                    SIT01A.VLT = header.VLT;
                                    SIT01A.TUKAR = header.TUKAR;
                                    SIT01A.TUKAR_PPN = header.TUKAR_PPN;
                                    SIT01A.KODE_SALES = header.KODE_SALES;
                                    SIT01A.KODE_WIL = header.KODE_WIL;
                                    SIT01A.NO_F_PAJAK = header.NO_F_PAJAK;
                                    SIT01A.TGL_F_PAJAK = header.TGL_F_PAJAK;
                                    SIT01A.KODE_PROYEK = header.KODE_PROYEK;
                                    SIT01A.BRUTO = header.BRUTO;
                                    SIT01A.DISCOUNT = header.DISCOUNT;
                                    SIT01A.NILAI_DISC = header.NILAI_DISC;
                                    SIT01A.PPN = header.PPN;
                                    SIT01A.NILAI_PPN = header.NILAI_PPN;
                                    //SIT01A.PPN_BM = header.PPN_BM;
                                    SIT01A.NILAI_PPNBM = header.NILAI_PPNBM;
                                    SIT01A.MATERAI = header.MATERAI;
                                    SIT01A.NETTO = header.NETTO;
                                    SIT01A.USERNAME = header.USERNAME;
                                    SIT01A.TGLINPUT = header.TGLINPUT;
                                    SIT01A.NO_MK = header.NO_MK;
                                    SIT01A.JENIS_RETUR = header.JENIS_RETUR;
                                    SIT01A.PRINT_COUNT = header.PRINT_COUNT;
                                    SIT01A.RETUR_PENUH = header.RETUR_PENUH;
                                    SIT01A.JTRAN = header.JTRAN;
                                    SIT01A.AL3 = header.AL3;
                                    SIT01A.AL2 = header.AL2;
                                    SIT01A.AL1 = header.AL1;
                                    SIT01A.AL = header.AL;
                                    SIT01A.U_MUKA = header.U_MUKA;
                                    SIT01A.TERM = header.TERM;
                                    SIT01A.PPN_ditangguhkan = header.PPN_ditangguhkan;
                                    SIT01A.PPN_Bln_Lapor = header.PPN_Bln_Lapor;
                                    SIT01A.PPN_Thn_Lapor = header.PPN_Thn_Lapor;
                                    SIT01A.JENIS = header.JENIS;
                                    SIT01A.CUST_QQ = header.CUST_QQ;
                                    SIT01A.NAMA_CUST_QQ = header.NAMA_CUST_QQ;
                                    SIT01A.TGL_JT_TEMPO = header.TGL_JT_TEMPO;
                                    SIT01A.KIRIM_PENUH = header.KIRIM_PENUH;
                                    SIT01A.NO_FAKTUR_PPN_AR = header.NO_FAKTUR_PPN_AR;
                                    SIT01A.U_MUKA_FA = header.U_MUKA_FA;
                                    SIT01A.NO_FAKTUR_LAMA = header.NO_FAKTUR_LAMA;
                                    SIT01A.BATAL = header.BATAL;
                                    SIT01A.SJ_ADA_FAKTUR = header.SJ_ADA_FAKTUR;
                                    SIT01A.STATUS_LOADING = header.STATUS_LOADING;
                                    SIT01A.NO_FA_OUTLET = header.NO_FA_OUTLET;
                                    SIT01A.NO_LPB = header.NO_LPB;
                                    SIT01A.NO_PO_CUST = header.NO_PO_CUST;
                                    SIT01A.GROUP_LIMIT = header.GROUP_LIMIT;
                                    SIT01A.KODE_ANGKUTAN = header.KODE_ANGKUTAN;
                                    SIT01A.JENIS_MOBIL = header.JENIS_MOBIL;
                                    SIT01A.NILAI_ANGKUTAN = header.NILAI_ANGKUTAN;
                                    SIT01A.PENGIRIM = header.PENGIRIM;
                                    //SIT01A.NAMA_PENGIRIM = header.NAMA_PENGIRIM;
                                    SIT01A.ZONA = header.ZONA;
                                    SIT01A.JAMKIRIM = header.JAMKIRIM;
                                    SIT01A.UCAPAN = header.UCAPAN;
                                    SIT01A.N_UCAPAN = header.N_UCAPAN;
                                    SIT01A.PEMESAN = header.PEMESAN;
                                    //SIT01A.NAMA_PEMESAN = header.NAMA_PEMESAN;
                                    SIT01A.KOMISI = header.KOMISI;
                                    SIT01A.N_KOMISI = header.N_KOMISI;
                                    SIT01A.JML_VOUCHER = header.JML_VOUCHER;
                                    SIT01A.NO_SERI_VOUCHER = header.NO_SERI_VOUCHER;
                                    SIT01A.N_VOUCHER = header.N_VOUCHER;
                                    SIT01A.APPROVAL = header.APPROVAL;
                                    SIT01A.TOTAL_TITIPAN = header.TOTAL_TITIPAN;
                                    SIT01A.SUPP = header.SUPP;
                                    SIT01A.TGL_POSTING = header.TGL_POSTING;
                                    SIT01A.USERNAME_POSTING = header.USERNAME_POSTING;
                                    SIT01A.USERNAME_APPROVAL = header.USERNAME_APPROVAL;
                                    //SIT01A.RecNum = header.RecNum;

                                    SIDbContext.SIT01A.Add(SIT01A);
                                    SIDbContext.SaveChanges();

                                }

                                List<SIT01B> detail = new List<SIT01B>();

                                var ListDetail = data.data.listDetail;
                                foreach (var BARANG in ListDetail)
                                {
                                    string DETAIL_ID = DateTime.UtcNow.AddHours(7).ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds.ToString();


                                    var SIT01B = new SIT01B
                                    {
                                        JENIS_FORM = BARANG.JENIS_FORM,
                                        NO_BUKTI = BARANG.NO_BUKTI,
                                        BRG = BARANG.BRG,
                                        BRG_CUST = BARANG.BRG_CUST,
                                        H_SATUAN = BARANG.H_SATUAN,
                                        SATUAN = BARANG.SATUAN,
                                        QTY = BARANG.QTY,
                                        GUDANG = BARANG.GUDANG,
                                        DISCOUNT = BARANG.DISCOUNT,
                                        NILAI_DISC = BARANG.NILAI_DISC,
                                        HARGA = BARANG.HARGA,
                                        QTY_KIRIM = BARANG.QTY_KIRIM,
                                        AUTO_LOAD = BARANG.AUTO_LOAD,
                                        USERNAME = BARANG.USERNAME,
                                        TGLINPUT = BARANG.TGLINPUT,
                                        QTY_RETUR = BARANG.QTY_RETUR,
                                        WRITE_KONFIG = BARANG.WRITE_KONFIG,
                                        DISCOUNT_2 = BARANG.DISCOUNT_2,
                                        DISCOUNT_3 = BARANG.DISCOUNT_3,
                                        DISCOUNT_4 = BARANG.DISCOUNT_4,
                                        DISCOUNT_5 = BARANG.DISCOUNT_5,
                                        NILAI_DISC_1 = BARANG.NILAI_DISC_1,
                                        NILAI_DISC_2 = BARANG.NILAI_DISC_2,
                                        NILAI_DISC_3 = BARANG.NILAI_DISC_3,
                                        NILAI_DISC_4 = BARANG.NILAI_DISC_4,
                                        NILAI_DISC_5 = BARANG.NILAI_DISC_5,
                                        TOTAL_LOT = BARANG.TOTAL_LOT,
                                        TOTAL_QTY = BARANG.TOTAL_QTY,
                                        TGL_KIRIM = BARANG.TGL_KIRIM,
                                        NO_URUT_SO = BARANG.NO_URUT_SO,
                                        CATATAN = BARANG.CATATAN,
                                        QTY_BESAR = BARANG.QTY_BESAR,
                                        QTY_KECIL = BARANG.QTY_KECIL,
                                        BRG_SO = DETAIL_ID,
                                        TRANS_NO_URUT = BARANG.TRANS_NO_URUT,
                                        SATUAN_N = BARANG.SATUAN_N,
                                        QTY_N = BARANG.QTY_N,
                                        NTITIPAN = BARANG.NTITIPAN,
                                        DISC_TITIPAN = BARANG.DISC_TITIPAN,
                                        QOH = BARANG.QOH
                                    };
                                    SIDbContext.SIT01B.Add(SIT01B);

                                    detail.Add(SIT01B);
                                    SIDbContext.SaveChanges();

                                    var ResultData = new data()
                                    {
                                        no_bukti = BARANG.NO_BUKTI,
                                        id_detail_terima = DETAIL_ID,
                                        id_detail_kirim = BARANG.DETAIL_ID
                                    };

                                    result.data.Add(ResultData);
                                }

                                var NilaiPPN = Math.Ceiling((double)DbContext.PPN * ((double)detail.Sum(a => a.HARGA) - (double)DbContext.NILAI_DISC) / 100);

                                var NETTO = detail.Sum(a => a.HARGA) + DbContext.NILAI_PPN + DbContext.MATERAI - DbContext.NILAI_DISC;

                                if (DbContext != null)
                                {
                                   
                                    DbContext.TGL = header.TGL;
                                    DbContext.STATUS = header.STATUS;
                                    DbContext.ST_POSTING = header.ST_POSTING;
                                    DbContext.TGL_KIRIM = header.TGL_KIRIM;
                                    DbContext.NO_REF = header.NO_REF;
                                    DbContext.NO_SO = header.NO_SO;
                                    DbContext.CUST = KodeCustomerErasoft;
                                    DbContext.NAMA_CUST = NamaCustomerErasoft;
                                    DbContext.KODE_ALAMAT = header.KODE_ALAMAT;
                                    DbContext.NO_KENDARAAN = header.NO_KENDARAAN;
                                    DbContext.SOPIR = header.SOPIR;
                                    DbContext.KET = header.KET;
                                    DbContext.VLT = header.VLT;
                                    DbContext.TUKAR = header.TUKAR;
                                    DbContext.TUKAR_PPN = header.TUKAR_PPN;
                                    DbContext.KODE_SALES = header.KODE_SALES;
                                    DbContext.KODE_WIL = header.KODE_WIL;
                                    DbContext.NO_F_PAJAK = header.NO_F_PAJAK;
                                    DbContext.TGL_F_PAJAK = header.TGL_F_PAJAK;
                                    DbContext.KODE_PROYEK = header.KODE_PROYEK;
                                    DbContext.BRUTO = detail.Sum(a => a.HARGA);
                                    DbContext.DISCOUNT = header.DISCOUNT;
                                    DbContext.NILAI_DISC = header.NILAI_DISC;
                                    DbContext.PPN = header.PPN;
                                    DbContext.NILAI_PPN = NilaiPPN;
                                    //DbContext.PPN_BM = header.PPN_BM;
                                    DbContext.NILAI_PPNBM = header.NILAI_PPNBM;
                                    DbContext.MATERAI = header.MATERAI;
                                    DbContext.NETTO = NETTO;
                                    DbContext.USERNAME = header.USERNAME;
                                    DbContext.TGLINPUT = header.TGLINPUT;
                                    DbContext.NO_MK = header.NO_MK;
                                    DbContext.JENIS_RETUR = header.JENIS_RETUR;
                                    DbContext.PRINT_COUNT = header.PRINT_COUNT;
                                    DbContext.RETUR_PENUH = header.RETUR_PENUH;
                                    DbContext.JTRAN = header.JTRAN;
                                    DbContext.AL3 = header.AL3;
                                    DbContext.AL2 = header.AL2;
                                    DbContext.AL1 = header.AL1;
                                    DbContext.AL = header.AL;
                                    DbContext.U_MUKA = header.U_MUKA;
                                    DbContext.TERM = header.TERM;
                                    DbContext.PPN_ditangguhkan = header.PPN_ditangguhkan;
                                    DbContext.PPN_Bln_Lapor = header.PPN_Bln_Lapor;
                                    DbContext.PPN_Thn_Lapor = header.PPN_Thn_Lapor;
                                    DbContext.JENIS = header.JENIS;
                                    DbContext.CUST_QQ = header.CUST_QQ;
                                    DbContext.NAMA_CUST_QQ = header.NAMA_CUST_QQ;
                                    DbContext.TGL_JT_TEMPO = header.TGL_JT_TEMPO;
                                    DbContext.KIRIM_PENUH = header.KIRIM_PENUH;
                                    DbContext.NO_FAKTUR_PPN_AR = header.NO_FAKTUR_PPN_AR;
                                    DbContext.U_MUKA_FA = header.U_MUKA_FA;
                                    DbContext.NO_FAKTUR_LAMA = header.NO_FAKTUR_LAMA;
                                    DbContext.BATAL = header.BATAL;
                                    DbContext.SJ_ADA_FAKTUR = header.SJ_ADA_FAKTUR;
                                    DbContext.STATUS_LOADING = header.STATUS_LOADING;
                                    DbContext.NO_FA_OUTLET = header.NO_FA_OUTLET;
                                    DbContext.NO_LPB = header.NO_LPB;
                                    DbContext.NO_PO_CUST = header.NO_PO_CUST;
                                    DbContext.GROUP_LIMIT = header.GROUP_LIMIT;
                                    DbContext.KODE_ANGKUTAN = header.KODE_ANGKUTAN;
                                    DbContext.JENIS_MOBIL = header.JENIS_MOBIL;
                                    DbContext.NILAI_ANGKUTAN = header.NILAI_ANGKUTAN;
                                    DbContext.PENGIRIM = header.PENGIRIM;
                                    //DbContext.NAMA_PENGIRIM = header.NAMA_PENGIRIM;
                                    DbContext.ZONA = header.ZONA;
                                    DbContext.JAMKIRIM = header.JAMKIRIM;
                                    DbContext.UCAPAN = header.UCAPAN;
                                    DbContext.N_UCAPAN = header.N_UCAPAN;
                                    DbContext.PEMESAN = header.PEMESAN;
                                    //DbContext.NAMA_PEMESAN = header.NAMA_PEMESAN;
                                    DbContext.KOMISI = header.KOMISI;
                                    DbContext.N_KOMISI = header.N_KOMISI;
                                    DbContext.JML_VOUCHER = header.JML_VOUCHER;
                                    DbContext.NO_SERI_VOUCHER = header.NO_SERI_VOUCHER;
                                    DbContext.N_VOUCHER = header.N_VOUCHER;
                                    DbContext.APPROVAL = header.APPROVAL;
                                    DbContext.TOTAL_TITIPAN = header.TOTAL_TITIPAN;
                                    DbContext.SUPP = header.SUPP;
                                    DbContext.TGL_POSTING = header.TGL_POSTING;
                                    DbContext.USERNAME_POSTING = header.USERNAME_POSTING;
                                    DbContext.USERNAME_APPROVAL = header.USERNAME_APPROVAL;
                                    //DbContext.RecNum = header.RecNum;

                                    //SIDbContext.SIT01B.Add(DbContext);
                                    SIDbContext.SaveChanges();

                                }
                                else 
                                {
                                    SIDbContext.Database.ExecuteSqlCommand("update SIT01A set Bruto = '"+ detail.Sum(a => a.HARGA) + "', NETTO = '"+ NETTO + "', NILAI_PPN = '"+ NilaiPPN +"' where NO_BUKTI = '"+ header.NO_BUKTI +"' ");
                                }

                                //SIDbContext.SaveChanges();

                                result.code = 200;
                                result.message = "Success";
                                //result.data = null;
                                //result.recSit01a = "1";
                            }
                            else
                            {
                                result.code = 400;
                                result.message = "Kode Cust tidak ditemukan di Erasoft.";
                                result.data = null;

                            }
                        }
                        else
                        {
                            result = new JsonApi()
                            {
                                code = 401,
                                message = "USERNAME tidak ditemukan.",
                                data = null
                            };

                            return Json(result);
                        }
                    }
                    else
                    {
                        result.code = 400;
                        result.message = "Faktur tidak boleh kosong.";
                        result.data = null;
                    }
                    //                    var EDB = new DatabaseSQL(dbPathEra);
                    //                    string EDBConnID = EDB.GetConnectionString("ConnId");
                    //                    var sqlStorage = new SqlServerStorage(EDBConnID);

                    //                    var Jobclient = new BackgroundJobClient(sqlStorage);

                    //#if (DEBUG || Debug_AWS)
                    //                    Task.Run(() => new PartnerApiControllerJob().prosesStokOpname(data.batch, data.noStok, data.email, data.token, data.isAccurate, data.DatabasePathErasoft, data.dbSourceEra)).Wait();
                    //#else
                    //                    Jobclient.Enqueue<PartnerApiControllerJob>(x => x.prosesStokOpname(data.batch, data.noStok, data.email, data.token, data.isAccurate, data.DatabasePathErasoft, data.dbSourceEra));
                    //#endif


                }
                catch (Exception ex)
                {
                    result.code = 401;
                    result.message = "Error API. Please check Support Masteronline";
                    result.data = null;
                }

                return Json(result);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [System.Web.Http.Route("api/sales-invoice/delete-item")]
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public async Task<IHttpActionResult> ProsesDeleteDetailFaktur([FromBody] dataEra data)
        {

            try
            {
                //var data = new dataEra();

                //var data = JsonConvert.DeserializeObject(data2, typeof(dataEra)) as dataEra;

                JsonApi result;
                string apiKey = "";
                string dbPathEra = "";
                string userName = "";

                var re = Request;
                var headers = re.Headers;

                //data.token = "";

                //if (headers.Contains("X-API-KEY"))
                //{
                //    apiKey = headers.GetValues("X-API-KEY").First();
                //}

                //if (apiKey != "UPDATESTOKMP_M@STERONLINE4P1K3Y")
                //{
                //    result = new JsonApi()
                //    {
                //        code = 401,
                //        message = "Wrong API KEY!",
                //        data = null
                //    };

                //    return Json(result);
                //}

                if (headers.Contains("DBPATHERA"))
                {
                    dbPathEra = headers.GetValues("DBPATHERA").First();
                }
                else
                {
                    result = new JsonApi()
                    {
                        code = 401,
                        message = "DBPATHERA can not be empty!",
                        data = null
                    };

                    return Json(result);
                }

                if (headers.Contains("USERNAME"))
                {
                    userName = headers.GetValues("USERNAME").First();
                }
                else
                {
                    result = new JsonApi()
                    {
                        code = 401,
                        message = "USERNAME can not be empty!",
                        data = null
                    };

                    return Json(result);
                }

                if (data == null)
                {
                    result = new JsonApi()
                    {
                        code = 401,
                        message = "faktur can not be empty!",
                        data = null
                    };

                    return Json(result);
                }

                result = new JsonApi();

                try
                {
                    //var tesinsert = ERAMODbContext.Database.ExecuteSqlCommand("INSERT INTO [temp_api_tokped] (response, cust, tglinput) VALUES ('TES', 'ERASOFT', DATEADD(HOUR, +7, GETUTCDATE()))");
                    //cek dataErasoft ada isinya atau tidak 
                    var tesinsert = data.data.Header.NO_BUKTI;
                    if (!string.IsNullOrEmpty(tesinsert))
                    {
                        //cek sit01a 
                        //disesuaikan dengan sit01a punya erasoft 
                        //cek sit01b 
                        //cek arf01 //detail marketplace 
                        //cek arf01c //pembeli
                        //dimasukkan ke table erasoft 
                        //MO butuh balikan recnum erasoft 

                        //var A = JsonConvert.SerializeObject(data);
                        //var listDetail = data.data.listDetail.ToList();
                        var AKUN_Object = AKUN_ERA_MODbContext.Database.SqlQuery<AKUN>("SELECT * FROM AKUN NOLOCK where Email = '" + userName + "'").FirstOrDefault();

                        if (AKUN_Object != null)
                        {
                            var CS = AKUN_Object.SIConnectionString;
                            var server = CS.Substring(CS.LastIndexOf("Data Source=") + 8);

                            int index = CS.IndexOf(';');
                            if (index > 0)
                            {
                                server = CS.Substring(0, index);
                            }

                            server = server.Substring(12);

                            var DatabaseName = CS.Substring(CS.LastIndexOf("=") + 1);

                            var header = data.data.Header;
                            SIDbContext = new SIDbContext(server, DatabaseName);

                            //string Query = "update SIT01A set Tgl = '"+ header.TGL+"'";
                            //SIDbContext.Database.ExecuteSqlCommand("update SIT01A set ");

                            //jangan lupa lakukan pengecekan ke SIT01A ada record atau tidak baru lakukan fungsi di bawah.

                            var ListDetail = data.data.listDetail;
                            foreach (var BARANG in ListDetail)
                            {
                                var DetailRecord = SIDbContext.SIT01B.First(c => c.BRG_SO == BARANG.DETAIL_ID);

                                SIDbContext.SIT01B.Remove(DetailRecord);

                                SIDbContext.SaveChanges();

                            }

                            var listDetail_Remaining = SIDbContext.Database.SqlQuery<SIT01B>("select * from SIT01B NOLOCK where NO_BUKTI = '" + header.NO_BUKTI + "' ").ToList();

                            var NilaiPPN = Math.Ceiling((double)header.PPN * ((double)listDetail_Remaining.Sum(a => a.HARGA) - (double)header.NILAI_DISC) / 100);

                            var NETTO = listDetail_Remaining.Sum(a => a.HARGA) + header.NILAI_PPN + header.MATERAI - header.NILAI_DISC;

                            SIDbContext.Database.ExecuteSqlCommand("update SIT01A set Bruto = '" + listDetail_Remaining.Sum(a => a.HARGA) + "', NETTO = '" + NETTO + "', NILAI_PPN = '" + NilaiPPN + "' where NO_BUKTI = '" + header.NO_BUKTI + "' ");

                            result.code = 200;
                            result.message = "Success";
                            result.data = null;
                            //result.recSit01a = "1";
                        }
                    }
                    else
                    {
                        result.code = 400;
                        result.message = "Error, nomor bukti tidak boleh kosong.";
                        result.data = null;
                    }
                    //                    var EDB = new DatabaseSQL(dbPathEra);
                    //                    string EDBConnID = EDB.GetConnectionString("ConnId");
                    //                    var sqlStorage = new SqlServerStorage(EDBConnID);

                    //                    var Jobclient = new BackgroundJobClient(sqlStorage);

                    //#if (DEBUG || Debug_AWS)
                    //                    Task.Run(() => new PartnerApiControllerJob().prosesStokOpname(data.batch, data.noStok, data.email, data.token, data.isAccurate, data.DatabasePathErasoft, data.dbSourceEra)).Wait();
                    //#else
                    //                    Jobclient.Enqueue<PartnerApiControllerJob>(x => x.prosesStokOpname(data.batch, data.noStok, data.email, data.token, data.isAccurate, data.DatabasePathErasoft, data.dbSourceEra));
                    //#endif


                }
                catch (Exception ex)
                {
                    result.code = 401;
                    result.message = "Error API. Please check Support Masteronline";
                    result.data = null;
                }

                return Json(result);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [System.Web.Http.Route("api/sales-invoice/delete-invoice")]
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public async Task<IHttpActionResult> ProsesDeleteHeaderFaktur([FromBody] dataEra data)
        {

            try
            {
                //var data = new dataEra();

                //var data = JsonConvert.DeserializeObject(data2, typeof(dataEra)) as dataEra;

                JsonApi result;
                string apiKey = "";
                string dbPathEra = "";
                string userName = "";

                var re = Request;
                var headers = re.Headers;

                //data.token = "";

                //if (headers.Contains("X-API-KEY"))
                //{
                //    apiKey = headers.GetValues("X-API-KEY").First();
                //}

                //if (apiKey != "UPDATESTOKMP_M@STERONLINE4P1K3Y")
                //{
                //    result = new JsonApi()
                //    {
                //        code = 401,
                //        message = "Wrong API KEY!",
                //        data = null
                //    };

                //    return Json(result);
                //}

                if (headers.Contains("DBPATHERA"))
                {
                    dbPathEra = headers.GetValues("DBPATHERA").First();
                }
                else
                {
                    result = new JsonApi()
                    {
                        code = 401,
                        message = "DBPATHERA can not be empty!",
                        data = null
                    };

                    return Json(result);
                }

                if (headers.Contains("USERNAME"))
                {
                    userName = headers.GetValues("USERNAME").First();
                }
                else
                {
                    result = new JsonApi()
                    {
                        code = 401,
                        message = "USERNAME can not be empty!",
                        data = null
                    };

                    return Json(result);
                }

                if (data == null)
                {
                    result = new JsonApi()
                    {
                        code = 401,
                        message = "faktur can not be empty!",
                        data = null
                    };

                    return Json(result);
                }

                result = new JsonApi();

                try
                {
                    //var tesinsert = ERAMODbContext.Database.ExecuteSqlCommand("INSERT INTO [temp_api_tokped] (response, cust, tglinput) VALUES ('TES', 'ERASOFT', DATEADD(HOUR, +7, GETUTCDATE()))");
                    //cek dataErasoft ada isinya atau tidak 
                    var tesinsert = data.data.Header.NO_BUKTI;
                    if (!string.IsNullOrEmpty(tesinsert))
                    {
                        //cek sit01a 
                        //disesuaikan dengan sit01a punya erasoft 
                        //cek sit01b 
                        //cek arf01 //detail marketplace 
                        //cek arf01c //pembeli
                        //dimasukkan ke table erasoft 
                        //MO butuh balikan recnum erasoft 

                        //var A = JsonConvert.SerializeObject(data);
                        //var listDetail = data.data.listDetail.ToList();

                        //ke tabel akun untuk get db
                        //lakukan validasi standar seperti fungsi insert faktur

                        var AKUN_Object = AKUN_ERA_MODbContext.Database.SqlQuery<AKUN>("SELECT * FROM AKUN NOLOCK where Email = '" + userName + "'").FirstOrDefault();

                        if (AKUN_Object != null)
                        {
                            var header = data.data.Header;
                            SIDbContext = new SIDbContext("");

                            var Record = SIDbContext.SIT01A.First(a => a.NO_BUKTI == header.NO_BUKTI);

                            if (Record != null)
                            {
                                SIDbContext.SIT01A.Remove(Record);
                                SIDbContext.SaveChanges();

                                result.code = 200;
                                result.message = "Success";
                                result.data = null;
                                //result.recSit01a = "1";
                            }
                            else
                            {
                                result.code = 400;
                                result.message = "No Bukti "+ header.NO_BUKTI +" tidak ditemukan.";
                                result.data = null;

                            }
                        }
                    }
                    else
                    {
                        result.code = 400;
                        result.message = "Error";
                        result.data = null;
                    }
                    //                    var EDB = new DatabaseSQL(dbPathEra);
                    //                    string EDBConnID = EDB.GetConnectionString("ConnId");
                    //                    var sqlStorage = new SqlServerStorage(EDBConnID);

                    //                    var Jobclient = new BackgroundJobClient(sqlStorage);

                    //#if (DEBUG || Debug_AWS)
                    //                    Task.Run(() => new PartnerApiControllerJob().prosesStokOpname(data.batch, data.noStok, data.email, data.token, data.isAccurate, data.DatabasePathErasoft, data.dbSourceEra)).Wait();
                    //#else
                    //                    Jobclient.Enqueue<PartnerApiControllerJob>(x => x.prosesStokOpname(data.batch, data.noStok, data.email, data.token, data.isAccurate, data.DatabasePathErasoft, data.dbSourceEra));
                    //#endif


                }
                catch (Exception ex)
                {
                    result.code = 401;
                    result.message = "Error API. Please check Support Masteronline";
                    result.data = null;
                }

                return Json(result);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public string MutasiStok(string jsonData)
        {
            string ret = "";
//#if (AWS)
//	        //string url = "https://api.masteronline.co.id/webhook/api/invoice/";
//#else

//#endif

            string url = "https://devapi.masteronline.co.id/api/receivestock-erasoft";

            HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create(url);

            myReq.Method = "POST";
            myReq.Accept = "application/x-www-form-urlencoded";
            myReq.ContentType = "application/json";

            myReq.ContentLength = 0;


            string responseFromServer = "";

            try
            {
                myReq.ContentLength = jsonData.Length;
                using (var dataStream = myReq.GetRequestStream())
                {
                    dataStream.Write(System.Text.Encoding.UTF8.GetBytes(jsonData), 0, jsonData.Length);
                }
                using (WebResponse response = myReq.GetResponse())
                {
                    using (Stream stream = response.GetResponseStream())
                    {
                        StreamReader reader = new StreamReader(stream);
                        responseFromServer = reader.ReadToEnd();

                    }
                }
            }
            catch (Exception ex)
            {

            }

            //cek jika response from server tidak blank atau null maka update ke STF09_API set Status = '1' berdasarkan recnum
            return "OK";
        }

        public async Task<string> MutasiStok_2(string jsonData, STDbContext stDB, APIDbContext apiDB, int RECNUM)
        {
            try
            {
                string urll = "https://devapi.masteronline.co.id/api/receivestock-erasoft";

                string responseFromServer = "";
                var isSuccess = false;
                var client = new HttpClient();
                //client.DefaultRequestHeaders.Add("Authorization", ("Bearer " + iden.token));
                //client.DefaultRequestHeaders.Add("Authorization", ("Bearer " + "123"));

                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json");
                HttpResponseMessage clientResponse = await client.PostAsync(urll, content);

                if (clientResponse != null)
                {
                    if (clientResponse.IsSuccessStatusCode)
                    {
                        isSuccess = true;
                    }
                    using (HttpContent responseContent = clientResponse.Content)
                    {
                        using (var reader = new StreamReader(await responseContent.ReadAsStreamAsync()))
                        {
                            responseFromServer = await reader.ReadToEndAsync();
                        }
                    };
                }

                var httpReason = clientResponse.ReasonPhrase;
                if (!string.IsNullOrEmpty(responseFromServer))
                {
                    try
                    {
                        //ActOrderResult result = Newtonsoft.Json.JsonConvert.DeserializeObject(responseFromServer, typeof(ActOrderResult)) as ActOrderResult;
                        if (isSuccess)
                        {
                            //update ke STF09_API set Status
                            //manageAPI_LOG_MARKETPLACE(api_status.Success, ErasoftDbContext, iden, currentLog);

                            stDB.Database.ExecuteSqlCommand("update STF09A_API set Status = '1' where Recnum = '" + RECNUM + "' ");
                            apiDB.Database.ExecuteSqlCommand("insert into LOG_API values (DATEADD(HOUR, +7, GETUTCDATE()), 'Log_ProsesMutasiStok','" + jsonData+"', '"+responseFromServer+"', '1')");
                        }
                        else if (httpReason == "Bad Request")
                        {
                            //manageAPI_LOG_MARKETPLACE(api_status.Success, ErasoftDbContext, iden, currentLog);
                            //stDB.Database.ExecuteSqlCommand("update STF09A_API set Status = '1' where Recnum = '" + RECNUM + "' ");
                            apiDB.Database.ExecuteSqlCommand("insert into LOG_API values (DATEADD(HOUR, +7, GETUTCDATE()), 'Log_ProsesMutasiStok','" + jsonData + "', '" + responseFromServer + "', '1')");
                        }
                        else
                        {
                            //manageAPI_LOG_MARKETPLACE(api_status.Failed, ErasoftDbContext, iden, currentLog);
                            //stDB.Database.ExecuteSqlCommand("update STF09A_API set Status = '1' where Recnum = '" + RECNUM + "' ");
                            apiDB.Database.ExecuteSqlCommand("insert into LOG_API values (DATEADD(HOUR, +7, GETUTCDATE()), 'Log_ProsesMutasiStok','" + jsonData + "', '" + responseFromServer + "', '1')");
                        }

                    }
                    catch (Exception ex)
                    {
                        //insert ke table log
                        apiDB.Database.ExecuteSqlCommand("insert into LOG_API values (DATEADD(HOUR, +7, GETUTCDATE()), 'Log_ProsesMutasiStok','" + jsonData + "', '" + responseFromServer + "', '1')");
                    }
                }
            }
            catch (Exception ex)
            {
                //insert ke table log
                apiDB.Database.ExecuteSqlCommand("insert into LOG_API values (DATEADD(HOUR, +7, GETUTCDATE()), 'Log_ProsesMutasiStok','" + jsonData + "', '', '1')");
            }
            return "OK";
            
        }

        //[RecurringJob("*/5 * * * *", "SE Asia Standard Time", "default", RecurringJobId = "ProsesStokOpname")]
        //[RecurringJob("0 6,18 */1 * *", "SE Asia Standard Time", "default", RecurringJobId = "ProsesStokOpname")] change to 30 7,14 * * sun-sat
        //[RecurringJob("30 7,14 * * sun-sat", "SE Asia Standard Time", "default", RecurringJobId = "ProsesStokOpname")]
        [RecurringJob("5 * * * *", "SE Asia Standard Time", "default", RecurringJobId = "ProsesMutasiStok")]
        public void ProsesMutasiStok()
        {
            //var tesinsert = ERAMODbContext.Database.ExecuteSqlCommand("INSERT INTO [temp_api_tokped] (response, cust, tglinput) VALUES ('TES', 'ERASOFT', DATEADD(HOUR, +7, GETUTCDATE()))");
            //SELECT KE STF09A_API
            //HASIL SELECT NYA DIMASUKKAN KE DALAM CLASS STF09A_Temp
            //CONSERT KE JSON STF09A_Temp
            //PANGGIL MUTASISTOK DENGAN PARAMETER JSON STF09A_Temp
            //var listBrgRISA = ERADbContext.Database.SqlQuery<STF09A_API>("SELECT * FROM STF09A_API").ToList();


            var AKUN_Object = AKUN_ERA_MODbContext.Database.SqlQuery<AKUN>("SELECT * FROM AKUN NOLOCK").ToList();


            if(AKUN_Object.Count() > 0)
            {
                foreach (var AKUN in AKUN_Object)
                {
                    //lakukan insert ke table log
                    //nama functionnya : Log_ProsesMutasiStok, tgl diisi tgl hari ini. getdate(). +hours 7.
                    //json diisi email.
                    var CS = AKUN.STConnectionString;
                    var server = CS.Substring(CS.LastIndexOf("Data Source=") + 8);

                    int index = CS.IndexOf(';');
                    if (index > 0)
                    {
                        server = CS.Substring(0, index);
                    }

                    server = server.Substring(12);

                    var DatabaseName = CS.Substring(CS.LastIndexOf("=") + 1);

                    var STDbContextA = new STDbContext(server, DatabaseName);
                    var APIDbContextA = new APIDbContext(server, DatabaseName);

                    APIDbContextA.Database.ExecuteSqlCommand("insert into LOG_API values (getdate(), 'Log_ProsesMutasiStok','" + AKUN.Email +"', '', '1')");

                    var API_Integration = STDbContextA.Database.SqlQuery<API_Integration>("SELECT * FROM API_Integration NOLOCK").FirstOrDefault();

                    if (API_Integration != null)
                    {
                        var listSTF09A_API = STDbContextA.Database.SqlQuery<STF09A_API_Erasoft>("SELECT * FROM STF09A_API NOLOCK where Status != '1' ").ToList();

                        if (listSTF09A_API.Count() > 0)
                        {

                            foreach (var STF09A_API in listSTF09A_API)
                            {
                                var listSTF09A = new List<STF09A_API>();

                                var STF09A = new STF09A_API
                                {
                                    Brg = STF09A_API.Brg,
                                    Tgl = STF09A_API.Tgl,
                                    Bukti = STF09A_API.Bukti,
                                    MK = STF09A_API.MK,
                                    Ket = STF09A_API.Ket,
                                    Qty = STF09A_API.Qty,
                                    HPokok = STF09A_API.HPokok,
                                    GD = STF09A_API.GD,
                                    DR_GD = STF09A_API.DR_GD,
                                    id = Convert.ToString(STF09A_API.RECNUM),
                                    status_transaksi = STF09A_API.status_transaksi
                                };

                                listSTF09A.Add(STF09A);

                                //var D = SIDbContext.Database.SqlQuery<SIF01>("select top(5) * from SIF01 NOLOCK").ToList();

                                var STF09A_API_Bundle = new listStf09a_API();

                                STF09A_API_Bundle.databaseId = API_Integration.databaseId;
                                STF09A_API_Bundle.email = API_Integration.email;
                                STF09A_API_Bundle.fs_id = API_Integration.fs_id;
                                STF09A_API_Bundle.token = API_Integration.token;
                                STF09A_API_Bundle.stf09a_list = listSTF09A;
                                //STF09A_API_Bundle.stf09a = STF09A;

                                //var B = ERAMODbContext.ARF01.Where(a => a.CUST == "00001").FirstOrDefault();
                                var A = JsonConvert.SerializeObject(STF09A_API_Bundle);
                                if (!string.IsNullOrEmpty(A))
                                {
                                    MutasiStok_2(A, STDbContextA, APIDbContextA, STF09A_API.RECNUM);
                                }
                            }
                        }
                    }
                }
            }
           

            //    var CS = AKUN_Object.STConnectionString;
            //var server = CS.Substring(CS.LastIndexOf("Data Source=") + 8);

            //int index = CS.IndexOf(';');
            //if (index > 0)
            //{
            //    server = CS.Substring(0, index);
            //}

            //server = server.Substring(12);

            //var DatabaseName = CS.Substring(CS.LastIndexOf("=") + 1);

            //var STDbContextA = new STDbContext(server, DatabaseName);

            //var listSTF09A = STDbContextA.Database.SqlQuery<STF09A_API>("SELECT * FROM STF09A_API NOLOCK").ToList();


            //var D = SIDbContext.Database.SqlQuery<SIF01>("select top(5) * from SIF01 NOLOCK").ToList();

            //var STF09A_API_Bundle = new listStf09a_API();

            //STF09A_API_Bundle.databaseId = "1";
            //STF09A_API_Bundle.email = "2";
            //STF09A_API_Bundle.fs_id = "3";
            //STF09A_API_Bundle.token = "4";
            //STF09A_API_Bundle.stf09a_list = listSTF09A;

            ////var B = ERAMODbContext.ARF01.Where(a => a.CUST == "00001").FirstOrDefault();
            //var A = JsonConvert.SerializeObject(STF09A_API_Bundle);
            //if (!string.IsNullOrEmpty(A))
            //{
            //    MutasiStok(A);
            //}

            ////string ret = "";
            //// create bukti stok opname
            //var lastBukti = GenerateAutoNumber(ERAMODbContext, "OP", "STT04A", "NOBUK");
            //var noStokOP = "OP" + DateTime.UtcNow.AddHours(7).Year.ToString().Substring(2, 2) + Convert.ToString(Convert.ToInt32(lastBukti) + 1).PadLeft(6, '0');
            //var namaGudang = ERAMODbContext.STF18.Where(a => a.Kode_Gudang == "001").SingleOrDefault().Nama_Gudang;

            //var stt04a = new STT04A
            //{
            //    GUD = "001",
            //    NAMA_GUDANG = namaGudang,
            //    USERNAME = "AUTOPROCESS",
            //    NOBUK = noStokOP,
            //    TGL = DateTime.Today,
            //    POSTING = "0"
            //};

            //ERAMODbContext.STT04A.Add(stt04a);
            ////ERAMODbContext.SaveChanges();

            //var listBrgMO = ERAMODbContext.STF02.Select(a => a.BRG).ToList();
            //var listBrgRISA = ERADbContext.STOKINFO.Where(b => listBrgMO.Contains(b.TYPE)).ToList();
            //foreach (var barang in listBrgRISA)
            //{
            //    var stt04b = new STT04B
            //    {
            //        Gud = "001",
            //        Brg = barang.TYPE,
            //        Qty = barang.STOCK_COMBINED,
            //        Tgl = DateTime.Today,
            //        HPokok = 0,
            //        BK = "",
            //        Stn = "",
            //        WO = "",
            //        Nama_Barang = barang.NAMA,
            //        Qty_Berat = 0,
            //        QTY_KECIL = 0,
            //        QTY_BESAR = 0,
            //        QTY_3 = 0,
            //        QTY_4 = 0,
            //        LKS = "",
            //        USERNAME = "AUTOPROCESS",
            //        NOBUK = noStokOP
            //    };
            //    ERAMODbContext.STT04B.Add(stt04b);
            //    ERAMODbContext.SaveChanges();
            //}

            //// proses stok opname
            //List<STT01A> newSTT01A = new List<STT01A>();
            //List<STT01B> newSTT01B = new List<STT01B>();

            //var stokOP = ERAMODbContext.STT04A.Where(a => a.NOBUK == noStokOP).Single();
            //var stokOPDetail = ERAMODbContext.STT04B.Where(b => b.NOBUK == noStokOP).ToList();

            //var lastBuktiOM = GenerateAutoNumber(ERAMODbContext, "OM", "STT01A", "Nobuk");
            //var noStokOM = "OM" + DateTime.UtcNow.AddHours(7).Year.ToString().Substring(2, 2) + Convert.ToString(Convert.ToInt32(lastBuktiOM) + 1).PadLeft(6, '0');
            //var lastBuktiOK = GenerateAutoNumber(ERAMODbContext, "OK", "STT01A", "Nobuk");
            //var noStokOK = "OK" + DateTime.UtcNow.AddHours(7).Year.ToString().Substring(2, 2) + Convert.ToString(Convert.ToInt32(lastBuktiOK) + 1).PadLeft(6, '0');

            //int jmRowOM = 0; int jmRowOK = 0;
            //foreach (var item in stokOPDetail)
            //{
            //    //Cek Stok Fisik
            //    string sSQL = "SELECT ISNULL(SUM(QAwal+QM1+QM2+QM3+QM4+QM5+QM6+QM7+QM8+QM9+QM10+QM11+QM12) - SUM(QK1+QK2+QK3+QK4+QK5+QK6+QK7+QK8+QK9+QK10+QK11+QK12), 0)  AS STOK_FISIK " +
            //        "FROM STF08A WHERE Tahun = YEAR(GETDATE()) ";
            //    sSQL += "AND BRG='" + item.Brg + "' AND GD = '" + item.Gud + "'";
            //    var stok = ERAMODbContext.Database.SqlQuery<getStokFisik>(sSQL).Single();

            //    STT01A stokOpnameA = new STT01A
            //    {
            //        Jenis_Form = 1,
            //        STATUS_LOADING = "0",
            //        Tgl = stokOP.TGL,
            //        Satuan = "",
            //        Ket = "",
            //        ST_Cetak = "",
            //        ST_Posting = "",
            //        JRef = "6",
            //        Ref = stokOP.NOBUK,
            //        UserName = stokOP.USERNAME,
            //        TglInput = DateTime.Now,
            //        Retur_Penuh = false,
            //        Terima_Penuh = false,
            //        VALUTA = "IDR",
            //        TUKAR = 1,
            //        TERIMA_PENUH_PO_QC = false,
            //        JLH_KARYAWAN = 0,
            //        NILAI_ANGKUTAN = 0,
            //        KOLI = 0,
            //        BERAT = 0,
            //        VOLUME = 0
            //    };

            //    STT01B stokOpnameB = new STT01B
            //    {
            //        Jenis_Form = 1,
            //        Kobar = item.Brg,
            //        Satuan = "2",
            //        Harsat = 0,
            //        Harga = 0,
            //        UserName = stokOP.USERNAME,
            //        TglInput = DateTime.Now,
            //        Qty_Retur = 0,
            //        Qty_Berat = 0,
            //        TOTAL_LOT = 0,
            //        TOTAL_QTY = 0,
            //        QTY_TERIMA = 0,
            //        QTY_CLAIM = 0,
            //        NO_URUT_PO = 0,
            //        NO_URUT_SJ = 0,
            //        QTY_TERIMA_PO_QC = 0,
            //    };

            //    if (stok.STOK_FISIK < item.Qty)
            //    {
            //        // Stok Masuk
            //        double selisihOM = item.Qty - stok.STOK_FISIK;

            //        stokOpnameB.Nobuk = noStokOM;
            //        stokOpnameB.Ke_Gd = item.Gud;
            //        stokOpnameB.Dr_Gd = "";
            //        stokOpnameB.Qty = selisihOM;

            //        jmRowOM++;

            //        if (jmRowOM == 1)
            //        {
            //            stokOpnameA.Nobuk = noStokOM;
            //            stokOpnameA.JTran = "M";
            //            stokOpnameA.MK = "M";
            //            newSTT01A.Add(stokOpnameA);
            //            ERAMODbContext.STT01A.AddRange(newSTT01A);
            //        }

            //        newSTT01B.Add(stokOpnameB);
            //        ERAMODbContext.STT01B.AddRange(newSTT01B);
            //    }

            //    if (stok.STOK_FISIK > item.Qty)
            //    {
            //        //Stok Keluar
            //        double selisihOK = stok.STOK_FISIK - item.Qty;

            //        stokOpnameB.Nobuk = noStokOK;
            //        stokOpnameB.Ke_Gd = "";
            //        stokOpnameB.Dr_Gd = item.Gud;
            //        stokOpnameB.Qty = selisihOK;

            //        jmRowOK++;


            //        if (jmRowOK == 1)
            //        {
            //            stokOpnameA.Nobuk = noStokOK;
            //            stokOpnameA.JTran = "K";
            //            stokOpnameA.MK = "K";
            //            newSTT01A.Add(stokOpnameA);
            //            ERAMODbContext.STT01A.AddRange(newSTT01A);
            //        }

            //        newSTT01B.Add(stokOpnameB);
            //        ERAMODbContext.STT01B.AddRange(newSTT01B);
            //    }

            //}

            //// update status stok opname POSTING
            //using (System.Data.Entity.DbContextTransaction transaction = ERAMODbContext.Database.BeginTransaction())
            //{
            //    try
            //    {
            //        ERAMODbContext.STT04A.Where(p => p.NOBUK == stokOP.NOBUK).Update(p => new STT04A() { POSTING = "1" });

            //        ERAMODbContext.SaveChanges();

            //        transaction.Commit();

            //        Task.Run(() => TestCallApi().Wait());


            //    }
            //    catch (Exception ex)
            //    {
            //        transaction.Rollback();
            //    }
            //}
            ////return ret;
        }

        public string GenerateAutoNumber(ERAMODbContext context, string Prefix, string TableName, string FieldName)
        {
            string ret = "";
            string tahun = DateTime.UtcNow.AddHours(7).Year.ToString().Substring(2, 2);
            string startIndex = (Prefix.Length + 3).ToString();

            ret = context.Database.SqlQuery<string>("SELECT ISNULL(SUBSTRING(MAX(" + FieldName + "), " + startIndex + ", 6), '0') FROM " + TableName + " WHERE " + FieldName + " LIKE '" + Prefix + tahun + "%'").First();
            return ret;
        }

        public class getStokFisik
        {
            public double STOK_FISIK { get; set; }
        }
    }
}
