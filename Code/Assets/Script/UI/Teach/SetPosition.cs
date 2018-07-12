using UnityEngine;
using System.Collections;

public class SetPosition : MonoBehaviour {

	public UIWidget BoxLeft;
	public UIWidget BoxRight;
	public UIWidget BoxUp;
	public UIWidget BoxDown;
	public UISprite Arrow;
	
	protected void Awake()
	{

	}

	void Start ()
	{
		
	}

	void Update ()
	{
	
	}

	public void SetUp(float curWidth,float curHeight,Vector3 arrowPositon,float arrowRotate)
	{
		BoxLeft.transform.localPosition = new Vector3(-curWidth/2,0f,0f);
		BoxRight.transform.localPosition = new Vector3(curWidth/2,0f,0f);
		BoxUp.transform.localPosition = new Vector3(0f,curHeight/2,0f);
		BoxDown.transform.localPosition = new Vector3(0f,-curHeight/2,0f);
		Arrow.transform.localPosition = arrowPositon;
		Arrow.transform.localEulerAngles = new Vector3(0f,arrowRotate,-15f);

	}

}
