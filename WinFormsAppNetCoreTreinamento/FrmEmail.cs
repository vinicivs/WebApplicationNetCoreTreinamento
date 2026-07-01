using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Windows.Forms;
using MailKit.Net.Imap;
using MailKit;
using MimeKit;

namespace WinFormsAppNetCoreTreinamento
{
    public partial class FrmEmail : Form
    {
        public FrmEmail()
        {
            InitializeComponent();
        }

        private void BtnCriar_Click(object sender, EventArgs e)
        {
            try
            {
                var mail = new MailMessage();
                mail.From = new MailAddress("contato@vgti.com.br");
                mail.To.Add("vinicivs@gmail.com");
                mail.Subject = "Teste de envio";
                mail.Body = "Olá, este é um e-mail enviado pelo Windows Forms .Net 10!";

                using (var smtp = new SmtpClient("email-ssl.com.br", 587))
                {
                    smtp.Credentials = new NetworkCredential("contato@vgti.com.br", "Dye122700@@123");
                    smtp.EnableSsl = true;
                    smtp.Send(mail);
                }

                MessageBox.Show("E-mail enviado com sucesso!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro: " + ex.Message);
            }
        }

        private void BtnLer_Click(object sender, EventArgs e)
        {
            using (var client = new ImapClient())
            {
                client.Connect("email-ssl.com.br", 993, true);
                client.Authenticate("contato@vgti.com.br", "Dye122700@@123");

                var inbox = client.Inbox;
                inbox.Open(FolderAccess.ReadOnly);

                var mensagem = inbox.GetMessage(inbox.Count - 1); // última mensagem
                MessageBox.Show($"De: {mensagem.From}\nAssunto: {mensagem.Subject}\n\n{mensagem.TextBody}");

                client.Disconnect(true);
            }
        }

        private void BtnSair_Click(object sender, EventArgs e)
        {
            // Fecha o formulário atual
            Close();
        }
    }
}
