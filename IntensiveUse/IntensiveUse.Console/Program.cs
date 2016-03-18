using IntensiveUse.Form;
using IntensiveUse.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace IntensiveUse.Console
{
    class Program
    {
        private static bool _flag { get; set; }
        private static int _times { get; set; }
        private static FileManager _fileManager { get; set; }
        static void Main(string[] args)
        {
            _flag = true;
            _fileManager = new FileManager();
            _times = 500;
            while (_flag)
            {
                var uploadfile = _fileManager.GetAnalyzeFile();
                if (uploadfile != null)
                {
                    _times = 500;
                    System.Console.WriteLine(string.Format("开始分析表格：{0} 文件大小：{1}", uploadfile.FileName, uploadfile.Length));
                    var engine = new TableNationWide();
                    System.Console.WriteLine(string.Format("开始分析数据.................."));
                    engine.Gain(uploadfile.SavePath);
                    System.Console.WriteLine(string.Format("分析数据结束.................."));
                    System.Console.WriteLine(string.Format("开始导入数据到数据库中........."));
                    engine.Save(_fileManager);
                    System.Console.WriteLine(string.Format("成功导入数据到数据库中........."));
                    _fileManager.SetAnalyzed(uploadfile.ID);
                    System.Console.WriteLine(string.Format("完成对表格：{0} 的分析", uploadfile.FileName));
                }
                else
                {
                    System.Console.WriteLine("当前不存在没有分析过的Excel文件");
                    _times = 10000;
                }
                try
                {
                   
                }
                catch(Exception ex)
                {
                    System.Console.WriteLine(ex.Message + ex.ToString());
                }
                System.Console.ReadLine();
                Thread.Sleep(_times);
            }
        }
    }
}
