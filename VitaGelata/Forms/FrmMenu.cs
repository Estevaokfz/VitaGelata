using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VitaGelata.Forms;


namespace VitaGelata
{
    public partial class FrmMenu : Form
    {
        public FrmMenu()
        {
            InitializeComponent();
        }

        private void btnSabores_Click(object sender, EventArgs e)
        {
            FrmCadastroSabores frm = new FrmCadastroSabores();
            frm.ShowDialog();
        }

        private void btnInsumos_Click(object sender, EventArgs e)
        {
            FrmCadastroInsumos frm = new FrmCadastroInsumos();
            frm.ShowDialog();
        }

        private void btnProducao_Click(object sender, EventArgs e)
        {
             FrmProducaoGelato frm = new FrmProducaoGelato();
             frm.ShowDialog();
        }

        private void btnVendas_Click(object sender, EventArgs e)
        {
           FrmRegistroVendas frm = new FrmRegistroVendas();
           frm.ShowDialog();
        }

        private void btnRelatorios_Click(object sender, EventArgs e)
        {
            FrmRelatorioDeVendas frm = new FrmRelatorioDeVendas();
            frm.ShowDialog();
        }

        private void btnSair_Click(object sender, EventArgs e)
        {
            Close(); 
        }

        private void FrmMenu_Load(object sender, EventArgs e)
        {

        }

        private void btnConsultaGeral_Click(object sender, EventArgs e)
        {
            FrmConsultaGeral consulta = new FrmConsultaGeral();
            consulta.ShowDialog(); // Abre a janela de forma modal (trava o menu até fechar)
        }
    }
}
