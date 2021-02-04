using System.Text.Json.Serialization;

namespace LogModule.Application.Enums
{
    /// <summary>
    /// Log Servity Filter, 0 = All (Default), 
    /// Information = 1, Error = 2, Warning = 3, Verbose = 8
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum LogLevels
    {
        /// <summary>
        /// All servity levels
        /// </summary>
        ALL = 0,
        /// <summary>
        /// usually info
        /// </summary>
        Information = 1,
        /// <summary>
        /// failed action's that raised exceptions
        /// </summary>
        Error = 2,
        /// <summary>
        /// not very important but it's like an alert that need attention
        /// </summary>
        Warning = 3,
        /// <summary>
        /// about framework actions
        /// </summary>
        Debug = 4,
        /// <summary>
        /// failure in system modules/services
        /// </summary>
        Fatal = 5,
        /// <summary>
        /// system failure or crash
        /// </summary>
        Critical = 6,
        /// <summary>
        /// trace code blocks
        /// </summary>
        Trace = 7,
        /// <summary>
        /// everything about code's are running
        /// </summary>
        Verbose = 8,

    }
}
