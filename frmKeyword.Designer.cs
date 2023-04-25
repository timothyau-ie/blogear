namespace blocal
{
    partial class frmKeyword
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
            this.chk_CaseSensitive = new System.Windows.Forms.CheckBox();
            this.txt_Expression = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txt_Results = new System.Windows.Forms.TextBox();
            this.btn_Search = new System.Windows.Forms.Button();
            this.btn_Directory = new System.Windows.Forms.Button();
            this.txt_Directory = new System.Windows.Forms.TextBox();
            this.Label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // chk_CaseSensitive
            // 
            this.chk_CaseSensitive.AutoSize = true;
            this.chk_CaseSensitive.Location = new System.Drawing.Point(12, 64);
            this.chk_CaseSensitive.Name = "chk_CaseSensitive";
            this.chk_CaseSensitive.Size = new System.Drawing.Size(96, 17);
            this.chk_CaseSensitive.TabIndex = 11;
            this.chk_CaseSensitive.Text = "Case Sensitive";
            this.chk_CaseSensitive.UseVisualStyleBackColor = true;
            // 
            // txt_Expression
            // 
            this.txt_Expression.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txt_Expression.Location = new System.Drawing.Point(73, 38);
            this.txt_Expression.Name = "txt_Expression";
            this.txt_Expression.Size = new System.Drawing.Size(117, 20);
            this.txt_Expression.TabIndex = 0;
            this.txt_Expression.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txt_Expression_KeyDown);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 41);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(58, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Expression";
            // 
            // txt_Results
            // 
            this.txt_Results.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txt_Results.Location = new System.Drawing.Point(12, 87);
            this.txt_Results.Multiline = true;
            this.txt_Results.Name = "txt_Results";
            this.txt_Results.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txt_Results.Size = new System.Drawing.Size(325, 95);
            this.txt_Results.TabIndex = 10;
            // 
            // btn_Search
            // 
            this.btn_Search.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_Search.Location = new System.Drawing.Point(196, 38);
            this.btn_Search.Name = "btn_Search";
            this.btn_Search.Size = new System.Drawing.Size(141, 42);
            this.btn_Search.TabIndex = 5;
            this.btn_Search.Text = "Search";
            this.btn_Search.UseVisualStyleBackColor = true;
            this.btn_Search.Click += new System.EventHandler(this.btn_Search_Click);
            // 
            // btn_Directory
            // 
            this.btn_Directory.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_Directory.Location = new System.Drawing.Point(315, 13);
            this.btn_Directory.Name = "btn_Directory";
            this.btn_Directory.Size = new System.Drawing.Size(22, 19);
            this.btn_Directory.TabIndex = 14;
            this.btn_Directory.Text = "...";
            this.btn_Directory.UseVisualStyleBackColor = true;
            this.btn_Directory.Click += new System.EventHandler(this.btn_Directory_Click);
            // 
            // txt_Directory
            // 
            this.txt_Directory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txt_Directory.Location = new System.Drawing.Point(73, 12);
            this.txt_Directory.Name = "txt_Directory";
            this.txt_Directory.Size = new System.Drawing.Size(231, 20);
            this.txt_Directory.TabIndex = 13;
            // 
            // Label1
            // 
            this.Label1.AutoSize = true;
            this.Label1.Location = new System.Drawing.Point(9, 16);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(52, 13);
            this.Label1.TabIndex = 12;
            this.Label1.Text = "Directory:";
            // 
            // frmKeyword
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(349, 194);
            this.Controls.Add(this.btn_Directory);
            this.Controls.Add(this.txt_Directory);
            this.Controls.Add(this.Label1);
            this.Controls.Add(this.chk_CaseSensitive);
            this.Controls.Add(this.txt_Expression);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txt_Results);
            this.Controls.Add(this.btn_Search);
            this.Name = "frmKeyword";
            this.Text = "Search for your keyword";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox chk_CaseSensitive;
        private System.Windows.Forms.TextBox txt_Expression;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txt_Results;
        private System.Windows.Forms.Button btn_Search;
        private System.Windows.Forms.Button btn_Directory;
        private System.Windows.Forms.TextBox txt_Directory;
        private System.Windows.Forms.Label Label1;
    }
}