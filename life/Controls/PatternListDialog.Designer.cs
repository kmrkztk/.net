namespace life.Controls
{
    partial class PatternListDialog
    {
        /// <summary> 
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージド リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region コンポーネント デザイナーで生成されたコード

        /// <summary> 
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を 
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this._panel = new System.Windows.Forms.TableLayoutPanel();
            this._filter = new System.Windows.Forms.TextBox();
            this._list = new life.Controls.PatternList();
            this._panel.SuspendLayout();
            this.SuspendLayout();
            // 
            // _panel
            // 
            this._panel.ColumnCount = 1;
            this._panel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this._panel.Controls.Add(this._filter, 0, 0);
            this._panel.Controls.Add(this._list, 0, 1);
            this._panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._panel.Location = new System.Drawing.Point(0, 0);
            this._panel.Name = "_panel";
            this._panel.RowCount = 2;
            this._panel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            this._panel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._panel.Size = new System.Drawing.Size(375, 472);
            this._panel.TabIndex = 0;
            // 
            // _filter
            // 
            this._filter.Dock = System.Windows.Forms.DockStyle.Fill;
            this._filter.Location = new System.Drawing.Point(3, 3);
            this._filter.Name = "_filter";
            this._filter.Size = new System.Drawing.Size(369, 19);
            this._filter.TabIndex = 1;
            // 
            // _list
            // 
            this._list.Dock = System.Windows.Forms.DockStyle.Fill;
            this._list.Filtering = null;
            this._list.FullRowSelect = true;
            this._list.HideSelection = false;
            this._list.Location = new System.Drawing.Point(3, 26);
            this._list.Name = "_list";
            this._list.Size = new System.Drawing.Size(369, 443);
            this._list.TabIndex = 2;
            this._list.UseCompatibleStateImageBehavior = false;
            this._list.View = System.Windows.Forms.View.Details;
            // 
            // PatternListDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(375, 472);
            this.Controls.Add(this._panel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "PatternListDialog";
            this._panel.ResumeLayout(false);
            this._panel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel _panel;
        private System.Windows.Forms.TextBox _filter;
        private PatternList _list;
    }
}
