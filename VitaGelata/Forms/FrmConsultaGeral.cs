using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VitaGelata.Utils;

namespace VitaGelata.Forms
{
    public partial class FrmConsultaGeral : Form
    {
        public FrmConsultaGeral()
        {
            InitializeComponent();
        }

        private void FrmConsultaGeral_Load(object sender, EventArgs e)
        {
            AtualizarGridEstoque();
            AtualizarGridSabores();
        }

        private void AtualizarGridEstoque()
        {
            dgvEstoque.DataSource = null;
            dgvEstoque.DataSource = Repositorio.Insumos.Select(i => new
            {
                i.Id,
                i.Nome,
                Unidade = i.UnidadeMedida,
                Quantidade = i.QuantidadeEstoque,
                Validade = i.Validade.ToShortDateString(),
                i.Fornecedor
            }).ToList();
        }

        private void AtualizarGridSabores()
        {
            dgvReceita.DataSource = null;
            dgvReceita.DataSource = Repositorio.Sabores.Select(s => new
            {
                s.Id,
                s.Nome,
                s.Preco,
                s.Ativo,
                s.Ingredientes
            }).ToList();
        }

        private void dgvSabores_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var nomeSelecionado = dgvReceita.Rows[e.RowIndex].Cells["Nome"].Value.ToString();
                var sabor = Repositorio.Sabores.FirstOrDefault(s => s.Nome == nomeSelecionado);

                if (sabor != null)
                {
                    dgvReceita.DataSource = null;
                    dgvReceita.DataSource = sabor.Receita.Select(r => new
                    {
                        Insumo = r.Insumo.Nome,
                        Quantidade = r.QuantidadeUtilizada
                    }).ToList();
                }
            }
        }
    }
}
