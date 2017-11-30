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

		public List<EBC_Instances> GetEBC_Instances()
		{
			return Set.ToList();
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
