using UnityEngine;
using System.Collections;
using DG.Tweening;

public class ScoreItem : MonoBehaviour
{
	public UILabel scoreText;
	private int score_;
	private int type_;
	public void Init(int type ,int score)
	{
		score_ = score;
		type_ = type;
		EliminationMgr.Instance.addScoreItemFly.Add (this);
		if (null != Globals.Instance.MGUIManager.GetGUIWindow<GUIPhotoGraph> ()) {
			transform.SetParent(Globals.Instance.MGUIManager.GetGUIWindow<GUIPhotoGraph> ().transform,true);
		}
//		transform.SetParent(Globals.Instance.UISTown.transform,true);
		transform.localScale = Vector3.one;
		scoreText.text = score.ToString();
		transform.DOLocalMove(new Vector3(-500f,767f,0),1f).SetEase(Ease.InOutQuad).SetDelay(0.5f).OnComplete(delegate(){
			EliminationMgr.Instance.addScoreItemFly.Remove(this);
			EliminationMgr.Instance.PopupScore(type_,score);
			Destroy(gameObject);
		});
	}

//	public void OnMoveComplete()
//	{
//		EliminationMgr.Instance.PopupScore(type_,score_);
//		Destroy(gameObject);
//	}

}

