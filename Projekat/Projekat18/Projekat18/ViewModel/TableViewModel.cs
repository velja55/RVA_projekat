using Projekat18.Helpers;
using Projekat18.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekat18.ViewModel
{
    public class TablesViewModel : BaseViewModel
    {
        public ObservableCollection<Table> TablesToShow { get; set; }

        public TablesViewModel(ObservableCollection<Table> tables)
        {
            TablesToShow = tables;
        }
    }
}
