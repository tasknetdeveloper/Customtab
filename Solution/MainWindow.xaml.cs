using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Solution
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private int selectedIndex = 0;
        //private void TabEvents() {

        //    this.MainTabControl.GotFocus += (s, e) => {
        //        var b1 = false;
        //    };
        //    this.tabGrid.PreviewKeyDown += (s, e) => {
        //        var b1 = false;
        //    };
        //    this.MainTabControl.KeyDown += (s, e) => {
        //        var b1 = false;
        //    };
        //    this.MainTabControl.PreviewKeyDown += (s, e) =>{
        //        if (e.SystemKey == Key.RightCtrl && e.Key == Key.PageDown)
        //        {
        //            //var b = false;
        //            //var r = (TabControl)s;
        //            //var sl = this.MainTabControl.SelectedItem as TabControl;
        //            selectedIndex++;
        //            this.MainTabControl.SelectedIndex = selectedIndex;
        //        }
        //    };

        //    //this.MainTabControl.PreviewKeyUp += (s, e) => {
        //    //    if (e.SystemKey == Key.RightCtrl && e.Key == Key.PageDown)
        //    //    {
        //    //        //var b = false;
        //    //        //var r = (TabControl)s;
        //    //        //var sl = this.MainTabControl.SelectedItem as TabControl;
        //    //        selectedIndex--;
        //    //        this.MainTabControl.SelectedIndex = selectedIndex;
        //    //    }
        //    //};
        //}
    }
}
