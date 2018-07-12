using UnityEngine;  
using System.Collections;  
  
public class WatchEncounter:MonoBehaviour {  
	
//	public UISprite SpriteHundrandValue;
//	public UISprite SpriteTenValue;
//	public UISprite SpriteOneValue;
	public UISprite MinusMark;
	public string mColorName = "Zjm";
//	private int mInitValue;
	private int mCurrentValue;
	private int mIncreaseValue;
	private int mDecreaseValue;
	private int mIngValue;
	
//	private int mHundrandValue = 0;
//	private int mTenValue = 0;
//	private int mOneValue  = 0;
	
	
	public delegate void OnIncreaseOrDecreaseEvent(GameObject gameObj);
	[HideInInspector] public event WatchEncounter.OnIncreaseOrDecreaseEvent FinishedEnterEvents = null;
	
	
	public UILabel labelNumber;
	
	public int NumberText = 0;
	
    void Start()  
    {  
   		if (MinusMark != null)
			NGUITools.SetActive(MinusMark.gameObject,false);
    }  
	
	public void setColorName(string name)
	{
		mColorName = name;
	}
   
	public int getWatchValue()
	{
		return mCurrentValue ;
	}
	
	private void setMinusMarkShow(bool show)
	{
		if (MinusMark != null)
		{
			MinusMark.spriteName = "Num" + mColorName + "Jian";
			NGUITools.SetActive(MinusMark.gameObject,show);
		}
	}
	
	public void setInitValue(int iValue)
	{
//		mInitValue = iValue;
		mCurrentValue = iValue;
//		mHundrandValue = iValue / 100;
//		mTenValue = (iValue % 100)/10;
//		mOneValue = (iValue % 100)%10;
		mIngValue = iValue;
//		if (mHundrandValue > 0)
//		{
//			NGUITools.SetActive(SpriteHundrandValue.gameObject,true);
//			SpriteHundrandValue.spriteName = "Num" + mColorName + mHundrandValue.ToString();
//		}
//		else
//		  	NGUITools.SetActive(SpriteHundrandValue.gameObject,false);
//			
//		if (mTenValue > 0 || mInitValue > 100)
//		{
//			NGUITools.SetActive(SpriteTenValue.gameObject,true);
//			SpriteTenValue.spriteName = "Num" + mColorName + mTenValue.ToString();
//		}
//		else
//			NGUITools.SetActive(SpriteTenValue.gameObject,false);
//		
//		if (mOneValue >= 0 || mInitValue > 10 )
//		{
//			NGUITools.SetActive(SpriteOneValue.gameObject,true);
//			SpriteOneValue.spriteName = "Num" + mColorName + mOneValue.ToString();
//		}
//		else
//			NGUITools.SetActive(SpriteOneValue.gameObject,false);
		NumberText = iValue;
		labelNumber.text = iValue.ToString();
		
		if (MinusMark != null)
			NGUITools.SetActive(MinusMark.gameObject,false);
	}
	public void increaseValue(int increaseValue)
	{
		mIncreaseValue += increaseValue;
		mCurrentValue = mCurrentValue + mIncreaseValue;
		if (mIncreaseValue == 0 && FinishedEnterEvents != null )
			FinishedEnterEvents(gameObject);
	}
	
	public void increaseByTimes(int times)
	{
		if (times > 1)
		{
			mIncreaseValue = mCurrentValue*(times - 1);
			if (mIncreaseValue == 0 && FinishedEnterEvents != null )
				FinishedEnterEvents(gameObject);
			mCurrentValue = mCurrentValue + mIncreaseValue;
		}
	}
	
	public void increaseByFloatTimes(float times)
	{
		if (times > 0)
		{
			mIncreaseValue = Mathf.Abs((int)((float)mCurrentValue*times));
			if (mIncreaseValue == 0 && FinishedEnterEvents != null )
				FinishedEnterEvents(gameObject);
			 mIncreaseValue = (int)Mathf.Ceil(mIncreaseValue);
			if(mIncreaseValue == 0)
			{
				mIncreaseValue = 1;
			}
			mCurrentValue = mCurrentValue + mIncreaseValue;
		}
	}
	
	
	public void decreaseValue(int decValue)
	{
		mDecreaseValue = decValue;
		if (mDecreaseValue == 0 && FinishedEnterEvents != null )
				FinishedEnterEvents(gameObject);
		mCurrentValue = mCurrentValue - mDecreaseValue;
	}
	
	public void decreaseByTimes(float times)
	{
		if (times > 0)
		{
			mDecreaseValue = (int) (mCurrentValue*(times));
			if (mIncreaseValue == 0 && FinishedEnterEvents != null )
				FinishedEnterEvents(gameObject);
			mCurrentValue = mCurrentValue - mIncreaseValue;
		}
	}
	
	public void Update()
	{
		if (mIncreaseValue > 0)
		{
			mIncreaseValue--;
			mIngValue++;
				
			labelNumber.text = mIngValue.ToString();
			
			if (mIncreaseValue == 0)
				if (FinishedEnterEvents != null)
					FinishedEnterEvents(gameObject);
		}
		
		if (mDecreaseValue>0)
		{
			mDecreaseValue--;
			mIngValue--;
				
			labelNumber.text = mIngValue.ToString();
			if (mDecreaseValue == 0)
				if (FinishedEnterEvents != null)
					FinishedEnterEvents(gameObject);
		}
	}
      
}  