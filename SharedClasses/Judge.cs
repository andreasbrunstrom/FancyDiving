using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace SharedClasses
{
    [Serializable]
    [XmlRoot("Judge")]
    public class Judge : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public Judge() { }

        public Judge(int _id, int _judgeSeat, string _name, string _nationality)
        {
            id = _id;
            judgeSeat = _judgeSeat;
            name = _name;
            nationality = _nationality;
        }

        [XmlElement("judgeseat")]
        private int _judgeSeat;

        public int judgeSeat
        {
            get { return _judgeSeat; }
            set
            {
                _judgeSeat = value;
                PropertyChanged?.Invoke(this,  new PropertyChangedEventArgs("judgeSeat"));
            }
        }

        [XmlElement("name")]
        private string _name;
        public string name
        {
            get { return _name; }
            set
            {
                _name = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("name"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("displayNameNationality"));
            }
        }

        [XmlElement("nationality")]
        private string _nationality;

        public string nationality
        {
            get { return _nationality; }
            set
            {
                _nationality = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("nationality"));
            }
        }

        public string displayNameNationality => name + ", " + nationality;
        private int _id = -1;
        private bool _occupied { get; set; }
        public bool occupied
        {
            get { return _occupied;  }
            set
            {
                _occupied = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("occupied"));
            }
        }

        public int id
        {
            get { return _id; }
            set
            {
                _id = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("id"));
            }
        }

        public void saveToDatabase()
        {
            var db = new Database();
            var _id = (id == -1) ? "NULL" : id.ToString();
            var query = "INSERT INTO `judges` (id, name, nationality) VALUES (" + _id + ", '" + name + "', '" + nationality + "') " +
                        "ON DUPLICATE KEY UPDATE name = '" + name + "', nationality = '" + nationality + "'";
            var returnid = db.set(query);
            if (id == -1)
            {
                id = returnid;
            }

        }

        public void deleteFromDatabase()
        {
            var db = new Database();
            var query = "DELETE FROM judges WHERE id = " + id;
            db.set(query);
        }
    }
}
