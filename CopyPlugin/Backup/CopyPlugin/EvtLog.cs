﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace CopyPlugin
{
    class EvtLog
    {
        /// <summary>
        /// 事件源名称
        /// </summary>
        private string eventSourceName;
        EventLogEntryType eventLogType;
        public EvtLog()
        {
            eventSourceName = "Central Hospital Log";
            eventLogType = EventLogEntryType.Error;
        }

        /// <summary>
        /// 消息事件源名称
        /// </summary>
        public string EventSourceName
        {
            set { eventSourceName = value; }
        }

        /// <summary>
        /// 消息事件类型
        /// </summary>
        public EventLogEntryType EventLogType
        {
            set { eventLogType = value; }
        }

        /// <summary>
        /// 写入系统日志
        /// </summary>
        /// <param name="message">事件内容</param>
        public void LogEvent(string message)
        {
            if (!EventLog.SourceExists(eventSourceName))
            {
                EventLog.CreateEventSource(eventSourceName, "Central Hospital Service");
            }
            EventLog.WriteEntry(eventSourceName, message, EventLogEntryType.Error);
        }
    }
}
