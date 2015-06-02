using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace IntensiveUse.Models
{
    /// <summary>
    /// 用于存储理想确定依据
    /// </summary>
    [Table("foundation")]
    public class Foundation
    {
        [Key]
        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string PUII { get; set; }
        public string EUII1 { get; set; }
        public string EUII2 { get; set; }
        public string PGCI1 { get; set; }
        public string EGCI1 { get; set; }
        public string EGCI2 { get; set; }
        public string EGCI3 { get; set; }
        public string PEII { get; set; }
        public string EEI { get; set; }
        public string ULAPI1 { get; set; }
        public string ULAPI2 { get; set; }
        public int Year { get; set; }
        public int RID { get; set; }
    }
}