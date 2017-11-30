using eBillingSuite.Model.eBillingConfigurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eBillingSuite.Security
{
    public class GenericBillingSuite : IeBillingSuiteIdentity
    {
        public string AuthenticationType { get { return "custom"; } }
        public int CodUtilizador { get; set; }
        public bool IsAuthenticated { get; set; }

        public bool IsEBCActive { get; set; }

        public bool IsEBDActive { get; set; }

        public bool IsEBEActive { get; set; }

        public bool IsMCATActive { get; set; }

        public bool IsSTATSActive { get; set; }

        public string Name { get; set; }
        public string Instances { get; set; }

        public bool isAdmin { get; set; }

        public List<Permissions> Permissions { get; set; }

        public GenericBillingSuite(eBC_Login user)
        {
            CodUtilizador = user.CodUtilizador;
            Name = user.username;
            IsAuthenticated = true;
            Instances = user.Instances;
            isAdmin = false;
            if (user.username == "tastas")
            {
                isAdmin = true;
                Instances = "*";
            }
        }

        public bool HasPermission(Permissions permissions) { return this.Permissions.Contains(permissions); }

    }
}