namespace TelegramFolderScanner
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Button btnScan;
        private System.Windows.Forms.Panel scrollablePanel;
        private System.Windows.Forms.Button btnOpenAll;
        private System.Windows.Forms.Button btnPlaceAll;


        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }


        private void InitializeComponent()
        {
            this.btnScan = new System.Windows.Forms.Button();
            this.btnOpenAll = new System.Windows.Forms.Button(); 
            this.btnPlaceAll = new System.Windows.Forms.Button();
            this.scrollablePanel = new System.Windows.Forms.Panel();
            this.SuspendLayout();

            // 
            // btnScan
            // 
            this.btnScan.Location = new System.Drawing.Point(10, 10);
            this.btnScan.Name = "btnScan";
            this.btnScan.Size = new System.Drawing.Size(140, 30);
            this.btnScan.TabIndex = 0;
            this.btnScan.Text = "Сканировать";
            this.btnScan.UseVisualStyleBackColor = true;
            this.btnScan.Click += new System.EventHandler(this.btnScan_Click);

            // 
            // btnOpenAll
            // 
            this.btnOpenAll.Location = new System.Drawing.Point(170, 10);  // Разместим рядом
            this.btnOpenAll.Name = "btnOpenAll";
            this.btnOpenAll.Size = new System.Drawing.Size(140, 30);
            this.btnOpenAll.TabIndex = 2;
            this.btnOpenAll.Text = "Открыть все";
            this.btnOpenAll.UseVisualStyleBackColor = true;
            this.btnOpenAll.Click += new System.EventHandler(this.btnOpenAll_Click);  // Обработчик нажатия

            // 
            // btnPlaceAll
            // 
            this.btnPlaceAll.Location = new System.Drawing.Point(330, 10);  // Разместим рядом
            this.btnPlaceAll.Name = "btnPlaceAll";
            this.btnPlaceAll.Size = new System.Drawing.Size(140, 30);
            this.btnPlaceAll.TabIndex = 2;
            this.btnPlaceAll.Text = "Разместить все";
            this.btnPlaceAll.UseVisualStyleBackColor = true;
            this.btnPlaceAll.Click += new System.EventHandler(this.btnPlaceAll_Click);  // Обработчик нажатия

            // 
            // scrollablePanel
            // 
            this.scrollablePanel.AutoScroll = true;
            this.scrollablePanel.Location = new System.Drawing.Point(10, 50);
            this.scrollablePanel.Name = "scrollablePanel";
            this.scrollablePanel.Size = new System.Drawing.Size(460, 640);
            this.scrollablePanel.TabIndex = 1;
            this.scrollablePanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;

            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(480, 700);
            this.Controls.Add(this.scrollablePanel);
            this.Controls.Add(this.btnScan);
            this.Controls.Add(this.btnOpenAll);
            this.Controls.Add(this.btnPlaceAll);
            this.Name = "MainForm";
            this.Text = "Telegram Manager";
            this.ResumeLayout(false);
        }

    }
}
