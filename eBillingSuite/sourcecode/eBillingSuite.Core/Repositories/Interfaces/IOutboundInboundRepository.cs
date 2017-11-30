using eBillingSuite.Model.HelpingClasses;
using eBillingSuite.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eBillingSuite.Security;

namespace eBillingSuite.Repositories.Interfaces
{
    public interface IOutboundInboundRepository
    {
        List<OutboundInbound> GetAllPackets(IeBillingSuiteIdentity userIdentity);
        List<OutboundInbound> GetAllPacketsCurrentMonth();
        string GetOutboundInboundSumValue();
        string[] GetOutboundInboudMothValues();

        void UpdatePackets(int[] finalOutIds, int[] finalInIds);

        PacketDetailVM GetPacketById(int? id, string direction);

        string GetFilePath(string filename, string sentido, string ano, string mes);

        string GetFilenameWithRealExtension(string filename);
        void UpdateOutStatToDelivered(string id);
    }
}
