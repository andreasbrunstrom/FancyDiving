using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharedClasses;

namespace AdminApp
{

    public delegate void DelegateShowMessageBox(string message);

    public interface IAdmin
    {
        void autoGotoNext(bool val);

        void fetchCompetitions();
        void fetchContestants();
        void fetchJudges();
        void fetchJumpCodes();

        void startCompetition(Competition c);
        void stopCompetition();
        void changeRoundCount(Competition c, int val);
        void gotoNextJump(Competition c);
        void newCompetition();
        void editCompetition(Competition c);
        void deleteCompetition(Competition c);

        void newContestant(Contestant c);
        void editContestant(Contestant c);
        void deleteContestant(Contestant c);
        void addContestant(Contestant c, Competition co);
        void removeSelectedContestant(Competition c);

        void newJudge(Judge j);
        void editJudge(Judge j);
        void deleteJudge(Judge j);
        void addJudge(Judge j, Competition c);
        void removeSelectedJudge(Competition c);

        void newJump();
        void editJump(Jump j, Competition c);
        void deleteJump(Jump j);

        event DelegateShowMessageBox eventShowMessageBox;
    }
}
