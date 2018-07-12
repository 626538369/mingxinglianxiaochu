using UnityEngine;
using System.Collections;

public class KnightFullInfo : MonoBehaviour 
{
	public SpriteText nameText;
	public SpriteText pingzhiText;
	public SpriteText levelText;
	public SpriteText jinHuaText;
	public SpriteText expText;
	
	public SpriteText attackText;
	public SpriteText defenseText;
	public SpriteText lifeText;
	public SpriteText rangeText;
	public SpriteText hitText;
	public SpriteText dodgeText;
	public SpriteText critText;
	
	public SpriteText[] signatureTextList;
	public UIImageButton[] signatureBtnList;
	
	public Transform chatBubbleTf;
	[HideInInspector] Bubble chatBubble = null;
	private float bubbleDuration = 1.0f;
	private bool isBubbleVisible = false;
	
	public void Hide(bool hide)
	{
		gameObject.SetActiveRecursively(!hide);
	}
	
	public void UpdateFullInfo(GirlData data)
	{
		if (null == data)
		{
			return;
		}
		
//		nameText.Text = data.GetDisplayName();
		//pingzhiText.Text = data.BasicData.GetDisplayQualityName();
		
		string szFormat = Globals.Instance.MDataTableManager.GetWordText(23700020);
		levelText.Text = string.Format(szFormat, data.PropertyData.Level);
		
		string evoltStr = string.Format(Globals.Instance.MDataTableManager.GetWordText(29000000),data.XiakeEvolutionLevel);
		if (jinHuaText != null)
			jinHuaText.Text = evoltStr;

	}
	
	private void OnClickSignatureBtn(GameObject obj)
	{
		UIImageButton signBtn = (UIImageButton)obj.GetComponent<UIImageButton>();
		bubbleDuration = 1.0f;
		string descriptStr =  (string)signBtn.Data;
		Vector3 bubblePos = signBtn.transform.position;
		bubblePos.x -= 120.0f;
		bubblePos.y += 1.2f* signBtn.GetComponent<Collider>().bounds.size.y;
		bubblePos.z = chatBubble.transform.parent.transform.position.z;
			
		chatBubble.transform.parent.transform.position = bubblePos;
		chatBubble.transform.parent.gameObject.SetActiveRecursively(true);
		chatBubble.SetText(descriptStr);
		isBubbleVisible = true;
			
		//StartCoroutine("BubbleInvisible");
	}
	

	
	IEnumerator BubbleInvisible()
	{
		yield return new WaitForSeconds(bubbleDuration);
		
		isBubbleVisible = false;
		chatBubble.transform.parent.gameObject.SetActiveRecursively(false);
	}
	
	public void Update () 
	{
		if (isBubbleVisible)
		{
			bubbleDuration -= Time.deltaTime;
			if (bubbleDuration< 0)
			{
				isBubbleVisible = false;
				chatBubble.transform.parent.gameObject.SetActiveRecursively(false);
			}
		}
	}
	
	void UpdatePropInfo(GirlData data)
	{
		attackText.Text = Globals.Instance.MDataTableManager.GetWordText(11030001) 
			+ data.PropertyData.SeaAttack.ToString();
		defenseText.Text = Globals.Instance.MDataTableManager.GetWordText(11030002) 
			+ data.PropertyData.Defense.ToString();
		lifeText.Text = Globals.Instance.MDataTableManager.GetWordText(11030003) 
			+ data.PropertyData.MaxLife.ToString();
		
		rangeText.Text = Globals.Instance.MDataTableManager.GetWordText(11030004) 
			+ (data.PropertyData.FillSpeed).ToString();
		hitText.Text = Globals.Instance.MDataTableManager.GetWordText(11030005) 
			+ (data.PropertyData.HitRate / 100).ToString();
		dodgeText.Text = Globals.Instance.MDataTableManager.GetWordText(11030006) 
			+ (data.PropertyData.DodgeRate / 100).ToString();
		critText.Text = Globals.Instance.MDataTableManager.GetWordText(11030007) 
			+ (data.PropertyData.CritRate / 100).ToString();
	}
}
