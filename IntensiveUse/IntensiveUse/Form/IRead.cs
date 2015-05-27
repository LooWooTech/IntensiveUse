using IntensiveUse.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntensiveUse.Form
{
    interface IRead
    {
       void Read(string FilePath, ManagerCore Core, string City,int Year);
    }
}
