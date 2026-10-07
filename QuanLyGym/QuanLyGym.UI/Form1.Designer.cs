namespace QuanLyGym.UI
{
    partial class Form1
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
            btnXinChao = new Button();
            SuspendLayout();
            // 
            // btnXinChao
            // 
            btnXinChao.Location = new Point(301, 166);
            btnXinChao.Name = "btnXinChao";
            btnXinChao.Size = new Size(112, 34);
            btnXinChao.TabIndex = 0;
            btnXinChao.Text = "Bấm vào đây";
            btnXinChao.UseVisualStyleBackColor = true;
            btnXinChao.Click += btnXinChao_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(btnXinChao);
            Name = "Form1";
            Text = "Form1";
            ResumeLayout(false);
        }

        #endregion

        private Button btnXinChao;
    }
}
