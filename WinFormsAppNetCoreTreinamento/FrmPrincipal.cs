using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace WinFormsAppNetCoreTreinamento
{
    public partial class FrmPrincipal : Form
    {
        public FrmPrincipal()
        {
            InitializeComponent();
        }

        private void sairToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Fecha o formulário atual
            Close();
        }

        private void arquivoTxtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Abre o formulário FrmTxt
            FrmTxt frmTxt = new FrmTxt();
            frmTxt.ShowDialog();

        }

        private void arquivoJsonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Abre o formulário FrmJson
            FrmJson frmJson = new FrmJson();
            frmJson.ShowDialog();
        }

        private void arquivoXlsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Abre o formulário FrmXls
            FrmXls frmXls = new FrmXls();
            frmXls.ShowDialog();
        }

        private void arquivoXlsxToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Abre o formulário FrmXlsx
            FrmXlsx frmXlsx = new FrmXlsx();
            frmXlsx.ShowDialog();
        }

        private void arquivoXmlToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Abre o formulário FrmXml
            FrmXml frmXml = new FrmXml();
            frmXml.ShowDialog();
        }

        private void emailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Abre o formulário FrmEmail
            FrmEmail frmEmail = new FrmEmail();
            frmEmail.ShowDialog();
        }
    }
}
