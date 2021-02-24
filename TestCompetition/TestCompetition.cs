using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharedClasses;



namespace TestCompetition
{
    [TestClass]
    public class TestCompetition
    {

        private Competition c = new Competition(1, "ÖSA", false, true, 3, new DateTime(2017 - 01 - 03), "junior");


        [TestMethod]
        public void setUp()
        {
            
        }
        [TestMethod]
        public void addJudges()
        {
            
            Judge judge1 = new Judge(10, 1, "Anton", "SWE");
            Judge judge2 = new Judge(20, 2, "Olle", "SWE");
            Judge judge3 = new Judge(30, 3, "Kalle", "SWE");
            Judge judge4 = new Judge(40, 4, "Nils", "SWE");
            Judge judge5 = new Judge(50, 5, "Sven", "SWE");
            Judge judge6 = new Judge(60, 6, "Maj-Britt", "SWE");
            Judge judge7 = new Judge(70, 7, "Inga-lill", "SWE");
            Judge judge8 = new Judge(80, 8, "Klas-Göran", "SWE");
            Judge judge9 = new Judge(90, 9, "Klas-Bertil", "SWE");

            c.judges.Add(judge1);
            c.judges.Add(judge2);
            c.judges.Add(judge3);
            c.judges.Add(judge4);
            c.judges.Add(judge5);
            c.judges.Add(judge6);
            c.judges.Add(judge7);
            c.judges.Add(judge8);
            c.judges.Add(judge9);

            Assert.AreEqual(4, judge4.judgeSeat);
            Assert.AreEqual("Klas-Göran", judge8.name);
            Assert.AreEqual(9, c.judges.Count);
            Assert.AreEqual("SWE", judge7.nationality);
            Assert.AreNotEqual("wut", judge7.name);
        }
        [TestMethod]
        public void addContestants()
        {


            Contestant contestant1 = new Contestant(1, "Anton", "", "Sweden", true, new DateTime(1988, 01, 03));
            Contestant contestant2 = new Contestant(1, "Andreas", "", "Norge", true, new DateTime(1978, 01, 03));
            Contestant contestant3 = new Contestant(1, "Johan", "", "Danmark", true, new DateTime(1985, 01, 03));
            DateTime date = new DateTime(1988, 01, 03).Date;
            c.contestants.Add(contestant1);
            c.contestants.Add(contestant2);
            c.contestants.Add(contestant3);

            Assert.AreEqual("Anton", contestant1.name);
            Assert.AreEqual(true, contestant2.gender);
            Assert.AreEqual(date, contestant1.birthdate);
        }

        [TestMethod]
        public void addScore_calculateScoreFor_3_5_and_7_judges()
        {
            Contestant contestant1 = new Contestant(1, "Anton", "", "Sweden", true, new DateTime(1988, 01, 03));
            Jump jump1 = new Jump(0, 3, false, "100D");
            Jump jump2 = new Jump(0, 3, false, "100C");
            Score score1 = new Score(10, 1, 1);
            Score score2 = new Score(10, 2, 2);
            Score score3 = new Score(10, 3, 3);
            Score score4 = new Score(10, 4, 4);
            Score score5 = new Score(10, 5, 5);
            Score score6 = new Score(10, 6, 6);
            Score score7 = new Score(10, 7, 7);

            c.contestants.Add(contestant1);
            c.contestants[0].jumps.Add(jump1);

            c.contestants[0].jumps[0].scores.Add(score1);
            c.contestants[0].jumps[0].scores.Add(score2);
            c.contestants[0].jumps[0].scores.Add(score3);
            c.contestants[0].jumps[0].scores.Add(score4);
            c.contestants[0].jumps[0].scores.Add(score5);
            c.contestants[0].jumps[0].scores.Add(score6);
            c.contestants[0].jumps[0].scores.Add(score7);

            Assert.AreEqual(c.contestants[0].jumps.Count, 1);
            Assert.AreEqual(12, c.contestants[0].jumps[0].calculateJumpScore());

            c.contestants[0].calculateTotalScore();
            Assert.AreEqual(12, c.contestants[0].totalScore);

            c.contestants[0].jumps.Add(jump2);

            c.contestants[0].jumps[1].scores.Add(score1);
            c.contestants[0].jumps[1].scores.Add(score2);
            c.contestants[0].jumps[1].scores.Add(score3);
            c.contestants[0].jumps[1].scores.Add(score4);
            c.contestants[0].jumps[1].scores.Add(score5);
            c.contestants[0].jumps[1].scores.Add(score6);
            c.contestants[0].jumps[1].scores.Add(score7);

            c.contestants[0].jumps[1].calculateJumpScore();
            c.contestants[0].calculateTotalScore();
            Assert.AreEqual(24, c.contestants[0].totalScore);
        }

