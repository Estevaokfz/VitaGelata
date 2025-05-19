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
    public partial class FrmCadastroSabores : Form
    {
       
        int proximoId = 1;
        int? idSelecionado = null;
        

        private void LimparCampos()
        {
            txtNome.Clear();
            txtIngredientes.Clear();
            txtPreco.Clear();
            chkAtivo.Checked = true;
            idSelecionado = null;
        }

        public FrmCadastroSabores()
        {
            InitializeComponent();
        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            if (idSelecionado == null)
            {
                MessageBox.Show("Selecione um sabor para excluir.");
                return;
            }

            var sabor = Repositorio.Sabores.FirstOrDefault(s => s.Id == idSelecionado);

            if (sabor != null)
            {
                Repositorio.Sabores.Remove(sabor);
                AtualizarGridSabores();
                LimparCampos();
                receitaTemporaria.Clear();
                dgvReceita.DataSource = null;
            }

        }








        private void btnSalvar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNome.Text) || string.IsNullOrWhiteSpace(txtPreco.Text))
            {
                MessageBox.Show("Preencha todos os campos obrigatórios.");
                return;
            }
            if (Repositorio.Sabores.Any(s => s.Nome.Equals(txtNome.Text, StringComparison.OrdinalIgnoreCase)))
            {
                MessageBox.Show("Já existe um sabor com esse nome.");
                return;
            }

            if (!decimal.TryParse(txtPreco.Text, out decimal preco))
            {
                MessageBox.Show("Digite um valor válido para o preço.");
                return;
            }

            Sabor novo = new Sabor()
            {
                Id = proximoId++,
                Nome = txtNome.Text,
                Ingredientes = txtIngredientes.Text,
                Preco = preco,
                Ativo = chkAtivo.Checked,
                Receita = new List<InsumoConsumido>(receitaTemporaria)
            };

            Repositorio.Sabores.Add(novo);          // salvar no repositório central
            AtualizarGridSabores();                 // atualizar grid com todos os sabores
            LimparCampos();                         // limpar campos do sabor
            receitaTemporaria.Clear();              // limpar lista temporária
            dgvReceita.DataSource = null;           // limpar grid da receita
        }

        private void FrmCadastroSabores_Load(object sender, EventArgs e)
        {
            cmbInsumo.Items.Clear();
            foreach (var insumo in Repositorio.Insumos)
            {
                cmbInsumo.Items.Add(insumo.Nome);
            }
            if (cmbInsumo.Items.Count > 0)
                cmbInsumo.SelectedIndex = 0;

            AtualizarGridSabores();
        }

 
        private void chkAtivo_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (idSelecionado == null)
            {
                MessageBox.Show("Selecione um sabor.");
                return;
            }

            var sabor = Repositorio.Sabores.FirstOrDefault(s => s.Id == idSelecionado);

            if (sabor != null)
            {
                sabor.Nome = txtNome.Text;
                sabor.Ingredientes = txtIngredientes.Text;
                sabor.Preco = decimal.Parse(txtPreco.Text);
                sabor.Ativo = chkAtivo.Checked;
                sabor.Receita = new List<InsumoConsumido>(receitaTemporaria);

                AtualizarGridSabores();
                LimparCampos();
                receitaTemporaria.Clear();
                dgvReceita.DataSource = null;
            }
        }


        private void dgvSabores_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var nomeSelecionado = dgvSabores.Rows[e.RowIndex].Cells["Nome"].Value.ToString();
                var selecionado = Repositorio.Sabores.FirstOrDefault(s => s.Nome == nomeSelecionado);
                idSelecionado = selecionado.Id;
                txtNome.Text = selecionado.Nome;
                txtIngredientes.Text = selecionado.Ingredientes;
                txtPreco.Text = selecionado.Preco.ToString();
                chkAtivo.Checked = selecionado.Ativo;

                receitaTemporaria = new List<InsumoConsumido>(selecionado.Receita);
                AtualizarGridReceita(); // Para mostrar a receita no dgvReceita
            }

        }
        List<InsumoConsumido> receitaTemporaria = new List<InsumoConsumido>();
        private void btnAddInsumo_Click(object sender, EventArgs e)
        {
            

          
                if (!decimal.TryParse(txtQtdInsumo.Text, out decimal qtd) || qtd <= 0)
                {
                    MessageBox.Show("Digite uma quantidade válida.");
                    return;
                }

                var insumo = Repositorio.Insumos.FirstOrDefault(i => i.Nome == cmbInsumo.SelectedItem.ToString());
                if (insumo == null)
                {
                    MessageBox.Show("Insumo não encontrado.");
                    return;
                }

                receitaTemporaria.Add(new InsumoConsumido
                {
                    Insumo = insumo,
                    QuantidadeUtilizada = qtd
                });

                AtualizarGridReceita();
                txtQtdInsumo.Clear();
            }
        private void AtualizarGridReceita()
        {
            dgvReceita.DataSource = null;
            dgvReceita.DataSource = receitaTemporaria.Select(r => new
            {
                Insumo = r.Insumo.Nome,
                Quantidade = r.QuantidadeUtilizada
            }).ToList();
        }
        private void AtualizarGridSabores()
        {
            dgvSabores.DataSource = null;
            dgvSabores.DataSource = Repositorio.Sabores.Select(s => new
            {
                s.Id,
                s.Nome,
                s.Preco,
                s.Ativo,
                s.Ingredientes
            }).ToList();
        }

    }
    }

