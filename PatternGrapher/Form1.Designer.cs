namespace PatternGrapher
{
    partial class Form1
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
            this.components = new System.ComponentModel.Container();
            this.picGraph = new System.Windows.Forms.PictureBox();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.varLabel0 = new System.Windows.Forms.Label();
            this.varLabel1 = new System.Windows.Forms.Label();
            this.varLabel2 = new System.Windows.Forms.Label();
            this.varLabel3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.picGraph)).BeginInit();
            this.SuspendLayout();
            // 
            // picGraph
            // 
            this.picGraph.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.picGraph.BackColor = System.Drawing.Color.Black;
            this.picGraph.Location = new System.Drawing.Point(340, 112);
            this.picGraph.Margin = new System.Windows.Forms.Padding(0);
            this.picGraph.Name = "picGraph";
            this.picGraph.Size = new System.Drawing.Size(1000, 1000);
            this.picGraph.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picGraph.TabIndex = 0;
            this.picGraph.TabStop = false;
            this.picGraph.Visible = false;
            // 
            // timer
            // 
            this.timer.Enabled = true;
            this.timer.Interval = 50;
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // varLabel0
            // 
            this.varLabel0.AutoSize = true;
            this.varLabel0.ForeColor = System.Drawing.Color.White;
            this.varLabel0.Location = new System.Drawing.Point(5, 5);
            this.varLabel0.Name = "varLabel0";
            this.varLabel0.Size = new System.Drawing.Size(0, 13);
            this.varLabel0.TabIndex = 2;
            // 
            // varLabel1
            // 
            this.varLabel1.AutoSize = true;
            this.varLabel1.ForeColor = System.Drawing.Color.White;
            this.varLabel1.Location = new System.Drawing.Point(5, 20);
            this.varLabel1.Name = "varLabel1";
            this.varLabel1.Size = new System.Drawing.Size(0, 13);
            this.varLabel1.TabIndex = 3;
            // 
            // varLabel2
            // 
            this.varLabel2.AutoSize = true;
            this.varLabel2.ForeColor = System.Drawing.Color.White;
            this.varLabel2.Location = new System.Drawing.Point(5, 35);
            this.varLabel2.Name = "varLabel2";
            this.varLabel2.Size = new System.Drawing.Size(0, 13);
            this.varLabel2.TabIndex = 4;
            // 
            // varLabel3
            // 
            this.varLabel3.AutoSize = true;
            this.varLabel3.ForeColor = System.Drawing.Color.White;
            this.varLabel3.Location = new System.Drawing.Point(5, 50);
            this.varLabel3.Name = "varLabel3";
            this.varLabel3.Size = new System.Drawing.Size(0, 13);
            this.varLabel3.TabIndex = 5;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(1065, 584);
            this.Controls.Add(this.varLabel3);
            this.Controls.Add(this.varLabel2);
            this.Controls.Add(this.varLabel1);
            this.Controls.Add(this.varLabel0);
            this.Controls.Add(this.picGraph);
            this.KeyPreview = true;
            this.Name = "Form1";
            this.Text = "Form1";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.picGraph)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox picGraph;
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.Label varLabel0;
        private System.Windows.Forms.Label varLabel1;
        private System.Windows.Forms.Label varLabel2;
        private System.Windows.Forms.Label varLabel3;
    }
}

