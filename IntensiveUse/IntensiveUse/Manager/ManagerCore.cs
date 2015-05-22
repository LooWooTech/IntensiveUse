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

        private PeopleManager _peopleManager;
        public PeopleManager PeopleManager
        {
            get { return _peopleManager == null ? _peopleManager = new PeopleManager() : _peopleManager; }
        }

        private EconomyManager _economyManager;
        public EconomyManager EconmoyManager
        {
            get { return _economyManager == null ? _economyManager = new EconomyManager() : _economyManager; }
        }
        private LandSupplyManager _landsupplyManager;
        public LandSupplyManager LandSupplyManager
        {
            get { return _landsupplyManager == null ? _landsupplyManager = new LandSupplyManager() : _landsupplyManager; }
        }

        private LandUseManager _landuseManager;
        public LandUseManager LandUseManager
        {
            get { return _landuseManager == null ? _landuseManager = new LandUseManager() : _landuseManager; }
        }
    }
}