namespace DalClassGen
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.bnt_gen = new System.Windows.Forms.Button();
            this.txtSql = new System.Windows.Forms.TextBox();
            this.resTxt = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.dbTxt = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // bnt_gen
            // 
            this.bnt_gen.Location = new System.Drawing.Point(411, 317);
            this.bnt_gen.Name = "bnt_gen";
            this.bnt_gen.Size = new System.Drawing.Size(140, 44);
            this.bnt_gen.TabIndex = 0;
            this.bnt_gen.Text = "Generate";
            this.bnt_gen.UseVisualStyleBackColor = true;
            this.bnt_gen.Click += new System.EventHandler(this.button1_Click);
            // 
            // txtSql
            // 
            this.txtSql.Location = new System.Drawing.Point(12, 73);
            this.txtSql.Multiline = true;
            this.txtSql.Name = "txtSql";
            this.txtSql.Size = new System.Drawing.Size(939, 214);
            this.txtSql.TabIndex = 1;
            // 
            // resTxt
            // 
            this.resTxt.Location = new System.Drawing.Point(12, 389);
            this.resTxt.Multiline = true;
            this.resTxt.Name = "resTxt";
            this.resTxt.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.resTxt.Size = new System.Drawing.Size(939, 397);
            this.resTxt.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "Database Name:";
            // 
            // dbTxt
            // 
            this.dbTxt.Location = new System.Drawing.Point(108, 30);
            this.dbTxt.Name = "dbTxt";
            this.dbTxt.Size = new System.Drawing.Size(202, 21);
            this.dbTxt.TabIndex = 4;
            this.dbTxt.Text = "liaoqu";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(963, 821);
            this.Controls.Add(this.dbTxt);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.resTxt);
            this.Controls.Add(this.txtSql);
            this.Controls.Add(this.bnt_gen);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button bnt_gen;
        private System.Windows.Forms.TextBox txtSql;
        private System.Windows.Forms.TextBox resTxt;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox dbTxt;
    }
}

