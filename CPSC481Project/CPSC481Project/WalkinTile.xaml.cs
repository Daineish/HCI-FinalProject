using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public ObservableCollection<WalkinTile> m_collection { get; set; }
        public ListBox m_list { get; set; }

        public WalkinTile()
        {
            m_collection = null;
            m_list = null;
            InitializeComponent();
            Height = 162.5;
            Width = 177;
            VerticalAlignment = VerticalAlignment.Center;
            HorizontalAlignment = HorizontalAlignment.Center;
        }

        //public WalkinTile(String name, String HC, int position)
        //{
        //    m_collection = null;
        //    m_list = null;
        //    InitializeComponent();
        //    Height = 162.5;
        //    Width = 177;
        //    VerticalAlignment = VerticalAlignment.Center;
        //    HorizontalAlignment = HorizontalAlignment.Center;
        //    NameLabel.Content = name;
        //    HCLabel.Content = HC;
        //    Position.Content = position;
        //}

        public WalkinTile(String name, String HC, int position, ObservableCollection<WalkinTile> c, ListBox l)
        {
            m_collection = c;
            m_list = l;
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
            if(m_collection != null && m_list != null)
            {
                m_collection.Remove(this);
                m_list.ItemsSource = m_collection;

                m_collection = null;
                m_list = null;
            }
        }

        public void UpdatePosition(int count)
        {
            Position.Content = count;
        }
    }
}
