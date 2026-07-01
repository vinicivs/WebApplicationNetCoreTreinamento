using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace WinFormsAppNetCoreTreinamento
{
    public partial class FrmXml : Form
    {
        public FrmXml()
        {
            InitializeComponent();
        }

        // Classe que será serializada
        public class Pessoa
        {
            public string Nome { get; set; }
            public int Idade { get; set; }
        }

        private void BtnCriar_Click(object sender, EventArgs e)
        {
            Pessoa pessoa = new Pessoa
            {
                Nome = "Vinícius",
                Idade = int.Parse("45")
            };

            XmlSerializer serializer = new XmlSerializer(typeof(Pessoa));
            using (FileStream fs = new FileStream("pessoa.xml", FileMode.Create))
            {
                serializer.Serialize(fs, pessoa);
            }

            MessageBox.Show("Objeto salvo em pessoa.xml");

        }

        private void BtnLer_Click(object sender, EventArgs e)
        {
            string nome, idade;

            XmlSerializer serializer = new XmlSerializer(typeof(Pessoa));
            using (FileStream fs = new FileStream("pessoa.xml", FileMode.Open))
            {
                Pessoa pessoa = (Pessoa)serializer.Deserialize(fs);
                nome = pessoa.Nome;
                idade = pessoa.Idade.ToString();
            }

            MessageBox.Show("Objeto carregado do XML " + nome + " - " + idade, "Alerta", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void BtnSair_Click(object sender, EventArgs e)
        {
            // Fecha o formulário atual
            Close();
        }
    }
}
