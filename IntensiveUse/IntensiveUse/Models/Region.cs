using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IntensiveUse.Models
{
    [Table("region")]
    public class Region
    {
        [Key]
        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        /// <summary>
        /// 地区
        /// </summary>
        public string Zone { get; set; }
        /// <summary>
        /// 省份
        /// </summary>
        public string Province { get; set; }
        /// <summary>
        /// 所在地级市
        /// </summary>
        public string BelongCity { get; set; }
        /// <summary>
        /// 区域评价对象
        /// </summary>
        //public Evaluator Evalutaor { get; set; }
        public string Evalutaor { get; set; }
        /// <summary>
        /// 要素代码
        /// </summary>
        public string FactorCode { get; set; }
        /// <summary>
        /// 行政区名  区域名称
        /// </summary>
        [MaxLength(1023)]
        public string Name { get; set; }
        /// <summary>
        /// 行政区代码  区域代码
        /// </summary>
        [MaxLength(255)]
        public string Code { get; set; }
        /// <summary>
        /// 区域级别
        /// </summary>
        public string Degree { get; set; }

    }

    public enum Evaluator
    {
        [Description("地级以上城市辖区整体")]
        PeopeDom,
        [Description("地级以上城市下辖县市区")]
        Urban,
        [Description("县级市")]
        County,
        [Description("市区")]
        City
    }
}