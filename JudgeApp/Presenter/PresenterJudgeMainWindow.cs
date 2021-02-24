using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using JudgeApp.Model;
using JudgeApp.View;
using SharedClasses;
using JudgeModel = JudgeApp.Model.JudgeModel;

namespace JudgeApp.Presenter
{
    /// <summary>
    /// Presenter class for the JudgeApp, and the midle hand for the communication between view and model.
    /// </summary>
    class PresenterJudgeMainWindow
    {
        #region Prop

        public IJudgeMainWindow view { get; set; }
        public IJudgeModel model { get; set; }

        #endregion

        #region Constr

        public PresenterJudgeMainWindow(IJudgeMainWindow _view, JudgeModel _model)
        {
            model = _model;
            view = _view;
           
            view.setJudgeList(_model.judgeList);

            view.eventSendScore += sendScore;
            view.eventSelectJudge += selectJudge;
            view.eventConnectToAdmin += connectToAdmin;
            view.eventJudgeSelectionChanged += selectionChanged;
            view.eventJudgeExit += judgeExit;

            model.eventShowMessageBox += showMessageBox;
            model.eventStateChanged += stateChanged;
        }

        #endregion

        #region Functions
        public void connectToAdmin(string ipAdress)
        {
            model.connectToAdmin(ipAdress);
        }

        public void selectionChanged(int index)
        {
            model.selectionChanged(index);
        }

        public void selectJudge()
        {
            model.sendSelectedJudge();
        }

        public void sendScore(string score)
        {
            model.sendScore(score);
        }

        public void showMessageBox(string message)
        {
            view.showMessageBox(message);
        }

        public void stateChanged(string state)
        {
            switch(state)
            {
                case "init":
                    view.goToInit();
                    break;
                case "select":
                    view.goToSelect();
                    break;
                case "points":
                    view.goToScore();
                    break;
                case "wait":
                    view.goToWait();
                    break;
                case "exit":
                    view.goToExit();
                    break;
            }
        }

        public void judgeExit()
        {
            model.judgeExit();
        }
        #endregion
    }
}
