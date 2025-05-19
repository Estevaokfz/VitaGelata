using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VitaGelata.Models
{
    public class Venda
    {
        public int Id { get; set; }
        public Sabor ProdutoVendido { get; set; }
        public int Quantidade { get; set; }
        public string FormaPagamento { get; set; }
        public decimal ValorTotal { get; set; }
        public DateTime DataHora { get; set; }
    }
}
