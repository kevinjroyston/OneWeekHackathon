using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryScript : MonoBehaviour {
    public GameObject victoryOverlay;
    public static VictoryScript victoryScript;
    void Awake()
    {
        victoryScript = this;
    }
    public void ShowVictory()
    {
        victoryOverlay.SetActive(true);
    }

}
