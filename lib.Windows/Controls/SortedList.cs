using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lib.Windows.Controls
{
    public class SortedList : ListView
    {
        string _filter;
        public string Filtering
        {
            get => _filter;
            set
            {
                _filter = value;
                Reload();
            }
        }
        public SortedList() : base()
        {
            this.GridLines = false;
            this.View = View.Details;
            this.FullRowSelect = true;
            this.ListViewItemSorter = new Sorter(0, true);
            this.ColumnClick += (sender, e) => Sort(e.Column);
        }
        public void Reload()
        {
            Items.Clear();
            if (!this.DesignMode) Items.AddRange(GetItemList().Where(_ => Filter(_)).ToArray());
            this.Sort();
        }
        public void Sort(int index)
        {
            var sort = this.ListViewItemSorter as Sorter;
            this.ListViewItemSorter = new Sorter(index, index == sort.Index ? !sort.Ascending : sort.Ascending);
        }
        protected virtual bool Filter(ListViewItem item) => string.IsNullOrEmpty(_filter) || Regex.IsMatch(item.Text, _filter, RegexOptions.IgnoreCase);
        protected virtual IEnumerable<ListViewItem> GetItemList() => new ListViewItem[] { };
        class Sorter : IComparer
        {
            public int Index { get; }
            public bool Ascending { get; }
            public Sorter(int index, bool asc)
            {
                Index = index;
                Ascending = asc;
            }
            public int Compare(object x, object y)
            {
                var asc = Ascending ? 1 : -1;
                var x_ = x as ListViewItem;
                var y_ = y as ListViewItem;
                var xx = x_?.SubItems[Index].Text ?? "";
                var yy = y_?.SubItems[Index].Text ?? "";
                if (int.TryParse(xx, out var xi) && int.TryParse(yy, out var yi)) return (xi - yi) * asc;
                return string.Compare(xx, yy) * asc;
            }
        }
    }
}
