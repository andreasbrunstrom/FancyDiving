using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using AdminApp.View;
using SharedClasses;

namespace AdminApp.Presenter
{
    class PresenterAdminMainWindow
    {
        #region Properties
        public IAdminMainWindow _view { get; set; }
        public IAdmin _model { get; set; }
        #endregion

        #region Constructor
        public PresenterAdminMainWindow(IAdminMainWindow view, Admin ad)
        {
            _model = ad;
            _view = view;

            _view.eventAutoGotoNext         += ad.autoGotoNext;
            
            _view.setCompetitionsList(ad.allCompetitions);
            _view.setContestantsList(ad.allContestants);
            _view.setJudgesList(ad.allJudges);

            _view.eventFetchCompetitions    += ad.fetchCompetitions;
            _view.eventFetchContestants     += ad.fetchContestants;
            _view.eventFetchJudges          += ad.fetchJudges;
            _view.eventFetchJumpCodes       += ad.fetchJumpCodes;

            _view.eventStartCompetition     += startCompetition;
            _view.eventStopCompetition      += stopCompetition;
            _view.eventChangeRoundCount     += changeRoundCount;
            _view.eventGotoNextJump         += gotoNextJump;
            _view.eventNewCompetition       += newCompetition;
            _view.eventEditCompetition      += editCompetition;
            _view.eventDeleteCompetition    += deleteCompetition;

            _view.eventNewContestant        += newContestant;
            _view.eventEditContestant       += editContestant;
            _view.eventDeleteContestant     += deleteContestant;
            _view.eventAddContestant        += addContestant;
            _view.eventRemoveSelectedContestant     += removeContestant;

            _view.eventNewJudge             += newJudge;
            _view.eventEditJudge            += editJudge;
            _view.eventDeleteJudge          += deleteJudge;
            _view.eventAddJudge             += addJudge;
            _view.eventRemoveSelectedJudge  += removeSelectedJudge;

            _view.eventNewJump              += newJump;
            _view.eventEditJump             += editJump;
            _view.eventDeleteJump           += deleteJump;

            _model.eventShowMessageBox += showMessageBox;
        }
        #endregion

        #region Admin connections
        public void fetchCompetitions()
        {
            _model.fetchCompetitions();
        }

        public void fetchContestants()
        {
            _model.fetchContestants();
        }

        public void fetchJudges()
        {
            _model.fetchJudges();
        }

        public void fetchJumpCodes()
        {
            _model.fetchJumpCodes();
        }
        public void showMessageBox(string message)
        {
            _view.showMessageBox(message);
        }

        #endregion

        #region Competition

        public void startCompetition(Competition c)
        {
            _model.startCompetition(c);
        }

        public void stopCompetition()
        {
            _model.stopCompetition();
        }

        public void changeRoundCount(Competition c, int val)
        {
            _model.changeRoundCount(c, val);
        }

        public void gotoNextJump(Competition c)
        {
            _model.gotoNextJump(c);
        }

        public void newCompetition()
        {
            _model.newCompetition();
        }

        public void editCompetition(Competition c)
        {
            _model.editCompetition(c);
        }

        public void deleteCompetition(Competition c)
        {
            _model.deleteCompetition(c);
        }
        #endregion
        
        #region Contestant
        public void newContestant(Contestant c)
        {
            _model.newContestant(c);
        }

        public void editContestant(Contestant c)
        {
            _model.editContestant(c);
        }

        public void deleteContestant(Contestant c)
        {
            _model.deleteContestant(c);
        }

        public void addContestant(Contestant c, Competition co)
        {
            _model.addContestant(c, co);
        }

        public void removeContestant(Competition c)
        {
            _model.removeSelectedContestant(c);
        }
        #endregion

        #region Judge
        public void newJudge(Judge j)
        {
            _model.newJudge(j);
        }

        public void editJudge(Judge j)
        {
            _model.editJudge(j);
        }

        public void deleteJudge(Judge j)
        {
            _model.deleteJudge(j);
        }

        public void addJudge(Judge j, Competition c)
        {
            _model.addJudge(j, c);
        }

        public void removeSelectedJudge(Competition c)
        {
            _model.removeSelectedJudge(c);
        }
        #endregion

        #region Jumps
        public void newJump()
        {
            _model.newJump();
        }

        public void editJump(Jump j, Competition c)
        {
            _model.editJump(j, c);
        }

        public void deleteJump(Jump j)
        {
            _model.deleteJump(j);
        }
        #endregion
    }  
}
