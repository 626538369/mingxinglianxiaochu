using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class InfoProgressBar : MonoBehaviour {
	

	private string		mFormerLabel = "";
	public UISprite 		MainProgressBarHeader;
	public UISprite         SpriteHua;
	public UISprite         BackroundSprite;
	public bool				PercentStyle = false;
	public bool 			UseColor = false;
	public bool 			UseEffect = false;
	public string			Label;
	public Color[] colors = new Color[] { Color.red, Color.yellow, Color.green };
	public GameObject[] Effect;
	
	private long			mValue = 0;
	private long			mMaxValue = 100;
	private long			mStartValue = 0;
	private float			mStartValueFloat = 0;
	private float			mIncreateDeltaTime = 0;
	private int 			EffectState = -1;
	
	void Awake(){
		mFormerLabel 	= Label;
//		if (SpriteHua != null)
//			SpriteHua.transform.localPosition = Vector3.zero;

	}
	
	/// <summary>
	/// Sets the max value.
	/// </summary>
	/// <param name='maxValue'>
	/// Max value.
	/// </param>
	public void SetMaxValue(long maxValue){
		mMaxValue	= maxValue;
	}
	
	/// <summary>
	/// Gets the start value.
	/// </summary>
	/// <value>
	/// The start value.
	/// </value>
	public long StartValue{
		get{return mStartValue;}
	}
	
	// Use this for initialization
	public void SetValue (long val,long startVal = 0,float incTime = 1.0f) {
		mValue				= val;
		mStartValue			= startVal;
		
		if(mStartValue > mValue){
			mStartValue = mValue;
		}
		
		mStartValueFloat	= (float)mStartValue;
		if(incTime > 0){
			mIncreateDeltaTime	= (mValue - mStartValue) / incTime;
		}else{
			mIncreateDeltaTime	= 99999;
		}
		RefreshProgressBar();
	}
	
	// Update is called once per frame
	void Update () {
		if(mStartValue < mValue){
			mStartValueFloat += mIncreateDeltaTime * Time.deltaTime;
			mStartValue = (int)mStartValueFloat;
			
			if(mStartValue >= mValue){
				mStartValue = mValue;
			}
			
			RefreshProgressBar();
		}
		
		if(mFormerLabel != Label){
			mFormerLabel = Label;
			//mLabelText.Text = Label;
		}
	}
	
	public void Hide(bool hide){

	}
	
	
	/// <summary>
	/// Refreshs the progress bar by value
	/// </summary>
	private void RefreshProgressBar(){
		MainProgressBarHeader.fillAmount = mStartValueFloat/mMaxValue;
	}
	
	public void setProgressBarValue(float prograValue)
	{
		mStartValue = 1; 
		mValue = 0;
		MainProgressBarHeader.fillAmount = prograValue/(float)mMaxValue;
		float flowerLength = BackroundSprite.transform.localScale.x;
		if (SpriteHua != null)
		SpriteHua.transform.localPosition = new Vector3(flowerLength*-0.5f + MainProgressBarHeader.fillAmount*flowerLength,0,-5);
	}
	
	public void SetRadiaProgressBarValue(float angle,float Radiu)
	{
		MainProgressBarHeader.fillAmount = angle;
		float RadiaAngle = 2*angle*Mathf.PI;
		SpriteHua.transform.localPosition = new Vector3( Radiu*Mathf.Sin(RadiaAngle),Radiu*Mathf.Cos(RadiaAngle),0);
		
		if(UseColor)
		{
			SetColorToProgressBar(angle);
		}
		if(UseEffect)
		{
			if(EffectState != (int)(angle/(1.0f/Effect.Length)))
			{
				EffectState = (int)(angle/(1.0f/Effect.Length));
				SwitchEffect();
			}
			
		}
	}
	
	private void SwitchEffect()
	{
		if(EffectState == Effect.Length)
		{
			NGUITools.SetActive(Effect[Effect.Length - 1].gameObject,true);
			ParticleSystem effectObj = Effect[Effect.Length - 1].gameObject.GetComponent<ParticleSystem>();
			effectObj.Play();
			
			for(int i = 0; i < Effect.Length-1; i++)
			{
				NGUITools.SetActive(Effect[i].gameObject,false);
			}
		}else
		{		
			for(int i = 0; i < Effect.Length; i++)
			{
				if(i == EffectState)
				{
					NGUITools.SetActive(Effect[i].gameObject,true);
					ParticleSystem effectObj = Effect[i].gameObject.GetComponent<ParticleSystem>();
					effectObj.Play();
				}else
				{
					NGUITools.SetActive(Effect[i].gameObject,false);
				}
			}
		}
	}
	
	private void ReadEffect(GameObject obj,bool isShow)
	{
		if(obj != null)
		{
			if(isShow)
			{
				NGUITools.SetActive(obj.gameObject,true);
				ParticleSystem effectObj = obj.gameObject.GetComponent<ParticleSystem>();
				effectObj.Play();
			}else
			{
				NGUITools.SetActive(obj.gameObject,false);
			}
		}
	}
	
	private void SetColorToProgressBar(float angle)
	{
		
		if (MainProgressBarHeader == null || colors.Length == 0) return;
		float val = angle;
		val *= (colors.Length - 1);
		int startIndex = Mathf.FloorToInt(val);

		Color c = colors[0];

		if (startIndex >= 0)
		{
			if (startIndex + 1 < colors.Length)
			{
				float factor = (val - startIndex);
				c = Color.Lerp(colors[startIndex], colors[startIndex + 1], factor);
			}
			else if (startIndex < colors.Length)
			{
				c = colors[startIndex];
			}
			else c = colors[colors.Length - 1];
		}

		c.a = MainProgressBarHeader.color.a;
		MainProgressBarHeader.color = c;
	}

}

	
	