using UnityEngine;
using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

public static class Debug
{
	public enum LogLevel
	{
		Trace,
		Log,
		Warning,
		Error,
		Fatal
	}
	
	//! debug mode
	private static readonly bool		EnableDebugMode = true;
	
	//! log wirte file stream
	private static FileStream			sm_logFileStream	= null;
	private static StreamWriter			sm_logFileWriteStream = null;

	/// <summary>
	/// Initialize debug system
	/// </summary>
	public static void Initialize()
	{
		if (_initialized)
			return;

		_logLevelEnabled[LogLevel.Trace] = EnableDebugMode;
		_logLevelEnabled[LogLevel.Log] = EnableDebugMode;
		_logLevelEnabled[LogLevel.Warning] = EnableDebugMode;
		_logLevelEnabled[LogLevel.Error] = EnableDebugMode;
		_logLevelEnabled[LogLevel.Fatal] = EnableDebugMode;

		_initialized = true;
		
		try{
			sm_logFileStream 		= new FileStream(GetWriteLogFilename(),FileMode.Create,FileAccess.Write,FileShare.Read);
			sm_logFileWriteStream 	= new StreamWriter(sm_logFileStream);
		}catch(Exception ex){
			LogError("Debug.sm_logFileStream Initialize Failed Exception:" + ex.Message);
		}
	}

	//! get current write log filename
	public static string GetWriteLogFilename(){
		return Application.persistentDataPath + "/" + SystemInfo.deviceUniqueIdentifier + "_log.txt";
	}
	
	public static void Release()
	{
		UnregisterAllLogOutputListener();
		
		if(sm_logFileWriteStream != null){
			sm_logFileWriteStream.Close();
			sm_logFileWriteStream = null;
		}
		
		if(sm_logFileStream != null){
			sm_logFileStream.Close();
			sm_logFileStream = null;
		}
	}

	/// <summary>
	/// Enable/Disable the specific log level
	/// </summary>
	/// <param name="logLevel">Log level to be set</param>
	/// <param name="enable">Enable/Disable</param>
	public static void EnableLogLevel(LogLevel logLevel, bool enable)
	{
		Initialize();
		_logLevelEnabled[logLevel] = enable;
	}

	/// <summary>
	/// Check if the specific log level enabled.
	/// </summary>
	/// <param name="logLevel">Log level to check</param>
	/// <returns>Enable/Disable</returns>
	public static bool IsLogLevelEnabled(LogLevel logLevel)
	{
		Initialize();
		return _logLevelEnabled[logLevel];
	}

	/// <summary>
	/// Log a message at trace level
	/// </summary>
	/// <param name="msg">Message to log</param>
	public static void LogTrace(params object []msg)
	{
		LogObject(LogLevel.Trace, msg);
	}
	
	/// <summary>
	/// Logs a format message at trace level.
	/// </summary>
	/// <param name='format'>
	/// Format string.
	/// </param>
	/// <param name='msg'>
	/// Message to log.
	/// </param>
	public static void LogTraceFormat(string format, params object []msg)
	{
		LogFormat(LogLevel.Trace, format, msg);
	}

	/// <summary>
	/// Log a message at log level
	/// </summary>
	/// <param name="msg">Message to log</param>
	public static void Log(params object []msg)
	{
		LogObject(LogLevel.Log, msg);
	}
	
	/// <summary>
	/// Logs a format message at log level.
	/// </summary>
	/// <param name='format'>
	/// Format string.
	/// </param>
	/// <param name='msg'>
	/// Message to log.
	/// </param>
	public static void LogFormat(string format, params object []msg)
	{
		LogFormat(LogLevel.Log, format, msg);
	}

	/// <summary>
	/// Log a message at warning level
	/// </summary>
	/// <param name="msg">Message to log</param>
	public static void LogWarning(params object []msg)
	{
		LogObject(LogLevel.Warning, msg);
	}
	
	/// <summary>
	/// Logs a format message at warning level.
	/// </summary>
	/// <param name='format'>
	/// Format string.
	/// </param>
	/// <param name='msg'>
	/// Message to log.
	/// </param>
	public static void LogWarningFormat(string format, params object []msg)
	{
		LogFormat(LogLevel.Warning, format, msg);
	}

	/// <summary>
	/// Log a message at error level
	/// </summary>
	/// <param name="msg">Message to log</param>
	public static void LogError(params object []msg)
	{
		LogObject(LogLevel.Error, msg);
	}
	
