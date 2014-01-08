//---------------------------------------------------------------------
// File: EventFormatter.cs
// 
// Summary: Maintains methods for formating messages and exception info
//
//
//---------------------------------------------------------------------
// This file is part of the Microsoft ESB Guidance for BizTalk
//
// Copyright (c) Microsoft Corporation. All rights reserved.
//
// This source code is intended only as a supplement to Microsoft BizTalk
// Server 2006 R2 release and/or on-line documentation. See these other
// materials for detailed information regarding Microsoft code samples.
//
// THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY
// KIND, WHETHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
// IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR
// PURPOSE.
//---------------------------------------------------------------------

using System;
using System.Globalization;
using System.Reflection;
using System.Diagnostics;

namespace ForgeBot.Diagnostics
{
    /// <summary>
    /// This class is responsible for formatting all exception information
    /// and returning the formatted strings to the caller
    /// </summary>
    public static class EventFormatter
    {
        /// <summary>
        /// format strings for error source.  used by FormatError()
        /// </summary>
        private const string ErrorSourceFormat = "Class: {0} Method: {1} :: {2}";
        private const string ErrorMessageFormat = "Class: {0} Method: {1} : Exception: {2} Stack {3}";

        private const string EventLogFormat = "{0} \r\n\r\nSource: {1} \r\n\r\nMethod: {2} \r\n\r\nError Source: {3} \r\n\r\nError TargetSite: {4}  \r\n\r\nError StackTrace: {5}";
        private const string EventLogFaultContractFormat = "{0} \r\n\r\nService: {1} \r\n\r\nMethod: {2} \r\n\r\nError Id: {3} \r\n\r\nError Source: {4} \r\n\r\nError TargetSite: {5}  \r\n\r\nError StackTrace: {6}";
        private const string EventLogMessageFormat = "Source: {0} \r\n\r\nMethod: {1} \r\n\r\nMessage: {2}";

        /// <summary>
        /// Passed to FormatError by calling method to determine format to use
        /// </summary>
        public enum ErrorFormat { Source, Message };

        /// <summary>
        /// Used primarily to format warning information for logging 
        /// </summary>
        /// <param name="memberInfo">MemberInfo class to derive current 
        /// executing method signature information from</param>
        /// <param name="message">message to log</param>
        /// <returns>formatted string</returns>
        public static string FormatEvent(MemberInfo memberInfo, string message)
        {
            if (null == memberInfo)
                throw new ArgumentNullException("memberInfo");
            if (string.IsNullOrEmpty(message))
                throw new ArgumentNullException("message");


            return string.Format(CultureInfo.CurrentCulture, EventLogMessageFormat,
                                     memberInfo.ToString(), memberInfo.ReflectedType.ToString(),
                                     message);
        }

        /// <summary>
        /// Used primarily to format error information for logging 
        /// </summary>
        /// <param name="memberInfo">MemberInfo class to derive current 
        /// executing method signature information from</param>
        /// <param name="exception">System.Exception object to retreive string
        /// information from</param>
        /// <returns>formatted string</returns>
        public static string FormatEvent(MemberInfo memberInfo, System.Exception exception)
        {
            if (null == memberInfo)
                throw new ArgumentNullException("memberInfo");
            if (null == exception)
                throw new ArgumentNullException("exception");


            return string.Format(CultureInfo.CurrentCulture, EventLogFormat,
                                     exception.Message,memberInfo.ReflectedType.ToString(),
                                     memberInfo.ToString(), exception.Source ?? string.Empty, exception.TargetSite == null ? string.Empty : exception.TargetSite.ToString(), exception.StackTrace ?? string.Empty);
        }

        /// <summary>
        /// Used primarily to format error information for logging.  Called for WCF
        /// based soap faults to also tie in a unique ID so the error from the client
        /// can later be tied back to the server's event log
        /// </summary>
        /// <param name="stackFrame">StackFrame to derive current 
        /// executing method and service information from</param>
        /// <param name="exception">System.Exception object to retreive string
        /// information from</param>
        /// <param name="guid">Error Id generated for WCF fault Contract</param>
        /// <returns>formatted string</returns>
        public static string FormatEvent(StackFrame stackFrame, System.Exception exception, 
            Guid guid)
        {
            if (null == stackFrame)
                throw new ArgumentNullException("stackFrame");
            if (null == exception)
                throw new ArgumentNullException("exception");
            


            return string.Format(CultureInfo.CurrentCulture, EventLogFaultContractFormat,
                                     exception.Message, stackFrame.GetMethod().ReflectedType.FullName,
                                     stackFrame.GetMethod().Name, guid.ToString(), exception.Source.ToString(), exception.TargetSite.ToString(), exception.StackTrace.ToString());
        }
        /// <summary>
        /// Called within exception catch blocks to format the exception.source property.
        /// Mostly for Trace.Writes.
        /// </summary>
        /// <param name="memberInfo">Current method info via reflection</param>
        /// <param name="errorFormat">Enum of ErrorFormat determines if Message or Source</param>
        /// <param name="errorMessage">exception.Message property</param>
        /// <param name="errorSource">exception.Source property</param>
        /// <returns>fomatted string</returns>
        public static string FormatError(MemberInfo memberInfo, ErrorFormat errorFormat,
            String errorMessage, string errorSource)
        {

            if (null == memberInfo)
                throw new ArgumentNullException("memberInfo");

            if (string.IsNullOrEmpty(errorMessage)) { errorMessage = string.Empty; }

            switch (errorFormat)
            {
                case ErrorFormat.Message:
                    return string.Format(CultureInfo.CurrentCulture, ErrorMessageFormat,
                                     memberInfo.ReflectedType.ToString(),
                                     memberInfo.ToString(), errorMessage, errorSource);

                default: // default to source
                    return string.Format(CultureInfo.CurrentCulture, ErrorSourceFormat,
                                     memberInfo.ReflectedType.ToString(),
                                     memberInfo.ToString(), errorSource);

            }
        }
    }
}
