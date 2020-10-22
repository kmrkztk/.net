using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace life.Controls.Menus
{
    public class MenuPens : MenuItem
    {
        private IContainer components;
        private MenuPen1 _pen1;
        private MenuPen5 _pen5;
        private MenuPen10 _pen10;
        private MenuPen20 _pen20;
        private MenuPen50 _pen50;

        public override string Text { get => "Pen(&P)"; set => base.Text = value; }
        public Edit Component 
        {
            get => _pen1.Component;
            set
            {
                _pen1.Component = value;
                _pen5.Component = value;
                _pen10.Component = value;
                _pen20.Component = value;
                _pen50.Component = value;
            }
        }
        public MenuPens() : base() => InitializeComponent();
        public MenuPens(IContainer container) : base(container) => InitializeComponent();

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this._pen1 = new life.Controls.Menus.MenuPen1(this.components);
            this._pen5 = new life.Controls.Menus.MenuPen5(this.components);
            this._pen10 = new life.Controls.Menus.MenuPen10(this.components);
            this._pen20 = new life.Controls.Menus.MenuPen20(this.components);
            this._pen50 = new life.Controls.Menus.MenuPen50(this.components);
            // 
            // menuPen11
            // 
            this._pen1.CheckOnClick = true;
            this._pen1.Name = "_pen1";
            this._pen1.Size = new System.Drawing.Size(110, 22);
            this._pen1.Text = "1 x 1";
            // 
            // menuPen51
            // 
            this._pen5.CheckOnClick = true;
            this._pen5.Name = "_pen5";
            this._pen5.Size = new System.Drawing.Size(110, 22);
            this._pen5.Text = "5 x 5";
            // 
            // menuPen101
            // 
            this._pen10.CheckOnClick = true;
            this._pen10.Name = "_pen10";
            this._pen10.Size = new System.Drawing.Size(110, 22);
            this._pen10.Text = "10 x 10";
            // 
            // menuPen201
            // 
            this._pen20.CheckOnClick = true;
            this._pen20.Name = "_pen20";
            this._pen20.Size = new System.Drawing.Size(110, 22);
            this._pen20.Text = "20 x 20";
            // 
            // menuPen201
            // 
            this._pen50.CheckOnClick = true;
            this._pen50.Name = "_pen50";
            this._pen50.Size = new System.Drawing.Size(110, 22);
            this._pen50.Text = "50 x 50";
            // 
            // MenuPens
            // 
            this.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._pen1,
            this._pen5,
            this._pen10,
            this._pen20,
            this._pen50});

        }
    }
}
