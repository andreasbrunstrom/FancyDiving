using System;
using System.Collections;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
using System.Windows;
using log4net;
using log4net.Config;
using SharedClasses;

namespace AdminApp
{
    public class Admin : IAdmin
    {
        #region logging
        
        private static readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public static void loggingInit()
        {
            try
            {
                XmlConfigurator.Configure();
                var fi = new FileInfo(@"CaptainsLog.xml");
                if (fi.Exists)
                {
                    XmlConfigurator.Configure(fi);
                    logger.Info("File CaptainsLog.xml exists");
                }
                else
                {
                    BasicConfigurator.Configure();
                    logger.Error("Missing file CaptainsLog.xml");
                }
            }
            catch (Exception e)
            {
                BasicConfigurator.Configure();
                logger.Error("Missing file CaptainsLog.xml", e);
            }
        }

        #endregion

        public event DelegateShowMessageBox eventShowMessageBox;
        public BindingList<Contestant> allContestants { get; set; } = new BindingList<Contestant>();
        public BindingList<Judge> allJudges { get; set; } = new BindingList<Judge>();
        public BindingList<Competition> allCompetitions { get; set; } = new BindingList<Competition>();
        public static BindingList<string> allJumpCodes { get; set; } = new BindingList<string>();
        public Competition currentCompetition;
        public bool _autoGotoNext { get; set; }
        private Server tcpServer;

        private Hashtable clientsHashtable = new Hashtable();

        public void startTcpServer()
        {
            logger.Info("Trying to start TCP-Server");
            tcpServer = new Server();
            Server.eventIncommingDataCallback -= incommingData;
            Server.eventIncommingDataCallback += incommingData;
            Server.eventNewClientCallback     -= newJudgeClient;
            Server.eventNewClientCallback     += newJudgeClient;
            Server.eventDisconnectCallback    -= disconnectClient;
            Server.eventDisconnectCallback    += disconnectClient;
            tcpServer.startListening();
            logger.Info("TCP-Server started");
        }

        public void stopTcpServer()
        {
            sendExit();
            foreach (var judge in currentCompetition.judges)
            {
                judge.occupied = false;
            }
            tcpServer?.stopListening();
            tcpServer = null;
            logger.Info("TCP-Server Stopped");
        }

        public void disconnectClient(TcpClient tcpClient)
        {
            if (clientsHashtable[tcpClient] != null && currentCompetition != null)
            {
                var seat = (int)clientsHashtable[tcpClient];
                currentCompetition.judges[seat].occupied = false;
                clientsHashtable.Remove(seat);
            }
        }

        public void newJudgeClient(TcpClient tcpClient)
        {
            Server.sendData("judgelist", currentCompetition.judges, tcpClient);
            logger.Info("Judgelist sent to connected judge: ");
        }

        public void sendJump()
        {
            Server.sendDataToAll("jump", currentCompetition.currentJump);
            logger.Info("Jump sent to all");
        }

        public void sendExit()
        {
            Server.sendDataToAll("exit", "exit");
            logger.Info("exit sent to all");
        }