        [TestMethod]
        public void addScore_calculateScoreFor_9_Judges()
        {
            Contestant contestant1 = new Contestant(1, "Anton", "", "Sweden", true, new DateTime(1988, 01, 03));
            Jump jump1 = new Jump(0, 3, true, "100D");
            Score score1 = new Score(10, 1, 7.5);
            Score score2 = new Score(10, 2, 7.5);
            Score score3 = new Score(10, 3, 7.5);
            Score score4 = new Score(10, 4, 7.5);
            Score score5 = new Score(10, 5, 7.5);
            Score score6 = new Score(10, 6, 7.5);
            Score score7 = new Score(10, 7, 7.5);
            Score score8 = new Score(10, 8, 7.5);
            Score score9 = new Score(10, 9, 7.5);

            c.contestants.Add(contestant1);
            c.contestants[0].jumps.Add(jump1);

            c.contestants[0].jumps[0].scores.Add(score1);
            c.contestants[0].jumps[0].scores.Add(score2);
            c.contestants[0].jumps[0].scores.Add(score3);
            c.contestants[0].jumps[0].scores.Add(score4);
            c.contestants[0].jumps[0].scores.Add(score5);
            c.contestants[0].jumps[0].scores.Add(score6);
            c.contestants[0].jumps[0].scores.Add(score7);
            c.contestants[0].jumps[0].scores.Add(score8);
            c.contestants[0].jumps[0].scores.Add(score9);


            Assert.AreEqual(c.contestants[0].jumps.Count, 1);
            Assert.AreEqual(22.5, c.contestants[0].jumps[0].calculateJumpScore());

            c.contestants[0].calculateTotalScore();
            
            Assert.AreEqual(22.5, c.contestants[0].totalScore);
        }

        [TestMethod]
        public void addScore_calculateScoreFor_11_Judges()
        {
            Contestant contestant1 = new Contestant(1, "Anton", "", "Sweden", true, new DateTime(1988, 01, 03));
            Jump jump1 = new Jump(0, 3, true, "101A");
            Score score1 = new Score(10, 1, 7.5);
            Score score2 = new Score(10, 2, 7.5);
            Score score3 = new Score(10, 3, 7.5);
            Score score4 = new Score(10, 4, 7.5);
            Score score5 = new Score(10, 5, 7.5);
            Score score6 = new Score(10, 6, 7.5);
            Score score7 = new Score(10, 7, 7.5);
            Score score8 = new Score(10, 8, 7.5);
            Score score9 = new Score(10, 9, 7.5);
            Score score10 = new Score(10, 10, 7.5);
            Score score11 = new Score(10, 11, 7.5);

            c.contestants.Add(contestant1);
            c.contestants[0].jumps.Add(jump1);

            c.contestants[0].jumps[0].scores.Add(score1);
            c.contestants[0].jumps[0].scores.Add(score2);
            c.contestants[0].jumps[0].scores.Add(score3);
            c.contestants[0].jumps[0].scores.Add(score4);
            c.contestants[0].jumps[0].scores.Add(score5);
            c.contestants[0].jumps[0].scores.Add(score6);
            c.contestants[0].jumps[0].scores.Add(score7);
            c.contestants[0].jumps[0].scores.Add(score8);
            c.contestants[0].jumps[0].scores.Add(score9);
            c.contestants[0].jumps[0].scores.Add(score10);
            c.contestants[0].jumps[0].scores.Add(score11);


            Assert.AreEqual(c.contestants[0].jumps.Count, 1);
            Assert.AreEqual(36, c.contestants[0].jumps[0].calculateJumpScore());

            c.contestants[0].calculateTotalScore();

            Assert.AreEqual(36, c.contestants[0].totalScore);
        }

