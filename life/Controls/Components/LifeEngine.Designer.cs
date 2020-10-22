namespace life.Controls
{
    partial class LifeEngine
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
            this._animator = new life.Controls.LifeAnimator(this.components);
            this._calc = new life.Controls.LifeCalculator(this.components);
            // 
            // _animator
            // 
            this._animator.Calculator = this._calc;
            this._animator.Drawer = null;
            this._animator.FramePerSec = 60F;
            this._animator.Parent = this;
            // 
            // _calc
            // 
            this._calc.Map = null;
            this._calc.Parent = this;

        }

        #endregion

        private LifeAnimator _animator;
        private LifeCalculator _calc;
    }
}
