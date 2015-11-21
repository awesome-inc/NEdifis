using System.Collections.Generic;
using System.Diagnostics;
using NEdifis.Attributes;

namespace NEdifis.Diagnostics
{
    [TestedBy(typeof(TestTraceListener_Should))]
    public sealed class TestTraceListener : TraceListener
    {
        #region construct and dispose

        public TestTraceListener()
        {
            // Set default values
            Name = "Test Trace Listener (NEdifis)";
            ActiveTraceLevel = TraceLevel.Info;

            // Subscribe to all trace
            Trace.Listeners.Add(this);
        }

        protected override void Dispose(bool disposing)
        {
            Trace.Listeners.Remove(this);
            base.Dispose(disposing);
        }

        #endregion

        public TraceLevel ActiveTraceLevel { get; set; }

        private readonly Dictionary<int, List<string>> _levelMessages = new Dictionary<int, List<string>>
            {
                {(int)TraceLevel.Error, new List<string>()},
                {(int)TraceLevel.Warning, new List<string>()},
                {(int)TraceLevel.Info, new List<string>()},
                {(int)TraceLevel.Verbose, new List<string>()}
            };

        public IEnumerable<string> MessagesFor(TraceLevel level)
        {
            return _levelMessages[(int)level].AsReadOnly();
        }

        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string message)
        {
            // Check the type
            switch (eventType)
            {
                case TraceEventType.Critical:
                case TraceEventType.Error:
                    if ((ActiveTraceLevel == TraceLevel.Error) ||
                        (ActiveTraceLevel == TraceLevel.Warning) ||
                        (ActiveTraceLevel == TraceLevel.Info) ||
                        (ActiveTraceLevel == TraceLevel.Verbose))
                    {
                        _levelMessages[(int)TraceLevel.Error].Add(message);
                    }
                    break;

                case TraceEventType.Warning:
                    if ((ActiveTraceLevel == TraceLevel.Warning) ||
                        (ActiveTraceLevel == TraceLevel.Info) ||
                        (ActiveTraceLevel == TraceLevel.Verbose))
                    {
                        _levelMessages[(int)TraceLevel.Warning].Add(message);
                    }
                    break;

                case TraceEventType.Information:
                    if ((ActiveTraceLevel == TraceLevel.Info) ||
                        (ActiveTraceLevel == TraceLevel.Verbose))
                    {
                        _levelMessages[(int)TraceLevel.Info].Add(message);
                    }
                    break;

                case TraceEventType.Verbose:
                    if (ActiveTraceLevel == TraceLevel.Verbose)
                    {
                        _levelMessages[(int)TraceLevel.Verbose].Add(message);
                    }
                    break;
                case TraceEventType.Start:
                case TraceEventType.Stop:
                case TraceEventType.Suspend:
                case TraceEventType.Resume:
                case TraceEventType.Transfer:
                    break;
            }
        }

        public override void TraceEvent(
            TraceEventCache eventCache,
            string source,
            TraceEventType eventType,
            int id,
            string format,
            params object[] args)
        {
            TraceEvent(eventCache, source, eventType, id, string.Format(format, args));
        }

        public override void Write(string message)
        {
            WriteLine(message);
        }

        public override void WriteLine(string message)
        {
            // Invoke the event (but only when output is set to verbose)
            if (ActiveTraceLevel == TraceLevel.Verbose)
            {
                _levelMessages[(int)TraceLevel.Verbose].Add(message);
            }
        }
    }
}
