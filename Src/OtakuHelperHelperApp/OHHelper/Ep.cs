using System.ComponentModel;
using System.Runtime.CompilerServices;
using OHHelper.Annotations;

namespace OHHelper
{
    public class Ep : INotifyPropertyChanged
    {
        private bool _isCopied;
        private int _number;
        private string _url;

        public string Url
        {
            get { return _url; }
            set
            {
                if (value == _url) return;
                _url = value;
                OnPropertyChanged();
            }
        }

        public int Number
        {
            get { return _number; }
            set
            {
                if (value == _number) return;
                _number = value;
                OnPropertyChanged();
            }
        }

        public bool IsCopied
        {
            get { return _isCopied; }
            set
            {
                if (value.Equals(_isCopied)) return;
                _isCopied = value;
                OnPropertyChanged();
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}