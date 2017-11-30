using Ninject;
using Shortcut.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using eBillingSuite.Model.EBC_DB;

namespace eBillingSuite.Repositories
{
	public class InstancesRepository : GenericRepository<IeBillingSuiteEBCDBContext, EBC_Instances>, IInstancesRepository
	{

        [Inject]
		public InstancesRepository(IeBillingSuiteEBCDBContext context)
			: base(context)
		{
		}

		//public List<EBC_Instances> GetEBC_Instances(_context.UserIdentity.Instances)
		//{
		//	return Set.ToList();
		//}

        public List<EBC_Instances> GetEBC_Instances(string instances)
        {
            try
            {
                if (instances == "*")
                {
                    return Set.ToList();
                }
                else
                {
                    string[] splittedInstances = instances.Split(';');
                    return Where(o => splittedInstances.Contains(o.NIF)).ToList();
                }
            }
            catch (Exception)
            {
                return null;
            }

        }

        public Guid GetSpecificOptionIDByInstance(Guid id)
		{
			return this
				.Where(ebci => ebci.PKID == id)
				.FirstOrDefault()
				.FKSpecificDeliveryOptionsID;
		}
	}
}
