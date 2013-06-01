using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Media;

namespace Film_Spot.Model
{
    public class MoviesResult : INotifyPropertyChanged
    {
        private string _title;
        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                if (_title != value)
                {
                    _title = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private string _link;
        public string Link
        {
            get
            {
                return _link;
            }
            set
            {
                if (_link != value)
                {
                    _link = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private string _released;
        public string Released
        {
            get
            {
                return _released;
            }
            set
            {
                if (_released != value)
                {
                    _released = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private string _imageUrl;
        public string ImageUrl
        {
            get { return _imageUrl; }
            set
            {
                if (_imageUrl != value)
                {
                    _imageUrl = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private string _star1;
        public string Star1
        {
            get { return _star1; }
            set
            {
                if (_star1 != value)
                {
                    _star1 = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private string _star2;
        public string Star2
        {
            get { return _star2; }
            set
            {
                if (_star2 != value)
                {
                    _star2 = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private string _star3;
        public string Star3
        {
            get { return _star3; }
            set
            {
                if (_star3 != value)
                {
                    _star3 = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private string _star4;
        public string Star4
        {
            get { return _star4; }
            set
            {
                if (_star4 != value)
                {
                    _star4 = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private string _star5;
        public string Star5
        {
            get { return _star5; }
            set
            {
                if (_star5 != value)
                {
                    _star5 = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private string _runtime;
        public string Runtime
        {
            get { return _runtime; }
            set
            {
                if (_runtime != value)
                {
                    _runtime = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private string _rating;
        public string Rating
        {
            get { return _rating; }
            set
            {
                if (_rating != value)
                {
                    _rating = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private string _imdbid;
        public string ImdbID
        {
            get { return _imdbid; }
            set
            {
                if (_imdbid != value)
                {
                    _imdbid = value;
                    NotifyPropertyChanged();
                }
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}

