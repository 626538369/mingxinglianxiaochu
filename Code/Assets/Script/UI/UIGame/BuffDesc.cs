using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffDesc : MonoBehaviour {
	public UITexture PropIcon;
	public UILabel descLabel;
	private  string desc_;

	public void init(UITexture t,string desc)
	{
		this.gameObject.SetActive (true);
		PropIcon.mainTexture = t.mainTexture;
		desc_ = desc;
	}
	public void Onclick()
	{
		descLabel.text = desc_;
		descLabel.transform.parent.gameObject.SetActive (true);
		StartCoroutine(showLabeIE ());
	}

	public IEnumerator showLabeIE () {
		yield return new WaitForSeconds (1.5f);
		descLabel.transform.parent.gameObject.SetActive (false);
	}
}
