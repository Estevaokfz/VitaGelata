﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VitaGelata.Utils;
using ClosedXML.Excel;
using System.IO;
using DocumentFormat.OpenXml.Office2010.PowerPoint;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;



namespace VitaGelata
{
    public partial class FrmRelatorioDeVendas : Form
    {
        public FrmRelatorioDeVendas()
        {
            InitializeComponent();
        }

        private void FrmRelatorioDeVendas_Load(object sender, EventArgs e)
        {
            // Preencher o ComboBox de sabores
            cmbSabor.Items.Clear();
            cmbSabor.Items.Add("Todos"); // opção para buscar tudo

            foreach (var sabor in Repositorio.Sabores)
            {
                cmbSabor.Items.Add(sabor.Nome);
            }

            if (cmbSabor.Items.Count > 0)
                cmbSabor.SelectedIndex = 0;

            // Datas padrão (do mês atual)
            dtpDataInicial.Value = DateTime.Today.AddDays(-30);
            dtpDataFinal.Value = DateTime.Today;

            AtualizarGrid();
        }

        private void AtualizarGrid()
        {
            // Filtros
            DateTime dataInicial = dtpDataInicial.Value.Date;
            DateTime dataFinal = dtpDataFinal.Value.Date;
            string saborSelecionado = cmbSabor.SelectedItem.ToString();

            // Filtrando as vendas
            var vendasFiltradas = Repositorio.Vendas.Where(v =>
                v.Data.Date >= dataInicial &&
                v.Data.Date <= dataFinal &&
                (saborSelecionado == "Todos" || v.NomeSabor == saborSelecionado)
            ).ToList();

            // Carregar no DataGridView
            dgvRelatorio.DataSource = null;
            dgvRelatorio.DataSource = vendasFiltradas.Select(v => new
            {
                Data = v.Data.ToShortDateString(),
                Sabor = v.NomeSabor,
                Quantidade = v.Quantidade + "g",
                Valor = v.Valor.ToString("C")
            }).ToList();

            // Calcular o total
            decimal total = vendasFiltradas.Sum(v => v.Valor);

            txtTotal.Text = total.ToString("C");
        }

        private void btnExportarExcel_Click(object sender, EventArgs e)
        {
            if (dgvRelatorio.Rows.Count == 0)
            {
                MessageBox.Show("Não há dados para exportar.");
                return;
            }

            SaveFileDialog salvar = new SaveFileDialog();
            salvar.Filter = "Arquivo Excel (*.xlsx)|*.xlsx";
            salvar.FileName = "RelatorioVendas.xlsx";

            if (salvar.ShowDialog() == DialogResult.OK)
            {
                using (var workbook = new XLWorkbook())
                {
                    var planilha = workbook.Worksheets.Add("Relatório de Vendas");

                    // Cabeçalho
                    for (int i = 0; i < dgvRelatorio.Columns.Count; i++)
                    {
                        planilha.Cell(1, i + 1).Value = dgvRelatorio.Columns[i].HeaderText;
                        planilha.Cell(1, i + 1).Style.Font.Bold = true;
                    }

                    // Dados
                    for (int i = 0; i < dgvRelatorio.Rows.Count; i++)
                    {
                        for (int j = 0; j < dgvRelatorio.Columns.Count; j++)
                        {
                            planilha.Cell(i + 2, j + 1).Value = dgvRelatorio.Rows[i].Cells[j].Value?.ToString();
                        }
                    }

                    // Salvar o arquivo
                    workbook.SaveAs(salvar.FileName);
                }

                MessageBox.Show("Relatório exportado com sucesso!", "Exportação", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }


        private void btnExportarPDF_Click(object sender, EventArgs e)
        {
            if (dgvRelatorio.Rows.Count == 0)
            {
                MessageBox.Show("Não há dados para exportar.");
                return;
            }

            SaveFileDialog salvar = new SaveFileDialog();
            salvar.Filter = "Arquivo PDF (*.pdf)|*.pdf";
            salvar.FileName = "RelatorioVendas.pdf";

            if (salvar.ShowDialog() == DialogResult.OK)
            {
                Document doc = new Document(PageSize.A4.Rotate(), 10, 10, 20, 20);
                PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(salvar.FileName, FileMode.Create));

                doc.Open();

                // 🎯 Título
                var titulo = new Paragraph("RELATÓRIO DE VENDAS", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 18));
                titulo.Alignment = Element.ALIGN_CENTER;
                doc.Add(titulo);

                doc.Add(new Paragraph("\n"));

                // 🎯 Filtros aplicados
                string filtros = $"Período: {dtpDataInicial.Value.ToShortDateString()} a {dtpDataFinal.Value.ToShortDateString()}";
                if (cmbSabor.SelectedItem != null && cmbSabor.SelectedItem.ToString() != "Todos")
                    filtros += $" | Sabor: {cmbSabor.SelectedItem}";

                var filtroParagrafo = new Paragraph(filtros, FontFactory.GetFont(FontFactory.HELVETICA, 12));
                filtroParagrafo.Alignment = Element.ALIGN_LEFT;
                doc.Add(filtroParagrafo);

                doc.Add(new Paragraph("\n"));

                // 🎯 Tabela
                PdfPTable table = new PdfPTable(dgvRelatorio.Columns.Count);
                table.WidthPercentage = 100;

                // Cabeçalho da tabela
                foreach (DataGridViewColumn column in dgvRelatorio.Columns)
                {
                    PdfPCell cell = new PdfPCell(new Phrase(column.HeaderText, FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12)));
                    cell.BackgroundColor = new BaseColor(230, 230, 230);
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    table.AddCell(cell);
                }

                // Dados da tabela
                foreach (DataGridViewRow row in dgvRelatorio.Rows)
                {
                    if (!row.IsNewRow)
                    {
                        foreach (DataGridViewCell cell in row.Cells)
                        {
                            table.AddCell(new Phrase(cell.Value?.ToString() ?? ""));
                        }
                    }
                }

                doc.Add(table);

                // 🎯 Totalizador
                doc.Add(new Paragraph("\n"));
                var totalizador = new Paragraph($"Total Geral: {txtTotal.Text}", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 14));
                totalizador.Alignment = Element.ALIGN_RIGHT;
                doc.Add(totalizador);

                doc.Close();
                writer.Close();

                MessageBox.Show("Relatório PDF exportado com sucesso!", "Exportação", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }



        private void dtpDataInicial_ValueChanged(object sender, EventArgs e)
        {
            AtualizarGrid();
        }

        private void dtpDataFinal_ValueChanged(object sender, EventArgs e)
        {
            AtualizarGrid();
        }

        private void cmbSabor_SelectedIndexChanged(object sender, EventArgs e)
        {
            AtualizarGrid();
        }
    }
}

