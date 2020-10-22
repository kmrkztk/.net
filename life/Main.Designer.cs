namespace life
{
    partial class Main
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

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this._menu = new System.Windows.Forms.MenuStrip();
            this.fileFToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editEToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._view = new life.Controls.MapView();
            this._engine = new life.Controls.LifeEngine(this.components);
            this._location = new life.Controls.DrawPosition(this.components);
            this._scale = new life.Controls.DrawScale(this.components);
            this._resize = new life.Controls.MapResize(this.components);
            this._file = new life.Controls.MapFile(this.components);
            this._edit = new life.Controls.Edit(this.components);
            this._shadow = new life.Controls.Shadow(this.components);
            this.menuOpen1 = new life.Controls.Menus.MenuOpen(this.components);
            this.menuSave1 = new life.Controls.Menus.MenuSave(this.components);
            this.menuSaveAs1 = new life.Controls.Menus.MenuSaveAs(this.components);
            this.menuRun1 = new life.Controls.Menus.MenuRun(this.components);
            this.menuClear1 = new life.Controls.Menus.MenuClear(this.components);
            this.menuPens1 = new life.Controls.Menus.MenuPens(this.components);
            this.menuErasers1 = new life.Controls.Menus.MenuErasers(this.components);
            this.menuPatterns1 = new life.Controls.Menus.MenuPatterns(this.components);
            this.menuSize1 = new life.Controls.Menus.MenuSize(this.components);
            this._menu.SuspendLayout();
            this.SuspendLayout();
            // 
            // _menu
            // 
            this._menu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileFToolStripMenuItem,
            this.editEToolStripMenuItem});
            this._menu.Location = new System.Drawing.Point(0, 0);
            this._menu.Name = "_menu";
            this._menu.Size = new System.Drawing.Size(500, 24);
            this._menu.TabIndex = 3;
            this._menu.Text = "menuStrip1";
            // 
            // fileFToolStripMenuItem
            // 
            this.fileFToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuOpen1,
            this.menuSave1,
            this.menuSaveAs1});
            this.fileFToolStripMenuItem.Name = "fileFToolStripMenuItem";
            this.fileFToolStripMenuItem.Size = new System.Drawing.Size(51, 20);
            this.fileFToolStripMenuItem.Text = "File(&F)";
            // 
            // editEToolStripMenuItem
            // 
            this.editEToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuRun1,
            this.menuClear1,
            this.menuPens1,
            this.menuErasers1,
            this.menuPatterns1,
            this.menuSize1});
            this.editEToolStripMenuItem.Name = "editEToolStripMenuItem";
            this.editEToolStripMenuItem.Size = new System.Drawing.Size(53, 20);
            this.editEToolStripMenuItem.Text = "Edit(&E)";
            // 
            // _view
            // 
            this._view.Components.Add(this._engine);
            this._view.Components.Add(this._location);
            this._view.Components.Add(this._scale);
            this._view.Components.Add(this._resize);
            this._view.Components.Add(this._file);
            this._view.Components.Add(this._edit);
            this._view.Dock = System.Windows.Forms.DockStyle.Fill;
            this._view.Layer.Add(this._shadow);
            this._view.Location = new System.Drawing.Point(0, 24);
            this._view.Name = "_view";
            this._view.Size = new System.Drawing.Size(500, 500);
            this._view.TabIndex = 2;
            // 
            // _engine
            // 
            this._engine.Parent = this;
            // 
            // _location
            // 
            this._location.Parent = this;
            // 
            // _scale
            // 
            this._scale.Parent = this;
            // 
            // _resize
            // 
            this._resize.Parent = this;
            this._resize.Size = new System.Drawing.Size(500, 500);
            // 
            // _file
            // 
            this._file.Author = null;
            this._file.Comments = null;
            this._file.Name = null;
            this._file.Parent = this;
            // 
            // _edit
            // 
            this._edit.Parent = this;
            this._edit.Shadow = this._shadow;
            // 
            // _shadow
            // 
            this._shadow.AliveColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(0)))));
            this._shadow.DeathColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this._shadow.Location = new System.Drawing.Point(0, 0);
            this._shadow.Opacity = ((byte)(128));
            this._shadow.Parent = this;
            this._shadow.Visible = false;
            // 
            // menuOpen1
            // 
            this.menuOpen1.Component = this._file;
            this.menuOpen1.Name = "menuOpen1";
            this.menuOpen1.Size = new System.Drawing.Size(151, 22);
            this.menuOpen1.Text = "Open(&O)";
            // 
            // menuSave1
            // 
            this.menuSave1.Component = this._file;
            this.menuSave1.Enabled = false;
            this.menuSave1.Name = "menuSave1";
            this.menuSave1.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.menuSave1.Size = new System.Drawing.Size(151, 22);
            this.menuSave1.Text = "Save(&S)";
            // 
            // menuSaveAs1
            // 
            this.menuSaveAs1.Component = this._file;
            this.menuSaveAs1.Name = "menuSaveAs1";
            this.menuSaveAs1.Size = new System.Drawing.Size(151, 22);
            this.menuSaveAs1.Text = "SaveAs(&A)";
            // 
            // menuRun1
            // 
            this.menuRun1.Component = this._engine;
            this.menuRun1.Name = "menuRun1";
            this.menuRun1.ShortcutKeys = System.Windows.Forms.Keys.F5;
            this.menuRun1.Size = new System.Drawing.Size(180, 22);
            this.menuRun1.Text = "Run(&S)";
            // 
            // menuClear1
            // 
            this.menuClear1.Component = this._view;
            this.menuClear1.Name = "menuClear1";
            this.menuClear1.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Delete)));
            this.menuClear1.Size = new System.Drawing.Size(180, 22);
            this.menuClear1.Text = "Clear(&C)";
            // 
            // menuPens1
            // 
            this.menuPens1.Component = this._edit;
            this.menuPens1.Name = "menuPens1";
            this.menuPens1.Size = new System.Drawing.Size(180, 22);
            this.menuPens1.Text = "Pen(&P)";
            // 
            // menuErasers1
            // 
            this.menuErasers1.Component = this._edit;
            this.menuErasers1.Name = "menuErasers1";
            this.menuErasers1.Size = new System.Drawing.Size(180, 22);
            this.menuErasers1.Text = "Eraser(&E)";
            // 
            // menuPatterns1
            // 
            this.menuPatterns1.Checked = true;
            this.menuPatterns1.CheckOnClick = true;
            this.menuPatterns1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.menuPatterns1.Component = this._edit;
            this.menuPatterns1.Name = "menuPatterns1";
            this.menuPatterns1.ShortcutKeys = System.Windows.Forms.Keys.F6;
            this.menuPatterns1.Size = new System.Drawing.Size(180, 22);
            this.menuPatterns1.Text = "Patterns...";
            // 
            // menuSize1
            // 
            this.menuSize1.Component = this._resize;
            this.menuSize1.Name = "menuSize1";
            this.menuSize1.Size = new System.Drawing.Size(180, 22);
            this.menuSize1.Text = "Size(&Z)";
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(500, 524);
            this.Controls.Add(this._view);
            this.Controls.Add(this._menu);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this._menu;
            this.Name = "Main";
            this._menu.ResumeLayout(false);
            this._menu.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Controls.MapView _view;
        private Controls.LifeEngine _engine;
        private Controls.Shadow _shadow;
        private Controls.DrawPosition _location;
        private Controls.DrawScale _scale;
        private Controls.MapResize _resize;
        private Controls.MapFile _file;
        private Controls.Edit _edit;
        private System.Windows.Forms.MenuStrip _menu;
        private System.Windows.Forms.ToolStripMenuItem fileFToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editEToolStripMenuItem;
        private Controls.Menus.MenuOpen menuOpen1;
        private Controls.Menus.MenuSave menuSave1;
        private Controls.Menus.MenuSaveAs menuSaveAs1;
        private Controls.Menus.MenuRun menuRun1;
        private Controls.Menus.MenuClear menuClear1;
        private Controls.Menus.MenuPens menuPens1;
        private Controls.Menus.MenuErasers menuErasers1;
        private Controls.Menus.MenuPatterns menuPatterns1;
        private Controls.Menus.MenuSize menuSize1;
    }
}

