using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Data.Entity;
using System.Windows;
namespace ExampleLibrary 
{
    public class Library : INotifyPropertyChanged
    {
        public LibraryEntities context = new LibraryEntities();

        public ObservableCollection<Authors> authors
        {
            get;
            set;
        }
        ObservableCollection<Books> _booksOfAuthor;
        public ObservableCollection<Books> booksOfAuthor
        {
            get { return _booksOfAuthor; }
            set
            {
                _booksOfAuthor = value;
                OnPropertyChanged("booksOfAuthor");
            }
        }
        public Library()
        {
            context.Authors.Load();
            context.Books.Load();

            authors = context.Authors.Local;
            AddAuthor = new Authors();
            AddBook = new Books();
            AddBookAuthor = new Authors();
        }

        private Authors selectedAuthor;
        public Authors SelectedAuthor
        {
            get { return selectedAuthor; }
            set
            {
                selectedAuthor = value;
                var bcs = from b in context.Books
                             where b.AuthorId == selectedAuthor.Id
                             select b;
                booksOfAuthor = new ObservableCollection<Books>(bcs);
                OnPropertyChanged("SelectedAuthor");
            }
        }

        private Books selectedBook;
        public Books SelectedBook
        {
            get { return selectedBook; }
            set
            {
                selectedBook = value;
                OnPropertyChanged("SelectedBook");
            }
        }
        public Authors AddAuthor
        {
            get;
            set;
        }
        public Books AddBook
        {
            get;
            set;
        }
        public Authors AddBookAuthor
        {
            get;
            set;
        }
        //Команды

        // команда удаления
        private RelayCommand removeBookCommand;
        public RelayCommand RemoveBookCommand
        {
            get
            {
                return removeBookCommand ??
                  (removeBookCommand = new RelayCommand(obj =>
                  {
                      Books delBook = obj as Books;
                      if (delBook != null)
                      {
                          context.Books.Remove(delBook);                          
                          booksOfAuthor.Remove(delBook);
                          context.SaveChanges();
                      }
                  },
                 (obj) => context.Books.Count() > 0));
            }
        }
        private RelayCommand addAuthorCommand;
        public RelayCommand AddAuthorCommand
        {
            get
            {
                return addAuthorCommand ??
                     (addAuthorCommand = new RelayCommand(obj =>
                     {
                         if (AddAuthor != null)
                         {
                             context.Authors.Add(new Authors { Name = AddAuthor.Name, PenNames=AddAuthor.PenNames });
                             context.SaveChanges();
                         }
                     },
                    (obj) => true));
            }
        }
        private RelayCommand addBookCommand;
        public RelayCommand AddBookCommand
        {
            get
            {
                return addBookCommand ??
                     (addBookCommand = new RelayCommand(obj =>
                     {
                         if (AddBook != null)
                         {
                             context.Books.Add(new Books { Title = AddBook.Title, Abstract =AddBook.Abstract, AuthorId = AddBookAuthor.Id});
                             context.SaveChanges();
                         }
                     },
                    (obj) => context.Books.Count() > 0));
            }
        }
        private RelayCommand menuAboutCommand;
        public RelayCommand MenuAboutCommand
        {
            get
            {
                return menuAboutCommand ??
                     (menuAboutCommand = new RelayCommand(obj =>
                     {
                         MessageBox.Show("Пример приложения с использованием Entity Framework \n Суворов Иван 2018");
                     },
                    (obj) => true));
            }
        }
        private RelayCommand menuExitCommand;
        public RelayCommand MenuExitCommand
        {
            get
            {
                return menuExitCommand ??
                     (menuExitCommand = new RelayCommand(obj =>
                     {
                         Application.Current.MainWindow.Close();
                     },
                    (obj) => true));
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
