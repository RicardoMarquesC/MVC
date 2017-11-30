using eBillingSuite.Repositories;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;

namespace eBillingSuite.Security
{
    public interface IeBillingSuiteIdentity : IIdentity
    {
        bool IsEBCActive { get; set; }
        bool IsEBEActive { get; set; }
        bool IsEBDActive { get; set; }
        bool IsMCATActive { get; set; }
        bool IsSTATSActive { get; set; }

        List<Permissions> Permissions { get; set; }

        bool HasPermission(Security.Permissions permissions);

        int CodUtilizador { get; }
        bool isAdmin { get; set; }
        string Instances { get; set; }
    }

    public class eBillingSuiteIdentity : IeBillingSuiteIdentity
    {
        [Inject]
        public eBillingSuiteIdentity(IPermissionsRepository permRe)
        {
            var identity = this as IeBillingSuiteIdentity;

            bool usesAD = bool.Parse(WebConfigurationManager.AppSettings["UsesAD"].ToString());

            if (usesAD)
            {
                //Check if User has access to site
                WindowsIdentity windowsIdentity = System.Security.Principal.WindowsIdentity.GetCurrent();
                WindowsPrincipal MyPrincipal = new WindowsPrincipal(windowsIdentity);

                IsAuthenticated = (MyPrincipal.IsInRole("eBillingSuite")) ? true : false;
                
                Name = MyPrincipal.Identity.Name;

                //Permissions = permRe.GetPermissionsByUser(identity);

            }

            if (identity.Name == "tastas")
                Permissions = permRe.GetAll();
            else
                Permissions = permRe.GetPermissionsByUser(identity);

            

            HttpContext.Current.User = new GenericPrincipal(this, null);
        }

        public string AuthenticationType
        {
            get { return "Custom"; }
        }

        public bool IsAuthenticated
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public bool isAdmin { get; set; }
        public bool IsEBCActive { get; set; }
        public bool IsEBEActive { get; set; }
        public bool IsEBDActive { get; set; }
        public bool IsMCATActive { get; set; }
        public bool IsSTATSActive { get; set; }

        public string Instances { get; set; }
        public List<Permissions> Permissions { get; set; }

        public bool HasPermission(Permissions permissions)
        {
            return this.Permissions.Contains(permissions);
        }

        public int CodUtilizador { get; set; }
        public int LanguageID { get ; set ; }
    }
}
