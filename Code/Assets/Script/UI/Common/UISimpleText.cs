using UnityEngine;
using System.Collections;
/// <summary>
/// just to show simple text which was attach to a parent object
/// </summary>
/// 
/// 
/// 
namespace EZGUI.Common
{
	public class UISimpleText
	{
		public SpriteText SimpleText
		{
			get{ return _simpleText; }
		}
		
		public UISimpleText(Object uISimpleTextPreb,Transform parent,Vector3 offset,string textStr,float characterSize,
			Color color,SpriteText.Anchor_Pos anchor)
		{
		//	//get the position of the parent 
		//	Vector3 parentPosition = parent.position;   
		//
		//	GameObject spriteTextObj = (GameObject) GameObject.Instantiate(uISimpleTextPreb,new Vector3(0,0,0),Quaternion.identity);
		//	_simpleText = spriteTextObj.GetComponent<SpriteText>();
		//	_simpleText.transform.parent = parent;
		//	
		//	//set the offset
		//	_simpleText.transform.position = new Vector3(parent.position.x + offset.x,parent.position.y + offset.y,parent.position.z + offset.z);
		//	
		//	_simpleText.Text = textStr;
		//	_simpleText.SetCharacterSize(characterSize);
		//	_simpleText.Color = color;
		//	_simpleText.Anchor = anchor;
		
		}
		
		//construct 
		public UISimpleText(Transform parent,Vector3 offset,string textStr)
		{
			GameObject simpleTextObj = (GameObject) GameObject.Instantiate(UIResourceLoadUtil.LoadSpriteText(),Vector3.zero,Quaternion.identity);
			simpleTextObj.transform.parent = parent;
			simpleTextObj.transform.localPosition = offset;
			_simpleText = (SpriteText) simpleTextObj.GetComponent<SpriteText>();
			_simpleText.Text = textStr;
		}

		//set the attribute of the text
		public void SetText(string textStr)
		{
			_simpleText.Text = textStr;
		}
		
		//get text
		public string GetText()
		{
			return _simpleText.text.ToString();   
		}
		//set the color
		public void SetColor(Color color)
		{
			if (null == _simpleText)
				return;
			
			_simpleText.SetColor(color);
		}
		
		public void SetColorPre(int index)
		{
			_simpleText.SetColor(_textColor[index]);
		}
		
		//get the color
		public Color GetColor()
		{
			return _simpleText.color;
		}
		
		
		
		//set the size
		public void SetSize(float size)
		{
			_simpleText.SetCharacterSize(size);
		}
		
		//set enable
		public void SetEnable(bool state)
		{
			_simpleText.gameObject.active = state;
		}
		
		// set the anchor
		public void SetAnchor(SpriteText.Anchor_Pos anchor)
		{
			_simpleText.Anchor = anchor;
		}
		
		//set alignment
		public void SetAlignment(SpriteText.Alignment_Type type)
		{
			_simpleText.alignment = type;
		}
		
		//set the max width
		public void SetMaxWidth(float width)
		{
			_simpleText.maxWidth = width;
		}
		
		//change language
		public void SetLanguage()
		{
			
		}
	
		private SpriteText _simpleText;
	
		private float _offsetZ;
	
		private float _characterSpacing;
		private float _lineSpacing;
	 	private SpriteText.Anchor_Pos _anchor;
		private Color[] _textColor = 
		{
			new Color32(244,218,159,255),
			new Color32(188,149,67,255),
			new Color32(255,138,0,255),
			new Color32(255,234,0,255),
			new Color32(89,210,0,255),
			new Color32(20,125,255,255),
			new Color32(208,22,242,255),
			new Color32(255,28,12,255),
			new Color32(226,67,255,255),
			new Color32(68,147,255,255),
			new Color32(220,71,71,255),
			new Color32(232,211,140,255),
		};
	}

}
