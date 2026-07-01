namespace WinFormsAppNetCoreTreinamento
{
    partial class FrmPrincipal
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            MspMenu = new MenuStrip();
            arquivoTxtToolStripMenuItem = new ToolStripMenuItem();
            arquivoJsonToolStripMenuItem = new ToolStripMenuItem();
            arquivoXlsToolStripMenuItem = new ToolStripMenuItem();
            arquivoXlsxToolStripMenuItem = new ToolStripMenuItem();
            arquivoXmlToolStripMenuItem = new ToolStripMenuItem();
            emailToolStripMenuItem = new ToolStripMenuItem();
            sairToolStripMenuItem = new ToolStripMenuItem();
            arquivoPdfToolStripMenuItem = new ToolStripMenuItem();
            MspMenu.SuspendLayout();
            SuspendLayout();
            // 
            // MspMenu
            // 
            MspMenu.Items.AddRange(new ToolStripItem[] { arquivoTxtToolStripMenuItem, arquivoJsonToolStripMenuItem, arquivoXlsToolStripMenuItem, arquivoXlsxToolStripMenuItem, arquivoXmlToolStripMenuItem, emailToolStripMenuItem, arquivoPdfToolStripMenuItem, sairToolStripMenuItem });
            MspMenu.Location = new Point(0, 0);
            MspMenu.Name = "MspMenu";
            MspMenu.Size = new Size(800, 24);
            MspMenu.TabIndex = 0;
            MspMenu.Text = "menuStrip1";
            // 
            // arquivoTxtToolStripMenuItem
            // 
            arquivoTxtToolStripMenuItem.Name = "arquivoTxtToolStripMenuItem";
            arquivoTxtToolStripMenuItem.Size = new Size(82, 20);
            arquivoTxtToolStripMenuItem.Text = "Arquivo .Txt";
            arquivoTxtToolStripMenuItem.Click += arquivoTxtToolStripMenuItem_Click;
            // 
            // arquivoJsonToolStripMenuItem
            // 
            arquivoJsonToolStripMenuItem.Name = "arquivoJsonToolStripMenuItem";
            arquivoJsonToolStripMenuItem.Size = new Size(90, 20);
            arquivoJsonToolStripMenuItem.Text = "Arquivo .Json";
            arquivoJsonToolStripMenuItem.Click += arquivoJsonToolStripMenuItem_Click;
            // 
            // arquivoXlsToolStripMenuItem
            // 
            arquivoXlsToolStripMenuItem.Name = "arquivoXlsToolStripMenuItem";
            arquivoXlsToolStripMenuItem.Size = new Size(82, 20);
            arquivoXlsToolStripMenuItem.Text = "Arquivo .Xls";
            arquivoXlsToolStripMenuItem.Click += arquivoXlsToolStripMenuItem_Click;
            // 
            // arquivoXlsxToolStripMenuItem
            // 
            arquivoXlsxToolStripMenuItem.Name = "arquivoXlsxToolStripMenuItem";
            arquivoXlsxToolStripMenuItem.Size = new Size(87, 20);
            arquivoXlsxToolStripMenuItem.Text = "Arquivo .Xlsx";
            arquivoXlsxToolStripMenuItem.Click += arquivoXlsxToolStripMenuItem_Click;
            // 
            // arquivoXmlToolStripMenuItem
            // 
            arquivoXmlToolStripMenuItem.Name = "arquivoXmlToolStripMenuItem";
            arquivoXmlToolStripMenuItem.Size = new Size(88, 20);
            arquivoXmlToolStripMenuItem.Text = "Arquivo .Xml";
            arquivoXmlToolStripMenuItem.Click += arquivoXmlToolStripMenuItem_Click;
            // 
            // emailToolStripMenuItem
            // 
            emailToolStripMenuItem.Name = "emailToolStripMenuItem";
            emailToolStripMenuItem.Size = new Size(53, 20);
            emailToolStripMenuItem.Text = "E-mail";
            emailToolStripMenuItem.Click += emailToolStripMenuItem_Click;
            // 
            // sairToolStripMenuItem
            // 
            sairToolStripMenuItem.Name = "sairToolStripMenuItem";
            sairToolStripMenuItem.Size = new Size(38, 20);
            sairToolStripMenuItem.Text = "Sair";
            sairToolStripMenuItem.Click += sairToolStripMenuItem_Click;
            // 
            // arquivoPdfToolStripMenuItem
            // 
            arquivoPdfToolStripMenuItem.Name = "arquivoPdfToolStripMenuItem";
            arquivoPdfToolStripMenuItem.Size = new Size(85, 20);
            arquivoPdfToolStripMenuItem.Text = "Arquivo .Pdf";
            arquivoPdfToolStripMenuItem.Click += arquivoPdfToolStripMenuItem_Click;
            // 
            // FrmPrincipal
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(MspMenu);
            MainMenuStrip = MspMenu;
            Name = "FrmPrincipal";
            Text = "Principal";
            MspMenu.ResumeLayout(false);
            MspMenu.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip MspMenu;
        private ToolStripMenuItem arquivoTxtToolStripMenuItem;
        private ToolStripMenuItem arquivoJsonToolStripMenuItem;
        private ToolStripMenuItem arquivoXlsToolStripMenuItem;
        private ToolStripMenuItem arquivoXlsxToolStripMenuItem;
        private ToolStripMenuItem arquivoXmlToolStripMenuItem;
        private ToolStripMenuItem emailToolStripMenuItem;
        private ToolStripMenuItem sairToolStripMenuItem;
        private ToolStripMenuItem arquivoPdfToolStripMenuItem;
    }
}