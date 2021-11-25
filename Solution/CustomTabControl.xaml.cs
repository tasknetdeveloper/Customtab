using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Solution
{

    internal interface ITabActions
    {
        List<Message2> GetAllTabData();
        void LoadTabData(Guid uid);
        void SaveData();
    }

    public partial class CustomTabControl : UserControl
    {
        private List<Message2> ListAllTabData = null;
        private int N = 5;
        private const int widthLeftRightButton = 14;
        private const int justCalcRightSize =  250;
        private const int Nmin = 5;
        private const int MinLimitTabs2 = 5;
        private System.Windows.Media.SolidColorBrush selectedColor = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Red);
        private System.Windows.Media.SolidColorBrush defaultColor = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.LightBlue);
        private int MinTabWidth = 120;//80;
        private Button selectedButton = new Button();
        //Число возможных табов на экране в зависимости от ширины экрана
        private ITabActions actions;

        public CustomTabControl()
        {
            InitializeComponent();
            this.Loaded += (s, e) => {
                LoadSomTabs();
                Ini(true);
            };
            

            this.PreviewKeyDown += (s, e) => {
                if (e.Key == Key.PageUp && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
                {
                    Move(true);
                }
            };
            this.PreviewKeyDown += (s, e) => {
                if (e.Key == Key.PageDown && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
                {
                    Move(false);
                }
            };

            this.btnMoveRight.PreviewMouseLeftButtonDown += (s, e) => {
                Move(false);
            };
            this.btnMoveLeft.PreviewMouseLeftButtonDown += (s, e) => {
                Move(true);
            };
        }

        private void LoadSomTabs() {
            var i=0;
            Button b = null;
            var yourStyle = (Style)Application.Current.Resources["FlatButton"];
            while (i<20)
            {
                b = new Button
                {
                    Content = "some " + i
                };
                //b.Width = 150;
                b.PreviewMouseDown += (s, e) => {
                    BtnPreviewMouseDown(s,e);
                };
                b.Style = yourStyle;
                ButtonStack.Children.Add(b);
                //ButtonStack.UpdateLayout();
                i++;
            }
            
        }

        private void Move(bool isLeft)
        {
            CheckAll(isLeft);
            if (this.actions != null)
                this.actions.LoadTabData((Guid)selectedButton.Tag);
        }

        internal void IniActions(ITabActions actions)
        {
            this.actions = actions;
            this.ListAllTabData = this.actions.GetAllTabData();
            LoadTabs();
        }

        private void LoadTabs()
        {
            if (this.actions == null) return;
            //TODO
            this.ListAllTabData.ForEach(x => {
                ButtonStack.Children.Add(new Button
                {
                    Tag = x.Uid,
                    Content = x.Name
                });
            });
        }

        private void BtnPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            var r = (Button)sender;
            selectedButton.Background = defaultColor;
            selectedButton = r;
            r.Background = selectedColor;
        }

        private void Ini(bool selectFirstTab = false)
        {
            selectedButton.Background = defaultColor;
            var i = 0;
            N = GetN();
            foreach (var item in ButtonStack.Children)
            {
                if (item is Button)
                {
                    var r = (Button)item;
                    r.Width = GetWidth(r);
                    if (i > N)
                        r.Width = 0;
                }
                i++;
            }

            if (selectFirstTab)
            {
                foreach (var item in ButtonStack.Children)
                {
                    if (item is Button)
                    {
                        var r = (Button)item;
                        selectedButton = r;
                        r.Background = selectedColor;
                        break;
                    }
                }
            }

            ControlButtonVisibility();
        }

        private void CheckAll(bool isLeft)
        {
            N = GetN();
            var calcTabNumbers = CalcTabNumberInMainWidth();
            ControlButtonVisibility();
            var b = false;
            double w = 0;
            if (!isLeft)//Right move
            {
                var i = 0;
                var ix = 0;
                foreach (var item in ButtonStack.Children)
                {
                    if (item is Button)
                    {
                        var r = (Button)item;

                        if (r == selectedButton)
                        {
                            b = true;
                            if (((i + 1) == ButtonStack.Children.Count)) return;

                            var r2 = (Button)ButtonStack.Children[i + 1];

                            if ((int)(this.ActualWidth - (w + justCalcRightSize)) <= 0 && (i + 1 >= N))
                            {
                                HideRightTabs();
                            }

                            selectedButton.Background = defaultColor;
                            r2.Background = selectedColor;
                            selectedButton = r2;

                            break;
                        }

                        if (r.Width != 0)
                        {
                            ix++;
                            w += r.Width;
                        }

                        i++;
                    }
                }
            }
            else //Left
            {
                var i = 0;
                foreach (var item in ButtonStack.Children)
                {
                    if (item is Button)
                    {
                        var r = (Button)item;

                        if (r == selectedButton)
                        {
                            if (i - 1 < 0) return;

                            if (i >= N)
                            {
                                var r2 = (Button)ButtonStack.Children[i - N];
                                r2.Width = 0;
                            }

                            var r0 = (Button)ButtonStack.Children[i - 1];
                            r0.Width = GetWidth(r0);

                            selectedButton.Background = defaultColor;
                            r0.Background = selectedColor;
                            selectedButton = r0;

                            break;
                        }

                        i++;
                    }
                }
            }
            this.UpdateLayout();
        }

        #region Utils
        private int CalcTabNumberInMainWidth()
        {
            var result = 0;
            double wloc = 0;
            var i = 0;
            foreach (var item in ButtonStack.Children)
            {
                if (item is Button)
                {
                    var r = (Button)item;
                    if (r.Width != 0)
                    {
                        wloc = wloc + r.Width + 2;
                    }
                    i++;
                }
            }
            if (wloc <= 0) return result;

            wloc = wloc / i;
            result = (int)(((int)this.ActualWidth - widthLeftRightButton * 2) / wloc);
            return result;
        }

        private void HideRightTabs()
        {
            var wselectedButton = selectedButton.Width;
            var wMain = this.ActualWidth - widthLeftRightButton * 2;
            double wloc = 0;

            foreach (var item in ButtonStack.Children)
            {
                if (item is Button)
                {
                    var r = (Button)item;
                    if (r.Width != 0)
                    {
                        wloc = wMain - r.Width - 2;

                        r.Width = 0;
                        if ((wloc + selectedButton.Width) < wMain)
                        {
                            break;
                        }
                        else
                            r.Width = 0;
                    }
                }
            }
        }

        private void ControlButtonVisibility()
        {
            if (ButtonStack.Children.Count < MinLimitTabs2)
            {
                btnMoveRight.Visibility = Visibility.Hidden;
                btnMoveLeft.Visibility = Visibility.Hidden;
            }
            else
            {
                btnMoveRight.Visibility = Visibility.Visible;
                btnMoveLeft.Visibility = Visibility.Visible;
            }
        }

        private double GetWidth(Button btn)
        {
            double result = 0;
            result = btn.Content.ToString().Length * btn.FontSize;
            if (result < MinTabWidth)
                result = MinTabWidth;
            return result;
        }

        //Определяем кол-во видимых табов
        private int GetN()
        {
            var result = 0;
            var w = (int)this.ActualWidth - widthLeftRightButton * 2;
            var wloc = 0;

            foreach (var item in ButtonStack.Children)
            {
                if (item is Button)
                {
                    var r = (Button)item;
                    if (r.Width != 0)
                    {
                        wloc = wloc + (int)r.Width + 2;
                        if (wloc >= w)
                            break;
                        result++;
                    }
                }
            }

            if (result < Nmin)
                result = Nmin;
            return result;
        }
        #endregion
    }

    
}
