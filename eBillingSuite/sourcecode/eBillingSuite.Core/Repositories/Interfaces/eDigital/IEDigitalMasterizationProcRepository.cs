﻿using eBillingSuite.Model.Desmaterializacao;
using Shortcut.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eBillingSuite.Repositories
{
    public interface IEDigitalMasterizationProcRepository : IGenericRepository<IeBillingSuiteDesmaterializacaoContext, ProcDocs>
    {
    }
}
