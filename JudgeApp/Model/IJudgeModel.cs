using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharedClasses;

namespace JudgeApp.Model
{
    /// <summary>
    /// Interface for the JudgeApp model. 
    /// </summary>

    public delegate void DelegateShowMessageBox(string message);
    public delegate void DelegateStateChanged(string state);

    public interface IJudgeModel
    {
        void sendScore(string score);
        void sendSelectedJudge();
        void connectToAdmin(string ipAddress);
        void selectionChanged(int index);
        void judgeExit();

        event DelegateStateChanged eventStateChanged;
        event DelegateShowMessageBox eventShowMessageBox;
    }
}
