using eBillingSuite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eBillingSuite.ViewModels
{
    public class SupportChatVM
    {
        public SupportChatVM(string tipoPedido, string descricao, string email)
        {
            this.TipoPedido = tipoPedido;
            this.Descricao = descricao;
            this.Email = email;
        }

        public SupportChatVM(TicketsData data)
        {
            this.TipoPedido = data.TipoPedido;
            this.Descricao = data.Descricao;
            this.Email = data.Email;
        }


        public string TipoPedido { get; set; }
        public string Descricao { get; set; }
        public string Email { get; set; }
    }
}