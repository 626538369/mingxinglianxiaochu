using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePreFieldTips : MonoBehaviour {

	public UIButton AddStepBtn;
	public UIButton ReStartBtn;
	public UIButton GiveUpBtn;

	public UISlider ScoreSlider;
	public GameObject Star1Obj;
	public GameObject Star1Anim;
	public GameObject Star2Obj;
	public GameObject Star2Anim;
	public GameObject Star3Obj;
	public GameObject Star3Anim;

	public UILabel CurrentScore;

	public GameObject IngrObject;
	public UISprite Ingr1Sprite;
	public UILabel Ingr1Num;
	public GameObject Ingr1Success;
	public GameObject Ingr1Fail;

	public UISprite Ingr2Sprite;
	public UILabel Ingr2Num;
	public GameObject Ingr2Success;
	public GameObject Ingr2Fail;

	public UISprite BlockSprite;
	public UILabel BlockNum;
	public GameObject BlockSuccess;
	public GameObject BlockFail;

	public UILabel ConditionText;

	void Awake(){

		UIEventListener.Get (AddStepBtn.gameObject).onClick += OnClickAddStepBtn;
		UIEventListener.Get (ReStartBtn.gameObject).onClick += OnClickReStartBtn;
		UIEventListener.Get (GiveUpBtn.gameObject).onClick += OnClickGiveUpBtn;
	}

	public void ShowPreFieldTips(){

		ScoreSlider.value = (EliminationMgr.Score*1f) / EliminationMgr.Instance.star3;
		float baseX = 630f / EliminationMgr.Instance.star3;
		float baseX2 = 630f / 2f;
		float star1X = baseX * EliminationMgr.Instance.star1 - baseX2;
		Star1Obj.transform.localPosition = new Vector3 (star1X , Star1Obj.transform.localPosition.y,Star1Obj.transform.localPosition.z);
		float star2X = baseX * EliminationMgr.Instance.star2 - baseX2;
		Star2Obj.transform.localPosition = new Vector3 (star2X , Star2Obj.transform.localPosition.y,Star2Obj.transform.localPosition.z);
		float star3X = baseX * EliminationMgr.Instance.star3 - baseX2;
		Star3Obj.transform.localPosition = new Vector3 (star3X , Star3Obj.transform.localPosition.y,Star3Obj.transform.localPosition.z);

		if(EliminationMgr.Score >= EliminationMgr.Instance.star1){
			Star1Anim.SetActive (true);
		}
		if(EliminationMgr.Score >= EliminationMgr.Instance.star2){
			Star2Anim.SetActive (true);
		}
		if(EliminationMgr.Score >= EliminationMgr.Instance.star3){
			Star3Anim.SetActive (true);
		}

		CurrentScore.text = EliminationMgr.Score.ToString ();


		IngrObject.SetActive (false);
		BlockSprite.gameObject.SetActive (false);
		ConditionText.gameObject.SetActive (false);


		if(EliminationMgr.Instance.target == Target.SCORE){
			ConditionText.gameObject.SetActive (true);

			ConditionText.text = Globals.Instance.MDataTableManager.GetWordText("6013") + EliminationMgr.Instance.star1.ToString();
		}else if(EliminationMgr.Instance.target == Target.COLLECT || EliminationMgr.Instance.target == Target.INGREDIENT){
			if(EliminationMgr.Instance.ingrMaxCountTarget[0] > 0 || EliminationMgr.Instance.ingrMaxCountTarget[1] > 0){
				IngrObject.SetActive (true);
				if (EliminationMgr.Instance.target == Target.INGREDIENT) {
					Ingr1Sprite.spriteName = "item" + (int)EliminationMgr.Instance.ingrTarget [0];
					Ingr2Sprite.spriteName = "item"+(int)EliminationMgr.Instance.ingrTarget [1];
				} else if (EliminationMgr.Instance.target == Target.COLLECT) {
					Ingr1Sprite.spriteName = "item" + (int)EliminationMgr.Instance.collectItems [0];
					Ingr2Sprite.spriteName = "item"+(int)EliminationMgr.Instance.collectItems [1];
				}

				if (EliminationMgr.Instance.ingrMaxCountTarget[0] == 0 && EliminationMgr.Instance.ingrMaxCountTarget[1] > 0) {
					Ingr1Sprite.gameObject.SetActive (false);
				} else if (EliminationMgr.Instance.ingrMaxCountTarget[0] > 0 && EliminationMgr.Instance.ingrMaxCountTarget[1] == 0) {
					Ingr2Sprite.gameObject.SetActive (false);
				}

				Ingr1Num.text = EliminationMgr.Instance.ingrMaxCountTarget[0].ToString();
				Ingr2Num.text = EliminationMgr.Instance.ingrMaxCountTarget[1].ToString();

				if (EliminationMgr.Instance.ingrCountTarget [0] <= 0) {
					Ingr1Success.SetActive (true);
					Ingr1Fail.SetActive (false);
				} else {
					Ingr1Success.SetActive (false);
					Ingr1Fail.SetActive (true);
				}

				if (EliminationMgr.Instance.ingrCountTarget [1] <= 0) {
					Ingr2Success.SetActive (true);
					Ingr2Fail.SetActive (false);
				} else {
					Ingr2Success.SetActive (false);
					Ingr2Fail.SetActive (true);
				}
			}
		}else if(EliminationMgr.Instance.target == Target.BLOCKS){
			BlockSprite.gameObject.SetActive (true);
			BlockSprite.spriteName = "block";
			BlockNum.text = EliminationMgr.Instance.TargetMaxBlocks.ToString ();
			if (EliminationMgr.Instance.TargetMaxBlocks <= 0) {
				BlockSuccess.SetActive (true);
				BlockFail.SetActive (false);
			} else {
				BlockSuccess.SetActive (false);
				BlockFail.SetActive (true);
			}
		}


		this.gameObject.SetActive (true);
	}

	public void OnClickGiveUpBtn(GameObject obj){
		
		EliminationMgr.Instance.GameStatus = GameStatusEnum.GameOver;
	}
	public void OnClickAddStepBtn(GameObject obj){

		if (Globals.Instance.MGameDataManager.MActorData.WealthData.GoldIngot < 50) {
			Globals.Instance.MGUIManager.PopupNotEnoughDiamond ();
		} else {
			GUIRadarScan.Show ();
			NetSender.Instance.C2GSEliminationAddStepReq ();
		}
	}

	public void AddStepRes(int addstep){
		this.gameObject.SetActive (false);
		// 具体的逻辑处理
		EliminationMgr.Instance.Limit += addstep;
		EliminationMgr.Instance.GameFieldAnimationEndStartGame ();
	}

	public void OnClickReStartBtn(GameObject obj){
		this.gameObject.SetActive (false);
		// 具体的逻辑处理
		EliminationMgr.Instance.ReStart();
	} 
}
