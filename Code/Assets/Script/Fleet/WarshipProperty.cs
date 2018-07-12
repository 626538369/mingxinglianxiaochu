using UnityEngine;
using System.Collections;

public class WarshipProperty : MonoBehaviour 
{
	public long WarshipID;
	public int WarshipLogicID;
	
	public int WarshipType;
	public int TypeID;
	public int Rarity;
	
	public long WarshipFleetID;
	public bool WarshipIsAttacker;
	public bool WarshipIsNpc;
	
	public string Name;
	public string WarshipFullModelName;
	public string WarshipFullParticle;
	public string WarshipImpairedModelName;
	public string WarshipImpairedParticle;
	public string WarshipIcon;
	public string WarshipAnimations;
	
	// The current life
	public int WarshipCurrentLife;
	public int WarshipSimulateLife; // Client simulate the current life
	public int WarshipMaxLife;
	
	// The current energy
	public int WarshipCurrentPower;
	public int WarshipSimulatePower; // Client simulate the current life
	public int WarshipMaxPower;
	
	public int CurrentFillSpeed;
	
	// The current move speed
	public float WarshipMoveSpeed;
	// The current attack power
	public int WarshipAttackDamage;
	
	// The current defense power
	public int WarshipDefence;
	// The current crit rate
	public float WarshipCritRate;
	// The current dodge rate
	public float WarshipDodgeRate;
	// The current attack multiple
	public float WarshipAttackMultiple;
	// The current attack multiple rate
	public float WarshipAttackMultipleRate;
	// The current shell fill speed
	public float WarshipShellFillSpeed;
	// The current attack gunshot [min, max]
	public float WarshipAttackRange;
	
	/*
	 * HP
	 * MP
	 * DC ATK
	 * DCMAX
	 * AC DEF
	 * MAC
	 * Range
	 * CRIT
	 * */
}

