using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Linq;

public class GetSpellCode : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public static string GetCharacter(string str)
	{
		string Initial = "";
		for(int i = 0; i < str.Length; i++)
		{
			Initial += GetInitial(str.Substring(i,1));
		}
		return Initial;
	}
	
	public static string GetInitial(string chars)
	{
		long Code;
		byte[] ZW = System.Text.Encoding.Default.GetBytes(chars);
		if(ZW.Length == 1) //如果是英文字符直接返回对应字母的大写//
		{
			return chars.ToUpper();
		}
		else
		{
			int i1 = (short)ZW[0];
			int i2 = (short)ZW[1];
			Code = i1 * 256 + i2;
		}
		if(Code >= 45217 && Code <= 45252)
		{
			return "A";
		}
		else if(Code >= 45253 && Code<= 45760)
		{
			return "B";
		}
		else if(Code >= 45761 && Code<= 46317)
		{
			return "C";
		}
		else if(Code >= 46318 && Code<= 46825)
		{
			return "D";
		}
		else if(Code >= 46826 && Code<= 47009)
		{
			return "E";
		}
		else if(Code >= 47010 && Code<= 47296)
		{
			return "F";
		}
		else if(Code >= 47297 && Code<= 47613)
		{
			return "G";
		}
		else if(Code >= 47614 && Code<= 48118)
		{
			return "H";
		}
		else if(Code >= 48119 && Code<= 49061)
		{
			return "J";
		}
		
		else if(Code >= 49062 && Code<= 49323)
		{
			return "K";
		}
		else if(Code >= 49324 && Code<= 49895)
		{
			return "L";
		}
		else if(Code >= 49896 && Code<= 50370)
		{
			return "M";
		}
		else if(Code >= 50371 && Code<= 50613)
		{
			return "N";
		}
		else if(Code >= 50614 && Code<= 50621)
		{
			return "O";
		}
		else if(Code >= 50622 && Code<= 50905)
		{
			return "P";
		}
		else if(Code >= 50906 && Code<= 51386)
		{
			return "Q";
		}
		else if(Code >= 51387 && Code<= 51445)
		{
			return "R";
		}
		else if(Code >= 51446 && Code<= 52217)
		{
			return "S";
		}
		else if(Code >= 52218 && Code<= 52697)
		{
			return "T";
		}
		else if(Code >= 52698 && Code<= 52979)
		{
			return "W";
		}
		else if(Code >= 52980 && Code<= 53640)
		{
			return "X";
		}
		else if(Code >= 53689 && Code<= 54480)
		{
			return "Y";
		}
		else if(Code >= 54481 && Code<= 55289)
		{
			return "Z";
		}
		else
		{
			return "?";
		}
			
		 
	}
}
