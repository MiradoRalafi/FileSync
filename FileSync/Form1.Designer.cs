namespace FileSync
{
    partial class Form1
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

        #region Code généré par le Concepteur Windows Form

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.start1 = new FileSync.Start();
            this.manager1 = new FileSync.Manager();
            this.SuspendLayout();
            // 
            // start1
            // 
            this.start1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.start1.Location = new System.Drawing.Point(0, 0);
            this.start1.Name = "start1";
            this.start1.Size = new System.Drawing.Size(545, 280);
            this.start1.TabIndex = 0;
            this.start1.form = this;
            // 
            // manager1
            // 
            this.manager1.Location = new System.Drawing.Point(0, 0);
            this.manager1.Name = "manager1";
            this.manager1.Size = new System.Drawing.Size(545, 280);
            this.manager1.TabIndex = 1;
            this.manager1.form = this;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(545, 280);
            this.Controls.Add(this.manager1);
            this.Controls.Add(this.start1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Form1";
            this.Text = "FileSync";
            this.ResumeLayout(false);

        }

        #endregion

        private Start start1;
        private Manager manager1;
    }
}

