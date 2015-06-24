using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace IntensiveUse.Models
{
    /// <summary>
    /// 指数权重
    /// </summary>
    [Table("indexweight")]
    public class IndexWeight
    {
        [Key]
        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        /// <summary>
        /// 利用强度指数
        /// </summary>
        public double UII { get; set; }
        /// <summary>
        /// 增长耗地指数
        /// </summary>
        public double GCI { get; set; }
        /// <summary>
        /// 用地弹性指数
        /// </summary>
        public double EI { get; set; }
        /// <summary>
        /// 管理绩效指数
        /// </summary>
        public double API { get; set; }
        /// <summary>
        /// 年份
        /// </summary>
        public int Year { get; set; }
        /// <summary>
        /// 行政区ID
        /// </summary>
        public int RID { get; set; }


        public static IndexWeight operator *(SubIndex c1, IndexWeight c2)
        {
            return new IndexWeight()
            {
                UII = (c1.PUII + c1.EUII) * c2.UII,
                GCI = (c1.PGCI + c1.EGCI) * c2.GCI,
                EI = (c1.PEI + c1.EEI) * c2.EI,
                API = c1.ULAPI * c2.API
            };
        }

        public static double operator *(IndexWeight c1, IndexWeight c2)
        {
            return c1.UII * c2.UII + c1.GCI * c2.GCI + c1.EI * c2.EI + c1.API * c2.API;
        }
    }
}