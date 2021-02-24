using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SharedClasses
{
    [Serializable]
    [XmlRoot("Score")]
    public class Score : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public Score() {}

        public Score(int _judgeId, int _judgeSeat, double _points)
        {
            judgeId = _judgeId;
            judgeSeat = _judgeSeat;
            points = _points;
        }

        [XmlElement("judgeId")]
        public int judgeId { get; set; }
        [XmlElement("judgeSeat")]
        public int judgeSeat { get; set; }

        private bool _ignore;
        public bool ignore
        {
            get
            {
                return _ignore;
            }
            set
            {
                _ignore = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ignore"));
            }
        }

        private double _points = -1;
        [XmlElement("points")]
        public double points
        {
            get { return _points; }
            set
            {
                if ((value <= 10 && value >= 0 && value % 0.5 == 0) || value == -1)
                {
                    _points = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("points"));
                }
                else
                {
                    throw new Exception("Invalid point input");                    
                }
            }
        }
    }
}
