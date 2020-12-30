namespace Jdenticon.WinForms.Sample
{
    partial class MainForm
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
            this.lbUnstyled = new System.Windows.Forms.Label();
            this.lbStyled1 = new System.Windows.Forms.Label();
            this.lbStyled2 = new System.Windows.Forms.Label();
            this.lbNoPadding = new System.Windows.Forms.Label();
            this.lbNoBackground = new System.Windows.Forms.Label();
            this.iconNoBackground = new Jdenticon.WinForms.IdenticonView();
            this.iconNoPadding = new Jdenticon.WinForms.IdenticonView();
            this.iconStyled2 = new Jdenticon.WinForms.IdenticonView();
            this.iconStyled1 = new Jdenticon.WinForms.IdenticonView();
            this.iconUnstyled = new Jdenticon.WinForms.IdenticonView();
            this.SuspendLayout();
            // 
            // lbUnstyled
            // 
            this.lbUnstyled.AutoSize = true;
            this.lbUnstyled.Location = new System.Drawing.Point(35, 35);
            this.lbUnstyled.Name = "lbUnstyled";
            this.lbUnstyled.Size = new System.Drawing.Size(98, 20);
            this.lbUnstyled.TabIndex = 5;
            this.lbUnstyled.Text = "Unstyled icon";
            // 
            // lbStyled1
            // 
            this.lbStyled1.AutoSize = true;
            this.lbStyled1.Location = new System.Drawing.Point(185, 35);
            this.lbStyled1.Name = "lbStyled1";
            this.lbStyled1.Size = new System.Drawing.Size(103, 20);
            this.lbStyled1.TabIndex = 6;
            this.lbStyled1.Text = "Styled icon #1";
            // 
            // lbStyled2
            // 
            this.lbStyled2.AutoSize = true;
            this.lbStyled2.Location = new System.Drawing.Point(335, 35);
            this.lbStyled2.Name = "lbStyled2";
            this.lbStyled2.Size = new System.Drawing.Size(103, 20);
            this.lbStyled2.TabIndex = 7;
            this.lbStyled2.Text = "Styled icon #2";
            // 
            // lbNoPadding
            // 
            this.lbNoPadding.AutoSize = true;
            this.lbNoPadding.Location = new System.Drawing.Point(485, 35);
            this.lbNoPadding.Name = "lbNoPadding";
            this.lbNoPadding.Size = new System.Drawing.Size(89, 20);
            this.lbNoPadding.TabIndex = 8;
            this.lbNoPadding.Text = "No padding";
            // 
            // lbNoBackground
            // 
            this.lbNoBackground.AutoSize = true;
            this.lbNoBackground.Location = new System.Drawing.Point(635, 35);
            this.lbNoBackground.Name = "lbNoBackground";
            this.lbNoBackground.Size = new System.Drawing.Size(112, 20);
            this.lbNoBackground.TabIndex = 9;
            this.lbNoBackground.Text = "No background";
            // 
            // iconNoBackground
            // 
            this.iconNoBackground.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.iconNoBackground.Location = new System.Drawing.Point(639, 66);
            this.iconNoBackground.Margin = new System.Windows.Forms.Padding(0, 3, 50, 3);
            this.iconNoBackground.Name = "iconNoBackground";
            this.iconNoBackground.Size = new System.Drawing.Size(100, 100);
            this.iconNoBackground.TabIndex = 4;
            this.iconNoBackground.Value = "icon2";
            // 
            // iconNoPadding
            // 
            this.iconNoPadding.Location = new System.Drawing.Point(489, 66);
            this.iconNoPadding.Margin = new System.Windows.Forms.Padding(0, 3, 50, 3);
            this.iconNoPadding.Name = "iconNoPadding";
            this.iconNoPadding.Padding = 0F;
            this.iconNoPadding.Size = new System.Drawing.Size(100, 100);
            this.iconNoPadding.TabIndex = 3;
            this.iconNoPadding.Value = "icon2";
            // 
            // iconStyled2
            // 
            this.iconStyled2.Location = new System.Drawing.Point(339, 66);
            this.iconStyled2.Margin = new System.Windows.Forms.Padding(0, 3, 50, 3);
            this.iconStyled2.Name = "iconStyled2";
            this.iconStyled2.Size = new System.Drawing.Size(100, 100);
            this.iconStyled2.TabIndex = 2;
            this.iconStyled2.Value = "icon2";
            // 
            // iconStyled1
            // 
            this.iconStyled1.Location = new System.Drawing.Point(189, 66);
            this.iconStyled1.Margin = new System.Windows.Forms.Padding(0, 3, 50, 3);
            this.iconStyled1.Name = "iconStyled1";
            this.iconStyled1.Size = new System.Drawing.Size(100, 100);
            this.iconStyled1.TabIndex = 1;
            this.iconStyled1.Value = "icon2";
            // 
            // iconUnstyled
            // 
            this.iconUnstyled.Location = new System.Drawing.Point(39, 66);
            this.iconUnstyled.Margin = new System.Windows.Forms.Padding(0, 3, 50, 3);
            this.iconUnstyled.Name = "iconUnstyled";
            this.iconUnstyled.Size = new System.Drawing.Size(100, 100);
            this.iconUnstyled.TabIndex = 0;
            this.iconUnstyled.Value = "icon2";
            // 
            // MainForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
            this.ClientSize = new System.Drawing.Size(800, 233);
            this.Controls.Add(this.lbNoBackground);
            this.Controls.Add(this.lbNoPadding);
            this.Controls.Add(this.lbStyled2);
            this.Controls.Add(this.lbStyled1);
            this.Controls.Add(this.lbUnstyled);
            this.Controls.Add(this.iconNoBackground);
            this.Controls.Add(this.iconNoPadding);
            this.Controls.Add(this.iconStyled2);
            this.Controls.Add(this.iconStyled1);
            this.Controls.Add(this.iconUnstyled);
            this.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.Black;
            this.Name = "MainForm";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private IdenticonView iconUnstyled;
        private IdenticonView iconStyled1;
        private IdenticonView iconStyled2;
        private IdenticonView iconNoPadding;
        private IdenticonView iconNoBackground;
        private System.Windows.Forms.Label lbUnstyled;
        private System.Windows.Forms.Label lbStyled1;
        private System.Windows.Forms.Label lbStyled2;
        private System.Windows.Forms.Label lbNoPadding;
        private System.Windows.Forms.Label lbNoBackground;
    }
}

