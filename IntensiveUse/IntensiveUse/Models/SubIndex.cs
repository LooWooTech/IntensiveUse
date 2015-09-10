using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace IntensiveUse.Models
{
    /// <summary>
    /// 分指数权重
    /// </summary>
    [Table("subindex")]
    public class SubIndex
    {
        [Key]
        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        /// <summary>
        /// 人口密度分指数
        /// </summary>
        public double PUII { get; set; }
        /// <summary>
        /// 经济强度分指数
        /// </summary>
        public double EUII { get; set; }
        /// <summary>
        /// 人口增长耗地指数
        /// </summary>
        public double PGCI { get; set; }
        /// <summary>
        /// 经济增长耗地分指数
        /// </summary>
        public double EGCI { get; set; }
        /// <summary>
        /// 人口用地弹性分指数
        /// </summary>
        public double PEI { get; set; }
        /// <summary>
        /// 经济用地弹性分指数
        /// </summary>
        public double EEI { get; set; }
        /// <summary>
        /// 城市用地管理绩效分指数
        /// </summary>
        public double ULAPI { get; set; }
        /// <summary>
        /// 年份
        /// </summary>
        public int Year { get; set; }
        /// <summary>
        /// 行政区ID
        /// </summary>
        public int RID { get; set; }

        public static SubIndex operator *(Exponent c1, SubIndex c2)
        {
            return new SubIndex()
            {
                PUII = c1.PUII1 * c2.PUII,
                EUII = (c1.EUII1 + c1.EUII2) * c2.EUII,
                PGCI = c1.PGCI * c2.PGCI,
                EGCI = (c1.EGCI1 + c1.EGCI2 + c1.EGCI3) * c2.EGCI,
                PEI = c1.PEI1 * c2.PEI,
                EEI = c1.EEI * c2.EEI,
                ULAPI = (c1.ULAPI1 + c1.ULAPI2) * c2.ULAPI
            };
        }

        public static SubIndex operator *(SubIndex c1, int c2)
        {
            return new SubIndex()
            {
                PUII = c1.PUII * c2,
                EUII = c1.EUII * c2,
                PGCI = c1.PGCI * c2,
                EGCI = c1.EGCI * c2,
                PEI = c1.PEI * c2,
                EEI = c1.EEI * c2,
                ULAPI = c1.ULAPI * c2
            };
        }

        public static IndexWeight operator *(SubIndex c1, SubIndex c2)
        {
            return new IndexWeight()
            {
                UII = c1.PUII * c2.PUII + c1.EUII * c2.EUII,
                GCI = c1.PGCI * c2.PGCI + c1.EGCI * c2.EGCI,
                EI = c1.PEI * c2.PEI + c1.EEI * c2.EEI,
                API = c1.ULAPI * c2.ULAPI
            };
        }
    }
}