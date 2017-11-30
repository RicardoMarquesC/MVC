using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eBillingSuite.Model.Desmaterializacao
{
    public partial class ProcDocs
    {
        [Key]
        public Guid pkid { get; set; }

        [Required]
        public string nomeficheiro { get; set; }

        [Required]
        public string fornecedor { get; set; }

        [Required]
        [StringLength(250)]
        public string company { get; set; }

        [Required]
        public DateTime dtaCriacao { get; set; }

        [Required]
        public int Estado { get; set; }

        [Required]
        public int Prioridade { get; set; }

        public DateTime DtaModificacao { get; set; }

        public string Utilizador { get; set; }
        
        public string tpoFatura { get; set;  }

        public string DocNumber { get; set; }
    }
}
