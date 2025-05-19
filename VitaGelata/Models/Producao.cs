using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace VitaGelata.Models
{
    public class Producao
    {
        public int Id { get; set; }
        public Sabor SaborProduzido { get; set; }
        public decimal Quantidade { get; set; }
        public DateTime DataProducao { get; set; }
        public List<InsumoConsumido> InsumosUtilizados { get; set; }
    }

    public class InsumoConsumido
    {
        public Insumo Insumo { get; set; }
        public decimal QuantidadeUtilizada { get; set; }
    }
}

