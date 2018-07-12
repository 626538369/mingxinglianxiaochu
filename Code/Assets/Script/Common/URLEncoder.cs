using System.Collections;
using System.Collections.Specialized;
using System.Text;
using System;
using System.IO;

/// <summary>
/// URL encoder.
/// </summary>
public class URLEncoder{
	
	/// <summary>
	/// Needs the convert.
	/// </summary>
	/// <returns>
	/// The convert.
	/// </returns>
	/// <param name='c'>
	/// If set to <c>true</c> c.
	/// </param>
    private static bool NeedConvert(char c) {
		if(c >= 'a' && c <= 'z'){
			return false;
		}
		
		if(c >= 'A' && c <= 'Z'){
			return false;
		}
		
		if(c >= '0' && c <= '9'){
			return false;
		}
		
		return true;
    }
	
	/// <summary>
	/// URLs the encode by utf8
	/// </summary>
	/// <returns>
	/// The encode.
	/// </returns>
	/// <param name='str'>
	/// String.
	/// </param>
	public static string Encode(string str){
		
        StringBuilder sb = new StringBuilder();	
		
        for (int i = 0; i < str.Length; i++)
        {
			char c = str[i];
			
			if(NeedConvert(c)){
				byte[] bytes = Encoding.UTF8.GetBytes(c.ToString());
				
				foreach(byte b in bytes){
					sb.Append("%" + b.ToString("X2"));
				}
				
			}else{
				sb.Append(c);
			}
            
        }
        
        return (sb.ToString());
    }
}
