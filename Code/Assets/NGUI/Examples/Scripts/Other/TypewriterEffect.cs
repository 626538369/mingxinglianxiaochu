//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2014 Tasharen Entertainment
//----------------------------------------------

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/// <summary>
/// Trivial script that fills the label's contents gradually, as if someone was typing.
/// </summary>

[RequireComponent(typeof(UILabel))]
[AddComponentMenu("NGUI/Examples/Typewriter Effect")]
public class TypewriterEffect : MonoBehaviour
{
	public int charsPerSecond = 16;

	UILabel mLabel;
	string mText;
	int mOffset = 0;
	float mNextChar = 0f;
	bool mTyping = false;
	
	public delegate void OnTypeWriteStopEvent();
	[HideInInspector] public event TypewriterEffect.OnTypeWriteStopEvent TypeWriteStopEvent = null;
	
 	private  void OnEnable()
	{
		if (mLabel == null)
		{
			mLabel = GetComponent<UILabel>();
			mLabel.supportEncoding = true;
			mLabel.pivot = UIWidget.Pivot.TopLeft;
		}
	}
	
	struct ColorInfo
	{
		public int startIndex;
		public string color;
	};
	
	private List<ColorInfo> colorList = new List<ColorInfo>();
	
	public void  EnabledTypewriter()
	{
		mTyping = true;
		mLabel.supportEncoding = true;
		mOffset = 0;
		mNextChar = 0;
		colorStr = "";
		mLabel.pivot = UIWidget.Pivot.TopLeft;
		colorList.Clear();
		int colorCount = 0;
		int addIter = 0;
		string CopymLabel = mLabel.text;
		while (mLabel.text.Contains("{"))
		{
			int index = mLabel.text.IndexOf("{");
			mLabel.text = mLabel.text.Remove(index,8);
		}
		mLabel.Wrap(mLabel.text, out mText);
		
		int NIndex = 0;
		
		while(mText.Contains("\n"))
		{
			NIndex = mText.IndexOf("\n");
			break;
		}
		while (CopymLabel.Contains("{"))
		{
			int index = CopymLabel.IndexOf("{");
			if(NIndex < 1)
			{
				addIter = 0;
			}else
			{
				if (index >= NIndex)
				addIter = 1;
				if(index >= NIndex*2)
				{
					addIter = 2;
				}
				if(index >= NIndex*3)
				{
					addIter = 3;
				}
			}
			ColorInfo aColorInfo = new ColorInfo();
			aColorInfo.startIndex = addIter + index + colorCount*8;
			aColorInfo.color =  CopymLabel.Substring(index,8);
			CopymLabel = CopymLabel.Remove(index,8);
			colorList.Add(aColorInfo);
			colorCount++;
		}
			
		
		for (int i=0; i<colorList.Count; i++)
		{
			mText = mText.Insert(colorList[i].startIndex,colorList[i].color);
		}
	}
	
	public bool IsTypewriterIng()
	{
		return mTyping;
	}
	
	public void FinishTypewriter(){  
		mTyping = false;
		colorList.Clear();
		int colorCount = 0;
		while(mText.Contains("{"))
		{
			int index = mText.IndexOf("{");
			ColorInfo aColorInfo = new ColorInfo();
			aColorInfo.startIndex = index + colorCount*8;
			aColorInfo.color =  mText.Substring(index,8);
			aColorInfo.color = aColorInfo.color.Replace('{','[');
			aColorInfo.color = aColorInfo.color.Replace('}',']');
			mText = mText.Remove(index,8);
			colorList.Add(aColorInfo);
			colorCount++;
		}
		
		for (int i=0; i<colorList.Count; i++)
		{
			mText = mText.Insert(colorList[i].startIndex,colorList[i].color);
		}
		
        mLabel.text  =  mText;                     
   	
		
		if(TypeWriteStopEvent!=null)
		{
			TypeWriteStopEvent();
			//TypeWriteStopEvent = null;
		}
    }  
	string colorStr = "";
	void Update ()
	{
		
		if (!mTyping)
			return ;
		
		if (mOffset < mText.Length)
		{
			mTyping = true;
			if (mNextChar <= Time.time)
			{
				charsPerSecond = Mathf.Max(1, charsPerSecond);

				// Periods and end-of-line characters should pause for a longer time.
				float delay = 1f / charsPerSecond;
				char c = mText[mOffset];
				if (c == '.' || c == '\n' || c == '!' || c == '?') delay *= 4f;
				if (c == '{')
				{
					colorStr = mText.Substring(mOffset,8);
					colorStr = colorStr.Replace('{','[');
					colorStr = colorStr.Replace('}',']');
					mText = mText.Remove(mOffset,8);
					mText = mText.Insert(mOffset,colorStr);
					mOffset+=8;
					mNextChar = Time.time + delay;
					mLabel.text =  mText.Substring(0, ++mOffset);
					colorStr = "";
				}
				else{
					mNextChar = Time.time + delay;
					mLabel.text =  mText.Substring(0, ++mOffset);
				}
	
		
			}
		}
		else
		{
			FinishTypewriter();
		}
	}
}
