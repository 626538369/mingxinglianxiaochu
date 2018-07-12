using UnityEngine;
using System.Collections;

public class TaskStateEffect : MonoBehaviour {

	void Start () {
		vecpos = gameObject.transform.localPosition;
	}
	
	void Update ()
	{
		gameObject.transform.localPosition = new Vector3(vecpos.x, vecpos.y+Mathf.Sin(Time.time*2)*2, vecpos.z);
	}
	Vector3 vecpos;
}
