using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace rolan
{
    public class tool_list : INotifyPropertyChanged
    {
        /// <summary>
        /// _name 目录
        /// </summary>
        string _name;

        public string Name
        {
            get { return _name; }
            set { _name = value; OnPropertyChanged("Name"); }
        }
        protected internal virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
    /// <summary>
    /// tool_window.xaml 的交互逻辑
    /// </summary>
    public partial class tool_window : Window
    {
        public string result;
        public ObservableCollection<tool_list> tlist = new ObservableCollection<tool_list>() { };

        public tool_window()
        {
            InitializeComponent();
            AnimalListBox.ItemsSource = tlist;
        }

        //private void tool_window_move(object sender, MouseEventArgs e)
        //{
        //    if (e.LeftButton == MouseButtonState.Pressed)
        //    {
        //        try
        //        {
        //            DragMove();
        //        }
        //        catch
        //        {

        //        }
        //    }
        //}
        private void add_click(object sender, RoutedEventArgs e)
        {
            if(AnimalTextBox.Text!="")
            {
                tlist.Add(new tool_list() { Name = AnimalTextBox.Text });
            }
        }

        private void Sample5_DialogHost_OnDialogClosing(object sender, MaterialDesignThemes.Wpf.DialogClosingEventArgs eventArgs)
        {

        }

        private void list_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (AnimalListBox.SelectedIndex == -1)
                return;
            result =  tlist[AnimalListBox.SelectedIndex].Name;
            this.Close();
        }
    }
}
