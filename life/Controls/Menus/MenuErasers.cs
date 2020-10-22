using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace life.Controls.Menus
{
    public class MenuErasers : MenuItem
    {
        private IContainer components;
        private MenuEraser1 _eraser1;
        private MenuEraser5 _eraser5;
        private MenuEraser10 _eraser10;
        private MenuEraser20 _eraser20;

        public override string Text { get => "Eraser(&E)"; set => base.Text = value; }
        public Edit Component
        {
            get => _eraser1.Component;
            set
            {
                _eraser1.Component = value;
                _eraser5.Component = value;
                _eraser10.Component = value;
                _eraser20.Component = value;
            }
        }
        public MenuErasers() : base() => InitializeComponent();
        public MenuErasers(IContainer container) : base(container) => InitializeComponent();

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this._eraser1 = new life.Controls.Menus.MenuEraser1(this.components);
            this._eraser5 = new life.Controls.Menus.MenuEraser5(this.components);
            this._eraser10 = new life.Controls.Menus.MenuEraser10(this.components);
            this._eraser20 = new life.Controls.Menus.MenuEraser20(this.components);
            // 
            // menuEraser11
            // 
            this._eraser1.CheckOnClick = true;
            this._eraser1.Name = "_eraser1";
            this._eraser1.Size = new System.Drawing.Size(98, 22);
            this._eraser1.Text = "1 x 1";
            // 
            // menuEraser51
            // 
            this._eraser5.CheckOnClick = true;
            this._eraser5.Name = "_eraser5";
            this._eraser5.Size = new System.Drawing.Size(32, 19);
            this._eraser5.Text = "5 x 5";
            // 
            // menuEraser101
            // 
            this._eraser10.CheckOnClick = true;
            this._eraser10.Name = "_eraser10";
            this._eraser10.Size = new System.Drawing.Size(32, 19);
            this._eraser10.Text = "10 x 10";
            // 
            // menuEraser201
            // 
            this._eraser20.CheckOnClick = true;
            this._eraser20.Name = "_eraser20";
            this._eraser20.Size = new System.Drawing.Size(98, 22);
            this._eraser20.Text = "20 x 20";
            // 
            // MenuErasers
            // 
            this.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._eraser1,
            this._eraser5,
            this._eraser10,
            this._eraser20});

        }
    }
}
