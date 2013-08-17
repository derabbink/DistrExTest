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

        [TestFixtureSetUp]
        public void FixtureSetUp()
        {
            _onWorkerOneProcess = Start(GetPathFromConfig("PingApp-worker-exe-file"), WorkerService);
            _onWorkerTwoProcess = Start(GetPathFromConfig("PongApp-worker-exe-file"), WorkerService);

            //Start the applications here 
            Start(GetPathFromConfig("PingApp-exe-file"), "PingApp.exe");
            Start(GetPathFromConfig("PongApp-exe-file"), "PongApp.exe");

            var _pingCallBackHandler = new ExecutorCallbackService();
            var _pongCallBackHandler = new ExecutorCallbackService();

            _onWorkerOne = OnWorker.FromEndpointConfigNames("PingApp-AssemblyManager", "PingApp-Executor", _pingCallBackHandler);
            _onWorkerTwo = OnWorker.FromEndpointConfigNames("PongApp-AssemblyManager", "PongApp-Executor", _pongCallBackHandler);
        }

        private static string GetPathFromConfig(string key)
        {
            return Path.GetFullPath(Path.GetDirectoryName(ConfigurationManager.AppSettings.Get(key)));
        }

        private static AutomatedApplication Start(string applicationPath, string processName)
        {
            var application = new OutOfProcessApplication(new OutOfProcessApplicationSettings
            {
                ProcessStartInfo = new ProcessStartInfo(applicationPath + "\\" + processName),
                ApplicationImplementationFactory = new UIAutomationOutOfProcessApplicationFactory()
            });

            application.Start();

            application.WaitForMainWindow(TimeSpan.FromSeconds(60));

            return application;
        }

        [Test]
        public void Test()
        {
            Instruction<string, bool> invokePing = (ct, rp, arg) =>
            {
                var mainWindow = AutomationElement.RootElement.WaitForFirstChild(arg);
                var pingButton = mainWindow.WaitForFirstChild("Ping");
                pingButton.Invoke();
                Thread.Sleep(2000);
                return !(pingButton.Current.IsEnabled); 
            };
            var result1 = Coordinator.Do(_onWorkerOne.Do(invokePing), "PingApp").ResultValue; 
            Assert.IsTrue(result1);

            Instruction<string, bool> invokePong = (ct, rp, arg) =>
            {
                var mainWindow = AutomationElement.RootElement.WaitForFirstChild(arg);
                var pongButton = mainWindow.WaitForFirstChild("Pong");
                pongButton.Invoke();
                Thread.Sleep(2000);
                return !(pongButton.Current.IsEnabled); 
            };

            var result2 = Coordinator.Do(_onWorkerTwo.Do(invokePong), "PongApp").ResultValue;
            Assert.IsTrue(result2);
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
