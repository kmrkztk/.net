namespace life.Controls
{
    partial class MapFile
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
            this._open = new System.Windows.Forms.OpenFileDialog();
            this._save = new System.Windows.Forms.SaveFileDialog();
            // 
            // _open
            // 
            this._open.DefaultExt = "rle";
            this._open.Filter = "|*.rle";
            // 
            // _save
            // 
            this._save.DefaultExt = "rle";
            this._save.Filter = "|*.rle";

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog _open;
        private System.Windows.Forms.SaveFileDialog _save;
    }
}
