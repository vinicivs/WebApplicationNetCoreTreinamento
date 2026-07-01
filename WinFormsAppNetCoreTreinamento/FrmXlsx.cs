using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ClosedXML.Excel;

namespace WinFormsAppNetCoreTreinamento
{
    public partial class FrmXlsx : Form
    {
        public FrmXlsx()
        {
            InitializeComponent();
        }

        public class Pessoa
        {
            public string Nome { get; set; }
            public int Idade { get; set; }
        }

        private void BtnLer_Click(object sender, EventArgs e)
        {
            var pessoas = new List<Pessoa>();

            using (var workbook = new XLWorkbook("pessoas.xlsx"))
            {
                var worksheet = workbook.Worksheet("Pessoas");
                int row = 2;

                while (!worksheet.Cell(row, 1).IsEmpty())
                {
                    var pessoa = new Pessoa
                    {
                        Nome = worksheet.Cell(row, 1).GetString(),
                        Idade = worksheet.Cell(row, 2).GetValue<int>()
                    };
                    pessoas.Add(pessoa);
                    row++;
                }
            }

            // Exibir os dados lidos (exemplo: concatenar em uma string)
            string resultado = "";
            foreach (var p in pessoas)
            {
                resultado += $"Nome: {p.Nome}, Idade: {p.Idade}\n";
            }

            MessageBox.Show("Lista carregada do Excel:\n\n" + resultado);
        }

        private void BtnSair_Click(object sender, EventArgs e)
        {
            // Fecha o formulário atual
            Close();
        }

        private void BtnCriar_Click(object sender, EventArgs e)
        {
            // Lista de pessoas (poderia vir de um banco ou da interface)
            var pessoas = new List<Pessoa>
            {
                new Pessoa { Nome = "Ana", Idade = 25 },
                new Pessoa { Nome = "Carlos", Idade = 32 },
                new Pessoa { Nome = "Mariana", Idade = 28 }
            };

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Pessoas");
                worksheet.Cell(1, 1).Value = "Nome";
                worksheet.Cell(1, 2).Value = "Idade";

                int row = 2;
                foreach (var pessoa in pessoas)
                {
                    worksheet.Cell(row, 1).Value = pessoa.Nome;
                    worksheet.Cell(row, 2).Value = pessoa.Idade;
                    row++;
                }

                workbook.SaveAs("pessoas.xlsx");
            }

            MessageBox.Show("Arquivo pessoas.xlsx criado com lista!");
        }
    }
}
