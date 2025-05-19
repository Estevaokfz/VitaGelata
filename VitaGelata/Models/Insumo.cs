using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VitaGelata.Models
{
    public class Insumo
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string UnidadeMedida { get; set; }
        public decimal QuantidadeEstoque { get; set; }
        public DateTime Validade { get; set; }
        public string Fornecedor { get; set; }
    }
}

