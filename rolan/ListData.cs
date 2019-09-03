using System.ComponentModel;
using System.IO;
using System.Windows.Media;

namespace my_class
{
    public class Catalog : INotifyPropertyChanged
    {
        ///目录名
        string _catalog;
        ///目录路径
        public string _path { get; set; }
        /// <summary>
        /// isroot true 表示文件夹 false 表示root文件夹
        /// </summary>
        bool _isroot;

        public bool IsRoot
        {
            get { return _isroot; }
            set { _isroot = value; OnPropertyChanged("IsRoot"); }
        }

        public string Name
        {
            get { return _catalog; }
            set { _catalog = value; OnPropertyChanged("Name"); }
        }
        protected internal virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }

    public class File : INotifyPropertyChanged
    {
        /// <summary>
        /// _filename 文件名
        /// </summary>
        string _filename;
        /// <summary>
        /// _filetype 文件类型
        /// </summary>
        public FileAttributes _filetype { get; set; }
        ImageSource _image;
        public ImageSource Pic
        {
            get { return _image; }
            set { _image = value; OnPropertyChanged("Pic"); }
        }
        //public int Id { get; set; }
        public string FileName
        {
            get { return _filename; }
            set { _filename = value; OnPropertyChanged("FileName"); }
        }
        protected internal virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
