namespace ForgeBot
{
    partial class CoordsForm
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
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.Coord = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.X = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Y = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LastYStep = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.WasSuccess = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Time = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Coord,
            this.X,
            this.Y,
            this.LastYStep,
            this.WasSuccess,
            this.Time});
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(644, 262);
            this.dataGridView1.TabIndex = 0;
            // 
            // Coord
            // 
            this.Coord.HeaderText = "Coord";
            this.Coord.Name = "Coord";
            this.Coord.ReadOnly = true;
            // 
            // X
            // 
            this.X.HeaderText = "X";
            this.X.Name = "X";
            // 
            // Y
            // 
            this.Y.HeaderText = "Y";
            this.Y.Name = "Y";
            // 
            // LastYStep
            // 
            this.LastYStep.HeaderText = "Last Y step";
            this.LastYStep.Name = "LastYStep";
            // 
            // WasSuccess
            // 
            this.WasSuccess.HeaderText = "Click successful?";
            this.WasSuccess.Name = "WasSuccess";
            // 
            // Time
            // 
            this.Time.HeaderText = "Time";
            this.Time.Name = "Time";
            // 
            // CoordsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(644, 262);
            this.Controls.Add(this.dataGridView1);
            this.Name = "CoordsForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "CoordsForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CoordsForm_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Coord;
        private System.Windows.Forms.DataGridViewTextBoxColumn X;
        private System.Windows.Forms.DataGridViewTextBoxColumn Y;
        private System.Windows.Forms.DataGridViewTextBoxColumn LastYStep;
        private System.Windows.Forms.DataGridViewTextBoxColumn WasSuccess;
        private System.Windows.Forms.DataGridViewTextBoxColumn Time;
    }
}