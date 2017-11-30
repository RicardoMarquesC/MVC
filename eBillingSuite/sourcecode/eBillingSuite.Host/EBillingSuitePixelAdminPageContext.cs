using eBillingSuite.Model.eBillingConfigurations;
using eBillingSuite.Repositories;
using eBillingSuite.Repositories.Interfaces;
using eBillingSuite.Resources;
using eBillingSuite.Security;
using Ninject;
using Shortcut.PixelAdmin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;

namespace eBillingSuite
{
    public class EBillingSuitePixelAdminPageContext : PixelAdminPageContext
    {
        private IPermissionsRepository _permissionsRepository;
        private IProductsRepository _productsRepository;
        private IeBillingSuiteRequestContext _context;
        private ILoginRepository _loginRepository;
        private IeBillingSuiteIdentity identity;
        private UrlHelper url;
        private List<eSuite_Produtos> data;

        [Inject]
        public EBillingSuitePixelAdminPageContext(
            ILoginRepository loginRepository,
            IPermissionsRepository permissionsRepository,
            IProductsRepository productsRepository,
            IeBillingSuiteRequestContext context)
        {
            _context = context;
            _permissionsRepository = permissionsRepository;
            _productsRepository = productsRepository;
            _loginRepository = loginRepository;

            var requestContext = HttpContext.Current.Request.RequestContext;

            var controller = requestContext.RouteData.Values["controller"].ToString().ToLower();
            var action = requestContext.RouteData.Values["action"].ToString().ToLower();

            url = new UrlHelper(requestContext);

            Menu = new List<BasePixelAdminMenuEntry>();
            List<BasePixelAdminMenuEntry> Sub = new List<BasePixelAdminMenuEntry>();

            PageTitle = "";
            ApplicationName = "eBilling Suite";
            ApplicationLogo = url.Content("~/assets/PI/Images/logotipo.png");


            identity = _context.UserIdentity;
            identity.isAdmin = false;

            if (identity != null)
            {
                var user = _loginRepository.Find(identity.CodUtilizador);
                if (identity.Name == "tastas")
                {
                    identity.isAdmin = true;
                    user = new eBC_Login
                    {
                        CodUtilizador = 0,
                        username = "tastas",
                        LastLogin = DateTime.Now,
                    };
                }
                if (user != null)
                    UserName = user.username;

                // set all modules as active
                #region com identity
                identity.IsEBCActive = true;
                identity.IsEBEActive = true;
                identity.IsEBDActive = true;
                identity.IsMCATActive = true;
                identity.IsSTATSActive = true;
                #endregion


                if (!identity.IsAuthenticated)
                    return;
                else
                {
                    if (identity.Name == "tastas")
                        identity.Permissions = _permissionsRepository.GetAll();
                    else
                        identity.Permissions = _permissionsRepository.GetPermissionsByUser(identity);

                    string ebcIcon = "fa-envelope", ebdIcon = "fa-inbox", ediIcon = "fa-cogs", mcatIcon = "fa-tasks",
                        ebcStatsIcon = "fa-bar-chart-o", ebcWwfIcon = "fa-stack-overflow", ebcuserIcon = "fa-users";

                    string ebcAction = url.Action("", "eConnector"), ebdAction = url.Action("", "eDigital"), ediAction = url.Action("", "eEDI"),
                        mcatAction = url.Action("", "eMCAT"), ebcStatsAction = url.Action("Index", "Stats"), ebcWwfAction = url.Action("", "eWWF"), ebcusersAction = url.Action("", "eUser");

                    string inactiveModuleIcon = "fa-minus-circle";
                    string inactiveModuleDefaultAction = "InactiveModule";

                    if (identity.Name == "tastas")
                        data = _productsRepository.GetAllProducts();
                    else
                        data = _permissionsRepository.GetProductsByUser(identity);

                    data = data.OrderBy(o=> _productsRepository.GetProductsByID(o.produto)).ToList();
                    foreach (var p in data)
                    {
                        string product = _productsRepository.GetProductsByID(p.produto);
                        if (product.Equals("PI eBilling Connector"))
                        {
                            if (p.activo.HasValue && p.activo.Value)
                            {
                                Sub.Add(new PixelAdminMenuEntry
                                {
                                    Label = Texts.Configuracoes,
                                    Icon = "fa-cog",
                                    Url = url.Action("", "eConnectorConfigs"),
                                    Active = controller == "econnectorconfigs",
                                });
                                Sub.Add(new PixelAdminMenuEntry
                                {
                                    Label = Texts.RemetentesClientes,
                                    Icon = "fa-users",
                                    Url = url.Action("", "eConnectorSenders"),
                                    Active = controller == "econnectorsenders",
                                });
                                Sub.Add(new PixelAdminMenuEntry
                                {
                                    Label = Texts.UnknownList,
                                    Icon = "fa-envelope",
                                    Url = url.Action("", "readEmail"),
                                    Active = controller == "reademail",
                                });
                            }
                            else
                            {
                                identity.IsEBCActive = false;
                                //user.IsEBCActive = false;
                                ebcIcon = inactiveModuleIcon;
                                ebcAction = url.Action(inactiveModuleDefaultAction, "eConnector");
                            }

                            Menu.Add(new PixelAdminMenuEntry
                            {
                                Label = product,
                                Icon = ebcIcon,
                                Url = ebcAction,
                                Active = controller == "econnector",
                                SubMenu = Sub
                            });
                        }
                        else if (product.Equals("PI eBilling Digital"))
                        {
                            if (p.activo.HasValue && !p.activo.Value)
                            {
                                identity.IsEBDActive = false;
                                //user.IsEBDActive = false;
                                ebdIcon = inactiveModuleIcon;
                                ebdAction = url.Action(inactiveModuleDefaultAction, "eDigital");
                            }

                            Menu.Add(new PixelAdminMenuEntry
                            {
                                Label = product,
                                Icon = ebdIcon,
                                //Url = ebdAction,
                                Url = checkpermission(identity, url),
                                Active = controller == "edigital"
                            });
                        }
                        else if (product.Equals("PI eBilling EDI"))
                        {
                            if (p.activo.HasValue && !p.activo.Value)
                            {
                                identity.IsEBEActive = false;
                                //user.IsEBEActive = false;
                                ediIcon = inactiveModuleIcon;
                                ediAction = url.Action(inactiveModuleDefaultAction, "eEDI");
                            }

                            Menu.Add(new PixelAdminMenuEntry
                            {
                                Label = product,
                                Icon = ediIcon,
                                Url = ediAction,
                                Active = controller == "eedi"
                            });
                        }
                        else if (product.Equals("PI eSuite Stats"))
                        {
                            //ebcStatsAction = System.Configuration.ConfigurationManager.AppSettings.Get("eSuiteStatsURL");
                            //bool openNewWindow = true;

                            if (p.activo.HasValue && !p.activo.Value)
                            {
                                identity.IsSTATSActive = false;
                                //user.IsSTATSActive = false;
                                ebcStatsIcon = inactiveModuleIcon;
                                ebcStatsAction = "";
                                //openNewWindow = false;
                            }

                            Menu.Add(new PixelAdminMenuEntry
                            {
                                Label = product,
                                Icon = ebcStatsIcon,
                                Url = ebcStatsAction,
                                Active = controller == "estats",
                                //OpenInNewWindow = openNewWindow // TODO: Enquanto as estatísticas estão num projeto à parte
                            });
                        }
                        else if (product.Equals("PI eBilling WWF"))
                        {
                            if (p.activo.HasValue && !p.activo.Value)
                            {
                                ebcWwfIcon = inactiveModuleIcon;
                                ebcWwfAction = url.Action(inactiveModuleDefaultAction, "eWWF");
                            }

                            Menu.Add(new PixelAdminMenuEntry
                            {
                                Label = product,
                                Icon = ebcWwfIcon,
                                Url = ebcWwfAction,
                                Active = controller == "ewwf"
                            });
                        }
                        else if (product.Equals("PI eSuite Users"))
                        {
                            if (p.activo.HasValue && !p.activo.Value)
                            {
                                identity.IsMCATActive = false;
                                //user.IsMCATActive = false;
                                ebcuserIcon = inactiveModuleIcon;
                                ebcusersAction = url.Action(inactiveModuleDefaultAction, "eUser");
                            }

                            Menu.Add(new PixelAdminMenuEntry
                            {
                                Label = product,
                                Icon = ebcuserIcon,
                                Url = ebcusersAction,
                                Active = controller == "euser"
                            });
                        }
                    }
                    
                }
            }
        }

