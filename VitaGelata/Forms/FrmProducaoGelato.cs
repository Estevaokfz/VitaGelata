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

      

        private void btnConfirmar_Click(object sender, EventArgs e)
        {
            if (cmbSabor.SelectedItem == null)
            {
                MessageBox.Show("Selecione um sabor.");
                return;
            }

            if (!int.TryParse(txtQuantidade.Text, out int quantidade) || quantidade <= 0)
            {
                MessageBox.Show("Digite uma quantidade válida.");
                return;
            }

            string nomeSabor = cmbSabor.SelectedItem.ToString();
            var sabor = Repositorio.Sabores.FirstOrDefault(s => s.Nome == nomeSabor);

            if (sabor == null)
            {
                MessageBox.Show("Sabor não encontrado.");
                return;
            }

            // Verificar se há insumos suficientes
            foreach (var item in sabor.Receita)
            {
                var insumo = Repositorio.Insumos.FirstOrDefault(i => i.Id == item.Insumo.Id);
                decimal quantidadeNecessaria = item.QuantidadeUtilizada * quantidade;

                if (insumo == null || insumo.QuantidadeEstoque < quantidadeNecessaria)
                {
                    MessageBox.Show($"Estoque insuficiente para o insumo: {item.Insumo.Nome}");
                    return;
                }
            }

            // Subtrair insumos
            foreach (var item in sabor.Receita)
            {
                var insumo = Repositorio.Insumos.FirstOrDefault(i => i.Id == item.Insumo.Id);
                insumo.QuantidadeEstoque -= item.QuantidadeUtilizada * quantidade;
            }

            MessageBox.Show("Produção registrada com sucesso!");
            txtQuantidade.Clear();

            Repositorio.Producoes.Add(new Producao
            {
                NomeSabor = sabor.Nome,
                Quantidade = quantidade,
                Data = dtpDataProducao.Value
            });

            AtualizarGridProducoes();
        }
        private void btnLimpar_Click(object sender, EventArgs e)
        {
            if (cmbSabor.Items.Count > 0)
                cmbSabor.SelectedIndex = 0;

            txtQuantidade.Clear();
            dtpDataProducao.Value = DateTime.Today;
        }
        private void AtualizarGridProducoes()
        {
            dgvProducoes.DataSource = null;

            // Declarando a variável corretamente
            var producoes = Repositorio.Producoes.Select(p => new
            {
                p.NomeSabor,
                p.Quantidade,
                Data = p.Data.ToShortDateString()
            }).ToList();

            dgvProducoes.DataSource = producoes;

            // Mostrar ou ocultar o label com base no conteúdo
            lblSemProducoes.Visible = producoes.Count == 0;
        }




        private void ProducaoGelato_Load(object sender, EventArgs e)
        {
            // carregar os sabores no ComboBox
            cmbSabor.Items.Clear();
            foreach (var sabor in Repositorio.Sabores)
            {
                cmbSabor.Items.Add(sabor.Nome);
            }
            if (cmbSabor.Items.Count > 0)
                cmbSabor.SelectedIndex = 0;

            dtpDataProducao.Value = DateTime.Today;

            // carregar produções existentes
            AtualizarGridProducoes();
        }
    }
}
