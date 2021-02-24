using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JudgeApp.Model;
using SharedClasses;

namespace JudgeApp.View
{
    /// <summary>
    /// This is the interface for the JudgeMainWindow, all delegates and events is declared.
    /// </summary>
    /// <param name="ipAdress"></param>
    
    public delegate void DelegateConnectToAdmin(string ipAdress);
    public delegate void DelegateSelectJudge();
    public delegate void DelegateJudgeSelectionChanged(int index);
    public delegate void DelegateSendScore(string score);
    public delegate void DelegateJudgeExit();

    public interface IJudgeMainWindow
    {
        void setJudgeList(BindingList<Judge> judgeList);
        void goToWait();
        void goToScore();
        void goToInit();
        void goToSelect();
        void goToExit();
        void showMessageBox(string message);

        event DelegateConnectToAdmin eventConnectToAdmin;
        event DelegateSelectJudge eventSelectJudge;
        event DelegateJudgeSelectionChanged eventJudgeSelectionChanged;
        event DelegateSendScore eventSendScore;
        event DelegateJudgeExit eventJudgeExit;
    }
}
