using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AdminApp;
using SharedClasses;

namespace TestAdminClass
{
    [TestClass]
    public class TestAdminClass
    {
        [TestMethod]
        public void TestConnection()
        {

            var test = new Database();
           
                Assert.AreEqual(true, test.openConnection());
                Assert.IsFalse(!test.closeConnection());
                                

        }
        [TestMethod]
        public void Test_AdminEmptyContestant()
        {
            var adm = new Admin();


            Assert.AreEqual(adm.allContestants.Count, 0);
            Assert.AreNotEqual(adm.allContestants.Count,1);
        }
        [TestMethod]
        public void Test_AdminEmptyJudges()
        {
            var adm = new Admin();


            Assert.AreEqual(adm.allJudges.Count, 0);
            Assert.AreNotEqual(adm.allJudges.Count, 1);
        }
        [TestMethod]
        public void Test_AdminEmptyCompetitions()
        {
            var adm = new Admin();


            Assert.AreEqual(adm.allCompetitions.Count, 0);
            Assert.AreNotEqual(adm.allCompetitions.Count, 1);
        }
        [TestMethod]
        public void Test_AdminAddContestant()
        {
            var adm = new Admin();
            var contestant1 = new Contestant(1, "Dilbert", "", "USA", true, DateTime.Now);
            adm.allContestants.Add(contestant1);

            Assert.AreNotEqual(adm.allContestants.Count, 0);
            Assert.AreEqual(adm.allContestants.Count, 1);

            var contestant2 = new Contestant(2, "Mort", "", "USA", true, DateTime.Now);
            adm.allContestants.Add(contestant2);

            Assert.AreNotEqual(adm.allContestants.Count, 0);
            Assert.AreEqual(adm.allContestants.Count, 2);
        }
        [TestMethod]
        public void Test_AdminAddJudges()
        {
            var adm = new Admin();
            var judge1 = new Judge(1, 1, "Minsk", "USA");
            adm.allJudges.Add(judge1);

            Assert.AreNotEqual(adm.allJudges.Count, 0);
            Assert.AreEqual(adm.allJudges.Count, 1);

            var judge2 = new Judge(2, 2, "Boo", "USA");
            adm.allJudges.Add(judge2);

            Assert.AreNotEqual(adm.allJudges.Count, 0);
            Assert.AreEqual(adm.allJudges.Count, 2);
        }
        [TestMethod]
        public void Test_AdminAddCompetitions()
        {
            var adm = new Admin();
            var competition1 = new Competition(1, "Tävling1", true, false, 3.0, DateTime.Now, "Junior");
            adm.allCompetitions.Add(competition1);

            Assert.AreNotEqual(adm.allCompetitions.Count, 0);
            Assert.AreEqual(adm.allCompetitions.Count, 1);

            var competition2 = new Competition(1, "Tävling2", true, false, 1.0, DateTime.Now, "Junior");
            adm.allCompetitions.Add(competition2);

            Assert.AreNotEqual(adm.allCompetitions.Count, 0);
            Assert.AreEqual(adm.allCompetitions.Count, 2);
        }
    }
}
