using UnityEngine;
using System.Collections;

public class FixedTimeRefreshPosition : MonoBehaviour {

	Vector3[] vec = new Vector3[]{new Vector3(-360,-610,0) , new Vector3(440,-260,0),new Vector3(-180,-140,0) , new Vector3(-30,-450,0)};
	void Start () {
	
		InvokeRepeating("InvokeRefresh" , 0.5f, 0.5f);
	}
	
	void InvokeRefresh()
	{
		this.gameObject.transform.localPosition = vec[Random.Range(0,vec.Length)];
	}

	public void StopRefresh()
	{
		CancelInvoke("InvokeRefresh");
		this.gameObject.transform.localPosition = vec[3];
	}

	void Update () {
	
	}
}