        public void incommingData(TcpClient tcpClient, string str)
        {
            Application.Current.Dispatcher.Invoke(delegate
            {
                try
                {
                    if (str != "")
                    {
                        var splitmessage = str.Split('@');

                        var cmd = splitmessage[0];
                        var data = splitmessage[1];

                        switch (cmd)
                        {
                            case "judge":
                                lock (currentCompetition.judges)
                                {
                                    var judge = SendReceive.deserializer<Judge>(data);
                                    var j = currentCompetition.judges.First(x => x.id == judge.id);
                                    if (j.occupied)
                                    {
                                        Server.sendData("occupied", currentCompetition.judges, tcpClient);
                                        logger.Error("Judge occupied and denied by server");
                                    }
                                    else
                                    {
                                        j.occupied = true;
                                        clientsHashtable.Add(tcpClient, j.judgeSeat);
                                        Server.sendData("ok", j.id.ToString(), tcpClient);
                                        logger.Info("Judge selection ok " + j.name);
                                        if (currentCompetition.judges.All(x => x.occupied))
                                        {
                                            if (currentCompetition.currentJump.scores.All(x => x.points == -1))
                                            {
                                                Server.sendDataToAll("jump", currentCompetition.currentJump);
                                            }
                                            else
                                            {
                                                Server.sendData("jump", currentCompetition.currentJump,tcpClient);
                                            }
                                        }
                                    }
                                }
                                break;

                            case "score":
                                lock (currentCompetition)
                                {
                                    var score = SendReceive.deserializer<Score>(data);
                                    currentCompetition.currentJump.scores[score.judgeSeat].points = score.points;
                                    currentCompetition.currentJump.scores[score.judgeSeat].judgeSeat = score.judgeSeat;
                                    currentCompetition.currentJump.scores[score.judgeSeat].judgeId = score.judgeId;
                                    logger.Info("Judge id: " + score.judgeId + " On seat: " + score.judgeSeat +
                                                " Sent score: " + score.points);

                                    if (currentCompetition.currentJump.scores.TrueForAll(x => x.points != -1))
                                    {
                                        currentCompetition.currentJump.calculateJumpScore();
                                        currentCompetition.currentContestant.calculateTotalScore();

                                        if (_autoGotoNext)
                                        {
                                            gotoNextJump(currentCompetition);
                                            logger.Info("Auto Going to next jump");
                                        }
                                    }
                                }
                                break;
                        }
                    }
                    else
                    {
                        Server.removeClient(tcpClient);
                    }
                }
                catch (Exception e)
                {
                    logger.Error("Error in function IncomingData: "+e.Message);
                    logger.Info(str);
                }
            });
        }

        #region Interface

        public void autoGotoNext(bool val)
        {
            _autoGotoNext = val;
            if (val == true)
            {
                if (currentCompetition.currentJump.scores.TrueForAll(x => x.points != -1))
                {
                    gotoNextJump(currentCompetition);
                }
            }
        }

        #region fetch

        public void fetchContestants()
        {
            var db = new Database();
            var result = db.get("SELECT * FROM contestants ORDER BY name ASC");

            foreach (var row in result)
            {
                var c = new Contestant
                {
                    id = int.Parse(row[0]),
                    name = row[1],
                    club = row[2],
                    nationality = row[3],
                    gender = row[4] == "True",
                    birthdate = Convert.ToDateTime(row[5]).Date
                };
                allContestants.Add(c);
            }
        }

        public void fetchCompetitions()
        {
            var db = new Database();
            var result = db.get("SELECT * FROM competitions ORDER BY finished ASC, date DESC");

            foreach (var row in result)
            {
                var c = new Competition
                {
                    id = int.Parse(row[0]),
                    name = row[1],
                    gender = row[2] == "True",
                    syncro = row[3] == "True",
                    height = float.Parse(row[4]),
                    date = Convert.ToDateTime(row[5]).Date,
                    ageGroup = row[6],
                    rounds = int.Parse(row[7]),
                    currentContestantindex = int.Parse(row[8]),
                    currentRound = int.Parse(row[9]),
                    finished = row[10] == "True"
                };
                allCompetitions.Add(c);
            }
        }

        public void fetchJudges()
        {
            var db = new Database();
            var result = db.get("SELECT * FROM judges ORDER BY name ASC");

            foreach (var row in result)
            {
                var j = new Judge
                {
                    id = int.Parse(row[0]),
                    name = row[1],
                    nationality = row[2]
                };
                allJudges.Add(j);
            }
        }

        public void fetchJumpCodes()
        {
            var db = new Database();
            var result = db.get("SELECT DISTINCT id FROM `jumpcodes`");
            foreach (var code in result)
            {
                allJumpCodes.Add(code[0]);
            }
        }
        #endregion

        #region competition

        public void stopCompetition()
        {
            if(currentCompetition == null) return;
            currentCompetition.started = false;
            currentCompetition.saveToDatabase();
            currentCompetition.sortContestants();
            stopTcpServer();
            currentCompetition = null;
            logger.Info("Competition stoped");
        }

