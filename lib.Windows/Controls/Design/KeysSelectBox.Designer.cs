namespace Lib.Windows.Controls.Design
{
    partial class KeysSelectBox
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
            this._ctrl = new System.Windows.Forms.CheckBox();
            this._alt = new System.Windows.Forms.CheckBox();
            this._shift = new System.Windows.Forms.CheckBox();
            this._codes = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // _ctrl
            // 
            this._ctrl.AutoSize = true;
            this._ctrl.Location = new System.Drawing.Point(12, 38);
            this._ctrl.Name = "_ctrl";
            this._ctrl.Size = new System.Drawing.Size(43, 16);
            this._ctrl.TabIndex = 0;
            this._ctrl.Text = "Ctrl";
            this._ctrl.UseVisualStyleBackColor = true;
            // 
            // _alt
            // 
            this._alt.AutoSize = true;
            this._alt.Location = new System.Drawing.Point(61, 38);
            this._alt.Name = "_alt";
            this._alt.Size = new System.Drawing.Size(39, 16);
            this._alt.TabIndex = 1;
            this._alt.Text = "Alt";
            this._alt.UseVisualStyleBackColor = true;
            // 
            // _shift
            // 
            this._shift.AutoSize = true;
            this._shift.Location = new System.Drawing.Point(106, 38);
            this._shift.Name = "_shift";
            this._shift.Size = new System.Drawing.Size(48, 16);
            this._shift.TabIndex = 2;
            this._shift.Text = "Shift";
            this._shift.UseVisualStyleBackColor = true;
            // 
            // _codes
            // 
            this._codes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._codes.FormattingEnabled = true;
            this._codes.Location = new System.Drawing.Point(12, 12);
            this._codes.Name = "_codes";
            this._codes.Size = new System.Drawing.Size(176, 20);
            this._codes.TabIndex = 3;
            // 
            // TriggerKeyEditorBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.Controls.Add(this._codes);
            this.Controls.Add(this._shift);
            this.Controls.Add(this._alt);
            this.Controls.Add(this._ctrl);
            this.Name = "TriggerKeyEditorBox";
            this.Size = new System.Drawing.Size(200, 60);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox _ctrl;
        private System.Windows.Forms.CheckBox _alt;
        private System.Windows.Forms.CheckBox _shift;
        private System.Windows.Forms.ComboBox _codes;
    }
}
