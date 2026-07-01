using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using NPOI.HSSF.UserModel; // Para XLS (formato antigo)
using NPOI.SS.UserModel;

namespace WinFormsAppNetCoreTreinamento
{
    public partial class FrmXls : Form
    {
        public FrmXls()
        {
            InitializeComponent();
        }

        private void BtnCriar_Click(object sender, EventArgs e)
        {
            // Criar novo arquivo Excel
            IWorkbook workbook = new HSSFWorkbook(); // XLS
            ISheet sheet = workbook.CreateSheet("Pessoas");

            // Cabeçalho
            IRow header = sheet.CreateRow(0);
            header.CreateCell(0).SetCellValue("Nome");
            header.CreateCell(1).SetCellValue("Idade");

            // Dados
            IRow row = sheet.CreateRow(1);
            row.CreateCell(0).SetCellValue("Vinícius");
            row.CreateCell(1).SetCellValue(int.Parse("44"));

            using (FileStream fs = new FileStream("pessoas.xls", FileMode.Create, FileAccess.Write))
            {
                workbook.Write(fs);
            }

            MessageBox.Show("Arquivo pessoas.xls criado!");
        }

        private void BtnSair_Click(object sender, EventArgs e)
        {
            // Fecha o formulário atual
            Close();
        }

        private void BtnLer_Click(object sender, EventArgs e)
        {
            string nome;
            int idade;
            using (FileStream fs = new FileStream("pessoas.xls", FileMode.Open, FileAccess.Read))
            {
                IWorkbook workbook = new HSSFWorkbook(fs);
                ISheet sheet = workbook.GetSheet("Pessoas");

                IRow row = sheet.GetRow(1);
                nome = row.GetCell(0).StringCellValue;
                idade = (int)row.GetCell(1).NumericCellValue;

            }

            MessageBox.Show("Dados lidos do arquivo Excel!" + nome + " - " + idade);
        }
    }
}
