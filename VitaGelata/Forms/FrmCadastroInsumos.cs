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
    public partial class FrmCadastroInsumos : Form
    {
        int proximoId = 1;
        int? idSelecionado = null;

        public FrmCadastroInsumos()
        {
            InitializeComponent();
        }

        private void FrmCadastroInsumos_Load(object sender, EventArgs e)
        {
            AtualizarGrid();
        }

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNome.Text) || string.IsNullOrWhiteSpace(txtQuantidade.Text))
            {
                MessageBox.Show("Preencha os campos obrigatórios.");
                return;
            }

            if (!decimal.TryParse(txtQuantidade.Text, out decimal quantidade))
            {
                MessageBox.Show("Quantidade inválida.");
                return;
            }

            Insumo novo = new Insumo()
            {
                Id = proximoId++,
                Nome = txtNome.Text,
                UnidadeMedida = txtUnidade.Text,
                QuantidadeEstoque = quantidade,
                Validade = dtpValidade.Value,
                Fornecedor = txtFornecedor.Text
            };

            Repositorio.Insumos.Add(novo);
            AtualizarGrid();
            LimparCampos();
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (idSelecionado == null)
            {
                MessageBox.Show("Selecione um insumo.");
                return;
            }

            var insumo = Repositorio.Insumos.FirstOrDefault(i => i.Id == idSelecionado);
            if (insumo != null)
            {
                insumo.Nome = txtNome.Text;
                insumo.UnidadeMedida = txtUnidade.Text;
                insumo.QuantidadeEstoque = decimal.Parse(txtQuantidade.Text);
                insumo.Validade = dtpValidade.Value;
                insumo.Fornecedor = txtFornecedor.Text;

                AtualizarGrid();
                LimparCampos();
            }
        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            if (idSelecionado == null)
            {
                MessageBox.Show("Selecione um insumo para excluir.");
                return;
            }

            var insumo = Repositorio.Insumos.FirstOrDefault(i => i.Id == idSelecionado);
            if (insumo != null)
            {
                Repositorio.Insumos.Remove(insumo);
                AtualizarGrid();
                LimparCampos();
            }
        }

        private void btnLimpar_Click(object sender, EventArgs e)
        {
            LimparCampos();
        }

        private void dgvInsumos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var nomeSelecionado = dgvInsumos.Rows[e.RowIndex].Cells["Nome"].Value.ToString();
                var insumo = Repositorio.Insumos.FirstOrDefault(i => i.Nome == nomeSelecionado);
                if (insumo != null)
                {
                    idSelecionado = insumo.Id;
                    txtNome.Text = insumo.Nome;
                    txtUnidade.Text = insumo.UnidadeMedida;
                    txtQuantidade.Text = insumo.QuantidadeEstoque.ToString();
                    dtpValidade.Value = insumo.Validade;
                    txtFornecedor.Text = insumo.Fornecedor;
                }
            }
        }

        private void AtualizarGrid()
        {
            dgvInsumos.DataSource = null;
            dgvInsumos.DataSource = Repositorio.Insumos.Select(i => new
            {
                i.Id,
                i.Nome,
                i.UnidadeMedida,
                i.QuantidadeEstoque,
                i.Validade,
                i.Fornecedor
            }).ToList();
        }

        private void LimparCampos()
        {
            txtNome.Clear();
            txtUnidade.Clear();
            txtQuantidade.Clear();
            txtFornecedor.Clear();
            dtpValidade.Value = DateTime.Today;
            idSelecionado = null;
        }
    }
}