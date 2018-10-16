using System;
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
using System.Data.Entity;
namespace ExampleLibrary
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Library library = new Library();
        public MainWindow()
        {
            InitializeComponent();
            //DataContext = library;
            //lb.DataContext = library;
            using (LibraryEntities db = new LibraryEntities())
            {
                var books = db.Books;
                string tt = "";
                foreach (Books b in books)
                    tt = b.Title;
            }
        }
    }
}