	/// <summary>
	/// Logs a format message at error level.
	/// </summary>
	/// <param name='format'>
	/// Format string.
	/// </param>
	/// <param name='msg'>
	/// Message to log.
	/// </param>
	public static void LogErrorFormat(string format, params object []msg)
	{
		LogFormat(LogLevel.Error, format, msg);
	}

	/// <summary>
	/// Log a message at fatal level
	/// </summary>
	/// <param name="msg">Message to log</param>
	public static void LogFatal(params object []msg)
	{
		LogObject(LogLevel.Fatal, msg);
	}
	
	/// <summary>
	/// Logs a format message at fatal level.
	/// </summary>
	/// <param name='format'>
	/// Format string.
	/// </param>
	/// <param name='msg'>
	/// Message to log.
	/// </param>
	public static void LogFatalFormat(string format, params object []msg)
	{
		LogFormat(LogLevel.Fatal, format, msg);
	}
	
	/// <summary>
	/// Asset if condition is false
	/// </summary>
	/// <param name="condition">Condition</param>
	/// <param name="msg">Message to display</param>
	public static void Assert(bool condition, string msg)
	{
		if (!condition)
		{
			throw new AssertExpeption(msg);
		}
	}

	/// <summary>
	/// Register a listener to receive log
	/// </summary>
	/// <param name="listener">listener</param>
	public static void RegisterLogOutputListener(ILogOutputListener listener)
	{
		if (_logOutputListeners.Contains(listener) == false)
		{
			_logOutputListeners.Add(listener);
		}

		if (_logOutputListeners.Count != 0)
		{
			UnityEngine.Application.RegisterLogCallback(LogCallback);
		}
	}

	/// <summary>
	/// Unregister a log listener
	/// </summary>
	/// <param name="listener">listener</param>
	public static void UnregisterLogOutputListener(ILogOutputListener listener)
	{
		_logOutputListeners.Remove(listener);

		if (_logOutputListeners.Count == 0)
		{
			UnityEngine.Application.RegisterLogCallback(null);
		}
	}

	/// <summary>
	/// Unregister all log listeners
	/// </summary>
	/// <param name="listener">listener</param>
	public static void UnregisterAllLogOutputListener()
	{
		_logOutputListeners.Clear();
		UnityEngine.Application.RegisterLogCallback(null);
	}
	
	private static void LogFormat(LogLevel logLevel, string format, params object []msg)
	{
		StringBuilder bd = new StringBuilder();
		bd.AppendFormat(format, msg);
		
		Log(logLevel, bd.ToString());
	}
	
	private static void LogObject(LogLevel logLevel, params object []msg)
	{
		StringBuilder bd = new StringBuilder();
		
		foreach (object p in msg)
		{
			bd.AppendFormat(" {0}", ( p == null ? "null" : p.ToString()));
		}
		
		Log(logLevel, bd.ToString());
	}
	
	public static void DrawLine(Vector3 begin, Vector3 end , Color color)
	{
	}
	
	private static void Log(LogLevel logLevel, string msg)
	{	
		if (IsLogLevelEnabled(logLevel))
		{
			StringBuilder bd = new StringBuilder();
			bd.AppendFormat("[{2}] [{1}] {0}", msg, logLevel, GetCurrTimeStamp());
			
			string logMessage = bd.ToString();

			switch (logLevel)
			{
			case LogLevel.Trace:
			case LogLevel.Log:
				UnityEngine.Debug.Log(logMessage);
				break;
			case LogLevel.Warning:
				UnityEngine.Debug.LogWarning(logMessage);
				break;
			case LogLevel.Error:
				UnityEngine.Debug.LogError(logMessage);
				break;
			case LogLevel.Fatal:
				UnityEngine.Debug.LogError(logMessage);
				UnityEngine.Debug.Break();
				break;
			}
			
			if(sm_logFileWriteStream != null){
				sm_logFileWriteStream.WriteLine(logMessage);
				sm_logFileWriteStream.Flush();
			}
		}
	}
	
	private static long GetCurrTimeStamp(){
		// 
		//long t_delta = System.DateTime.Parse("1970-01-01 00:00:00").Ticks;
		//t_delta == 621355968000000000
		//
		return (System.DateTime.UtcNow.Ticks - 621355968000000000L) / 10000;
	}

	private static void LogCallback(string condition, string stackTrace, UnityEngine.LogType type)
	{
		foreach (ILogOutputListener linstener in _logOutputListeners)
		{
			linstener.OnLog(condition, stackTrace, type);
		}
	}

	private static bool _initialized = false;
	private static Dictionary<LogLevel, bool> _logLevelEnabled = new Dictionary<LogLevel, bool>();
	private static List<ILogOutputListener> _logOutputListeners = new List<ILogOutputListener>();
}
