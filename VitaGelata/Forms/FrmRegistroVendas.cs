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
    public partial class FrmRegistroVendas : Form
    {
        public FrmRegistroVendas()
        {
            InitializeComponent();
        }

        private void FrmRegistroVendas_Load(object sender, EventArgs e)
        {
            cmbSabor.Items.Clear();

            foreach (var sabor in Repositorio.Sabores)
            {
                cmbSabor.Items.Add(sabor.Nome);
            }

            if (cmbSabor.Items.Count > 0)
                cmbSabor.SelectedIndex = 0;

            dtpDataVenda.Value = DateTime.Today;

            AtualizarGridVendas();
        }

        private void btnRegistrar_Click(object sender, EventArgs e)
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

            if (!decimal.TryParse(txtValor.Text, out decimal valor) || valor <= 0)
            {
                MessageBox.Show("Digite um valor válido para a venda.");
                return;
            }

            Repositorio.Vendas.Add(new Venda
            {
                NomeSabor = cmbSabor.SelectedItem.ToString(),
                Quantidade = quantidade,
                Data = dtpDataVenda.Value,
                Valor = valor
            });

            MessageBox.Show("Venda registrada com sucesso!");
            AtualizarGridVendas();
            LimparCampos();
        }

        private void AtualizarGridVendas()
        {
            dgvVendas.DataSource = null;
            dgvVendas.DataSource = Repositorio.Vendas.Select(v => new
            {
                v.NomeSabor,
                Quantidade = v.Quantidade + "g",
                Valor = "R$ " + v.Valor.ToString("F2"),
                Data = v.Data.ToShortDateString()
            }).ToList();
        }

        private void LimparCampos()
        {
            if (cmbSabor.Items.Count > 0)
                cmbSabor.SelectedIndex = 0;

            txtQuantidade.Clear();
            txtValor.Clear();
            dtpDataVenda.Value = DateTime.Today;
        }

        private void CalcularValorTotal()
        {
            if (cmbSabor.SelectedItem == null) return;
            if (!int.TryParse(txtQuantidade.Text, out int quantidade) || quantidade <= 0) return;

            var nomeSabor = cmbSabor.SelectedItem.ToString();
            var sabor = Repositorio.Sabores.FirstOrDefault(s => s.Nome == nomeSabor);

            if (sabor != null)
            {
                decimal valorPorGrama = sabor.Preco / 1000m;
                decimal valorTotal = valorPorGrama * quantidade;
                txtValor.Text = valorTotal.ToString("F2");
            }
        }

        private void cmbSabor_SelectedIndexChanged(object sender, EventArgs e)
        {
            CalcularValorTotal();
        }

        private void txtQuantidade_TextChanged(object sender, EventArgs e)
        {
            CalcularValorTotal();
        }

        private void btnLimpar_Click(object sender, EventArgs e)
        {
            LimparCampos();
        }
    }
}
