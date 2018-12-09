﻿using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using static CPSC481Project.MainWindow;

namespace CPSC481Project
{
    /// <summary>
    /// Interaction logic for WalkinTile.xaml
    /// </summary>
    public partial class WalkinTile : UserControl
    {

        public WalkinTile()
        {
            InitializeComponent();
            Height = 162.5;
            Width = 177;
            VerticalAlignment = VerticalAlignment.Center;
            HorizontalAlignment = HorizontalAlignment.Center;
        }

        public WalkinTile(String name, String HC, int position)
        {
            InitializeComponent();
            Height = 162.5;
            Width = 177;
            VerticalAlignment = VerticalAlignment.Center;
            HorizontalAlignment = HorizontalAlignment.Center;
            NameLabel.Content = name;
            HCLabel.Content = HC;
            Position.Content = position;
        }
        public String getHC()
        {
            return HCLabel.Content.ToString();
        }
        private void OnDeleteButton(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("AASDFJLSDKFJ");
            ListBox a = (ListBox) (this.Parent);

            Grid a2 = (Grid)(a.Parent);
            Grid a3 = (Grid)(a2.Parent);
            MainWindow b = (MainWindow)(a3.Parent);
            b.walkin_delete(this);
        }

        public void UpdatePosition(int count)
        {
            Position.Content = count;
        }
    }
}
