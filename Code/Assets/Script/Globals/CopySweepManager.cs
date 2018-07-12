using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TimerTickPublisher : EventManager.Publisher
{
	public static string NAME = "TimerTick";
	public static string TIMER_TICK = "Tick";
	public static string TIMER_END = "End";
	public static string TIMER_START = "Start";
	public static string TIMER_REFRESH = "Refresh";
	
	public override string Name 
	{
		get { return NAME; }
	}
	
	public void NotifyTimer(int timeCount)
	{
		base.Notify(TIMER_TICK, timeCount);
	}
	public void NotifyTimeEnd(int timeCount)
	{
		base.Notify(TIMER_END, timeCount);
	}
	public void NotifyTimeStart(int timeCount)
	{
		base.Notify(TIMER_START,timeCount);
	}
	public void NotifyTimeRefresh()
	{
		base.Notify(TIMER_REFRESH);
	}
	
}

public class CopySweepData
{
	public class BaseInfo // base info such as money and oil and so on 
	{
		public int basicContribution;//声望
		public int basicExp;//经验
		public int basicMoney;//黄金
		public int teachnology;//科技点
		public int featValue;//功绩
	}
	
	public class ItemInfo // single item base info
	{
		public int amount;
		public int itemId;
		public int packType;
		public int serialNumber;
		public bool isSale;
		public ItemData mItemData;
	}
	
	public class SingleBattleData //单次NPC掉落
	{
		public int battleIndex;
		public BaseInfo battleBaseInfo;
		public List<CopySweepData.ItemInfo> listBattleItemInfo = new List<CopySweepData.ItemInfo>();
	}
		
	public class SingleCopyData //单次副本掉落
	{ 
		public int copyIndex;
		public bool isFinished;
		public BaseInfo CopyBaseInfo;
		public SortedDictionary<int,SingleBattleData> dictSingleBattleData = new SortedDictionary<int, SingleBattleData>();
	}
	
	public SortedDictionary<int,SingleCopyData> DictSingleCopyData
	{
		get{return _dictSingleCopyData; }
		set{_dictSingleCopyData = value;}
	}
	
	public void ClearCopyData()
	{
		_dictSingleCopyData.Clear();
	}
	
	private SortedDictionary<int,SingleCopyData> _dictSingleCopyData = new SortedDictionary<int, SingleCopyData>();
	public bool isClearCopyData;
}


public class CopySweepManager : MonoBehaviour 
{
	public bool IsSweeping
	{
		get{ return _isSweeping; }
		set{ _isSweeping = value;}
	}
		
	
	void Start () 
	{
		_copySweepData = new CopySweepData();
	//	selectConsumeRadioType = GUICopySweep.EButtonType.Null;
		
	//	RegisterAppFocusSubscribes();
	}
	
	ISubscriber appFocusSub = null;
	void RegisterAppFocusSubscribes()
	{
		appFocusSub = EventManager.Subscribe(MonoEventPublisher.NAME + ":" + MonoEventPublisher.MONO_FOCUS);
		appFocusSub.Handler = delegate (object[] args)
		{
			bool focus = (bool)args[0];
			long ms = (long) args[1];
			
			if (focus)
			{
				_timerLeft -= Mathf.FloorToInt(ms / 1000.0f);
				
				if (_isSweeping)
					TimerTickNotify();
			}
		};
	}
	
	public void TimerStart()
	{
		_isSweeping = true;
		_lastTime = _timerLeft;
		_timerPublisher.NotifyTimeStart(_timerLeft);
		InvokeRepeating("TimerTickNotify",0,1);
	}
	
	public void TimerEnd()
	{
		CancelInvoke();
		_isSweeping = false;
		Debug.LogError("扫荡结束。。。");
		_timerPublisher.NotifyTimeEnd(_timerLeft);
	}
	
	public void StopTimer()
	{
		CancelInvoke();
	}
	
	public void TimerRefresh()
	{
		_timerPublisher.NotifyTimeRefresh();
		_lastTime = _timerLeft;
	}
	
	public void TimerTickNotify()
	{
		if((_lastTime - _timerLeft) >= _timeSpan)
		{
			// LiHaojie 2013.01.20 always notify
			_timerPublisher.NotifyTimer(0);
			
			NetSender.Instance.RequestGetRaid(_copyID);
			Debug.Log("请求服务器时间： " + _timerLeft);
		}
		
		if(_timerLeft > 0)
		{
			_timerPublisher.NotifyTimer(_timerLeft--);
		}
		
		if(_timerLeft <= 0)
		{
			TimerEnd();
			NetSender.Instance.RequestGetRaid(_copyID);
		}
	}
	
	public int TimeLeft
	{
		get{ return _timerLeft; }
		set{ _timerLeft = value;}
	}
	
	public int CopyID
	{
		get{ return _copyID; }
		set{ _copyID = value;} 
	}
	
	public long BeginTime
	{
		get{ return _beginTime; }
		set{ _beginTime = value;}
	}
	
	public int AlreadyNumber
	{
		set{ _alreadyNumber = value;}
	}
	
	public bool IsSweepNormalFinish
	{
		set{ _isSweepNormalFinish = value;}
	}
	
	public int ConsumeIngot
	{
		set{ _consumeIngot = value;}
	}
	
	public bool ShowSweepUI
	{
		set{ _showSweepUI = value;}
		get{ return _showSweepUI; }
	}
	
	private bool _isSweeping; 
	private int _copyID; // curr sweep copy
	private int _timerLeft;
	private int _lastTime;
	private int _timeSpan = 100;
	private long _beginTime;
	private int _alreadyNumber;
	private bool _isSweepNormalFinish;//whether the sweep is normally finish
	private int _consumeIngot; // speed sweep consume ingot
	private bool _showSweepUI;
	public bool autoSaleItem;
	
	private TimerTickPublisher _timerPublisher = new TimerTickPublisher();
	//public GUICopySweep.EButtonType selectConsumeRadioType;
	public CopySweepData _copySweepData;
}
