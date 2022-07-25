using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace MO_Erasoft.Models
{
    public class STF09A_API
    {
        public string Brg { get; set; }

        public DateTime Tgl { get; set; }

        public string Bukti { get; set; }

        public string MK { get; set; }

        public string Ket { get; set; }

        public double Qty { get; set; }

        public double HPokok { get; set; }

        public string GD { get; set; }

        public string DR_GD { get; set; }

        public string id { get; set; }

        public string status_transaksi { get; set; }
    }

    public class STF09A_API_Erasoft
    {
        public string Brg { get; set; }

        public DateTime Tgl { get; set; }

        public string Bukti { get; set; }

        public string MK { get; set; }

        public string Ket { get; set; }

        public double Qty { get; set; }

        public double HPokok { get; set; }

        public string GD { get; set; }

        public string Ref { get; set; }

        public string JTran { get; set; }

        public string WO { get; set; }

        public double Qty_Berat { get; set; }

        public string No_Faktur { get; set; }

        public int RECNUM { get; set; }

        public double DetailNoUrut { get; set; }

        public string Work_Center { get; set; }

        public string DR_GD { get; set; }

        public string BRG_UNIT { get; set; }

        public DateTime Tgl_Input { get; set; }

        public string Status { get; set; }

        public int RECNUM_Asal { get; set; }

        public string status_transaksi { get; set; }
    }

    public class listStf09a_API
    {
        public string token { get; set; }
        public string email { get; set; }
        public string databaseId { get; set; }
        public string fs_id { get; set; }
        public List<STF09A_API> stf09a_list { get; set; } = new List<STF09A_API>();

        //public STF09A_API stf09a { get; set; }
    }

    public class API_Integration
    {
        public string token { get; set; }
        public string email { get; set; }
        public string databaseId { get; set; }
        public string fs_id { get; set; }
    }
}