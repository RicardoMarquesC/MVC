using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eBillingSuite.Support
{
    public class PhysicalPackage
    {
        public string InstallFolder { get; set; }

        public const string PACKAGE_SUFFIX = ".package";

        public const string UNSIGNED_PDF_FILE_SUFFIX = ".unsignedpdf";
        public const string SIGNED_PDF_FILE_SUFFIX = ".signedpdf";
        public const string POSTSCRIPT_FILE_SUFFIX = ".baseps";
        public const string BASIC_METADATA_FILE_SUFFIX = ".xml";
        public const string CUSTOM_METADATA_FILE_SUFFIX = ".custommetadata";
        public const string ATT1_SUFFIX = ".att1";
        public const string ATT2_SUFFIX = ".att2";
        public const string CANCELLATION_SUFFIX = ".cancel";
        public const string EMAIL_FILE_SUFFIX = ".email";
        public const string UNDELIVERED_RECEIPT_FILE_SUFFIX = ".undeliveredreceipt";
        public const string DELIVERED_RECEIPT_FILE_SUFFIX = ".deliveredreceipt";
        public const string READ_SUFFIX = ".emailreadreceipt";
        public const string RELAY_SUFFIX = ".emailrelay";
        public const string DELAY_SUFFIX = ".emaildelay";
        public const string EMAIL_REPLY_SUFFIX = ".emailreply";
        public const string SUSPENSION_SUFFIX = ".suspensiondata";

        public const string INBOUND_EMAIL_SUFFIX = ".eml";
        public const string INBOUND_PDF_SUFFIX = ".pdf";
        public const string INBOUND_XML_SUFFIX = ".xml";

        public PhysicalPackage()
        {
            // get installation folder
            InstallFolder = EbcConfiguration.GetConfiguration("InstallDir");
        }

        public bool PhysicalFileExists(string packageId, string fileExtension, string creationYear, string creationMonth, string sentido)
        {
            string path = String.Empty;

            if (sentido.ToLower() == "out")
                path = System.IO.Path.Combine(this.InstallFolder, "Storage", StringUtilities.padString('0', 4, creationYear.ToString(), false), StringUtilities.padString('0', 2, creationMonth.ToString(), false), packageId + PhysicalPackage.PACKAGE_SUFFIX, packageId + fileExtension);
            else
            {
                string subPath = String.Empty;

                switch (fileExtension.ToLower())
                {
                    case ".xml":
                        subPath = "INBOUND_METADATA";
                        break;
                    case ".pdf":
                        subPath = "INBOUND_DIGITAL_FILES";
                        break;
                    case ".eml":
                        subPath = "RECEIVED";
                        break;
                    default:
                        break;
                }

                path = System.IO.Path.Combine(this.InstallFolder, "IntegrationStorage", subPath, creationYear, creationMonth, packageId + fileExtension);
            }

            return (System.IO.File.Exists(path));
        }

        public string PhysicalFilePath(string fileName, string creationYear, string creationMonth, string sentido)
        {
            string id = fileName.Split('.').ElementAt(0);

            if (sentido.ToLower() == "out")
                return System.IO.Path.Combine(this.InstallFolder, "Storage", StringUtilities.padString('0', 4, creationYear.ToString(), false), StringUtilities.padString('0', 2, creationMonth.ToString(), false), id + PhysicalPackage.PACKAGE_SUFFIX, fileName);
            else
            {
                string extension = fileName.Split('.').ElementAt(1);
                string subPath = String.Empty;

                switch ("." + extension.ToLower())
                {
                    case ".xml":
                        subPath = "INBOUND_METADATA";
                        break;
                    case ".pdf":
                        subPath = "INBOUND_DIGITAL_FILES";
                        break;
                    case ".eml":
                        subPath = "RECEIVED";
                        break;
                    default:
                        break;
                }

                return System.IO.Path.Combine(this.InstallFolder, "IntegrationStorage", subPath, creationYear, creationMonth, fileName);

//                return System.IO.Path.Combine(this.InstallFolder, "IntegrationStorage", subPath, StringUtilities.padString('0', 4, creationYear.ToString(), false), StringUtilities.padString('0', 2, creationMonth.ToString(), false), fileName);
            }
        }

        internal string PhysicalFilenameWithRealExtension(string filename)
        {
            string[] filenameParts = filename.Split('.');

            string fakeExtension = filenameParts.ElementAt(1);

            string realExtension = String.Empty;

            switch ("." + fakeExtension.ToLower())
            {
                case UNSIGNED_PDF_FILE_SUFFIX:
                    realExtension = ".pdf";
                    break;
                case SIGNED_PDF_FILE_SUFFIX:
                    realExtension = ".pdf";
                    break;
                case POSTSCRIPT_FILE_SUFFIX:
                    realExtension = ".ps";
                    break;
                case BASIC_METADATA_FILE_SUFFIX:
                    realExtension = ".xml";
                    break;
                case CUSTOM_METADATA_FILE_SUFFIX:
                    realExtension = ".data" + EbcConfiguration.GetConfiguration("CustomFileExtension");
                    break;
                case ATT1_SUFFIX:
                    realExtension = ".pdf";
                    break;
                case ATT2_SUFFIX:
                    realExtension = ".pdf";
                    break;
                case CANCELLATION_SUFFIX:
                    realExtension = ".pdf";
                    break;
                case EMAIL_FILE_SUFFIX:
                    realExtension = ".eml";
                    break;
                case UNDELIVERED_RECEIPT_FILE_SUFFIX:
                    realExtension = ".eml";
                    break;
                case DELIVERED_RECEIPT_FILE_SUFFIX:
                    realExtension = ".eml";
                    break;
                case READ_SUFFIX:
                    realExtension = ".eml";
                    break;
                case RELAY_SUFFIX:
                    realExtension = ".eml";
                    break;
                case DELAY_SUFFIX:
                    realExtension = ".eml";
                    break;
                case EMAIL_REPLY_SUFFIX:
                    realExtension = ".eml";
                    break;
                case SUSPENSION_SUFFIX:
                    realExtension = ".eml";
                    break;
                default:
                    realExtension = fakeExtension;
                    break;
            }

            return (filenameParts.ElementAt(0) + realExtension);
        }
    }
}