        public void startCompetition(Competition c)
        {
            if (currentCompetition != null)
            {
                eventShowMessageBox?.Invoke("Tävlingen '"+currentCompetition.name+"' körs redan, avsluta den först.");
                return;
            }
            if (c.contestants.Count < 1)
            {
                eventShowMessageBox?.Invoke("Lägg till deltagare!");
                return;
            }
            if((c.syncro && !new[] {9,11}.Contains(c.judges.Count)) || (!c.syncro && !new[] {3,5,7}.Contains(c.judges.Count)))
            {
                eventShowMessageBox?.Invoke("Fel antal dommare!");
                return;
            }

            foreach (var contestant in c.contestants)
            {
                if (!contestant.jumps.TrueForAll(x => x.jumpCode != "" && x.height != 0))
                {
                    eventShowMessageBox?.Invoke("Deltagaren '"+contestant.name+"' saknar hoppkod eller höjd för ett av sina hopp.");
                    return;
                }
            }

            c.started = true;
            c.jumpsInCurrentCompetition.Clear();

            for (var i = 0; i < c.rounds; i++)
            {
                foreach (var contestant in c.contestants)
                {
                    var jump = contestant.jumps[i];
                    for (var j = jump.scores.Count; j < c.judges.Count; j++)

                    {
                        jump.scores.Add(new Score());
                    }

                    c.jumpsInCurrentCompetition.Add(jump);
                }
            }

            currentCompetition = c;
            currentCompetition.saveInfoToDb();
            startTcpServer();
            logger.Info("Competition started");                    
        }

        public void changeRoundCount(Competition c, int val)
        {
            c.rounds += val;
            if (c.rounds < 1)
            {
                c.rounds = 1;
            }
        }

        public void gotoNextJump(Competition c)
        {
            if (!c.currentJump.scores.TrueForAll(x => x.points != -1))
            {
                eventShowMessageBox?.Invoke("Alla dommare har inte gett poäng.");
                return;
            }

            c.currentJump.saveToDatabase(c.currentContestant.id, c.id, c.currentContestant.totalScore);
            if (c.gotoNextJump())
            {
                stopCompetition();
            }
            else
            {
                sendJump();
                c.saveInfoToDb();
            }
        }

        public void newCompetition()
        {
            allCompetitions.AddNew()?.saveToDatabase();
        }

        public void editCompetition(Competition c)
        {
            throw new NotImplementedException();
        }

        public void deleteCompetition(Competition c)
        {
            if(c == null) return;
            c.deleteFromDatabase();
            allCompetitions.Remove(c);
        }
        #endregion

        #region contestant

        public void newContestant(Contestant c)
        {
            allContestants.Add(c);
            c.saveToDatabase();
        }

        public void editContestant(Contestant c)
        {
            c.saveToDatabase();
        }

        public void deleteContestant(Contestant c)
        {
            if (c == null) return;
            
            c.deleteFromDatabase();
            allContestants.Remove(c);
        }

        public void addContestant(Contestant c, Competition comp)
        {
            comp.addContestant(c);
        }

        public void removeSelectedContestant(Competition c)
        {
            c.removeSelectedContestant();
        }
        #endregion

        #region judge

        public void newJudge(Judge j)
        {
            allJudges.Add(j);
            j.saveToDatabase();
        }

        public void editJudge(Judge j)
        {
            j.saveToDatabase();
        }

        public void deleteJudge(Judge j)
        {
            j.deleteFromDatabase();
            allJudges.Remove(j);
        }

        public void addJudge(Judge j, Competition c)
        {
            c.addJudge(j);
        }

        public void removeSelectedJudge(Competition c)
        {
            c.removeJudge();
        }
        #endregion

        #region jump

        public void newJump()
        {
            throw new NotImplementedException();
        }

        public void editJump(Jump j, Competition c)
        {
            c.selectedJump.height = j.height;
            c.selectedJump.jumpCode = j.jumpCode;
            c.selectedJump.difficulty =  c.selectedJump.getJumpDifficulty();
            c.selectedJump.saveToDatabase(c.selectedContestant.id, c.id, c.selectedContestant.totalScore);
        }

        public void deleteJump(Jump j)
        {
            throw new NotImplementedException();
        }
        #endregion

        #endregion
    }

}
