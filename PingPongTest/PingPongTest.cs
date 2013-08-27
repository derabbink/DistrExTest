using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using DistrEx.Common;
using DistrEx.Communication.Service.Executor;
using DistrEx.Coordinator.Interface;
using DistrEx.Coordinator.TargetSpecs;
using System.Configuration; 
using Microsoft.Test.ApplicationControl;
using NUnit.Framework;
using System.Windows.Automation;
using Coordinator = DistrEx.Coordinator.Coordinator;

namespace PingPongTest
{
    [TestFixture]
    public class PingPongTest
    {
        private TargetSpec _onWorkerOne;
        private TargetSpec _onWorkerTwo;

        private AutomatedApplication _onWorkerOneProcess;
        private AutomatedApplication _onWorkerTwoProcess;

        private const string WorkerService = "DistrEx.Worker.Host.exe";
        private string pingPath;
        private string pongPath; 

        [TestFixtureSetUp]
        public void FixtureSetUp()
        {
            _onWorkerOneProcess = Start(GetPathFromConfig("PingApp-worker-exe-file"), WorkerService);
            _onWorkerTwoProcess = Start(GetPathFromConfig("PongApp-worker-exe-file"), WorkerService);

            var _pingCallBackHandler = new ExecutorCallbackService();
            var _pongCallBackHandler = new ExecutorCallbackService();

            _onWorkerOne = OnWorker.FromEndpointConfigNames("PingApp-AssemblyManager", "PingApp-Executor", _pingCallBackHandler);
            _onWorkerTwo = OnWorker.FromEndpointConfigNames("PongApp-AssemblyManager", "PongApp-Executor", _pongCallBackHandler);
            
            Instruction<Tuple<string, string>, bool> openApp1 = (ct, rp, args) => openApp(ct,rp, args.Item1);
            Instruction<Tuple<string, string>, bool> openApp2 = (ct, rp, args) => openApp(ct,rp, args.Item2);

            pingPath = GetPathFromConfig("PingApp-exe-file");
            pongPath = GetPathFromConfig("PongApp-exe-file");

            var tuple = new Tuple<string, string>(pingPath + @"\PingApp.exe", pongPath + @"\PongApp.exe");
            DistrEx.Coordinator.Coordinator.Do(_onWorkerOne.Do(openApp1), _onWorkerTwo.Do(openApp2), tuple);
        }
        
        private static bool openApp(CancellationToken token, Action progress, string argument)
        {
                Process.Start(argument);
                return true;
        }
        
        private static string GetPathFromConfig(string key)
        {
            return Path.GetFullPath(Path.GetDirectoryName(ConfigurationManager.AppSettings.Get(key)));
        }

        private static AutomatedApplication Start(string applicationPath, string processName)
        {
            var psi = new ProcessStartInfo(applicationPath + "\\" + processName)
            {
                WorkingDirectory = applicationPath
            };
            var application = new OutOfProcessApplication(new OutOfProcessApplicationSettings
            {
                ProcessStartInfo = psi,
                ApplicationImplementationFactory = new UIAutomationOutOfProcessApplicationFactory()
            });

            application.Start();

            application.WaitForMainWindow(TimeSpan.FromSeconds(60));

            return application;
        }

        [Test]
        public void Test()
        {
            Instruction<string, string> invokePing = (ct, rp, arg) =>
            {
                var mainWindow = AutomationElement.RootElement.WaitForFirstChild(arg);
                var pingButton = mainWindow.WaitForFirstChild("Ping");
                pingButton.Invoke();
                return "Pinged"; 
            };

            Instruction<string, Tuple<string, string>> invokePong = (ct, rp, arg) =>
            {
                var mainWindow = AutomationElement.RootElement.WaitForFirstChild("PongApp");
                var pongButton = mainWindow.WaitForFirstChild("Pong");
                pongButton.Invoke();
                return new Tuple<string, string>(arg, "Ponged");
            };

            for (int i = 0; i < 10; i++)
            {
                var result = Coordinator.Do(_onWorkerOne.Do(invokePing), "PingApp").ThenDo(_onWorkerTwo.Do(invokePong)).ResultValue;
                var expectedResult = new Tuple<string, string>("Pinged", "Ponged");
                Assert.AreEqual(result, expectedResult);
            }
        }

        [TestFixtureTearDown]
        public void FixtureTearDown()
        {
            _onWorkerOne.ClearEverything();
            _onWorkerTwo.ClearEverything();

            StopService();
        }
        
        private void StopService()
        {
            _onWorkerOneProcess.Close();
            _onWorkerTwoProcess.Close();
        }
    }
}
