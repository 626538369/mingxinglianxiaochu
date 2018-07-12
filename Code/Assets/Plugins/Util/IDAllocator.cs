using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Identifier allocator.
/// </summary>
public class IDAllocator
{
	/// <summary>
	/// News one ID.
	/// </summary>
	/// <returns>
	/// The ID.
	/// </returns>
	public int NewID()
	{	
		do
		{
			_currentID ++;
			
			if ( _currentID >= Int32.MaxValue )
			{
				_currentID = ID_START;
				_idLoops ++;
			}
			
		}while ( _idLoops > 0 && _usedIDs.Contains( _currentID ) );
		
		_usedIDs.Add( _currentID );
		
		return _currentID;
	}
	
	/// <summary>
	/// Releases the ID.
	/// </summary>
	/// <param name='id'>
	/// Identifier.
	/// </param>
	public void ReleaseID( int id )
	{
		if ( id == Defines.INVALID_ID )
			return;
		
		_usedIDs.Remove( id );
	}
	
	protected const int ID_START = Defines.INVALID_ID + 1;
	protected int _currentID = ID_START;
	protected int _idLoops = 0;
	protected List<int> _usedIDs = new List<int>();
}
