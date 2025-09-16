using Contracts;
using Projekat18.DBStates;
using Projekat18.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using Administrator = Projekat18.Model.Administrator;

namespace Projekat18.MapperHelper
{
    public class AdministratorMapper
    {
        public static Administrator FromContract(Contracts.Administrator contractDb)
        {
            return new Administrator
            {
                UserName = contractDb.UserName,
                Password = contractDb.Password,
                Certificate = contractDb.Certificate,
                Permissions = contractDb.Permissions
            };
        }

        public static Contracts.Administrator FromModel(Administrator modelDb)
        {
            return new Contracts.Administrator
            {
                UserName = modelDb.UserName,
                Password = modelDb.Password,
                Certificate = modelDb.Certificate,
                Permissions = modelDb.Permissions
            };
        }
    }
}
