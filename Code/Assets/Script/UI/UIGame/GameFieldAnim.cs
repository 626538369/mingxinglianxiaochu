using UnityEngine;
using System.Collections;

public class GameFieldAnim : MonoBehaviour {

    void EndAnimGamField()
    {
		EliminationMgr.Instance.GameFieldAnimationEndStartGame ();
    }
}
