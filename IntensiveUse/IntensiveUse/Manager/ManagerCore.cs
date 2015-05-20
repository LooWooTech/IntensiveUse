using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IntensiveUse.Manager
{
    public class ManagerCore
    {
        public static ManagerCore Instance = new ManagerCore();

        private FileManager _fileManager;
        public FileManager FileManager
        {
            get { return _fileManager == null ? _fileManager = new FileManager() : _fileManager; }
        }
        private ExcelManager _excelManager;
        public ExcelManager ExcelManager
        {
            get { return _excelManager == null ? _excelManager = new ExcelManager() : _excelManager; }
        }
    }
}