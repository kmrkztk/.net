namespace Lib.Windows.Gaming
{
    partial class Display
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
            this.components = new System.ComponentModel.Container();
            this._g = new Lib.Windows.Controls.GraphicComponent(this.components);
            this._bg = new Lib.Windows.Gaming.BackGroundDrawer(this.components);
            this.SuspendLayout();
            // 
            // _g
            // 
            this._g.Parent = this;
            // 
            // _bg
            // 
            this._bg.Parent = this;
            // 
            // Display
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "Display";
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.GraphicComponent _g;
        private BackGroundDrawer _bg;
    }
}
