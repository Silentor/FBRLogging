using System;
using UnityEngine;
using System.Collections;
using Silentor.FBRLogger;

public class UnityLogToFBR : MonoBehaviour
{
    /// <summary>
    /// IP address of log viewer application host
    /// </summary>
    public string LogViewerHost = "127.0.0.1";

    /// <summary>
    /// Port of log viewer application host
    /// </summary>
    public int LogViewerPort = 9999;

    /// <summary>
    /// Name of the logger to send Unity3d log messages
    /// </summary>
    public string LoggerName = "Unity.Log";

    private LogMessageSender _sender;

    void Awake()
    {
        //Works only in development mode
        if (!Debug.isDebugBuild)
        {
            enabled = false;
            Destroy(this);
            return;
        }

        try
        {
            _sender = new LogMessageSender(LogViewerHost, LogViewerPort);
            _sender.Send(new LogMessage(LoggerName, "UnityLogToFBR script created", LogMessage.LogLevel.Trace));
        }
        catch (Exception)
        {
            gameObject.SetActive(false);
            throw;
        }
    }

    void OnDestroy()
    {
        if(_sender != null)
            _sender.Dispose();
    }

    void OnEnable()
    {
        Application.RegisterLogCallbackThreaded(UnityLogCallback);
        _sender.Send(new LogMessage(LoggerName, "UnityLogToFBR script initialized", LogMessage.LogLevel.Trace));
    }

    private void UnityLogCallback(string condition, string stacktrace, LogType type)
    {
        _sender.Send(new LogMessage(LoggerName, condition, UnityLogType2LogLevel(type), stacktrace, null));
    }

    void OnDisable()
    {
        Application.RegisterLogCallbackThreaded(null);
    }

    private static LogMessage.LogLevel UnityLogType2LogLevel(LogType logType)
    {
        switch (logType)
        {
            case LogType.Log:
                return LogMessage.LogLevel.Log;
            case LogType.Warning:
                return LogMessage.LogLevel.Warning;
            case LogType.Error:
                return LogMessage.LogLevel.Error;
            case LogType.Exception:
                return LogMessage.LogLevel.Error;
            case LogType.Assert:
                return LogMessage.LogLevel.Fatal;
            default:return LogMessage.LogLevel.Log;
        }
    }
}


