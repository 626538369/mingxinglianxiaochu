using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class ScriptForChangeWordConfig : MonoBehaviour {

	public int WordConfigID = 0;
	public UILabel ShowWord;
	void Start () {
		if (ShowWord == null) {
			ShowWord = this.GetComponent<UILabel>();
		}
		if (WordConfigID != 0) {
			ShowWord.text = Globals.Instance.MDataTableManager.GetWordText(WordConfigID);
		}else{
			ShowWord.text = "key_Not_Found";
			Debug.LogError(ShowWord.text);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
