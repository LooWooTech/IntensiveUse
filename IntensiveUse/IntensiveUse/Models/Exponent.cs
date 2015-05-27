using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace IntensiveUse.Models
{
    [Table("exponent")]
    public class Exponent
    {
        [Key]
        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        /// <summary>
        /// 人口密度分指数-城乡建设用地人口密度
        /// </summary>
        public double PUII1 { get; set; }
        /// <summary>
        /// 建设用地地均固定资产投资
        /// </summary>
        public double EUII1 { get; set; }
        /// <summary>
        /// 建设用地地均地区生产总值
        /// </summary>
        public double EUII2 { get; set; }
        /// <summary>
        /// 单位人口增长消耗新增城乡建设用地量 
        /// </summary>
        public double PGCI { get; set; }
        /// <summary>
        /// 单位地区生产总值耗地下降率 
        /// </summary>
        public double EGCI1 { get; set; }
        /// <summary>
        /// 单位地区生产总值增长消耗新增建设用地量 
        /// </summary>
        public double EGCI2 { get; set; }
        /// <summary>
        /// 单位固定资产投资消耗新增建设用地量
        /// </summary>
        public double EGCI3 { get; set; }
        /// <summary>
        /// 人口与城乡建设用地增长弹性系数
        /// </summary>
        public double PEI1 { get; set; }
        /// <summary>
        /// 地区生产总值与建设用地增长弹性系数
        /// </summary>
        public double EEI { get; set; }
        /// <summary>
        /// 城市存量土地供应比率
        /// </summary>
        public double ULAPI1 { get; set; }
        /// <summary>
        /// 城市批次土地供应比率
        /// </summary>
        public double ULAPI2 { get; set; }
        /// <summary>
        /// 区分是附表1A5中的指标权重还是附表1A6中理想值
        /// </summary>
        [Column(TypeName="int")]
        public IdealType Type { get; set; }
        /// <summary>
        /// 年份
        /// </summary>
        public string Year { get; set; }
        /// <summary>
        /// 行政区ID
        /// </summary>
        public int RID { get; set; }
    }


    public enum IdealType
    {
        [Description("指标理想值")]
        Value=0,
        [Description("理想值确定依据")]
        Foundation=1,
        [Description("指标权重")]
        Weight=2
    }
}