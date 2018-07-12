using UnityEngine;
using System.Collections;

public class TaskTrackButton : MonoBehaviour {
	
	void Start ()
	{
		enabled = false;
		return;
		
		GoToLeft();
		mPosSrc = new Vector3(gameObject.transform.localPosition.x,-120,0);
	}
	
	void Update ()
	{
		return;
		
		if(isToDown)
		{
			if(mTime < 0.4f)
				mTime += Time.deltaTime;
		}
		if(isToUp)
		{
			if(mTime > 0)
				mTime -= Time.deltaTime;
			else
				GoToLeft();
		}
		if(mTime > 0)
			gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x, mPosSrc.y-mSpeed*mTime*180, mPosSrc.z);
	}
	
	public void GoToDown()
	{
		return;
		
		isToDown = true;
		isToUp = false;
	}
	
	public void GoToUp()
	{
		return;
		
		isToDown = false;
		isToUp = true;
	}
	
	public void GoToLeft()
	{
		return;
		
		gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x,-12000,0);
	}
	
	private bool isToDown = false;
	private bool isToUp = false;
	private float mTime = 0;
	public float mSpeed;
	private Vector3 mPosSrc;
}
