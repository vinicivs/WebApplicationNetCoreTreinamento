using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using NPOI;
using PdfSharp;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using PdfDocument = PdfSharp.Pdf.PdfDocument;
using PdfReader = iTextSharp.text.pdf.PdfReader;

namespace WinFormsAppNetCoreTreinamento
{
    public partial class FrmPdf : Form
    {
        public FrmPdf()
        {
            InitializeComponent();
        }

        private void BtnSair_Click(object sender, EventArgs e)
        {
            // Fecha o formulário atual
            Close();
        }

        private void BtnCriar_Click(object sender, EventArgs e)
        {
            // Cria um novo documento PDF
            GerarPdfPdfSharp();
        }

        private void BtnLer_Click(object sender, EventArgs e)
        {
            LerTextoDePdf();
        }

        // Criar 
        public void GerarPdfPdfSharp()
    {
        // Cria um novo documento PDF
        using (var documento = new PdfDocument())
        {
            // Adiciona uma página
            var pagina = documento.AddPage();
            pagina.Size = PageSize.A4;

            // Obtém um objeto XGraphics para desenhar na página
            using (var gfx = XGraphics.FromPdfPage(pagina))
            {
                // Define uma fonte
                var fonte = new XFont("Arial", 20, XFontStyleEx.Bold);
                var fonteCorpo = new XFont("Arial", 12, XFontStyleEx.Regular);

                // Desenha um título
                gfx.DrawString("Título do Documento", fonte, XBrushes.Black,
                               new XRect(0, 50, pagina.Width, 50), XStringFormats.Center);

                // Desenha um texto simples
                gfx.DrawString("Este é um exemplo de PDF criado com PDFsharp.",
                               fonteCorpo, XBrushes.Black,
                               new XRect(40, 120, pagina.Width - 80, 20), XStringFormats.TopLeft);
            }

            // Salva o arquivo
            string caminhoArquivo = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "meu_documento.pdf");
            documento.Save(caminhoArquivo);

            MessageBox.Show($"PDF gerado com sucesso em: {caminhoArquivo}", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }

        // Método para ler o PDF
        public void LerTextoDePdf()
        {
            // Abre uma janela para o usuário selecionar o arquivo PDF
            using (var openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Arquivos PDF (*.pdf)|*.pdf";
                openFileDialog.Title = "Selecione um arquivo PDF para ler";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        string textoExtraido = "";
                        using (var reader = new PdfReader(openFileDialog.FileName))
                        {
                            // Itera por todas as páginas do PDF
                            for (int pagina = 1; pagina <= reader.NumberOfPages; pagina++)
                            {
                                // Extrai o texto da página atual
                                ITextExtractionStrategy strategy = new SimpleTextExtractionStrategy();
                                string textoPagina = PdfTextExtractor.GetTextFromPage(reader, pagina, strategy);
                                textoExtraido += textoPagina;
                            }
                        }

                        // Exibe o texto em uma caixa de mensagem ou em um TextBox
                        MessageBox.Show(textoExtraido, "Texto Extraído do PDF");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Erro ao ler o PDF: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }



    }
}
