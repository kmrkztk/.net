using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Lib.Windows.Controls;
using life.IO;

namespace life.Controls
{
    public class PatternList : SortedList
    {
        readonly PatternItem[] _patterns = life.Patterns.GetFiles().Select(_ => new PatternItem(_)).ToArray();
        [Browsable(false)] public MapFileInfo SelectedMap => SelectedItems.Cast<PatternItem>().FirstOrDefault()?.Info;
        public event EventHandler SelectedMapChanged;
        protected virtual void OnSelectedMapChanged() => SelectedMapChanged?.Invoke(this, EventArgs.Empty);
        public PatternList() : base()
        {
            this.Columns.Add("name", 200);
            this.Columns.Add("author", 100);
            this.Columns.Add("size");

            this.SelectedIndexChanged += (sender, e) => { if (SelectedMap != null) OnSelectedMapChanged(); };
        }
        protected override IEnumerable<ListViewItem> GetItemList() => _patterns;
        class PatternItem : ListViewItem
        {
            public MapFileInfo Info { get; }
            public PatternItem(MapFileInfo mi) : base()
            {
                Info = mi;
                Text = string.Format("{0} ({1}x{2})", mi.Name ?? mi.ResouceName.Replace("life.patterns.", ""), mi.Width, mi.Height);
                SubItems.Add(mi.Author ?? "Unknown");
                SubItems.Add((mi.Width * mi.Height).ToString());
            }
        }
    }
}
