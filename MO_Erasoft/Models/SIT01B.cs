using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace MO_Erasoft.Models
{
    public class SIT01B
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(1)]
        public string JENIS_FORM { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(15)]
        public string NO_BUKTI { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? NO_URUT { get; set; }

        [StringLength(20)]
        public string BRG { get; set; }

        [StringLength(20)]
        public string BRG_CUST { get; set; }

        public double? H_SATUAN { get; set; }

        [StringLength(1)]
        public string SATUAN { get; set; }

        public double QTY { get; set; }

        [StringLength(4)]
        public string GUDANG { get; set; }

        public double? DISCOUNT { get; set; }

        public double? NILAI_DISC { get; set; }

        public double? HARGA { get; set; }

        public double? QTY_KIRIM { get; set; }

        [StringLength(1)]
        public string AUTO_LOAD { get; set; }

        [StringLength(20)]
        public string USERNAME { get; set; }

       
        public DateTime? TGLINPUT { get; set; }

        public double? QTY_RETUR { get; set; }

        public bool WRITE_KONFIG { get; set; }

        public double? DISCOUNT_2 { get; set; }

        public double? DISCOUNT_3 { get; set; }

        public double? DISCOUNT_4 { get; set; }

        public double? DISCOUNT_5 { get; set; }

        public double? NILAI_DISC_1 { get; set; }

        public double? NILAI_DISC_2 { get; set; }

        public double? NILAI_DISC_3 { get; set; }

        public double? NILAI_DISC_4 { get; set; }

        public double? NILAI_DISC_5 { get; set; }

        public double? TOTAL_LOT { get; set; }

        public double? TOTAL_QTY { get; set; }

        public DateTime? TGL_KIRIM { get; set; }

        public double NO_URUT_SO { get; set; }

        
        [StringLength(255)]
        public string CATATAN { get; set; }

        public double? QTY_BESAR { get; set; }

        public double? QTY_KECIL { get; set; }

        [StringLength(20)]
        public string BRG_SO { get; set; }

        public double? TRANS_NO_URUT { get; set; }

        public int? SATUAN_N { get; set; }

        public double? QTY_N { get; set; }

        public double? NTITIPAN { get; set; }

        public double? DISC_TITIPAN { get; set; }

        public double? QOH { get; set; }

        //public string DETAIL_ID { get; set; }


    }

    public class SIT01B_MO
    {

        [StringLength(1)]
        public string JENIS_FORM { get; set; }

        [Key]
        [StringLength(15)]
        public string NO_BUKTI { get; set; }


        public int? NO_URUT { get; set; }

        [StringLength(20)]
        public string BRG { get; set; }

        [StringLength(20)]
        public string BRG_CUST { get; set; }

        public double? H_SATUAN { get; set; }

        [StringLength(1)]
        public string SATUAN { get; set; }

        public double QTY { get; set; }

        [StringLength(4)]
        public string GUDANG { get; set; }

        public double? DISCOUNT { get; set; }

        public double? NILAI_DISC { get; set; }

        public double? HARGA { get; set; }

        public double? QTY_KIRIM { get; set; }

        [StringLength(1)]
        public string AUTO_LOAD { get; set; }

        [StringLength(20)]
        public string USERNAME { get; set; }


        public DateTime? TGLINPUT { get; set; }

        public double? QTY_RETUR { get; set; }

        public bool WRITE_KONFIG { get; set; }

        public double? DISCOUNT_2 { get; set; }

        public double? DISCOUNT_3 { get; set; }

        public double? DISCOUNT_4 { get; set; }

        public double? DISCOUNT_5 { get; set; }

        public double? NILAI_DISC_1 { get; set; }

        public double? NILAI_DISC_2 { get; set; }

        public double? NILAI_DISC_3 { get; set; }

        public double? NILAI_DISC_4 { get; set; }

        public double? NILAI_DISC_5 { get; set; }

        public double? TOTAL_LOT { get; set; }

        public double? TOTAL_QTY { get; set; }

        public DateTime? TGL_KIRIM { get; set; }

        public double NO_URUT_SO { get; set; }


        [StringLength(255)]
        public string CATATAN { get; set; }

        public double? QTY_BESAR { get; set; }

        public double? QTY_KECIL { get; set; }

        [StringLength(20)]
        public string BRG_SO { get; set; }

        public double? TRANS_NO_URUT { get; set; }

        public int? SATUAN_N { get; set; }

        public double? QTY_N { get; set; }

        public double? NTITIPAN { get; set; }

        public double? DISC_TITIPAN { get; set; }

        public double? QOH { get; set; }

        public string DETAIL_ID { get; set; }


    }
}