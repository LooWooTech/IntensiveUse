using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace IntensiveUse.Models
{
    [Table("files")]
    public class UploadFile
    {
        public UploadFile()
        {
            CreateTime = DateTime.Now;
        }
        [Key]
        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        /// <summary>
        /// 文件名
        /// </summary>
        [MaxLength(127)]
        public string FileName { get; set; }
        /// <summary>
        /// 提交时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 保存路径
        /// </summary>
        [MaxLength(127)]
        public string SavePath { get; set; }
        /// <summary>
        /// 文件分析状态
        /// </summary>
        [Column("State",TypeName="INT")]
        public UploadFileProceedState State { get; set; }
        /// <summary>
        /// 文件分析信息
        /// </summary>
        [MaxLength(1023)]
        public string ProcessMessage { get; set; }
        /// <summary>
        /// 上传文件表格类型名称
        /// </summary>
        public string FileTypeName { get; set; }
    }

    public class CurrentSituation
    {
        public int Year { get; set; }
        public string[] Regions { get; set; }
    }


    public enum UploadFileExcel
    {
        表1 = 0,
        表2 = 1,
        表3 = 2
    }
    public enum SpecialExcel
    {
        区域用地状况整体评价指标现状值汇总表=0
    }

  

    public enum UploadFileProceedState
    {
        UnProceed,
        Proceeded,
        Error
    }
}