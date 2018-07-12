using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;


/// <summary>
	/// Www loader manager is used to create or destroy www loader.
	/// It manages one loader table to avoid create the reduplicate loader.
	/// </summary>
public class WwwLoaderManager
{
		
	public static uint MaxRunLoaderNumber { get { return _maxRunLoaderNumber; }  set { _maxRunLoaderNumber = value; } }
		
	public static bool Paused { get { return _paused; } set {_paused = value;} }
	
	protected static bool _paused = false;
	
	protected static uint _maxRunLoaderNumber = 1;
		
	protected static Dictionary<string, LinkedListNode<WwwLoader>> _loaders = new Dictionary<string, LinkedListNode<WwwLoader>>();
	protected static LinkedList<WwwLoader> _pendingLoaders = new LinkedList<WwwLoader>();
	protected static LinkedList<WwwLoader> _startedLoaders = new LinkedList<WwwLoader>();
	protected static LinkedList<WwwLoader> _finishedLoaders = new LinkedList<WwwLoader>();
	
		
	public static void CreateLoader(string url, int version)
	{
			
		// Check if the asset in the loader table
		if (_loaders.ContainsKey(url))
		{
			WwwLoader existingLoader = _loaders[url].Value;
			existingLoader.AddRef();
		}
	
			
		// Create loader
		WwwLoader loader = new WwwLoader(url, version);
		loader.AddRef();
	
		// Add to existing loader
		_loaders.Add(url, _pendingLoaders.AddLast(loader));
	
	}
		
	/// <summary>
		/// Destroies the loader.
		/// </summary>
		/// <param name='loader'>
		/// Loader.
		/// </param>
	public static void DestroyLoader(WwwLoader loader)
	{
		if (loader == null)
			return;
					
		loader.ReleaseRef();
	
		if (loader.RefCount == 0)
		{
			Debug.Assert(_loaders.ContainsKey(loader.AssetPath), "Must exist");
	
			LinkedListNode<WwwLoader> loaderNode = _loaders[loader.AssetPath];
			_loaders.Remove(loader.AssetPath);
			loaderNode.List.Remove(loaderNode);
			loaderNode.Value.Destroy();
		}
	
		loader = null;
	}
		
	/// <summary>
		/// Update www loader manager. This will drive all loaders.
		/// </summary>
	public static void Update()
	{
		LinkedListNode<WwwLoader> iter = _startedLoaders.First;	
		while (iter != null)
		{
			LinkedListNode<WwwLoader> nextIter = iter.Next;
	
			if (iter.Value.IsDone)
			{
				_startedLoaders.Remove(iter);
				_finishedLoaders.AddLast(iter);		
			}
	
			iter = nextIter;
		}
	
		if (!Paused)
		{
			iter = _pendingLoaders.First;
				
			while (_startedLoaders.Count < MaxRunLoaderNumber && iter != null)
			{
				LinkedListNode<WwwLoader> nextIter = iter.Next;
	
				iter.Value.Start();
				_pendingLoaders.Remove(iter);
				_startedLoaders.AddLast(iter);
	
				iter = nextIter;
			}
		}
	}
}
