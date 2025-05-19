using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VitaGelata.Models
{
    public class Sabor
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Ingredientes { get; set; }
        public decimal Preco { get; set; }
        public bool Ativo { get; set; }
        public List<InsumoConsumido> Receita { get; set; } = new List<InsumoConsumido>();
    }
}
