namespace FileSync
{
    partial class Manager
    {
        /// <summary> 
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur de composants

        /// <summary> 
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas 
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.btn_sync = new System.Windows.Forms.Button();
            this.btn_stop = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btn_sync
            // 
            this.btn_sync.Location = new System.Drawing.Point(104, 139);
            this.btn_sync.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btn_sync.Name = "btn_sync";
            this.btn_sync.Size = new System.Drawing.Size(211, 60);
            this.btn_sync.TabIndex = 0;
            this.btn_sync.Text = "Synchronize";
            this.btn_sync.UseVisualStyleBackColor = true;
            this.btn_sync.Click += new System.EventHandler(this.btn_sync_Click);
            // 
            // btn_stop
            // 
            this.btn_stop.Location = new System.Drawing.Point(404, 139);
            this.btn_stop.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btn_stop.Name = "btn_stop";
            this.btn_stop.Size = new System.Drawing.Size(211, 60);
            this.btn_stop.TabIndex = 1;
            this.btn_stop.Text = "Stop";
            this.btn_stop.UseVisualStyleBackColor = true;
            this.btn_stop.Click += new System.EventHandler(this.btn_stop_Click);
            // 
            // Manager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btn_stop);
            this.Controls.Add(this.btn_sync);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "Manager";
            this.Size = new System.Drawing.Size(727, 345);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btn_sync;
        private System.Windows.Forms.Button btn_stop;
    }
}
