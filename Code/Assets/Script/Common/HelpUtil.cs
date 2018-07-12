using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Diagnostics;
using System.Text;

public class HelpUtil
{
	public static void JsTime2CSharpDateTime(long jsTime,out System.DateTime dtt)
	{
		// The C# Ticks unit is 100 * 1ns, and the Java timeStamp unit is Milliseconds
		// 621355968000000000L Ticks is the TimeStamp of (from 0001-01-01 00:00:00  to 1970-01-01 00:00:00)
		// long t_delta = new System.DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).Ticks;
		// long t_delta = new System.DateTime(1970, 1, 1, 0, 0, 0).Ticks;

		// The GMT TimeZone is different with Local TimeZone
		long cTicks = jsTime * 10000 + 621355968000000000L; // GMT + 8Time zone
	    dtt = System.TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(cTicks));
	}
	public static string JsTime2CSharpHour(long jsTime)
	{
		// The C# Ticks unit is 100 * 1ns, and the Java timeStamp unit is Milliseconds
		// 621355968000000000L Ticks is the TimeStamp of (from 0001-01-01 00:00:00  to 1970-01-01 00:00:00)
		// long t_delta = new System.DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).Ticks;
		// long t_delta = new System.DateTime(1970, 1, 1, 0, 0, 0).Ticks;

		// The GMT TimeZone is different with Local TimeZone
		long cTicks = jsTime * 10000 + 621355968000000000L; // GMT + 8Time zone
		System.DateTime dtt = System.TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(cTicks));
		return dtt.ToString("HH:mm");
	}
	// 1 s = 10^3 ms = 10^6 us = 10^9 ns
	public static string JsTime2CSharp(long jsTime)
	{
		// The C# Ticks unit is 100 * 1ns, and the Java timeStamp unit is Milliseconds
		// 621355968000000000L Ticks is the TimeStamp of (from 0001-01-01 00:00:00  to 1970-01-01 00:00:00)
		// long t_delta = new System.DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).Ticks;
		// long t_delta = new System.DateTime(1970, 1, 1, 0, 0, 0).Ticks;

		// The GMT TimeZone is different with Local TimeZone
		long cTicks = jsTime * 10000 + 621355968000000000L; // GMT + 8Time zone
		System.DateTime dtt = System.TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(cTicks));
		return dtt.ToString("yyyy-MM-dd HH:mm:ss");
	}

    // 1 s = 10^3 ms = 10^6 us = 10^9 ns
    public static string JsTime2CSharpWithoutSeconds(long jsTime)
    {
        // The C# Ticks unit is 100 * 1ns, and the Java timeStamp unit is Milliseconds
        // 621355968000000000L Ticks is the TimeStamp of (from 0001-01-01 00:00:00  to 1970-01-01 00:00:00)
        // long t_delta = new System.DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).Ticks;
        // long t_delta = new System.DateTime(1970, 1, 1, 0, 0, 0).Ticks;

        // The GMT TimeZone is different with Local TimeZone
        long cTicks = jsTime * 10000 + 621355968000000000L; // GMT + 8Time zone
        System.DateTime dtt = System.TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(cTicks));
        return dtt.ToString("yyyy-MM-dd HH:mm");
    }
	
	/// <summary>
	/// Times the stamp format.
	/// 1365415509 will export 2013-04-08 18:05:09
	/// </summary>
	/// <returns>
	/// The stamp format.
	/// </returns>
	/// <param name='timeStamp'>
	/// Time stamp 
	/// </param>
	public static string TimeStampFormat(int timeStamp){
		
		// The GMT TimeZone is different with Local TimeZone
		long cTicks = timeStamp * 10000000L + 621355968000000000L; // GMT + 8Time zone
		System.DateTime dtt = System.TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(cTicks));

		return dtt.ToString("yyyy-MM-dd HH:mm:ss");
	}
	
	public static string GetObjectFullName(Transform t){
		string path = t.name;
		Transform pt = t.parent;
		
		while(pt != null){
			path = pt.name + "/" + path;
			pt = pt.parent;
		}
		
		return path;
	}
	
	public static string GetObjectFullName(GameObject go){
		return GetObjectFullName(go.transform);
	}
	
	/// <summary>
	/// Gets the call stack.
	/// </summary>
	/// <returns>
	/// The call stack.
	/// </returns>
	public static string GetCallStack(){

        StackTrace trace = new StackTrace(1, true);

        StringBuilder builder = new StringBuilder();
        for (int i = 0; i < trace.FrameCount; i++){
            StackFrame frame = trace.GetFrame(i);
            if (i != 0){
				builder.Append("  ");
			}

            builder.Append(frame.GetMethod());

            string fileName = frame.GetFileName();

            if (!string.IsNullOrEmpty(fileName)){
				builder.Append(" (").Append(fileName)
						.Append(": ").Append(frame.GetFileLineNumber()).Append(")");
			}
            builder.AppendLine();
        }

        return builder.ToString();
    }
	
	public static SkillDamageRange ParseSkillRange(string val)
	{
		// [[-1,-1],[1,-1],[-1,1],[1,1]]
		SkillDamageRange range = new SkillDamageRange();
		range._damageRectList.Clear();
		
		// Add the primary damage rect[0, 0]
		{
			SkillDamageRange.Rect rect = new SkillDamageRange.Rect();
			rect.x = 0;
			rect.z = 0;
			
			Vector3 gridPos = GetGridPosition(rect.x, rect.z);
			rect.left = gridPos.x - 0.5f * GameDefines.BATTLE_GRID_WIDTH;
			rect.right = gridPos.x + 0.5f * GameDefines.BATTLE_GRID_WIDTH;
			rect.top = gridPos.z + 0.5f * GameDefines.BATTLE_GRID_HEIGHT;
			rect.bottom = gridPos.z - 0.5f * GameDefines.BATTLE_GRID_HEIGHT;
			
			range._damageRectList.Add(rect);	
		}
		val = val.Replace("[", "");
		val = val.Replace("]", "");
		
		// Parse others
		char []splitter = {','};
		string[] vecs = val.Split(splitter);
		if (vecs.Length == 0)
		{
			return range;
		}
		
		if ( vecs.Length == 1 
			// Equal with "0"
			&& (vecs[0].CompareTo("0") == 0) )
		{
			return range;
		}
		
		if (vecs.Length % 2 != 0)
		{
			return range;
		}
		
		for (int i = 0; i < vecs.Length; i += 2)
		{
			SkillDamageRange.Rect rect = new SkillDamageRange.Rect();
			rect.x = StrParser.ParseDecInt(vecs[i], -10000);
			rect.z = StrParser.ParseDecInt(vecs[i + 1], -10000);
			
			Vector3 gridPos = GetGridPosition(rect.x, rect.z);
			rect.left = gridPos.x - 0.5f * GameDefines.BATTLE_GRID_WIDTH;
			rect.right = gridPos.x + 0.5f * GameDefines.BATTLE_GRID_WIDTH;
			rect.top = gridPos.z + 0.5f * GameDefines.BATTLE_GRID_HEIGHT;
			rect.bottom = gridPos.z - 0.5f * GameDefines.BATTLE_GRID_HEIGHT;
			
			range._damageRectList.Add(rect);
		}
		
		return range;
	}
	
	public static void CalculateGrid(string grid, out int x, out int z)
	{
		x = int.MinValue;
		z = int.MinValue;
		
		if (grid == null)
			return;
		
		List<int> values = StrParser.ParseDecIntList(grid, -1);
		if (0 == values.Count || 2 != values.Count)
			return;
		
		x = values[0];
		
		// The coordinate system is similar with window coordinate,so convert to 3d coordinate
		z = GameDefines.BATTLE_GRID_VERTICALMAX_NUM - values[1];
	}
	
	public static void Calculate2DGrid(string grid, out int x, out int z)
	{
		x = int.MinValue;
		z = int.MinValue;
		
		if (grid == null)
			return;
		
		List<int> values = StrParser.ParseDecIntList(grid, -1);
		if (0 == values.Count || 2 != values.Count)
			return;
		
		x = values[0];
		z = values[1];
	}
	
	public static Vector3 GetGridPosition(string grid)
	{
		if (grid == null)
			return Vector3.zero;
		
		List<int> values = StrParser.ParseDecIntList(grid, -1);
		if (0 == values.Count || 2 != values.Count)
			return Vector3.zero;
		
		int xGrid = values[0];
		// The coordinate system is similar with window coordinate,so convert to 3d coordinate
		int zGrid = GameDefines.BATTLE_GRID_VERTICALMAX_NUM - values[1];
		
		return new Vector3(xGrid * GameDefines.BATTLE_GRID_WIDTH, 0.0f, zGrid * GameDefines.BATTLE_GRID_HEIGHT);
	}
	
	public static Vector3 GetWarFiledGridPosition(string grid)
	{
		if (grid == null)
			return Vector3.zero;
		
		List<int> values = StrParser.ParseDecIntList(grid, -1);
		if (0 == values.Count || 2 != values.Count)
			return Vector3.zero;
		int xGrid = values[0];
		
		// by lsj for the reason which in the battle of port vie we need to reserve the fleet position so our fleet is always in the left of the screen
		if(GameStatusManager.Instance.MBattleStatus.MBattleResult.BattleType == GameData.BattleGameData.BattleType.PORT_VIE_BATTLE &&
			Globals.Instance.MPortVieManager.puFlagReverseFleetPosition)
		{
			xGrid = GameDefines.BATTLE_GRID_HORIZONTAL_MAX_NUM - 1 - values[0];
		}
		
		int centerX = GameDefines.BATTLE_GRID_HORIZONTAL_MAX_NUM;
		centerX = Mathf.FloorToInt( (float)centerX * 0.5f );
		xGrid -= centerX;
		
		// The coordinate system is similar with window coordinate,so convert to 3d coordinate
		int zGrid = GameDefines.BATTLE_GRID_VERTICALMAX_NUM - values[1];
		// int zGrid = values[1];
		int centerZ = GameDefines.BATTLE_GRID_VERTICALMAX_NUM;
		centerZ = Mathf.FloorToInt( (float)centerZ * 0.5f );
		zGrid -= centerZ;
		
		return new Vector3(xGrid * GameDefines.BATTLE_GRID_WIDTH, 0.0f, zGrid * GameDefines.BATTLE_GRID_HEIGHT);
	}
	
	public static Vector3 GetGridPosition(int x, int z)
	{
		return new Vector3(x * GameDefines.BATTLE_GRID_WIDTH, 0.0f, z * GameDefines.BATTLE_GRID_HEIGHT);
	}
	
	public static Vector3 GetSplitVector3(string pos)
	{
		List<float> values = StrParser.ParseFloatList(pos, float.MinValue);
		if (values.Count != 3)
		{
			throw new MissingReferenceException("1111");
		}
		
		return new Vector3(values[0], values[1], values[2]);
	}
	
	// Test the Line segment is intersect with Rect
	public static bool Intersect(Rect rect, Vector3 a, Vector3 b, out Vector3 crossPoint)
	{
		crossPoint = Vector3.zero;
		
		// All inside
		if ((rect.Contains(a) && rect.Contains(b)) || (!rect.Contains(a) && !rect.Contains(b))){
			return false;
		}
		
		if ((rect.Contains(a) && !rect.Contains(b)) || (rect.Contains(b) && !rect.Contains(a)) ){
			
			if(HelpUtil.FloatEquals(a.x,b.x)){
				
				crossPoint.x = b.x;
				
				if(b.y > rect.yMax){
					crossPoint.y = rect.yMax;	
				}else{
					crossPoint.y = rect.yMin;
				}
			}else if(HelpUtil.FloatEquals(a.y,b.y)){
				
				crossPoint.y = b.y;
				
				if(b.x > rect.xMax){
					crossPoint.x = rect.xMax;	
				}else{
					crossPoint.x = rect.xMin;
				}
			}else{
				
				
				// line : 
				// kx+c=y
				//
				float k = (b.y - a.y) / (b.x - a.x);
				float k1 = rect.height / rect.width;
				
				float c = b.y - (k * b.x);
				
				if(k > 0){
								
					if(k > k1){
						if(b.x > a.x){
							crossPoint.x = (rect.yMax - c) / k;
							crossPoint.y = rect.yMax;	
						}else{
							crossPoint.x = (rect.yMin - c) / k;
							crossPoint.y = rect.yMin;
						}
					}else{
						if(b.x > a.x){
							crossPoint.x = rect.xMax;
							crossPoint.y = k * rect.xMax + c;
						}else{
							crossPoint.x = rect.xMin;
							crossPoint.y = k * rect.xMin + c;
						}						
					}
					
				}else{
									
					if(Mathf.Abs(k) > k1){
						if(b.x > a.x){
							crossPoint.x = (rect.yMin - c) / k;
							crossPoint.y = rect.yMin;
						}else{
							crossPoint.x = (rect.yMax - c) / k;
							crossPoint.y = rect.yMax;
						}
						
					}else{
						if(b.x > a.x){
							crossPoint.x = rect.xMax;
							crossPoint.y = k * rect.xMax + c;
						}else{
							crossPoint.x = rect.xMin;
							crossPoint.y = k * rect.xMin + c;
						}
					}
				}
			}
				
			return true;
		}

		return false;
	}
	
	/// <summary>
	/// Gets the state of the button or touch
	/// </summary>
	/// <returns>
	/// The button state.
	/// </returns>
	/// <param name='downOrUp'>
	/// If set to <c>true</c> down or up.
	/// </param>
	public static bool GetButtonState(bool downOrUp){
				
		bool tState = false;
		
#if UNITY_ANDROID || UNITY_IPHONE
		if(Application.isEditor){
			tState = downOrUp?Input.GetButtonDown("Fire1"):Input.GetButtonUp("Fire1");
		}else{
			if(Input.touchCount > 0){
				if(Input.GetTouch(0).phase == (downOrUp?TouchPhase.Began:TouchPhase.Ended)){
					tState = true;
				}			
			}
		}
#else
		tState = downOrUp?Input.GetButtonDown("Fire1"):Input.GetButtonUp("Fire1");
#endif
		
		return tState;
	}
	
	/// <summary>
	/// Gets the button or touch position.
	/// </summary>
	/// <returns>
	/// The button position.
	/// </returns>
	public static Vector3 GetButtonPosition(){
		Vector3 tPos;
		
#if UNITY_ANDROID || UNITY_IPHONE
		if(Application.isEditor){
			tPos = Input.mousePosition;
		}else{
			Vector2 tmp = Input.GetTouch(0).position;
			tPos = new Vector3(tmp.x,tmp.y,0);		
		}
#else
		tPos = Input.mousePosition;
#endif
		
		return tPos;
	}
	
	/// <summary>
	/// return the current time stamp
	/// </summary>
	/// <returns>
	/// return the current time stamp
	/// </returns>
	public static long CurrTimeStamp(){
		// 
		//long t_delta = System.DateTime.Parse("1970-01-01 00:00:00").Ticks;
		//t_delta == 621355968000000000
		//
		return (System.DateTime.UtcNow.Ticks - 621355968000000000L) / 10000;
	}
	
	/**
	 * tzz added for get the money text (formatted)
	 */ 
	public static  string GetMoneyFormattedText(int iMoney){
		string tMoneyString = iMoney.ToString();
		
		if(tMoneyString.Length < 4)
		{
			return tMoneyString;
		}else if(tMoneyString.Length < 6)
		{
			return iMoney .ToString("N0");
		}
		else
		{
			return (iMoney / 1000f).ToString("N0") + Globals.Instance.MDataTableManager.GetWordText(1009);
		}
	}
	
	private static readonly Vector3 InvisiblePos= new Vector3(0,3000,0);
	private static Vector3 VisiblePos			= Vector3.zero;
	private static bool		InitializedPos		= false;
	
	/**
	 * tzz added
	 * set the item icon sub-function
	 * 
	 * @param	_itemIcon		item icon prefab
	 * @param	_itemInfo		item data information
	 */ 
	public static void SetItemIcon(Transform _itemIcon,ItemSlotData _itemInfo,bool _lockedOrEmpty = true){

		//SpriteText numTex = _itemIcon.FindChild("Count").GetComponent<SpriteText>();
		// SpriteText newTex = _itemIcon.FindChild("NewText").GetComponent<SpriteText>();
		PackedSprite sprite = _itemIcon.Find("Icon").GetComponent<PackedSprite>();
		PackedSprite Plus = _itemIcon.Find("PlusIndicator").GetComponent<PackedSprite>();
		PackedSprite sprite1 = _itemIcon.Find("IconQiangHua1").GetComponent<PackedSprite>();
		PackedSprite sprite2 = _itemIcon.Find("IconQiangHua2").GetComponent<PackedSprite>();
		PackedSprite sprite3 = _itemIcon.Find("IconQiangHua3").GetComponent<PackedSprite>();
		PackedSprite sprite4 = _itemIcon.Find("IconQiangHua4").GetComponent<PackedSprite>();
		PackedSprite sprite5 = _itemIcon.Find("IconQiangHua5").GetComponent<PackedSprite>();
		List<PackedSprite>  iconQianghuaList = new List<PackedSprite>();
		iconQianghuaList.Add(sprite1);
		iconQianghuaList.Add(sprite2);
		iconQianghuaList.Add(sprite3);
		iconQianghuaList.Add(sprite4);
		iconQianghuaList.Add(sprite5);
		for (int i=0; i<iconQianghuaList.Count; i++)
		{
			PackedSprite packeQianghua = iconQianghuaList[i];
			packeQianghua.transform.localScale = Vector3.zero;
		}
		
		if(!InitializedPos){
			InitializedPos = true;
			VisiblePos		=  new Vector3(0,0,0); //numTex.transform.localPosition;
		}
		
		// the PlayAnim has adjust whether gameobject is actived otherwise no effect
		//
		bool tFormerSpriteActive = sprite.gameObject.active;
		sprite.gameObject.active = true;
		
		if (_itemInfo != null && _itemInfo.IsUnLock())
		{
			if (null != _itemInfo.MItemData)
			{
				if (_itemInfo.MItemData.BasicData.Count > 1)
				{
					//numTex.transform.localPosition = VisiblePos;
					//numTex.Text = _itemInfo.MItemData.BasicData.Count.ToString();
					//if(numTex.transform.localScale.x == 1 && Globals.Instance.MGUIManager.widthRatio != 1){
						//numTex.transform.localScale = new Vector3(1/Globals.Instance.MGUIManager.widthRatio,
																//1/Globals.Instance.MGUIManager.heightRatio,1);					
					//}
				}else{
					//numTex.transform.localPosition = InvisiblePos;
				}
				
				// newText.Text = "New!";
				sprite.Hide(false);
				sprite.PlayAnim(_itemInfo.MItemData.BasicData.Icon);
				
				if ( _itemInfo.MItemData.BasicData.StrengthenLevel > 1)
				{
					int qianghuaXingLeve =( _itemInfo.MItemData.BasicData.StrengthenLevel -1) /20;
					for (int i=0; i<=qianghuaXingLeve; i++)
					{
						PackedSprite packeQianghua = iconQianghuaList[i];
						packeQianghua.transform.localScale = Vector3.one;
					}
				}
			}
			else
			{
				//numTex.transform.localPosition = InvisiblePos;
				Plus.transform.localPosition = InvisiblePos;
				
				sprite.PlayAnim(PackageItemIconSlot.IconNullNormal);
			}
		}
		else
		{
			//numTex.transform.localPosition = InvisiblePos;
			Plus.transform.localPosition = InvisiblePos;
			
			sprite.PlayAnim(_lockedOrEmpty ? PackageItemIconSlot.IconLock : PackageItemIconSlot.IconNullNormal);
		}
		
		// restore the active of gameobject
		sprite.gameObject.active = tFormerSpriteActive;		
	}
	
	/// <summary>
	/// judge whether two float number the equals.
	/// </summary>
	/// <returns>
	/// The equals.
	/// </returns>
	/// <param name='a'>
	/// If set to <c>true</c> a.
	/// </param>
	/// <param name='b'>
	/// If set to <c>true</c> b.
	/// </param>
	public static bool FloatEquals(float a,float b){
		return Mathf.Abs(a - b) < 1e-4;
	}
	
	/// <summary>
	/// Parses the hex string.
	/// </summary>
	/// <returns>
	/// The hex string.
	/// </returns>
	/// <param name='bytes'>
	/// Bytes.
	/// </param>
	public static string ParseHexString(byte[] bytes){
		System.Text.StringBuilder sb = new System.Text.StringBuilder();
		
		for(int i = 0;i < bytes.Length;i++){
			
			sb.Append(bytes[i].ToString("X2")).Append(" ");
			
			if((i + 1) % 16 == 0){
				sb.Append("\n");
			}
		}
		
		return sb.ToString();
	}
	
	public static int CalcPortOilOutput(long influence, int level)
	{
		float val = 5f * Mathf.Pow((float)level, 0.25f) + Mathf.Pow((float)influence, 0.5f) + 10f;
		return (int)val;
	}
	
	public static long GetTimeSpan(DateTime lt)
	{
		DateTime nowTime  = DateTime.Now;
		Debug.Log(nowTime.ToString());
		TimeSpan ts  = nowTime.Subtract(lt);
		
		return Mathf.RoundToInt(ts.Ticks / 10000);
	}
	
	
	public static void DestroyObject(UnityEngine.Object obj)
	{
		if (null == obj) return;
		
		if (Application.isEditor)
		{
			GameObject.DestroyImmediate(obj);
		}
		else
		{
			GameObject.DestroyObject(obj);
		}
	}
	
	public static void DelListInfo(Transform grid)
	{
		List<Transform> temp=new List<Transform>();
		foreach(Transform tf in grid)
		{
			temp.Add(tf);
		}

		foreach(Transform tf in temp)
		{
			GameObject.DestroyImmediate(tf.gameObject);
			//GameObject.DestroyObject(tf.gameObject);
		}
	}
	public static void HideListInfo(Transform grid,bool isHide)
	{
		List<Transform> temp=new List<Transform>();
		foreach(Transform tf in grid)
		{
			temp.Add(tf);
		}
		
		foreach(Transform tf in temp)
		{
			tf.gameObject.SetActive(!isHide);
		}
	}
	
	public static void SetRenderEnable(GameObject go,bool flag)
	{
		MeshRenderer render = go.GetComponent(typeof(MeshRenderer)) as MeshRenderer;
		if(render != null)
			go.GetComponent<Renderer>().enabled = flag;
		for (int i = 0; i < go.transform.GetChildCount(); ++i)
		{
			GameObject child = go.transform.GetChild(i).gameObject;
			SetRenderEnable(child,flag);
		}
	}
	
	public static void SetPlarticleState(GameObject particleObj,bool bPlay)
	{
		ParticleSystem[] systems  = particleObj.GetComponentsInChildren<ParticleSystem>();
		foreach(ParticleSystem Part in systems)
		{
			if(true == bPlay )
			{
				Part.Play();
			}
			else
			{
				Part.Stop();
			}
				
		}
	}
	
	public static void AndroidNativeCallStatic(string classFullname,string method, params object[] args){
		
		if(Application.isEditor){
			return;
		}
		
#if UNITY_ANDROID	
		using(AndroidJavaClass jc = new AndroidJavaClass(classFullname))  
        {
			jc.CallStatic(method, args);
			Debug.Log(classFullname + " Method ["+method+"] called");	
        }
#endif
	}
	
	public static T AndroidNativeCallStatic<T>(string classFullname,string method, params object[] args){
			
		if(!Application.isEditor){
			
#if UNITY_ANDROID
			using(AndroidJavaClass jc = new AndroidJavaClass(classFullname))  
	        {
				T result = jc.CallStatic<T>(method, args);
				Debug.Log(classFullname + " Method ["+method+"] called result [" + result + "]");	
				
				return result;
	        }
#endif
		}
		
		return System.Activator.CreateInstance<T>();
	}
}
