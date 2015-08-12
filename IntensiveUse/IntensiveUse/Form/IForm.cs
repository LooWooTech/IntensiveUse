using IntensiveUse.Manager;
using IntensiveUse.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntensiveUse.Form
{
    public interface IForm
    {
        void Gain(string FilePath);
        void Save(ManagerCore Core);
        void Update(int ID);
        Dictionary<string, Queue<double>> GetDictValue();

        string GetName();
        List<EconomyChange> GetChange();
    }
}
