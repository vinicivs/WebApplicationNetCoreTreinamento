namespace WinFormsAppNetCoreTreinamento
{
    partial class FrmXml
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
            BtnSair = new Button();
            BtnLer = new Button();
            BtnCriar = new Button();
            SuspendLayout();
            // 
            // BtnSair
            // 
            BtnSair.Location = new Point(171, 12);
            BtnSair.Name = "BtnSair";
            BtnSair.Size = new Size(75, 23);
            BtnSair.TabIndex = 5;
            BtnSair.Text = "&Sair";
            BtnSair.UseVisualStyleBackColor = true;
            BtnSair.Click += BtnSair_Click;
            // 
            // BtnLer
            // 
            BtnLer.Location = new Point(90, 12);
            BtnLer.Name = "BtnLer";
            BtnLer.Size = new Size(75, 23);
            BtnLer.TabIndex = 4;
            BtnLer.Text = "&Ler";
            BtnLer.UseVisualStyleBackColor = true;
            BtnLer.Click += BtnLer_Click;
            // 
            // BtnCriar
            // 
            BtnCriar.Location = new Point(9, 12);
            BtnCriar.Name = "BtnCriar";
            BtnCriar.Size = new Size(75, 23);
            BtnCriar.TabIndex = 3;
            BtnCriar.Text = "&Criar";
            BtnCriar.UseVisualStyleBackColor = true;
            BtnCriar.Click += BtnCriar_Click;
            // 
            // FrmXml
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(258, 44);
            Controls.Add(BtnSair);
            Controls.Add(BtnLer);
            Controls.Add(BtnCriar);
            MaximizeBox = false;
            Name = "FrmXml";
            Text = ".Xml";
            ResumeLayout(false);
        }

        #endregion

        private Button BtnSair;
        private Button BtnLer;
        private Button BtnCriar;
    }
}