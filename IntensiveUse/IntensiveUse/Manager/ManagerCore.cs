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

        private ExponentManager _exponentManager;
        public ExponentManager ExponentManager
        {
            get { return _exponentManager == null ? _exponentManager = new ExponentManager() : _exponentManager; }
        }

        private IndexManager _indexManager;
        public IndexManager IndexManager
        {
            get { return _indexManager == null ? _indexManager = new IndexManager() : _indexManager; }
        }
        private ConstructionLandManager _constructionlandManager;
        public ConstructionLandManager ConstructionLandManager
        {
            get { return _constructionlandManager == null ? _constructionlandManager = new ConstructionLandManager() : _constructionlandManager; }
        }

        private AgricultureManager _agricultureManager;
        public AgricultureManager AgricultureManager
        {
            get { return _agricultureManager == null ? _agricultureManager = new AgricultureManager() : _agricultureManager; }
        }

        private CommonManager _commonManager;
        public CommonManager CommonManager
        {
            get { return _commonManager == null ? _commonManager = new CommonManager() : _commonManager; }
        }

        private StatisticsManager _statisticManager;
        public StatisticsManager StatisticsManager
        {
            get { return _statisticManager == null ? _statisticManager = new StatisticsManager() : _statisticManager; }
        }

        private FoundationManager _foundationManager;
        public FoundationManager FoundationManager
        {
            get { return _foundationManager == null ? _foundationManager = new FoundationManager() : _foundationManager; }
        }
    }
}