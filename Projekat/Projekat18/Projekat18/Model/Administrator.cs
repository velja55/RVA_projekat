using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekat18.Model
{
    public class Administrator
    {
        public string UserName { get; set; }
        public string Certificate { get; set; }
        public string Permissions { get; set; }

        public Administrator() { }

        public Administrator(string userName, string certificate, string permissions)
        {
            UserName = userName;
            Certificate = certificate;
            Permissions = permissions;
        }
    }
}
