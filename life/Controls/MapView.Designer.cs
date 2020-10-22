namespace life.Controls
{
    partial class MapView
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
            this._mouse = new Lib.Windows.Controls.DragDropTrace(this.components);
            this._map = new life.Controls.MapData(this.components);
            this._drawer = new life.Controls.MapDrawer(this.components);
            this.SuspendLayout();
            // 
            // _mouse
            // 
            this._mouse.Parent = this;
            // 
            // _map
            // 
            this._map.Size = new System.Drawing.Size(1, 1);
            // 
            // _drawer
            // 
            this._drawer.AliveColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(0)))));
            this._drawer.DeathColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this._drawer.Location = new System.Drawing.Point(0, 0);
            this._drawer.Map = this._map;
            this._drawer.Parent = this;
            // 
            // MapView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Layer.Add(this._drawer);
            this.Name = "MapView";
            this.ResumeLayout(false);

        }

        #endregion
        private MapData _map;
        private Lib.Windows.Controls.DragDropTrace _mouse;
        private MapDrawer _drawer;
    }
}
