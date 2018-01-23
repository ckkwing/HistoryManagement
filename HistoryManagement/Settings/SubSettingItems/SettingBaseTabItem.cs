using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace HistoryManagement.Settings.SubSettingItems
{
    public class SettingBaseTabItem : TabItem, INotifyPropertyChanged
    {
       
        public ImageSource Icon
        {
            get { return (ImageSource)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        public static readonly DependencyProperty IconProperty =
        DependencyProperty.Register("Icon", typeof(ImageSource), typeof(SettingBaseTabItem));

        public double IconWidth
        {
            get { return (double)GetValue(IconWidthProperty); }
            set { SetValue(IconWidthProperty, value); }
        }

        public static readonly DependencyProperty IconWidthProperty =
        DependencyProperty.Register("IconWidth", typeof(double), typeof(SettingBaseTabItem));

        public double IconHeight
        {
            get { return (double)GetValue(IconHeightProperty); }
            set { SetValue(IconHeightProperty, value); }
        }

        public static readonly DependencyProperty IconHeightProperty =
        DependencyProperty.Register("IconHeight", typeof(double), typeof(SettingBaseTabItem));

        protected bool CanExcute(object parameter)
        {
            return true;
        }

        protected bool CanExcute()
        {
            return true;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(params string[] properties)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (handler != null)
            {
                foreach (var property in properties)
                {
                    handler(this, new PropertyChangedEventArgs(property));
                }
            }
        }
    }
}
