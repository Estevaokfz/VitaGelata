using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VitaGelata.Models;

namespace VitaGelata.Utils
{
    internal class Repositorio
    {
        public static List<Sabor> Sabores { get; set; } = new List<Sabor>();
        public static List<Insumo> Insumos { get; set; } = new List<Insumo>();
        public static List<Producao> Producoes { get; set; } = new List<Producao>();
        public static List<Venda> Vendas { get; set; } = new List<Venda>();

    }
}
