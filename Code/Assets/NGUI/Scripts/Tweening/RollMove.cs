using UnityEngine;
using System.Collections;

public class RollMove : MonoBehaviour {

	public float OnceTime = 20f;
	public float PositionX = -2320f;
	TweenPosition tween1 ;
	UIPanel panel ;
	void Start () {
		panel = this.transform.GetComponent<UIPanel>();
		StartCoroutine(TimeToWait());
	}
	void Update () {
		panel.clipOffset = new Vector2(this.transform.localPosition.x*-1 + 60 , 0);
	}

	IEnumerator TimeToWait()
	{
		yield return new WaitForSeconds(1f); 
		tween1 = TweenPosition.Begin(this.gameObject,OnceTime,new Vector3(PositionX,this.transform.localPosition.y,this.transform.localPosition.z));
		tween1.style = UITweener.Style.Loop;
	}
}
