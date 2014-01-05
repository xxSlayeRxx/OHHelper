using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using Microsoft.Expression.Interactivity.Core;
using Microsoft.Practices.Prism.Commands;
using OHHelper.AnimeServices;
using OHHelper.Annotations;
using MessageBox = System.Windows.MessageBox;

namespace OHHelper
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private readonly IAnimeParser _animeParser;
        private ICommand _addAllToOhCommand;
        private ICommand _addToListCommand;
        private ICommand _addToOhCommand;
        private ObservableCollection<Anime> _animes;
        private ICommand _deleteAllCommand;
        private ICommand _deleteSelectedCommand;
        private string _urlToAnime;
        private int _timeToWait;
        private bool _isBusy;

        public MainWindowViewModel()
        {
            var AnimeZone = new ServiceWithLink(new AnimeZoneService(), "animezone.pl");
            var AnimeOdcinki = new ServiceWithLink(new AnimeOdcinkiService(), "anime-odcinki.pl/articles.php");
            var AnimeOn = new ServiceWithLink(new AnimeOnService(), "animeon.pl/anime/");

            _animeParser = new AnimeParser().WithService(AnimeZone).And(AnimeOdcinki).And(AnimeOn);

            AddToListCommand = new ActionCommand(AddToListAction);
            AddToOhCommand = new DelegateCommand<IEnumerable>(AddToOhAction);
            AddAllToOhCommand = new ActionCommand(AddAllToOhAction);
            DeleteSelectedCommand = new DelegateCommand<IEnumerable>(DeleteSelectedAction);
            DeleteAllCommand = new ActionCommand(DeleteAllAction);

            Animes = new ObservableCollection<Anime>();

            TimeToWait = 150;
        }

        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                if (value.Equals(_isBusy)) return;
                _isBusy = value;
                OnPropertyChanged();
            }
        }


        public int TimeToWait
        {
            get { return _timeToWait; }
            set
            {
                if (value == _timeToWait) return;
                _timeToWait = value;
                OnPropertyChanged();
            }
        }


        public string UrlToAnime
        {
            get { return _urlToAnime; }
            set
            {
                if (value == _urlToAnime) return;
                _urlToAnime = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Anime> Animes
        {
            get { return _animes; }
            set
            {
                if (Equals(value, _animes)) return;
                _animes = value;
                OnPropertyChanged();
            }
        }


        public ICommand AddToListCommand
        {
            get { return _addToListCommand; }
            set
            {
                if (Equals(value, _addToListCommand)) return;
                _addToListCommand = value;
                OnPropertyChanged();
            }
        }

        public ICommand AddToOhCommand
        {
            get { return _addToOhCommand; }
            set
            {
                if (Equals(value, _addToOhCommand)) return;
                _addToOhCommand = value;
                OnPropertyChanged();
            }
        }

        public ICommand AddAllToOhCommand
        {
            get { return _addAllToOhCommand; }
            set
            {
                if (Equals(value, _addAllToOhCommand)) return;
                _addAllToOhCommand = value;
                OnPropertyChanged();
            }
        }

        public ICommand DeleteSelectedCommand
        {
            get { return _deleteSelectedCommand; }
            set
            {
                if (Equals(value, _deleteSelectedCommand)) return;
                _deleteSelectedCommand = value;
                OnPropertyChanged();
            }
        }

        public ICommand DeleteAllCommand
        {
            get { return _deleteAllCommand; }
            set
            {
                if (Equals(value, _deleteAllCommand)) return;
                _deleteAllCommand = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void DeleteAllAction()
        {
            Animes.Clear();
        }

        private void DeleteSelectedAction(IEnumerable selectedAnimes)
        {
            List<Anime> temp = selectedAnimes.Cast<Anime>().ToList();
            foreach (Anime anime in temp)
            {
                Animes.Remove(anime);
            }
        }

        private void AddAllToOhAction()
        {
            IsBusy = true;
            int i = 0;
            foreach (Anime anime in Animes)
            {
                foreach (Ep ep in anime.Eps)
                {
                    Clipboard.SetText(ep.Url);
                    i++;
                    Wait(TimeToWait);
                }
            }
            IsBusy = false;
            MessageBox.Show("Dodałem wszystkie odcinki" + Environment.NewLine + "Łącznie: " + i);
        }

        private void AddToOhAction(IEnumerable selectedAnimes)
        {
            IsBusy = true;
            int i = 0;
            foreach (Anime anime in selectedAnimes.Cast<Anime>())
            {
                foreach (Ep ep in anime.Eps)
                {
                    Clipboard.SetText(ep.Url);
                    i++;
                    Wait(TimeToWait);
                }
            }

            IsBusy = false;
            MessageBox.Show("Dodałem wybrane odcinki" + Environment.NewLine + "Łącznie: " + i);
        }

        private void Wait(int miliseconds)
        {
            var frame = new DispatcherFrame();
            new Thread((ThreadStart)(() =>
            {
                Thread.Sleep(TimeSpan.FromMilliseconds(miliseconds));
                frame.Continue = false;
            })).Start();
            Dispatcher.PushFrame(frame);
        }

        private void AddToListAction()
        {
            IsBusy = true;
            if (string.IsNullOrWhiteSpace(UrlToAnime))
            {
                IsBusy = false;
                return;
            }
            TaskScheduler scheduler = TaskScheduler.FromCurrentSynchronizationContext();

            Task.Factory.StartNew(() => { })
                .ContinueWith(AddToList, scheduler);
        }

        private void AddToList(Task task)
        {
            Anime anime = GetAnime();
            if (anime == null)
            {
                IsBusy = false;
                return;
            }
            Animes.Add(anime);
            IsBusy = false;
        }

        private Anime GetAnime()
        {
            Anime anime = _animeParser.Parse(UrlToAnime);
            if (anime == null)
            {
                UrlToAnime = string.Empty;
                return null;
            }
            return anime;
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}