using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using SharedClasses;

namespace AdminApp.View
{
    public delegate void DelegateAutoGotoNext(bool val);

    public delegate void DelegateFetchCompetitions();
    public delegate void DelegateFetchContestants();
    public delegate void DelegateFetchJudges();
    public delegate void DelegateFetchJumpCodes();

    public delegate void DelegateStartCompetition(Competition c);
    public delegate void DelegateStopCompetition();
    public delegate void DelegateChangeRoundCount(Competition c, int val);
    public delegate void DelegateGotoNextJump(Competition c);
    public delegate void DelegateNewCompetition();
    public delegate void DelegateEditCompetition(Competition c);
    public delegate void DelegateDeleteCompetition(Competition c);

    public delegate void DelegateNewContestant(Contestant c);
    public delegate void DelegateEditContestant(Contestant c);
    public delegate void DelegateDeleteContestant(Contestant c);
    public delegate void DelegateAddContestant(Contestant c, Competition co);
    public delegate void DelegateRemoveContestant(Competition c);
    
    public delegate void DelegateNewJudge(Judge j);
    public delegate void DelegateEditJudge(Judge j);
    public delegate void DelegateDeleteJudge(Judge j);
    public delegate void DelegateAddJudge(Judge j , Competition c);
    public delegate void DelegateRemoveJudge(Competition c);

    public delegate void DelegateNewJump();
    public delegate void DelegateEditJump(Jump j, Competition c);
    public delegate void DelegateDeleteJump(Jump j);

    public interface IAdminMainWindow
    {
        event DelegateAutoGotoNext eventAutoGotoNext;

        void setCompetitionsList(BindingList<Competition> competitions);
        void setContestantsList(BindingList<Contestant> contestants);
        void setJudgesList(BindingList<Judge> judges);

        event DelegateFetchCompetitions eventFetchCompetitions;
        event DelegateFetchContestants  eventFetchContestants;
        event DelegateFetchJudges       eventFetchJudges;
        event DelegateFetchJumpCodes    eventFetchJumpCodes;

        event DelegateStartCompetition  eventStartCompetition;
        event DelegateStopCompetition   eventStopCompetition;
        event DelegateChangeRoundCount  eventChangeRoundCount;
        event DelegateGotoNextJump      eventGotoNextJump;
        event DelegateNewCompetition    eventNewCompetition;
        event DelegateEditCompetition   eventEditCompetition;
        event DelegateDeleteCompetition eventDeleteCompetition;

        event DelegateNewContestant     eventNewContestant;
        event DelegateEditContestant    eventEditContestant;
        event DelegateDeleteContestant  eventDeleteContestant;
        event DelegateAddContestant     eventAddContestant;
        event DelegateRemoveContestant  eventRemoveSelectedContestant;

        event DelegateNewJudge          eventNewJudge;
        event DelegateEditJudge         eventEditJudge;
        event DelegateDeleteJudge       eventDeleteJudge;
        event DelegateAddJudge          eventAddJudge;
        event DelegateRemoveJudge       eventRemoveSelectedJudge;

        event DelegateNewJump           eventNewJump;
        event DelegateEditJump          eventEditJump;
        event DelegateDeleteJump        eventDeleteJump;

        void showMessageBox(string message, string header = "");
    }
}
