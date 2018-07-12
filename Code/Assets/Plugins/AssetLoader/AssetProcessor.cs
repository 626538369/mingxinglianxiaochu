using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ResAssetBundle
{
	/// <summary>
	/// Process data interface.
	/// </summary>
	public interface IProcessData
	{
		string Type{ get; }
		string LogString{ get; }
		Object DefaultReturn{ get; }
	}
	
	/// <summary>
	/// Asset processor base class.
	/// </summary>
	public class AssetProcessor<T> where T : IProcessData
	{
		/// <summary>
		/// Process the specified processData.
		/// </summary>
		/// <param name='processData'>
		/// Process data.
		/// </param>
		public Object Process(T processData)
		{
			if (!_processDelegates.ContainsKey(processData.Type))
				return processData.DefaultReturn;
			
			return _processDelegates[processData.Type](processData);
		}
		
		/// <summary>
		/// Register the specified type and processDelegate.
		/// </summary>
		/// <param name='type'>
		/// Process type.
		/// </param>
		/// <param name='processDelegate'>
		/// Process delegate.
		/// </param>
		public void Register(string type, ProcessDelegate processDelegate)
		{
			if (_processDelegates.ContainsKey(type))
				return;
			
			_processDelegates[type] = processDelegate;
		}
		
		public delegate Object ProcessDelegate(T processDatasss);
		protected Dictionary<string, ProcessDelegate> _processDelegates = new Dictionary<string, ProcessDelegate>();
	}
}

