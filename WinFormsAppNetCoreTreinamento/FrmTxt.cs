namespace WinFormsAppNetCoreTreinamento
{
    public partial class FrmTxt : Form
    {
        string caminhoArquivo = "teste.txt"; // arquivo será criado na pasta do executável

        public FrmTxt()
        {
            InitializeComponent();
        }

        private void BtnSair_Click(object sender, EventArgs e)
        {
            // Fecha o formulário
            Close();
        }

        private void BtnCriar_Click(object sender, EventArgs e)
        {
            // Cria um arquivo de texto exemplo.txt na pasta do projeto
            try
            {
                File.WriteAllText(caminhoArquivo, "Conteúdo de teste");
                MessageBox.Show("Arquivo salvo com sucesso!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao salvar: " + ex.Message);
            }

        }

        private void BtnLer_Click(object sender, EventArgs e)
        {
            // Lê o conteúdo do arquivo de texto exemplo.txt e exibe em um MessageBox
            try
            {
                if (File.Exists(caminhoArquivo))
                {
                    string conteudo = File.ReadAllText(caminhoArquivo);
                    MessageBox.Show(conteudo, "Alerta", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else
                {
                    MessageBox.Show("Arquivo não encontrado!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao ler: " + ex.Message);
            }
        }
    }
}
