using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SharedClasses;
using System.Net.Sockets;
using System.Windows;
using log4net;

namespace JudgeApp.Model
{
    /// <summary>
    /// JudgeApp model, this is where the logic and functionality originates.
    /// </summary>

    public class JudgeModel : SendReceive, IJudgeModel, INotifyPropertyChanged
    {
        #region logging
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static void judgeLoggingInit()
        {
            try
            {
                log4net.Config.XmlConfigurator.Configure();
                var fi = new FileInfo(@"StarLog.xml");
                if (fi.Exists)
                {
                    log4net.Config.XmlConfigurator.Configure(fi);
                    logger.Info("File StarLog.xml exists");
                }
                else
                {
                    log4net.Config.BasicConfigurator.Configure();
                    logger.Error("Missing file StarLog.xml");
                }
            }
            catch (Exception e)
            {
                log4net.Config.BasicConfigurator.Configure();
                logger.Error("Missing file StarLog.xml", e);
            }
        }
        #endregion

        public event PropertyChangedEventHandler PropertyChanged;
        public event DelegateShowMessageBox eventShowMessageBox;
        public event DelegateStateChanged eventStateChanged;
        
        #region state
        public enum State
        {
            init,
            select,
            points,
            wait,
            exit
        }
        private State _currentState = State.init;
        public State currentState
        {
            get { return _currentState; }
            set
            {
                _currentState = value;
                eventStateChanged?.Invoke(_currentState.ToString());
            }
        }
        #endregion

        private BindingList<Judge> _judgeList = new BindingList<Judge>();
        public BindingList<Judge> judgeList
        {
            get { return _judgeList; }
            set
            {
                _judgeList = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("judgeList"));
            }
        }
        private Judge _activejudge = new Judge();
        public Judge activeJudge
        {
            get { return _activejudge; }
            set
            {
                _activejudge = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("activeJudge"));
            }
        }
        public int selectedJudge = -1;
        public bool seated = false;
        private Score _currentScore = new Score();
        public Score currentScore
        {
            get { return _currentScore; }
            set
            {
                _currentScore = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("currentScore"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("currentJump"));
            }
        }
        private Jump _currentJump = new Jump();
        public Jump currentJump
        {
            get { return _currentJump; }
            set
            {
                _currentJump = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("currentJump"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("currentScore"));
            }
        }

        public void connectToAdmin(string ipAddress)
        {
            connect(ipAddress);
            if (connected)
            {
                currentState = State.wait;
                logger.Info("Connected to admin");
            }
        }

        public void selectionChanged(int index)
        {
            selectedJudge = index;
        }

        public void sendSelectedJudge()
        {
            if(selectedJudge == -1) { return; }
            activeJudge = judgeList[selectedJudge];
            sendData("judge", activeJudge, clientStream);
            currentState = State.wait;
            logger.Info("Selected judge sent to admin");
        }

        public void sendScore(string score)
        {
            currentScore.points = double.Parse(score);
            sendData("score", currentScore, clientStream);
            currentState = State.wait;
            currentScore.points = 0.0;
            logger.Info("Score sent to admin");
        }

        public void receiveFromAdmin(string str)
        {
            if (!string.IsNullOrEmpty(str))
            {
                var splitmessage = str.Split('@');

                var cmd = splitmessage[0];
                var data = splitmessage[1];

                switch (cmd)
                {
                    case "judgelist":
                    case "occupied":
                        if (cmd == "occupied")
                        {
                            eventShowMessageBox?.Invoke("Dommaren redan vald. Välj en annan");
                            logger.Info("Trying to pick an already occupied judge");
                        }
                        judgeList.Clear();
                        foreach (var judge in deserializer<BindingList<Judge>>(data))
                        {
                            if (!judge.occupied)
                            {
                                judgeList.Add(judge);
                                logger.Info("Judge: " + judge.name + " selected");
                            }
                        }
                        currentState = State.select;
                        break;

                    case "ok":
                        judgeAccepted = true;
                        currentScore.judgeId = activeJudge.id;
                        currentScore.judgeSeat = activeJudge.judgeSeat;
                        currentScore.points = 0.0;

                        currentState = State.wait;
                        break;

                    case "jump":
                        currentJump = deserializer<Jump>(data);
                        currentState = State.points;
                        break;

                    case "exit":
                        currentState = State.exit;
                        break;

                    default:
                        break;
                }
            }
        }

        public void judgeExit()
        {
            connected = false;
            closeConnection();
            logger.Info("Judge closing application");
        }

        #region TCP

        public int port { get; set; }
        public string serverAddress { get; set; }
        public TcpClient tcpClient { get; set; }
        public StreamReader clientStreamReader { get; set; }
        public StreamWriter clientStreamWriter { get; set; }
        public NetworkStream clientStream { get; set; }
        private bool connected;

        private delegate void UpdateCallback(string str);

        public bool judgeAccepted;
        private Thread recieveThread;

        public void connect(string ipAdress)
        {
            if (connected) return;
            try
            {
                tcpClient = new TcpClient(ipAdress, 9058);
                clientStream = tcpClient.GetStream();
                clientStreamReader = new StreamReader(clientStream);
                clientStreamWriter = new StreamWriter(clientStream);
                connected = true;
                tcpClient.GetStream().Flush();
                recieveThread = new Thread(() => receiveData(clientStream));
                recieveThread.Start();
                logger.Info("Connected to " + ipAdress);

            }
            catch (SocketException ce)
            {
                logger.Error("Connection failed");
                logger.Error(ce.Message);
                eventShowMessageBox?.Invoke(ce.Message);
            }
        }

        private void receiveData(NetworkStream networkStream)
        {
            while (connected)
            {
                try
                {
                    var text = receive(networkStream);
                    var mainThreadCallback = new UpdateCallback(receiveFromAdmin);

                    Application.Current.Dispatcher.Invoke(delegate
                    {
                    mainThreadCallback(text);
                    });
                }
                catch (Exception re)
                {
                    logger.Error("Error recieving data");
                    logger.Error(re.Message);
                }
            }
        }

        public void closeConnection()
        {
            try
            {
                recieveThread.Abort();
                tcpClient.GetStream().Dispose();
                tcpClient.Close();
                
                logger.Info("Connection closed");
                
            }
            catch (Exception cle)
            {
                logger.Error("Error closing TCP-Connection");
                logger.Error(cle.Message);
            }
        }
        #endregion
    }
}
