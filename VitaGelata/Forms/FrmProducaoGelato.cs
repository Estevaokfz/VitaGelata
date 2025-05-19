using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VitaGelata.Models;
using VitaGelata.Utils;

namespace VitaGelata
{
    public partial class FrmProducaoGelato : Form
    {
        List<Producao> listaProducoes = new List<Producao>();
        int proximoId = 1;

        public FrmProducaoGelato()
        {
            InitializeComponent();
        }

        private void FrmProducaoGelato_Load(object sender, EventArgs e)
        {
            // Exemplo de sabores disponíveis — substitua por dados reais depois
            cmbSabor.Items.Add("Chocolate");
            cmbSabor.Items.Add("Morango");
            cmbSabor.Items.Add("Baunilha");

            cmbSabor.SelectedIndex = 0;
            dtpData.Value = DateTime.Today;
        }

        private void btnConfirmar_Click(object sender, EventArgs e)
        {
            if (cmbSabor.SelectedItem == null || string.IsNullOrWhiteSpace(txtQuantidade.Text))
            {
                MessageBox.Show("Preencha todos os campos.");
                return;
            }

            if (!decimal.TryParse(txtQuantidade.Text, out decimal qtd))
            {
                MessageBox.Show("Quantidade inválida.");
                return;
            }

            // Simular insumos consumidos (padrão fictício)
            List<InsumoConsumido> insumos = new List<InsumoConsumido>()
            {
                new InsumoConsumido { Insumo = new Insumo { Nome = "Leite" }, QuantidadeUtilizada = qtd * 2 },
                new InsumoConsumido { Insumo = new Insumo { Nome = "Açúcar" }, QuantidadeUtilizada = qtd * 0.5m },
                new InsumoConsumido { Insumo = new Insumo { Nome = "Sabor Especial" }, QuantidadeUtilizada = qtd }
            };

            // Adiciona à lista de produções
            Producao producao = new Producao()
            {
                Id = proximoId++,
                SaborProduzido = new Sabor { Nome = cmbSabor.SelectedItem.ToString() },
                Quantidade = qtd,
                DataProducao = dtpData.Value,
                InsumosUtilizados = insumos
            };

            listaProducoes.Add(producao);

            // Exibir no grid
            dgvInsumosUsados.DataSource = null;
            dgvInsumosUsados.DataSource = insumos.ConvertAll(i => new
            {
                Nome = i.Insumo.Nome,
                Quantidade = i.QuantidadeUtilizada
            });

            MessageBox.Show("Produção registrada com sucesso.");
        }

        private void btnLimpar_Click(object sender, EventArgs e)
        {
            cmbSabor.SelectedIndex = 0;
            txtQuantidade.Clear();
            dtpData.Value = DateTime.Today;
            dgvInsumosUsados.DataSource = null;
        }
    
   

        private void ProducaoGelato_Load(object sender, EventArgs e)
        {
            cmbSabor.Items.Clear();
            foreach (var sabor in Repositorio.Sabores)
            {
                cmbSabor.Items.Add(sabor.Nome);
            }
            if (cmbSabor.Items.Count > 0)
                cmbSabor.SelectedIndex = 0;
        }
    }
}
