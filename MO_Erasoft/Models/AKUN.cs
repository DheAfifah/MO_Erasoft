using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MO_Erasoft.Models
{
    public class AKUN
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string SIConnectionString { get; set; }
        public string STConnectionString { get; set; }
        public string APIConnectionString { get; set; }

    }

    public class MARKETPLACE
    {
        public int Id { get; set; }
        public string Marketplace { get; set; }
        public string Customer { get; set; }
        public string Bank { get; set; }
        public string Pot_Pembayaran_Rek { get; set; }

    }

    public class LOG_API
    {
        public int Recnum { get; set; }
        public DateTime Tgl { get; set; }
        public string Function { get; set; }
        public string Json { get; set; }
        public string Response { get; set; }
        public string Status { get; set; }

    }
}