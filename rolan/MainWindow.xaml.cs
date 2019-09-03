using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using IWshRuntimeLibrary;
using my_class;

namespace rolan
{


    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    /// 

    public partial class MainWindow : Window
    {
        public bool hid_window = false;

        static public string s_path;

        public ObservableCollection<Catalog> Listbox_infos = new ObservableCollection<Catalog>() { };

        public ObservableCollection<my_class.File> FileBox_infos = new ObservableCollection<my_class.File>() { };

        public MainWindow()
        {
            InitializeComponent();

            this.Listbox.ItemsSource = Listbox_infos;
            this.FileBox.ItemsSource = FileBox_infos;
        }

        private void close_window(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void min_window(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void move_window(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                try
                {
                    DragMove();
                    hid_window = this.Top <= 10 ? true : false;
                }
                catch
                {

                }
            }
        }

        private void MainWindow_drop(object sender, DragEventArgs e)
        {
            try
            {
                string filePath = ((System.Array)e.Data.GetData(DataFormats.FileDrop)).GetValue(0).ToString();
                FileInfo fInfor = new FileInfo(filePath);
                if (fInfor.Extension == ".lnk")
                {
                    filePath = inktopath(filePath);
                    fInfor = new FileInfo(filePath);
                }
                if (fInfor.Attributes == FileAttributes.Directory)//文件夹
                {
                    foreach (var item in Listbox_infos)
                    {
                        if (item.Name == fInfor.Name)
                        {
                            return;
                        }
                    }
                    Catalog tmp_catalog = new Catalog() { IsRoot = true, _path = filePath, Name = fInfor.Name };
                    Listbox_infos.Add(tmp_catalog);
                    Listbox.SelectedIndex = Listbox_infos.Count - 1;
                    s_path = filePath;
                    //MessageBox.Show("是文件夹");
                }
                else//文件
                {
                    foreach (var item in FileBox_infos)
                    {
                        if (item.FileName == fInfor.Name)
                        {
                            return;
                        }
                    }

                    var t_window = new tool_window();
                    t_window.Top = Top + Height / 2 - t_window.Height / 2;
                    t_window.Left = Left + Width / 2 - t_window.Width / 2;
                    foreach (var item in Listbox_infos)
                    {
                        if (!item.IsRoot)
                        {
                            t_window.tlist.Add(new tool_list() { Name = item.Name });
                        }
                    }
                    t_window.ShowDialog();
                    //Console.WriteLine(t_window.result);
                    //  t_window.result;

                    bool bisName = false;
                    foreach (var item in Listbox_infos)
                    {
                        if (item.IsRoot)
                        {
                            continue;
                        }
                        if (item.Name == t_window.result)
                        {
                            bisName = true;
                            break;
                        }
                    }

                    if (bisName)
                    {
                        //存在这个目录
                        BitmapSource icon_image; myicon micon = new myicon();
                        icon_image = (BitmapSource)micon.GetIcon(filePath, false, false);

                        FileBox_infos.Add(new my_class.File() { Pic = icon_image, _filetype = FileAttributes.ReadOnly, FileName = fInfor.Name });
                    }
                    else
                    {
                        Listbox_infos.Add(new Catalog() { IsRoot = false, _path = filePath, Name = t_window.result });
                        Listbox.SelectedIndex = Listbox_infos.Count - 1;
                    }
                    //BitmapSource icon_image; myicon micon = new myicon();
                    //icon_image = (BitmapSource)micon.GetIcon(filePath, false, false);

                    //my_class.File Tmpfile = new my_class.File() { Pic = icon_image, _filetype = fInfor.Attributes, FileName = fInfor.Name };
                    //FileBox_infos.Add(Tmpfile);
                    // MessageBox.Show(fInfor.Name.Split('.')[0]);
                }
            }
            catch { }
            {
                return;
            }
        }

        private void Listbox_Catalog_Down(object sender, SelectionChangedEventArgs e)
        {
            if (Listbox.SelectedIndex == -1)
                return;
            string st_path = Listbox_infos[Listbox.SelectedIndex]._path;
            if (!Listbox_infos[Listbox.SelectedIndex].IsRoot)
            {
                if (System.IO.File.Exists(st_path))
                {
                    BitmapSource icon_image; myicon micon = new myicon();
                    icon_image = (BitmapSource)micon.GetIcon(st_path, false, false);
                    FileInfo fInfor = new FileInfo(st_path);
                    this.FileBox_infos.Add((new my_class.File() { Pic = icon_image, _filetype = FileAttributes.ReadOnly, FileName = fInfor.Name }));
                }
            }
            else
            {
                if (System.IO.Directory.Exists(st_path))
                {
                    AddFileBox(st_path);
                    s_path = st_path;
                }
                else
                {
                    Listbox_infos.RemoveAt(Listbox.SelectedIndex);
                }
            }

        }

        private void FileBox_Double_Click(object sender, MouseButtonEventArgs e)
        {
            if (FileBox.SelectedIndex == -1)
                return;
            if (FileBox_infos[FileBox.SelectedIndex]._filetype == FileAttributes.Directory)
            {
                s_path = s_path + "\\" + FileBox_infos[FileBox.SelectedIndex].FileName;
                AddFileBox(s_path);
            }
            else
            {
                string exepath = (s_path == null || s_path == "" ? Listbox_infos[Listbox.SelectedIndex]._path : s_path + "\\" + FileBox_infos[FileBox.SelectedIndex].FileName);
                if (System.IO.File.Exists(exepath))
                {
                    Process.Start(exepath);
                }
            }
        }

        private void AddFileBox(string sc_path)
        {
            FileBox_infos.Clear();
            BitmapSource icon_image; myicon micon = new myicon();
            DirectoryInfo TheFolder = new DirectoryInfo(sc_path);
            foreach (DirectoryInfo NextFolder in TheFolder.GetDirectories())
            {
                icon_image = (BitmapSource)micon.GetIcon(sc_path + "\\" + NextFolder.Name, false, true);
                this.FileBox_infos.Add((new my_class.File() { Pic = icon_image, _filetype = FileAttributes.Directory, FileName = NextFolder.Name }));
            }
            foreach (FileInfo NextFile in TheFolder.GetFiles())
            {
                icon_image = (BitmapSource)micon.GetIcon(sc_path + "\\" + NextFile.Name, false, false);
                this.FileBox_infos.Add((new my_class.File() { Pic = icon_image, _filetype = FileAttributes.ReadOnly, FileName = NextFile.Name }));
            }

        }

        public string inktopath(string sink)
        {
            if (System.IO.File.Exists(sink))
            {
                WshShell shell = new WshShell();
                IWshShortcut wshShortcut = shell.CreateShortcut(sink);
                return wshShortcut.TargetPath;
                //txtTarget.Text = link.TargetPath;
                //txtWorkingDir.Text = link.WorkingDirectory;
            }
            else
            {
                return "";
            }
        }

        private void window_MouseLeave(object sender, MouseEventArgs e)
        {
            var mouse_point = new MousePosition();
            //Console.WriteLine(mouse_point._GetCursorPos().X.ToString()+" "+ mouse_point._GetCursorPos().Y.ToString());
            hid_window = this.Top <= 10 && mouse_point._GetCursorPos().Y != 0 ? true : false;
            if (hid_window)
            {
                double CurrentTop = this.Top;
                while (CurrentTop >= -this.ActualHeight + 1.5)
                {
                    CurrentTop += -1;
                    this.Top = CurrentTop;
                }
            }
        }

        private void window_MouseEnter(object sender, MouseEventArgs e)
        {
            if (hid_window)
            {
                double CurrentTop = this.Top;
                while (CurrentTop < 0)
                {
                    CurrentTop += 1;
                    this.Top = CurrentTop;
                }
                hid_window = false;
            }
        }

        private void Listbox_del_click(object sender, RoutedEventArgs e)
        {
            if (Listbox.SelectedIndex == -1)
                return;


            int tmp_index = Listbox.SelectedIndex;
            Listbox_infos.RemoveAt(Listbox.SelectedIndex);
            if (Listbox_infos.Count > 0)
            {
                Listbox.SelectedIndex = tmp_index - 1;
            }
            else
            {
                FileBox_infos.Clear();
            }
        }

        private void Listbox_add_click(object sender, RoutedEventArgs e)
        {

        }
        //static int a = 0;
        //private void listbox_text_up(object sender, MouseButtonEventArgs e)
        //{
        //    TextBlock aItem = (TextBlock)sender;

        //    ContextMenu aMenu = new ContextMenu();

        //    MenuItem deleteMenu = new MenuItem();
        //    deleteMenu.Header = "删除";
        //    deleteMenu.Click += Listbox_del_click;

        //    aMenu.Items.Add(deleteMenu);

        //    //MenuItem editMenu = new MenuItem();
        //    //editMenu.Header = "编辑";
        //    //editMenu.Click += btEdit_Click;
        //    //aMenu.Items.Add(eidtMenu);

        //    aItem.ContextMenu = aMenu;

        //    Console.WriteLine(a++);

        //}
    }
}



