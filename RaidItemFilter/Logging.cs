using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaidArtifactsFilter
{
    public class Logging
    {
        private static string LogFilePath = "Logs.txt";

        public static void Log(string message)
        {
            File.AppendAllLines(LogFilePath, new []{ message });
        }
    }

    public class TraceService
    {
        private static TraceService _instance;
        public static TraceService Instance => _instance ?? (_instance = new TraceService());

        private readonly Stopwatch _stopwatch = Stopwatch.StartNew();

        public static long CurrentMs => Instance._stopwatch.ElapsedMilliseconds;

        private long _timerValue;

        public bool TimingEnable { get; set; } = true;
        public List<long> TimerArr;

        public TraceService()
        {
            _timerValue = _stopwatch.ElapsedMilliseconds;

            TimerArr = new List<long>();
            for (var i = 0; i < 40; i++)
            {
                TimerArr.Add(_timerValue);
            }
        }

        public void LogWrite(params string[] messages)
        {
            Logging.Log(string.Concat(messages));
        }

        public void TimingSpace()
        {
            if (TimingEnable)
            {
                LogWrite("");
            }
        }

        public void TimingArrInternal(string message, int number = 0)
        {
            if (TimingEnable)
            {
                var timeInterval = CurrentMs - TimerArr[number];
                TimerArr[number] = CurrentMs;
                if (message != "")
                {
                    LogWrite(message, ": ", timeInterval.ToString());
                }
            }
        }

        public static void TimingArr(string message, int number = 0)
        {
            Instance.TimingArrInternal(message, number);
        }

        public static void TimingArrStart(int number = 0)
        {
            Instance.TimingArrInternal("", number);
        }
    }
}
