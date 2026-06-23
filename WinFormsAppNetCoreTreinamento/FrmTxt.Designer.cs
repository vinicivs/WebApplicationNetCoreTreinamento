namespace WinFormsAppNetCoreTreinamento
{
    partial class FrmTxt
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            BtnCriar = new Button();
            BtnLer = new Button();
            BtnSair = new Button();
            SuspendLayout();
            // 
            // BtnCriar
            // 
            BtnCriar.Location = new Point(12, 12);
            BtnCriar.Name = "BtnCriar";
            BtnCriar.Size = new Size(75, 23);
            BtnCriar.TabIndex = 0;
            BtnCriar.Text = "&Criar";
            BtnCriar.UseVisualStyleBackColor = true;
            BtnCriar.Click += BtnCriar_Click;
            // 
            // BtnLer
            // 
            BtnLer.Location = new Point(93, 12);
            BtnLer.Name = "BtnLer";
            BtnLer.Size = new Size(75, 23);
            BtnLer.TabIndex = 1;
            BtnLer.Text = "&Ler";
            BtnLer.UseVisualStyleBackColor = true;
            BtnLer.Click += BtnLer_Click;
            // 
            // BtnSair
            // 
            BtnSair.Location = new Point(174, 12);
            BtnSair.Name = "BtnSair";
            BtnSair.Size = new Size(75, 23);
            BtnSair.TabIndex = 2;
            BtnSair.Text = "&Sair";
            BtnSair.UseVisualStyleBackColor = true;
            BtnSair.Click += BtnSair_Click;
            // 
            // FrmTxt
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(265, 55);
            Controls.Add(BtnSair);
            Controls.Add(BtnLer);
            Controls.Add(BtnCriar);
            MaximizeBox = false;
            Name = "FrmTxt";
            Text = ".Txt";
            ResumeLayout(false);
        }

        #endregion

        private Button BtnCriar;
        private Button BtnLer;
        private Button BtnSair;
    }
}
