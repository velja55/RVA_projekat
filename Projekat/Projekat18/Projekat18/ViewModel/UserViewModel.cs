using Projekat18.Helpers;
using Projekat18.Model;
using Projekat18.Model.Enums;
using Projekat18.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekat18.ViewModel
{
    public  class UserViewModel : BaseViewModel
    {
        private object _currentView;
        public object CurrentView
        {
            get => _currentView;
            set { _currentView = value; OnPropertyChanged(nameof(CurrentView)); }
        }

        public MyICommand ShowDatabaseViewCommand { get; }
        public MyICommand ShowTableViewCommand { get; }
        public MyICommand ShowLegacyViewCommand { get; }

        public UserViewModel(Administrator administrator)
        {

            ObservableCollection<Table> tables1 = new ObservableCollection<Table>
{
    new Table("Korisnici", new List<string> { "Id", "Ime", "Prezime", "Email" }),
    new Table("Porudzbine", new List<string> { "PorudzbinaId", "KorisnikId", "Datum", "Ukupno" }),
    new Table("Proizvodi", new List<string> { "ProizvodId", "Naziv", "Cena", "Kategorija" }),
    new Table("Kategorije", new List<string> { "KategorijaId", "Naziv" }),
};

            ObservableCollection<Table> tables2 = new ObservableCollection<Table>
{
    new Table("Korisnici2", new List<string> { "Id2", "Ime", "Prezime", "Email" }),
    new Table("Porudzbine2", new List<string> { "PorudzbinaId2", "KorisnikId", "Datum", "Ukupno" }),
    new Table("Proizvodi2", new List<string> { "ProizvodId2", "Naziv", "Cena", "Kategorija" }),
    new Table("Kategorije2", new List<string> { "KategorijaId2", "Naziv" }),
};

            ObservableCollection<Database> Databases = new ObservableCollection<Database>
            {


                new Database("SQL Server", DatabaseType.RELATIONAL, "T-SQL",tables1, new Administrator("Stefan", "Admin", "Add/Edit/Delete", "Stefan"), DatabaseState.Online),
                new Database("MongoDB", DatabaseType.NOSQL, "MongoQL", tables2, new Administrator("Veljko", "Admin", "Add/Edit/Delete", "Veljko"), DatabaseState.Offline)

            };
            CurrentView = new DatabaseView(this,administrator,Databases);
            ObservableCollection<Table> tables = new ObservableCollection<Table>();
            foreach (Database db in Databases) {
                foreach (Table t in db.Tables)
                {
                    tables.Add(t);
                }
            }
            ShowDatabaseViewCommand = new MyICommand(() => CurrentView = new DatabaseView(this,administrator,Databases));
            ShowTableViewCommand=new MyICommand(() => CurrentView = new TableView(tables));
        }
    }
}
