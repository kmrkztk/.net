using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using life.IO;

namespace life.Controls
{
    public partial class MapFile : Component
    {
        public MapFile() : this(null) { }
        public MapFile(IContainer container)
        {
            container?.Add(this);
            InitializeComponent();
        }
        MapFileInfo _fi;
        [Browsable(false)] public string Name { get; set; }
        [Browsable(false)] public string Author { get; set; }
        [Browsable(false)] public List<string> Comments { get; set; }
        [Browsable(false)] public string Source => _fi?.FullPath ?? _fi?.ResouceName;
        [Browsable(false)] public bool Savable => _fi?.Savable ?? false;
        public void Open() 
        {
            if (_open.ShowDialog() != DialogResult.OK) return;
            _fi = new MapFileInfo(_open.FileName);
            if (Owner != null) Owner.Map.Map = _fi.Load();
            Name = _fi.Name;
            Author = _fi.Author;
            Comments = _fi.Comments;
        }
        public void Save() 
        {
            if (Owner.Map?.Map == null) return;
            if (!Savable) return;
            _fi.Clear();
            _fi.Name = Name;
            _fi.Author = Author;
            _fi.Comments.AddRange(Comments);
            _fi.Save(Owner.Map?.Map);
        }
        public void SaveAs() 
        {
            if (Owner.Map?.Map == null) return;
            if (_save.ShowDialog() != DialogResult.OK) return;
            _fi = MapFileInfo.Save(_save.FileName, Owner.Map.Map, Name, Author, Comments.ToArray());
        }
    }
}
