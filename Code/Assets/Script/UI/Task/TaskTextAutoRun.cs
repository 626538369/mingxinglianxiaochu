using UnityEngine;
using System.Collections;

public class TaskTextAutoRun : MonoBehaviour {

	void Start ()
	{
		mShowText = gameObject.GetComponent( typeof(SpriteText) ) as SpriteText;
		Color c = new Color(r/255.0f, g/255.0f, b/255.0f, 0);
		mShowText.SetColor(c);
	}
	
	void Update ()
	{
		float y = gameObject.transform.localPosition.y;
		gameObject.transform.localPosition = new Vector3(-100,y+Time.deltaTime*(mEndPos-mStartPos)/(mInDurTime+mKeepDurTime+mOutDurTime),-10);
		
		if(y < mStartPos)
			return;
		
		time += Time.deltaTime;
		if(time <= mInDurTime)
		{
			Color c = new Color(r/255.0f, g/255.0f, b/255.0f, time/mInDurTime);
			mShowText.SetColor(c);
		}
		else if(time <= (mInDurTime+mKeepDurTime))
		{
		}
		else if(time <= (mInDurTime+mKeepDurTime+mOutDurTime))
		{
			Color c = new Color(r/255.0f, g/255.0f, b/255.0f, 1.0f-(time-mInDurTime-mKeepDurTime)/mOutDurTime);
			mShowText.SetColor(c);
		}
		else
		{
		}
		
	}
	
	SpriteText mShowText = null;
	float time = 0;
	
	public float mInDurTime = 2;
	public float mKeepDurTime = 3;
	public float mOutDurTime = 5;
	public float mStartPos = -240;
	public float mEndPos = 240;
	
	private float r = 244.0f;
	private float g = 218.0f;
	private float b = 159.0f;
}
