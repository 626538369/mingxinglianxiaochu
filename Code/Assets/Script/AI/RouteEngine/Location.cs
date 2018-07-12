using UnityEngine;
using System.Collections;

public class Location
{

    int _identifier;
    public Location()
    {

    }
	public Location(int id)
    {
		_identifier =id;
    }
    public int Identifier
    {
        get { return this._identifier; }
        set { this._identifier=value; }
    }
    public override string ToString()
    {
        return _identifier.ToString();
    }
}