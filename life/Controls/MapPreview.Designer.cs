namespace life.Controls
{
    partial class MapPreview
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
            this.components = new System.ComponentModel.Container();
            this._view = new life.Controls.MapView();
            this._align = new life.Controls.DrawAlign(this.components);
            this._location = new life.Controls.DrawPosition(this.components);
            this._scale = new life.Controls.DrawScale(this.components);
            this._direction = new life.Controls.MapDirection(this.components);
            this.SuspendLayout();
            // 
            // _view
            // 
            this._view.Components.Add(this._align);
            this._view.Components.Add(this._location);
            this._view.Components.Add(this._scale);
            this._view.Components.Add(this._direction);
            this._view.Dock = System.Windows.Forms.DockStyle.Fill;
            this._view.Location = new System.Drawing.Point(0, 0);
            this._view.Name = "_view";
            this._view.Size = new System.Drawing.Size(400, 400);
            this._view.TabIndex = 0;
            // 
            // _align
            // 
            this._align.Parent = this;
            // 
            // _location
            // 
            this._location.Parent = this;
            // 
            // _scale
            // 
            this._scale.Parent = this;
            // 
            // _direction
            // 
            this._direction.Parent = this;
            // 
            // MapPreview
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(400, 400);
            this.Controls.Add(this._view);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MapPreview";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.ResumeLayout(false);

        }

        #endregion
        private MapView _view;
        private DrawAlign _align;
        private DrawPosition _location;
        private DrawScale _scale;
        private MapDirection _direction;
    }
}