using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace SharedClasses
{
    public class Competition: INotifyPropertyChanged

    {
        public event PropertyChangedEventHandler PropertyChanged;

        public int id { get; set; }                              = -1;
        private string _name                                     = "Ny tävling";
        public string name
        {
            get { return _name; }
            set
            {
                _name = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("name"));
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
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("genderString"));
            }
        }
        public string genderString => gender ? "Män" : "Kvinnor";
        private bool _syncro = false;
        public bool syncro {
            get { return _syncro;  }
            set {
                _syncro = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("syncro"));
            }
        }
        public double height { get; set; }
        private DateTime _date = DateTime.Today;
        public DateTime date
        {
            get { return _date; }
            set
            {
                _date = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("date"));
            }
        }
        private string _ageGroup;
        public string ageGroup
        {
            get { return _ageGroup; }
            set
            {
                _ageGroup = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ageGroup"));
            }
        }
        private Jump _selectedJump;
        public Jump selectedJump
        {
            get { return _selectedJump; }
            set
            {
                _selectedJump = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("selectedJump"));
            }
        }
        private Contestant _selectedContestant;
        public Contestant selectedContestant
        {
            get { return _selectedContestant; }
            set
            {
                _selectedContestant = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("selectedContestant"));
            }
        }
        private Judge _selectedJudge;
        public Judge selectedJudge
        {
            get { return _selectedJudge; }
            set
            {
                _selectedJudge = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("selectedJudge"));
            }
        }
        public BindingList<Judge> judges { get; set; }           = new BindingList<Judge>();

        private BindingList<Contestant> _contestants = new BindingList<Contestant>();

        public BindingList<Contestant> contestants
        {
            get { return _contestants; }
            set
            {
                _contestants = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("contestants"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("highscore"));
            }
        }

        private int _currentContestantindex;
        public int currentContestantindex
        {
            get { return _currentContestantindex; }
            set
            {
                _currentContestantindex = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("currentContestantindex"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("currentContestant"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("currentJump"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("highscore"));
            }
        }
        public Contestant currentContestant => contestants.Count > currentContestantindex ? contestants[currentContestantindex] : null;
        private int _currentRound;
        public int currentRound
        {
            get { return _currentRound; }
            set
            {
                _currentRound = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("currentRound"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("currentJump"));
            }
        }
        public Jump currentJump {
            get
            {
                if (contestants.Count > currentContestantindex)
                {
                    return contestants[currentContestantindex].jumps.Count > currentRound ? contestants[currentContestantindex].jumps[currentRound] : null;
                }
                return null;
            }
        }
        private int _rounds = 1;
        public int rounds
        {
            get { return _rounds; }
            set
            {
                foreach (var contestant in contestants)
                {
                    while (contestant.jumps.Count != value)
                    {
                        if (contestant.jumps.Count < value)
                        {
                            contestant.jumps.Add(new Jump());
                        }
                        else
                        {
                            var jump = contestant.jumps.Last();
                            if (jump.id != -1)
                            {
                                jump.deleteFromDataBase();
                            }
                            contestant.jumps.Remove(jump);
                        }
                    }
                }
                _rounds = value;
                selectedContestant = null;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("rounds"));

                //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("selectedContestant"));
            }
        }
        private bool _finnished;
        public bool finished
        {
            get
            {
                return _finnished;
            }
            set
            {
                _finnished = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("finished"));
            }
        }
        private bool _started { get; set; }
        public bool started {
            get { return _started; }
            set
            {
                _started = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("started"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("highscore"));
            }
        }

        public BindingList<Contestant> highscore => new BindingList<Contestant>(contestants.OrderByDescending(x => x.totalScore).ToList());


        public BindingList<Jump> jumpsInCurrentCompetition { get; set; } = new BindingList<Jump>();

        public Competition() { }
        public Competition(int _id, string _name, bool _gender, bool _syncro, double _height, DateTime _date, string _ageGroup)
        {
            id = _id;
            name = _name;
            gender = _gender;
            syncro = _syncro;
            height = _height;
            date = _date.Date; // .Date för att bara få datumet och ej klockslag
            ageGroup = _ageGroup;
        }

        public bool gotoNextJump()
        {
            currentContestantindex++;
            if (currentContestantindex > contestants.Count - 1)
            {
                currentContestantindex = 0;
                currentRound++;
                if (currentRound > rounds - 1)
                {
                    finished = true;
                    started = false;
                    currentRound = 0;
                    currentContestantindex = 0;
                    return true;
                }
            }
            return false;
        }

        public void sortContestants()
        {
            contestants = new BindingList<Contestant>(contestants.OrderByDescending(x => x.totalScore).ToList());
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("contestants"));
        }

        public void shuffleContestants()
        {
            //contestants = new BindingList<Contestant>(contestants.OrderBy());
        }

        #region db
        public void removeJudge()
        {
            if (selectedJudge == null) { return; }
            var db = new Database();
            var query = "DELETE FROM `competition-judge` WHERE `competition.id` = " + id + " AND `judge.id` = " + selectedJudge.id;
            db.set(query);
            judges.Remove(selectedJudge);
            for (var i = 0; i < judges.Count; i++)
            {
                if (judges[i].judgeSeat != i)
                {
                    judges[i].judgeSeat = i;
                    query = "UPDATE `competition-judge` SET judgeSeat = "+ judges[i] .judgeSeat+ " WHERE `competition.id` = " + id + " AND `judge.id` = " + judges[i].id;
                    db.set(query);
                }
            }
        }

        public void addJudge(Judge j)
        {
            if (judges.Any(n => n.id == j.id)) { return; }
            j.judgeSeat = judges.Count;
            var db = new Database();
            var quuery = "INSERT INTO `competition-judge` (`competition.id`, `judge.id`, `judgeSeat`) VALUES (" + id + ", " + j.id + ", "+ j.judgeSeat +")";
            db.set(quuery);
            judges.Add(j);
        }

        public void removeSelectedContestant()
        {
            if(selectedContestant == null) { return; }
            foreach (var jump in selectedContestant.jumps)
            {
                jump.deleteFromDataBase();
            }
            var db = new Database();
            var quuery = "DELETE FROM `competition-contestant` WHERE `competition.id` = " + id + " AND `contestant.id` = "+selectedContestant.id;
            db.set(quuery);
            contestants.Remove(selectedContestant);
        }

        public void addContestant(Contestant c)
        {
            if (contestants.Any(n => n.id == c.id)) { return; }
            var db = new Database();
            var quuery = "INSERT INTO `competition-contestant` (`competition.id`, `contestant.id`) VALUES (" + id + ", " + c.id + ")";
            db.set(quuery);

            for (var i = 0; i < rounds; i++)
            {
                c.jumps.Add(new Jump());
            }
            contestants.Add(c);
            if (contestants.Count == 1)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("currentContestant"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("currentJump"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("highscore"));
            }
        }

        public void loadFromDatabase()
        {
            var db = new Database();
            var result = db.get("SELECT * FROM `contestants` JOIN `competition-contestant` ON `contestants`.`id` = `competition-contestant`.`contestant.id` WHERE `competition.id` = " + id + " ORDER BY totalScore DESC, name ASC");
            contestants.Clear();

            foreach (var row in result)
            {
                var c = new Contestant
                {
                    id = int.Parse(row[0]),
                    name = row[1],
                    club = row[2],
                    nationality = row[3],
                    gender = row[4] == "True",
                    birthdate = Convert.ToDateTime(row[5]).Date,
                    totalScore = int.Parse(row[8])
                };
                contestants.Add(c);
            }
            jumpsInCurrentCompetition.Clear();
            foreach (var contestant in contestants)
            {
                result = db.get("SELECT * FROM `jumps` JOIN `contestant-jump` ON `jumps`.`id` = `contestant-jump`.`jump.id` WHERE `contestant.id` = " + contestant.id + " AND `competition.id` = " + id);

                foreach (var row in result)
                {
                    var j = new Jump
                    {
                        id = int.Parse(row[0]),
                        height = float.Parse(row[1]),
                        syncro = row[2] == "true",
                        jumpCode = row[3],
                        jumpScore = float.Parse(row[4])
                    };
                    j.difficulty = j.getJumpDifficulty();
                    contestant.jumps.Add(j);
                }
                while (contestant.jumps.Count < rounds)
                {
                    contestant.jumps.Add(new Jump());
                }

            }
            result = db.get("SELECT * FROM `judges` JOIN `competition-judge` ON `judges`.`id` = `competition-judge`.`judge.id` WHERE `competition.id` = " + id + " ORDER BY `competition-judge`.judgeSeat ASC");
            judges.Clear();

            foreach (var row in result)
            {
                var j = new Judge
                {
                    id = int.Parse(row[0]),
                    name = row[1],
                    nationality = row[2],
                    judgeSeat = int.Parse(row[5])
                };
                judges.Add(j);
            }
        }

        public void saveInfoToDb()
        {
            var db = new Database();
            var _id = (id == -1) ? "NULL" : id.ToString();
            var query = "INSERT INTO competitions (id, name, gender, syncro, height, date, ageGroup, rounds, currentContestantindex, currentRound, finished, started) " +
                        "VALUES (" + _id + ", '" + name + "', " + gender + ", " + syncro + ", " + height + ", '" + date.Date +
                        "', '" + ageGroup + "', " + rounds + ", " + currentContestantindex + ", " + currentRound + ", " + finished + ", "+started+" ) " +
                        "ON DUPLICATE KEY " +
                        "UPDATE name = '" + name + "', gender = " + gender + ", syncro = " + syncro + ", height = " + height + ", date = '" + date.Date + "', ageGroup = '" + ageGroup + "', rounds = "
                        + rounds + ", currentContestantindex = " + currentContestantindex + ", currentRound = " + currentRound + ", finished = " + finished + ", started = " + started;
            var lastInsertId = db.set(query);
            if (id == -1)
            {
                id = lastInsertId;
            }
        }

        public void saveToDatabase()
        {
            var db  = new Database();

            var _id = (id == -1) ? "NULL" : id.ToString();
            var query = "INSERT INTO competitions (id, name, gender, syncro, height, date, ageGroup, rounds, currentContestantindex, currentRound, finished, started) " +
                        "VALUES (" + _id + ", '" + name + "', " + gender + ", " + syncro + ", " + height + ", '" + date.Date +
                        "', '" + ageGroup + "', " + rounds + ", " + currentContestantindex + ", " + currentRound + ", "+ finished + ", "+started+") " +
                        "ON DUPLICATE KEY " +
                        "UPDATE name = '"+name+"', gender = "+ gender + ", syncro = "+syncro+", height = "+height+", date = '"+date.Date+"', ageGroup = '"+ageGroup+"', rounds = "
                        + rounds + ", currentContestantindex = " + currentContestantindex + ", currentRound = " + currentRound + ", finished = "+ finished + ", started = " + started;
            var lastInsertId = db.set(query);
            if (id == -1)
            {
                id = lastInsertId;
            }           

            foreach (var c in contestants)
            {
                _id = (c.id == -1) ? "NULL" : c.id.ToString();
                query = "INSERT INTO `contestants` (id, name, club, nationality, gender, birthdate) VALUES (" + _id + ", '" + c.name + "', '" + c.club + "', '" + c.nationality+"', "+c.gender+", '"+c.birthdate.Date+ "') " +
                        "ON DUPLICATE KEY UPDATE name = '" + c.name + "', club = '" + c.club + "', nationality = '" + c.nationality + "', gender = " + c.gender + ", birthdate = '" + c.birthdate.Date + "'";
                lastInsertId = db.set(query);
                if (c.id == -1)
                {
                    c.id = lastInsertId;
                }
                query = "INSERT INTO `competition-contestant` (`competition.id`, `contestant.id`, `totalScore`) VALUES (" + id+", "+c.id+ ", '"+c.totalScore+"') " +
                        "ON DUPLICATE KEY UPDATE `totalScore` = '"+c.totalScore+"'";
                db.set(query);

                foreach (var j in c.jumps)
                {
                    _id = (j.id == -1) ? "NULL" : id.ToString();
                    query = "INSERT INTO `jumps` (id, height, syncro, totalscore) VALUES (" + _id + ", " +j.height + ", "+j.syncro+", '"+j.jumpScore+ "') " +
                            "ON DUPLICATE KEY UPDATE height = "+j.height+", syncro = "+j.syncro+", totalscore = '"+j.jumpScore+"'";

                    lastInsertId = db.set(query);
                    if (j.id == -1)
                    {
                       j.id = lastInsertId;
                    }

                    query = "INSERT INTO `contestant-jump` (`contestant.id`, `jump.id`, `competition.id`) VALUES (" + c.id + ", " + j.id + ", "+id+") " +
                            "ON DUPLICATE KEY UPDATE `jump.id` = " + j.id + "";
                    db.set(query);
                }
            }

            foreach (var j in judges)
            {
                query = "INSERT INTO `competition-judge` (`competition.id`, `judge.id`, judgeSeat) VALUES (" + id + ", " + j.id + ", "+j.judgeSeat+") " +
                        "ON DUPLICATE KEY UPDATE judgeSeat = " + j.judgeSeat + "";
                db.set(query);
            }
        }

        public void deleteFromDatabase()
        {
            var db = new Database();
            var query = "DELETE FROM competitions WHERE id = " + id;
            db.set(query);
        }
        #endregion
    }
}
