using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eBillingSuite.ViewModels;

namespace eBillingSuite.Repositories.Interfaces
{
    public interface IEBC_PackageRepository
    {
        PacketDetailVM GetPacketById(string id, string direction);

        string GetFilePath(string filename, string sentido, string ano, string mes);
        string GetFilenameWithRealExtension(string filename);
        void UpdateStatusByID(string ebcPackageId);
        void InserEventsByID(string ebcPackageId, int packagestate, int eventtype);
        void InserEventsByIDwObs(string ebcPackageId, int packagestate, int eventtype, string obs);
    }
}
