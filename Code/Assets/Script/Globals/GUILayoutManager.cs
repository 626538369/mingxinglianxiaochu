using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GUILayoutManager : MonoBehaviour 
{
	void Awake()
	{
		_mWindowLayout = null;
		_mCoexistNamePairList = new List<string>();
		
		_mCoexistNamePairList.Add("GUIBag");
		_mCoexistNamePairList.Add("GUIGeneral");
		_mCoexistNamePairList.Add("GUIGeneralPotion");
		_mCoexistNamePairList.Add("GUIGeneralProperty");
		_mCoexistNamePairList.Add("GUIGeneralTrain");
	}
	
	// Use this for initialization
	void Start () 
	{
	
	}
	
	void OnDestroy()
	{
		_mWindowLayout = null;
		_mCoexistNamePairList.Clear();
	}
	
	public void CreateDoubleLayout(GUIWindow w1, GUIWindow w2, float duration, float delay)
	{
		List<GUIWindow> winList = Globals.Instance.MGUIManager.GetGUIWindowList();
		
		// Close other coexsit windows
		winList.RemoveAll(delegate (GUIWindow w)
		{
			if (w == w1 || w == w2)
				return false;
			
			if (w.IsVisible && _mCoexistNamePairList.Contains(w.GetType().Name))
			{
				w.Close();
				return true;
			}
			
			return false;
		}
		);
		
		_mWindowLayout = new GUIWindowLayoutDouble(w1, w2, duration, delay);
		_mWindowLayout.Start();
	}
	
	public void RemoveFromLayout(GUIWindow w)
	{
		if (null != _mWindowLayout)
		{
			_mWindowLayout.Remove(w);
		}
	}
	
	private GUIWindowLayout _mWindowLayout;
	private List<string> _mCoexistNamePairList;
}
