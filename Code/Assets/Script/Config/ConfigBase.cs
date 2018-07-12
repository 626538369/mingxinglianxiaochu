using UnityEngine;
using System.Collections;
using System.Security;

public abstract class ConfigBase
{
	public abstract bool Load(SecurityElement element);
}
