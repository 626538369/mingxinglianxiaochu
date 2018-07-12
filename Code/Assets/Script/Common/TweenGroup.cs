using UnityEngine;  
using System.Collections;  
  
public class TweenGroup:MonoBehaviour {  
	
	enum TweenValueType
	{
		VECTOR3 = 1,
		FLOAT = 2,
		INTEGRE = 3,
	};
	
	public int TweenGroupID;
	public delegate void OnTweenGroupFinishedEvent(GameObject gameObj,bool isautoJump);
	[HideInInspector] public event TweenGroup.OnTweenGroupFinishedEvent TweenFinishedEvents = null;
	private int mTweenCellCount = 0;
	private TweenGroupConfig.TweenGroupObject mTweenGroupObject;
	
	
	public void setTweenGroupID(int id)
	{
		TweenGroupID = id;
	}
	
	
    void Start()  
    {  
   
    }  
	
	public void playTweenAnimation()
	{
		TweenGroupConfig tweenGroupConfig = Globals.Instance.MDataTableManager.GetConfig<TweenGroupConfig>();
		TweenGroupConfig.TweenGroupObject tweenGpObj;
		tweenGroupConfig.GetItemElement(TweenGroupID,out tweenGpObj);
		mTweenGroupObject = tweenGpObj;
		for (int i=0; i<tweenGpObj.TweenCellInfoList.Count; i++)
		{
			TweenGroupConfig.TweenCellInfo cellInfo = tweenGpObj.TweenCellInfoList[i];
			if (cellInfo.TweenName == "TweenPosition")
			{
				GameObject tweenObj;
				if (cellInfo.TweenGameObject != "")
					tweenObj = GameObject.Find(cellInfo.TweenGameObject);
				else
					tweenObj = gameObject;
				if(tweenObj == null)
					continue;
					
				if (cellInfo.TweenValueType == (int)TweenValueType.VECTOR3)
				{
					Vector3 toPosition = StrParser.ParseVec3(cellInfo.TweenValueTo);
					toPosition += gameObject.transform.localPosition;
					TweenPosition tweenPosition = TweenPosition.Begin(tweenObj,cellInfo.TweenDuration, toPosition);
					if (cellInfo.StartTime > 0)
						tweenPosition.delay = cellInfo.StartTime;
					EventDelegate.Add(tweenPosition.onFinished, onCellTweenFinished);
					mTweenCellCount ++;
				}
			}
			else if (cellInfo.TweenName == "TweenAlpha")
			{
				GameObject tweenObj;
				if (cellInfo.TweenGameObject != "")
					tweenObj = GameObject.Find(cellInfo.TweenGameObject);
				else
					tweenObj = gameObject;
				if (cellInfo.TweenValueType == (int)TweenValueType.FLOAT)
				{
					float toValue = StrParser.ParseFloat(cellInfo.TweenValueTo,0);
					TweenAlpha tweenAlpha = TweenAlpha.Begin(tweenObj,cellInfo.TweenDuration,toValue);
					tweenAlpha.from = StrParser.ParseFloat(cellInfo.TweenValueFrom,0);
					if (cellInfo.StartTime > 0)
						tweenAlpha.delay = cellInfo.StartTime;
					EventDelegate.Add(tweenAlpha.onFinished , onCellTweenFinished);
					mTweenCellCount ++;
				}
			}
			else if (cellInfo.TweenName == "Shake")
			{
				GameObject tweenObj;
				if (cellInfo.TweenGameObject != "")
					tweenObj = GameObject.Find(cellInfo.TweenGameObject);
				else
					tweenObj = gameObject;
		
				iTween.ShakePosition(tweenObj, new Vector3(20,20,0),1,delegate(){
					onCellTweenFinished();
				});
				
				mTweenCellCount ++;

			}
		}

	}
	
	private void onCellTweenFinished()
	{
		mTweenCellCount--;
		if (mTweenCellCount == 0)
		{
			if (TweenFinishedEvents != null)
			{
				bool isAutoJump = mTweenGroupObject.IsAutoJump == 1? true:false;
				TweenFinishedEvents(gameObject,(bool)isAutoJump);
			}
		}
	}
	
   

      
}  