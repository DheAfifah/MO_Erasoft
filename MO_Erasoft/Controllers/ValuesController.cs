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
        public ValuesController()
        {
            ERAMODbContext = new ERAMODbContext("");
            ERADbContext = new ERADbContext("");
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
            public object data { get; set; }
        }
        [System.Web.Http.Route("api/receive-faktur")]
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public async Task<IHttpActionResult> ProsesStokOpname_Webhook([FromBody]JsonData_StokOP data)
        {

            try
            {
                JsonApi result;
                string apiKey = "";
                string dbPathEra = "";
                string userName = "";

                var re = Request;
                var headers = re.Headers;

                if (headers.Contains("X-API-KEY"))
                {
                    apiKey = headers.GetValues("X-API-KEY").First();
                }

                if (apiKey != "UPDATESTOKMP_M@STERONLINE4P1K3Y")
                {
                    result = new JsonApi()
                    {
                        code = 401,
                        message = "Wrong API KEY!",
                        data = null
                    };

                    return Json(result);
                }

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
                        message = "Stock can not be empty!",
                        data = null
                    };

                    return Json(result);
                }

                result = new JsonApi();

                try
                {
                    var tesinsert = ERAMODbContext.Database.ExecuteSqlCommand("INSERT INTO [temp_api_tokped] (response, cust, tglinput) VALUES ('TES', 'ERASOFT', DATEADD(HOUR, +7, GETUTCDATE()))");
                    if (tesinsert > 0)
                    {
                        result.code = 200;
                        result.message = "Success";
                        result.data = null;
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
#if (AWS)
	        string url = "https://api.masteronline.co.id/webhook/api/invoice/";
#else
            string url = "https://devapi.masteronline.co.id/webhook/api/invoice/";
#endif

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
            return "OK";
        }

        //[RecurringJob("*/5 * * * *", "SE Asia Standard Time", "default", RecurringJobId = "ProsesStokOpname")]
        //[RecurringJob("0 6,18 */1 * *", "SE Asia Standard Time", "default", RecurringJobId = "ProsesStokOpname")] change to 30 7,14 * * sun-sat
        //[RecurringJob("30 7,14 * * sun-sat", "SE Asia Standard Time", "default", RecurringJobId = "ProsesStokOpname")]
        [RecurringJob("5 * * * *", "SE Asia Standard Time", "default", RecurringJobId = "ProsesMutasiStok")]
        public void DoStokOpname()
        {
            var tesinsert = ERAMODbContext.Database.ExecuteSqlCommand("INSERT INTO [temp_api_tokped] (response, cust, tglinput) VALUES ('TES', 'ERASOFT', DATEADD(HOUR, +7, GETUTCDATE()))");
            if(tesinsert > 0)
            {
                Task.Run(() => TestCallApi().Wait());
            }
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
            ////RcMODbContext.SaveChanges();

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
