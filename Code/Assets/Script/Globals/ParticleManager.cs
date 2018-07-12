using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ParticleManager 
{
	public ParticleManager()
	{}
	
	public void Initialize()
	{}
	
	public void Release()
	{
		foreach (ParticleEffect particle in _particleList)
		{
			particle.Release();
		}
		_particleList.Clear();
	}
	
	public void Update()
	{
		List<ParticleEffect> tempRemoveList = new List<ParticleEffect>();
		foreach (ParticleEffect particle in _particleList)
		{
			if (!particle._isAlive)
			{
				tempRemoveList.Add(particle);
			}
		}
		
		if (tempRemoveList.Count == 0)
			return;
		
		foreach (ParticleEffect particle in tempRemoveList)
		{
			RemoveParticle(particle);
		}
		tempRemoveList.Clear();
	}
	
	public ParticleEffect CreateParticle(string resourceName)
	{
		ParticleEffect particle = new ParticleEffect(resourceName);
		particle.Initialze();
		
		_particleList.Add(particle);
		return particle;
	}
	
	public void RemoveParticle(ParticleEffect particle)
	{
		// Just remove from particle list, C# will auto delete it and clean up the memory
		particle.Release();
		_particleList.Remove(particle);
	}
	
	public List<ParticleEffect> _particleList = new List<ParticleEffect>();
}
