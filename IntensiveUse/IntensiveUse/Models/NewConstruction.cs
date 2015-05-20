using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace IntensiveUse.Models
{
    public class NewConstruction
    {
        [Key]
        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        /// <summary>
        /// 新增建设用地
        /// </summary>
        public double Contrustion { get; set; }
        /// <summary>
        /// 新增城乡建设用地
        /// </summary>
        public double Town { get; set; }
        public int CID { get; set; }
        public string Year { get; set; }
    }
}