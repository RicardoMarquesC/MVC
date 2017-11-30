using eBillingSuite.Model.EBC_DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eBillingSuite.ViewModels
{
    public class EBCStats
    {
        public List<EBC_Instances> listInstances { get; set; }

        public EBCStats(IECCListRepositories _eBCConfigurationsRepository, string instances)
        {
            listInstances = _eBCConfigurationsRepository.instancesRepository.GetEBC_Instances(instances);
        }

    }
}