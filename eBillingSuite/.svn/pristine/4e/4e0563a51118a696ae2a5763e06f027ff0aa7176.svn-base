using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shortcut.Repositories;
using eBillingSuite.Model;
using eBillingSuite.Security;
using eBillingSuite.Model.EBC_DB;

namespace eBillingSuite.Repositories
{
    public interface IOutboundInboundRepository
    {
        List<OutboundInbound> GetAllPackets();
        List<OutboundInbound> GetAllPacketsCurrentMonth();
        string GetOutboundInboundSumValue();
        string[] GetOutboundInboudMothValues();

        void UpdatePackets(int[] finalOutIds, int[] finalInIds);

        object GetPacketById(int id, string direction);

        string GetFilePath(string filename, string sentido, string ano, string mes);

        string GetFilenameWithRealExtension(string filename);
        void UpdateOutStatToDelivered(string id);
    }
}
