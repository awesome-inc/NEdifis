using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace NEdifis.Diagnostics
{
    /// <summary>
    /// A trace listener that can be used for testing logging requirements based on <see cref="Trace"/>.
    /// </summary>
    public sealed class TestTraceListener : TraceListener
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TestTraceListener"/>.
        /// </summary>
        public TestTraceListener()
        {
            // Set default values
            Name = "Test Trace Listener (NEdifis)";
            ActiveTraceLevel = TraceLevel.Info;

            // Subscribe to all trace
            Trace.Listeners.Add(this);
        }

        /// <summary>
        /// Releases the unmanaged resources used by the <see cref="T:System.Diagnostics.TraceListener"/> and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources. </param>
        protected override void Dispose(bool disposing)
        {
            Trace.Listeners.Remove(this);
            base.Dispose(disposing);
        }

        /// <summary>
        /// The current trace level. Default is <see cref="TraceLevel.Info"/>.
        /// </summary>
        public TraceLevel ActiveTraceLevel { get; set; }

        private readonly Dictionary<int, List<string>> _levelMessages = new Dictionary<int, List<string>>
            {
                {(int)TraceLevel.Error, new List<string>()},
                {(int)TraceLevel.Warning, new List<string>()},
                {(int)TraceLevel.Info, new List<string>()},
                {(int)TraceLevel.Verbose, new List<string>()}
            };

        /// <summary>
        /// Returns the messages traced for the specified level.
        /// </summary>
        /// <param name="level">The trace level</param>
        public IEnumerable<string> MessagesFor(TraceLevel level)
        {
            return _levelMessages[(int)level].AsReadOnly();
        }

        /// <summary>
        /// Clears traced messages for the specified trace level.
        /// </summary>
        /// <param name="level">The trace level</param>
        public void ClearMessagesFor(TraceLevel level)
        {
            _levelMessages[(int)level].Clear();
        }

        /// <summary>
        /// Clears all traced messages.
        /// </summary>
        public void ClearMessages()
        {
            _levelMessages.Values.ToList().ForEach(l => l.Clear());
        }

        /// <summary>
        /// Writes trace information, a message, and event information to the listener specific output.
        /// </summary>
        /// <param name="eventCache">A <see cref="T:System.Diagnostics.TraceEventCache"/> object that contains the current process ID, thread ID, and stack trace information.</param><param name="source">A name used to identify the output, typically the name of the application that generated the trace event.</param><param name="eventType">One of the <see cref="T:System.Diagnostics.TraceEventType"/> values specifying the type of event that has caused the trace.</param><param name="id">A numeric identifier for the event.</param><param name="message">A message to write.</param>
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

        /// <summary>
        /// Writes trace information, a formatted array of objects and event information to the listener specific output.
        /// </summary>
        /// <param name="eventCache">A <see cref="T:System.Diagnostics.TraceEventCache"/> object that contains the current process ID, thread ID, and stack trace information.</param><param name="source">A name used to identify the output, typically the name of the application that generated the trace event.</param><param name="eventType">One of the <see cref="T:System.Diagnostics.TraceEventType"/> values specifying the type of event that has caused the trace.</param><param name="id">A numeric identifier for the event.</param><param name="format">A format string that contains zero or more format items, which correspond to objects in the <paramref name="args"/> array.</param><param name="args">An object array containing zero or more objects to format.</param>
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

        /// <summary>
        /// When overridden in a derived class, writes the specified message to the listener you create in the derived class.
        /// </summary>
        /// <param name="message">A message to write. </param>
        public override void Write(string message)
        {
            WriteLine(message);
        }

        /// <summary>
        /// When overridden in a derived class, writes a message to the listener you create in the derived class, followed by a line terminator.
        /// </summary>
        /// <param name="message">A message to write. </param>
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
