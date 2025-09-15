using Projekat18.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekat18
{
    public interface IDatabaseState
    {
        string Name { get; }
        string Color { get; } // za UI
        void Handle(Database db);
    }
}