        [TestMethod]
        public void getDifficultyFromDatabase()
        {
            Contestant contestant1 = new Contestant(1, "Anton", "", "Sweden", true, new DateTime(1988, 01, 03));
            
            Jump jump1 = new Jump(0, 3, false, "101B");
                        
            Score score1 = new Score(10, 1, 7.5);
            Score score2 = new Score(10, 2, 7.5);
            Score score3 = new Score(10, 3, 7.5);
            Score score4 = new Score(10, 4, 7.5);
            Score score5 = new Score(10, 5, 7.5);
            Score score6 = new Score(10, 6, 7.5);
            Score score7 = new Score(10, 7, 7.5);

            c.contestants.Add(contestant1);
            c.contestants[0].jumps.Add(jump1);

            c.contestants[0].jumps[0].scores.Add(score1);
            c.contestants[0].jumps[0].scores.Add(score2);
            c.contestants[0].jumps[0].scores.Add(score3);
            c.contestants[0].jumps[0].scores.Add(score4);
            c.contestants[0].jumps[0].scores.Add(score5);
            c.contestants[0].jumps[0].scores.Add(score6);
            c.contestants[0].jumps[0].scores.Add(score7);

            Assert.AreEqual(33.75, c.contestants[0].jumps[0].calculateJumpScore());
        }

        [TestMethod]
        [ExpectedException(typeof(System.Exception), "Invalid point input")]
        public void testPointException()
        {
            Contestant contestant1 = new Contestant(1, "Anton", "", "Sweden", true, new DateTime(1988, 01, 03));

            Jump jump1 = new Jump(0, 3, false, "101B");

            Score score1 = new Score(10, 1, 7.5);
            Score score2 = new Score(10, 2, 7.5);
            Score score3 = new Score(10, 3, 7.5);
            Score score4 = new Score(10, 4, 7.5);
            Score score5 = new Score(10, 5, 7.5);
            Score score6 = new Score(10, 6, 7.5);
            Score score7 = new Score(10, 7, 7.6);

            c.contestants.Add(contestant1);
            c.contestants[0].jumps.Add(jump1);

            c.contestants[0].jumps[0].scores.Add(score1);
            c.contestants[0].jumps[0].scores.Add(score2);
            c.contestants[0].jumps[0].scores.Add(score3);
            c.contestants[0].jumps[0].scores.Add(score4);
            c.contestants[0].jumps[0].scores.Add(score5);
            c.contestants[0].jumps[0].scores.Add(score6);
            c.contestants[0].jumps[0].scores.Add(score7);

            Assert.AreEqual(33.75, c.contestants[0].jumps[0].calculateJumpScore());
        }

        //[TestMethod]
        public void loopCompetition()
        {

            c.id = -1;
            c.rounds = 3;
            var judgeCount = 5;
            for (int i = 0; i < 10; i++)
            {
                var contestant = new Contestant(i+1, "Anton"+i, "", "Sweden", (i % 2) == 0, new DateTime(1980+i, 01, 03));
                for (int j = 0; j < c.rounds; j++)
                {
                    var jump = new Jump(-1, 1+j);
                    contestant.jumps.Add(jump);
                }
                c.contestants.Add(contestant);
            }

            c.saveToDatabase();
            //start av testet
            while (true)
            {
                if (c.currentContestantindex < c.contestants.Count)
                {
                    for (int i = 0; i < judgeCount; i++)
                    {
                        var score = new Score(i, i, 2*i);
                        c.contestants[c.currentContestantindex].jumps[c.currentRound].scores.Add(score);
                    }
                    c.contestants[c.currentContestantindex].jumps[c.currentRound].calculateJumpScore();
                    c.contestants[c.currentContestantindex].calculateTotalScore();
                    c.currentContestantindex++;
                }
                else if (c.rounds < c.currentRound)
                {
                    c.currentContestantindex = 0;
                    c.currentRound++;
                }
                else
                {
                    break;
                }
            }
        }
    }
}