using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;


namespace SharedClasses
{
    public class Jump : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private int _id = -1;
        public int id
        {
            get { return _id; }
            set
            {
                _id = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("id"));
            }
        }
        private double     _height;
        public double      height
        {
            get { return _height; }
            set
            {
                _height = value;
                PropertyChanged?.Invoke(this,new PropertyChangedEventArgs("height"));
            }
        }
        public bool        syncro { get; set; }
        private double     _jumpScore;
        public double      jumpScore
        {
            get { return _jumpScore;}
            set
            {
                _jumpScore = value;
                PropertyChanged?.Invoke(this,new PropertyChangedEventArgs("jumpScore"));
            }
        }
        public List<Score> scores { get; set; } = new List<Score>();
        private double _difficulty;
        public double difficulty
        {
            get { return _difficulty; }
            set
            {
                _difficulty = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("difficulty"));
            }
        }
        private string _jumpCode;
        public string jumpCode
        {
            get { return _jumpCode; }
            set
            {
                _jumpCode = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("jumpCode"));
            }
        }

        public Jump() { }

        public Jump(int _id, double _height, bool _syncro = false, string _jumpCode = "")
        {
            id = _id;
            height = _height;
            syncro = _syncro;
            //difficulty = 1;
            jumpCode = _jumpCode;
            difficulty = getJumpDifficulty();
        }

        public double getJumpDifficulty()
        {
            var db = new Database();
            var query = "SELECT DD FROM jumpcodes WHERE id='" + jumpCode + "' AND height=" + height;
            var result = db.get(query);
            if (result.Count > 0)
                return double.Parse(result[0][0]); 
            return 0;
        }

        public double calculateJumpScore()
        {
            var _scores = scores;

            if (!syncro)
            {
                if ((_scores.Count == 3)||(_scores.Count == 5)||(_scores.Count == 7))
                {
                    _scores.Sort((l, r) => l.points.CompareTo(r.points));           //sort 0-"number of scores" by points
                    for (var i = 0; _scores.Count > 3; i++)
                    {
                        _scores.RemoveAt(_scores.Count - 1);
                        _scores.RemoveAt(0);
                    }
                    jumpScore = (_scores.Sum(x => x.points) * difficulty);
                    
                    return jumpScore;
                }
                throw new Exception("Wrong number of scores");
            }
            else
            {
                _scores.Sort((l, r) => l.judgeSeat.CompareTo(r.judgeSeat));         //sort by judgeseat, regardless of 9 or 11 judges

                if (_scores.Count == 9)
                {
                    var scoreDiver12 = _scores.GetRange(0, 4);
                    scoreDiver12.Sort((l, r) => l.points.CompareTo(r.points));     //sort 0-3 by points
                    scoreDiver12.RemoveAt(0);
                    scoreDiver12.RemoveAt(scoreDiver12.Count-1);
                    var total = scoreDiver12.Sum(a => a.points);

                    var scoreSyncro = _scores.GetRange(4, 5);
                    scoreSyncro.Sort((l, r) => l.points.CompareTo(r.points));       //sort 4-9 by points
                    scoreSyncro.RemoveAt(0);
                    scoreSyncro.RemoveAt(scoreSyncro.Count-1);
                    total += scoreSyncro.Sum(b => b.points);

                    jumpScore = (total / 5) * 3 * difficulty;

                    return jumpScore;                    
                }

                if (_scores.Count == 11)
                {
                    var scoreDiver1 = _scores.GetRange(0, 3);
                    scoreDiver1.Sort((l, r) => l.points.CompareTo(r.points));       //sort 0-2 by points
                    scoreDiver1.RemoveAt(scoreDiver1.Count-1);
                    scoreDiver1.RemoveAt(0);
                    var total = scoreDiver1.Sum(a => a.points);

                    var scoreDiver2 = _scores.GetRange(3, 3);
                    scoreDiver2.Sort((l, r) => l.points.CompareTo(r.points));       //sort 3-5 by points
                    scoreDiver2.RemoveAt(scoreDiver2.Count-1);
                    scoreDiver2.RemoveAt(0);
                    total += scoreDiver2.Sum(b => b.points);

                    var scoreSyncro = _scores.GetRange(6, 5);
                    scoreSyncro.Sort((l, r) => l.points.CompareTo(r.points));       //sort 6-11 by points
                    scoreSyncro.RemoveAt(0);
                    scoreSyncro.RemoveAt(scoreSyncro.Count-1);
                    total += scoreSyncro.Sum(c => c.points);

                    jumpScore = (total / 5) * 3 * difficulty;

                    return jumpScore;
                }
                throw new Exception("Wrong number of scores");
            }
        }

        public void saveToDatabase(int contestantId = -1, int competitionId = -1, double totalScore = -1)
        {
            var db = new Database();
            var _id = (id == -1) ? "NULL" : id.ToString();
            var query = "INSERT INTO `jumps` (id, height, syncro, jumpcode, totalscore) VALUES (" + _id + ", " + height + ", " + syncro + ", '" + jumpCode + "', '" + jumpScore + "') " +
                    "ON DUPLICATE KEY UPDATE height = '" + height + "', syncro = " + syncro + ", jumpcode = '" + jumpCode + "', totalscore = '" + jumpScore+ "'";
            var returnid = db.set(query);
            if (id == -1)
            {
                id = returnid;

                query = "INSERT INTO `contestant-jump` (`contestant.id`, `jump.id`, `competition.id`) VALUES (" + contestantId+", "+id+", "+competitionId+")";
                db.set(query);
            }

            query = "INSERT INTO `" +
                    "competition-contestant" +
                    "` (`competition.id`, `contestant.id`, `totalScore`) VALUES (" + competitionId + ", " + contestantId + ", '" + totalScore + "') " +
                       "ON DUPLICATE KEY UPDATE `totalScore` = '" + totalScore+"'";
            db.set(query);

            foreach (var score in scores)
            {
                query = "INSERT INTO scores (`judge.id`, `jump.id`, " +
                        "`points" +
                        "`) VALUES (" + score.judgeId+","+id+",'"+score.points+"') " + 
                    "ON DUPLICATE KEY UPDATE points = '"+ score.points+"'";
                db.set(query);
            }
        }

        public void deleteFromDataBase()
        {
            var db = new Database();
            var query = "DELETE FROM jumps WHERE id = " + id;
            db.set(query);
        }
    }
}
