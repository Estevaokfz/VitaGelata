using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace VitaGelata.Models
{
    public class Producao
    {
        public string NomeSabor { get; set; }
        public int Quantidade { get; set; }
        public DateTime Data { get; set; }
    }

    public class InsumoConsumido
    {
        public Insumo Insumo { get; set; }
        public decimal QuantidadeUtilizada { get; set; }
    }
}

