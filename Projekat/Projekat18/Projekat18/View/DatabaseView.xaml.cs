using Projekat18.Model;
using Projekat18.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Projekat18.Helpers;

namespace Projekat18.View
{
    /// <summary>
    /// Interaction logic for DatabaseView.xaml
    /// </summary>
    public partial class DatabaseView : UserControl
    {
        public DatabaseView(UserViewModel um, Administrator administrator,ObservableCollection<Database> dataBases)
        {
            InitializeComponent();

            DataContext = new DatabaseViewModel(um,administrator,dataBases);
            var test = DatabaseTypeHelper.GetValues;
            var test2 = DatabaseStateHelper.GetStates;
        }

       
    }
}
