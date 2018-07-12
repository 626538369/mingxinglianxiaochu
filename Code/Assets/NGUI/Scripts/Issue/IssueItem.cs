using UnityEngine;
using System.Collections;

public class IssueItem : MonoBehaviour {
	
	GameObject _currentObj;
	GameObject tweenObj;
	GameObject lastTweenObj;
	public float DisplayTime = 0.1f;
	//int Dep = 3;
	[HideInInspector]
	public GameObject CurrentObj
	{
		set{		
			//上一个隐藏//
			if(null != _currentObj && null!= lastTweenObj && value != _currentObj)
			{	
				UIPlayTween playTweenFalse = _currentObj.GetComponent<UIPlayTween>();
				TweenHeight tweenHeight = lastTweenObj.transform.parent.GetComponent<TweenHeight>();
				//if(NGUITools.GetActive(lastTweenObj))
				if(tweenHeight.direction == AnimationOrTween.Direction.Forward)
				{
					NGUITools.SetActive(lastTweenObj,false);
					playTweenFalse.Play(false);
					Transform Arrow = _currentObj.transform.Find("Picture").Find("ArrowObject");
					Arrow.GetComponent<TweenRotation>().PlayReverse();
				}
			}	
		 	//当前展开//
			_currentObj = value;
			lastTweenObj = tweenObj;
			StopCoroutine("Do");
			StartCoroutine("Do",tweenObj);
		}
	}
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	//由于不能用checkbox，需要自己显示或隐藏图片//
	public void DisplayDownObj(GameObject Obj,GameObject Obj2)
	{
		UIWidget widget = Obj.transform.Find("Picture").Find("CheckmarkNormal").GetComponent<UIWidget>();
		TweenHeight tweenHeight = Obj.GetComponent<TweenHeight>();
		//if(NGUITools.GetActive(Obj2) == false)
		if(tweenHeight.direction == AnimationOrTween.Direction.Reverse)
		{
			//NGUITools.SetActive(Obj,false);
			//widget.enabled = false;
			//widget.depth  = Dep -1;
			widget.alpha = 1.0f;
		}
		else
		{
			//NGUITools.SetActive(Obj,true);
			//widget.enabled = true;
			//widget.depth = Dep + 1;
			widget.alpha = 0.0f;
		}
	}
	//Obj为了使上一个返回,TweenObj为了控制显示内容//
	public void SetCurrentObj(GameObject Obj,GameObject TweenObj)
	{
		tweenObj = TweenObj;
		CurrentObj = Obj;	
	}
	IEnumerator  Do (GameObject Obj)  
	{  
		bool bActive = false;
		TweenHeight tweenHeight = _currentObj.GetComponent<TweenHeight>();
		//bActive = NGUITools.GetActive(tweenObj);
		bActive = tweenHeight.direction == AnimationOrTween.Direction.Forward;
		if( tweenHeight.tweenFactor == 0)	
		{
			UIWidget widget = _currentObj.transform.Find("Picture").Find("CheckmarkNormal").GetComponent<UIWidget>();
			//widget.depth = Dep + 1;
			widget.alpha = 0.0f;
			yield return new WaitForSeconds (DisplayTime);  
			NGUITools.SetActive(Obj,true);
		}	
		else
		{
			if(tweenHeight.direction == AnimationOrTween.Direction.Reverse)
			{
				yield return new WaitForSeconds (DisplayTime);  
			  	NGUITools.SetActive(tweenObj,true);
			}
			else
				NGUITools.SetActive(tweenObj,false);
		}
	} 

}
