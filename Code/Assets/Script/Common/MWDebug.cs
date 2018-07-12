using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;


public class MWConsole
{
	class LogEntry
	{
		public string m_msg;
		public float m_time;
		public string m_type;
	}

	
	UIPanel m_panel;
	Sprite m_consoleBg;
	//SpriteText m_consoleText;
	List<LogEntry> m_logs = new List<LogEntry>();
	int m_lastEntry = -1;
	

	public MWConsole(GameObject consoleObj)
	{
		m_panel = consoleObj.GetComponent<UIPanel>();
		Transform t = consoleObj.transform.Find("Bg");
		m_consoleBg = t.GetComponent<Sprite>();
		t = consoleObj.transform.Find("Text");
		//m_consoleText = t.GetComponent<SpriteText>();
	}

	static int CountLine(string str)
	{
		int cnt = 1;
		foreach (char c in str)
			if (c == '\n')
				cnt++;
		return cnt;
	}

	void UpdateCache()
	{
		if (m_logs.Count == 0 || !Visible)
			return;
		int lines = 0;
		int last = m_lastEntry < 0 ? (m_logs.Count - 1) : m_lastEntry;
		int first = 0;
		for (int i = last; i >= 0; i--)
		{
			LogEntry e = m_logs[i];
			int line = CountLine(e.m_msg);
			lines += line;
			if(lines >= 29)
			{
				first = i;
				break;
			}
		}

		StringBuilder sb = new StringBuilder();
		if (lines < 29)
		{
			sb.Append(new string('\n', 29 - lines));
		}
		

		
		for (int i = first; i <= last; i++)
		{
			LogEntry e = m_logs[i];
			if (e.m_type == "Error")
				sb.Append("[#ff0000]");
			else if (e.m_type == "Warning")
				sb.Append("[#ffff00]");
			else if (e.m_type == "Log")
				sb.Append("[#ffffff]");
			sb.Append("[");
			sb.Append(e.m_time.ToString("0.00"));
			sb.Append("]");
			sb.Append("[");
			sb.Append(e.m_type);
			sb.Append("] ");
			sb.Append(e.m_msg);
			sb.Append("\n");
		}
		//m_consoleText.Text = sb.ToString();
	}

	public void AddLog(string msg, string type)
	{
		LogEntry entry = new LogEntry();
		entry.m_msg = msg;
		entry.m_type = type;
		entry.m_time = Time.time;
		m_logs.Add(entry);

		UpdateCache();
	}

	public void AddLog(string msg)
	{
		AddLog(msg, "Log");
	}

	public void AddWarning(string msg)
	{
		AddLog(msg, "Warning");
	}

	public void AddError(string msg)
	{
		AddLog(msg, "Error");
	}

	public void PageUp()
	{
		if (m_lastEntry < 0)
			m_lastEntry = m_logs.Count - 1;
		m_lastEntry -= 25;
		if (m_lastEntry < 0)
			m_lastEntry = 0;
		UpdateCache();
	}

	public void PageDown()
	{
		if (m_lastEntry < 0)
			return;
		m_lastEntry += 25;
		if (m_lastEntry > m_logs.Count - 1)
			m_lastEntry = -1;
		UpdateCache();
	}

	public void LineUp()
	{
		if (m_lastEntry < 0)
			m_lastEntry = m_logs.Count - 1;
		m_lastEntry--;
		if (m_lastEntry < 0)
			m_lastEntry = 0;
		UpdateCache();
	}

	public void LineDown()
	{
		if (m_lastEntry < 0)
			return;
		m_lastEntry++;
		if (m_lastEntry > m_logs.Count - 1)
			m_lastEntry = -1;
		UpdateCache();
	}

	public bool Visible
	{
		get
		{
			return m_panel.transform.localPosition.z > 0;
		}
		set
		{
			m_panel.transform.localPosition = new Vector3(0, 0, value ? 10 : -10);
            if (value)
                UpdateCache();
		}

	}
}

[AddComponentMenu("M/MWDebug")]
public class MWDebug : MonoBehaviour {

	MWConsole m_console;
	

	void Awake()
	{
		Transform t = transform.Find("Console");
		m_console = new MWConsole(t.gameObject);

		m_console.Visible = false;

	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		//if(Input.anyKeyDown)
		//{
		//    KeyCode[] keycodes = (KeyCode[])System.Enum.GetValues(typeof(KeyCode));
		//    foreach (KeyCode k in keycodes)
		//    {
		//        if (Input.GetKeyDown(k))
		//        {
		//            Debug.Log("Keydown: " + k.ToString());
		//        }
		//    }
		//}
		
		if(Input.GetKeyDown(KeyCode.BackQuote))
		{
			m_console.Visible = !m_console.Visible;
		}
		if(Input.GetKeyDown(KeyCode.PageUp))
		{
			m_console.PageUp();
		}
		else if(Input.GetKeyDown(KeyCode.PageDown))
		{
			m_console.PageDown();
		}
		else if(Input.GetKey(KeyCode.UpArrow))
		{
			m_console.LineUp();
		}
		else if (Input.GetKey(KeyCode.DownArrow))
		{
			m_console.LineDown();
		}
		
	}

	public MWConsole Console
	{
		get
		{
			return m_console;
		}
	}

	public static void Log(string msg)
	{
		Debug.Log(msg);
	}

	public static void LogError(string msg)
	{
		Debug.LogError(msg);
	}

	public static void LogWarning(string msg)
	{
		Debug.LogWarning(msg);
	}
}
