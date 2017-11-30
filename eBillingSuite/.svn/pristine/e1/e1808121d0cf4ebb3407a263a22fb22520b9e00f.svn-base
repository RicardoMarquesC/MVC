using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eBillingSuite.Model.HelpingClasses
{
    public enum TiposEvento
    {
        Debug = 10,
        Info = 20,
        Warning = 30,
        Error = 40,
        Fatal = 50
    }

    public enum ModulosEvento
    {
        ActiveDirectory = 10,
        Jira = 20,
        Primavera = 30
    }

    public class RegistoEvento
    {
        public int ID { get; set; }
        public DateTime DataHora { get; set; }
        public TiposEvento Tipo { get; set; }
        public ModulosEvento Modulo { get; set; }
        public string Mensagem { get; set; }
        public string Detalhe { get; set; }

        public static List<RegistoEvento> GetDummy()
        {
            return new List<RegistoEvento>{
                new RegistoEvento{ID = 1, DataHora = new DateTime(2013,11,25, 12,25,00), Tipo = TiposEvento.Info, Modulo=ModulosEvento.ActiveDirectory, Mensagem = "Início de sincronização."},
                new RegistoEvento{ID = 2, DataHora = new DateTime(2013,11,25, 12,25,12), Tipo = TiposEvento.Debug, Modulo=ModulosEvento.ActiveDirectory, Mensagem = "Encontrou três utilizadores não sincronizados."},
                new RegistoEvento{ID = 3, DataHora = new DateTime(2013,11,25, 12,25,24), Tipo = TiposEvento.Info, Modulo=ModulosEvento.ActiveDirectory, Mensagem = "Sincronizou com sucesso o utilizador ISA\\Username001."},
                new RegistoEvento{ID = 4, DataHora = new DateTime(2013,11,25, 12,25,33), Tipo = TiposEvento.Info, Modulo=ModulosEvento.ActiveDirectory, Mensagem = "Sincronizou com sucesso o utilizador ISA\\Username002."},
                new RegistoEvento{ID = 5, DataHora = new DateTime(2013,11,25, 12,25,46), Tipo = TiposEvento.Warning, Modulo=ModulosEvento.ActiveDirectory, Mensagem = "Utilizador ISA\\Username003 não tem NomeCompleto definido na ActiveDirectory."},
                new RegistoEvento{ID = 6, DataHora = new DateTime(2013,11,25, 12,25,54), Tipo = TiposEvento.Error, Modulo=ModulosEvento.ActiveDirectory, Mensagem = "Não foi possível aceder à ActiveDirectory.", Detalhe="Permissões negadas."},
                new RegistoEvento{ID = 7, DataHora = new DateTime(2013,11,25, 12,25,54), Tipo = TiposEvento.Fatal, Modulo=ModulosEvento.ActiveDirectory, Mensagem = "Não foi possível comunicar com o JIRA.", Detalhe="Permissões negadas."}
            };
        }
    }
}
