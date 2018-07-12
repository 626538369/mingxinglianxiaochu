using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RichLabel : MonoBehaviour 
{

	public UILabel  textLabel;
	public UISprite spriteCommonPrefab;
	
	private const string IconTag = "[Icon:";
	private const string RichEnd = "end]";
	private float charWidth;
	private List<GameObject> dynamicGameObjectList = new List<GameObject>();
	
	void Awake()
	{
		charWidth = textLabel.font.texWidth;
		charWidth = textLabel.transform.localScale.x + 8;
	}
	
	void OnDestroy()
	{
	
	}
	
	public void setText(string text)
	{
		releaseDynamicGameObject();
		if (text.Contains(IconTag))
		{
			int startIndex = text.IndexOf(IconTag);
			int endIndex = text.IndexOf(RichEnd);
			int count = endIndex - startIndex + RichEnd.Length;
			string iconStr = text.Substring(startIndex,count);
			string priorStr = text.Substring(0,startIndex);
			Vector2 position = this.getStringSize(priorStr);
			UISprite icon1 = generateIconFromStr(iconStr);
			string spaceText = "    ";
			if ((textLabel.lineWidth - position.x) < icon1.transform.localScale.x)
			{
				spaceText += "    ";
				position.x = 0;
				position.y += charWidth;
			}
			
			icon1.transform.transform.localScale = new Vector3(charWidth*2,charWidth*2,1);
			//icon1.transform.localPosition = new Vector3(position.x - icon1.transform.localScale.x /2, position.y - icon1.transform.localScale.y /2 ,-6);
			icon1.transform.localPosition = new Vector3(position.x , position.y ,-6);
			
			text = text.Remove(startIndex,count);
			text = text.Insert(startIndex,spaceText);
						
			textLabel.text = text;			
		}
		else
		{
			textLabel.text = text;
		}
		
	}
	
	private Vector2 getStringSize(string str)
	{
		Vector2 size = new Vector2(0,0);
		int count = str.Length;
		float allLongth = (float)count*charWidth;
		float xWidth = allLongth %textLabel.lineWidth;
		int lineNum = (int)(allLongth / textLabel.lineWidth);
		float yWidth = (float)lineNum * charWidth;
		size.x = xWidth;
		size.y = yWidth;
		return size;
	}
	
	private UISprite generateIconFromStr(string iconStr)
	{
		UISprite uiSprite = null;
		string [] strList = iconStr.Split(':');
		if (strList.Length > 3)
		{
			if (strList[1] == "AtlasCommon")
			{
				uiSprite = GameObject.Instantiate(spriteCommonPrefab) as UISprite;
				uiSprite.transform.parent = this.gameObject.transform;
				dynamicGameObjectList.Add(uiSprite.gameObject);
			}
			
			uiSprite.spriteName = strList[2];
			return uiSprite;
		}
		return uiSprite;
	}
	
	private void releaseDynamicGameObject()
	{
		for (int i=0; i<dynamicGameObjectList.Count; i++)
		{
			GameObject.DestroyImmediate(dynamicGameObjectList[i]);
		}
		dynamicGameObjectList.Clear();
	}
}