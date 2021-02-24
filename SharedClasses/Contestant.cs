using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace SharedClasses
{
    public class Contestant : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public Contestant() { }
        public Contestant(int _id, string _name, string _club, string _nationality, bool _gender, DateTime _birthdate)
        {
            id = _id;
            name = _name;
            club = _club;
            nationality = _nationality;
            gender = _gender;
            birthdate = _birthdate;
        }

        public int id { get; set; }             = -1;
        private string _name;
        public string name
        {
            get { return _name; }
            set
            {
                _name = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("name"));
            }
        }

        private string _club;
        public string club
        {
            get { return _club; }
            set
            {
                _club = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("club"));
            }
        }

        private string _nationality;
        public string nationality
        {
            get {return _nationality;}
            set
            {
                _nationality = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("nationality"));
            }
        }

        private bool _gender;
        public bool gender
        {
            get { return _gender; }
            set
            {
                _gender = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("gender"));
            }
        }
        public string genderString => gender ? "Man" : "Kvinna";

        private DateTime _birtdate;
        public DateTime birthdate
        {
            get { return _birtdate; }
            set
            {
                _birtdate = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("birthdate"));
            }
        }

        public List<Jump> jumps { get; set; }   = new List<Jump>();
        private double _totalScore = 0;
        public double totalScore
        {
            get { return _totalScore; }
            set
            {
                _totalScore = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("totalScore"));
            }
        }

        public void calculateTotalScore()
        {
            totalScore = 0;
            foreach(var i in jumps)
            {
                totalScore += i.jumpScore;
            }
        }

        public void saveToDatabase()
        {
            var db = new Database();
            var _id = (id == -1) ? "NULL" : id.ToString();
            var query = "INSERT INTO `contestants` (id, name, club, nationality, gender, birthdate) VALUES (" + _id + ", '" + name + "', '" + club + "', '" + nationality + "', " + gender + ", '" + birthdate.Date + "') " +
                    "ON DUPLICATE KEY UPDATE name = '" + name + "', club = '" + club + "', nationality = '" + nationality + "', gender = " + gender + ", birthdate = '" + birthdate.Date + "'";
            var returnid = db.set(query);
            if (id == -1)
            {
                id = returnid;
            }
        }

        public void deleteFromDatabase()
        {
            var db = new Database();
            var query = "DELETE FROM contestants WHERE id = " + id;
            db.set(query);
        }
    }
}
