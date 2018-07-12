using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGame : MonoBehaviour {

	public Button PlayBtn;

	void Awake () {
		PlayBtn.onClick.AddListener (delegate() {
			PlayBtn.gameObject.SetActive(false);


			OnClickPlayBtn(1010010);
		});
	}



	public void OnClickPlayBtn(int level){


		EliminationMgr.Instance.PrepareGame (level.ToString());
	}
}