        private string checkpermission(IeBillingSuiteIdentity identity, UrlHelper url)
        {
            if (identity.Permissions.Contains(Permissions.EDIGITAL_FORNECEDORES))
                return url.Action("", "eDigital");
            else if (identity.Permissions.Contains(Permissions.EDIGITAL_TEMPLATES))
                return url.Action("Template", "eDigital");
            else if (identity.Permissions.Contains(Permissions.EDIGITAL_GESTAOXML))
                return url.Action("XmlManagment", "eDigital");
            else if (identity.Permissions.Contains(Permissions.EDIGITAL_EXPIRACAODOCUMENTOS))
                return url.Action("DocumentsExpiration", "eDigital");
            else if (identity.Permissions.Contains(Permissions.EDIGITAL_HISTORICODOCUMENTOS))
                return url.Action("DocumentsHistoric", "eDigital");
            else if (identity.Permissions.Contains(Permissions.EDIGITAL_SINCRONIZACAO))
                return url.Action("Synchronization", "eDigital");
            else if (identity.Permissions.Contains(Permissions.EDIGITAL_INTEGRACAO))
                return url.Action("IntegrationConfigurations", "eDigital");
            else if (identity.Permissions.Contains(Permissions.EDIGITAL_STATS))
                return url.Action("DigitalStats", "eDigital");
            else if (identity.Permissions.Contains(Permissions.EDIGITAL_PROCESSAMENTO))
                return url.Action("DigitalProc", "eDigital");
            else if (identity.Permissions.Contains(Permissions.EDIGITAL_MAIL))
                return url.Action("instancesMails", "eDigital");

            return url.Action("", "eDigital");
        }
    }
}