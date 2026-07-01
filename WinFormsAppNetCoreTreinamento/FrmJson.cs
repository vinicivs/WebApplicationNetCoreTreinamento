using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Text.Json;
using System.Windows.Forms;

namespace WinFormsAppNetCoreTreinamento
{
    public partial class FrmJson : Form
    {
        public FrmJson()
        {
            InitializeComponent();
        }

        // Classe que será serializada
        public class Pessoa
        {
            public string Nome { get; set; }
            public int Idade { get; set; }
        }

        private void BtnSair_Click(object sender, EventArgs e)
        {
            // Fecha o formulário atual
            Close();
        }

        private void BtnCriar_Click(object sender, EventArgs e)
        {
            Pessoa pessoa = new Pessoa
            {
                Nome = "Vinícius",
                Idade = int.Parse("45")
            };

            // SERIALIZAÇÃO: objeto -> JSON
            string json = JsonSerializer.Serialize(pessoa, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText("pessoa.json", json);

            MessageBox.Show("Objeto salvo em pessoa.json");
        }

        private void BtnLer_Click(object sender, EventArgs e)
        {
            string nome, idade;
            // DESSERIALIZAÇÃO: JSON -> objeto
            string json = File.ReadAllText("pessoa.json");
            Pessoa pessoa = JsonSerializer.Deserialize<Pessoa>(json);

            nome = pessoa.Nome;
            idade = pessoa.Idade.ToString();

            MessageBox.Show("Objeto carregado do JSON" + " - " + nome + " - " + idade, "Alerta", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
    }
}
