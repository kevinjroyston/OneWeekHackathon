using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WallHelper : MonoBehaviour {
    public bool isTransparent;
    private bool isActive = false;
	// Use this for initialization
	void Start () {
        this.ToggleWall(!isTransparent);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetColor(Color col)
    {

    }

    public void ToggleWall(bool toggleOn)
    {
        Color color = gameObject.GetComponent<Renderer>().material.color;
        if (toggleOn)
        {
            color.a = 1.0f;
            gameObject.GetComponent<NavMeshObstacle>().enabled = true;
        } else
        {
            color.a = 0.5f;
            gameObject.GetComponent<NavMeshObstacle>().enabled = false;
        }
        gameObject.GetComponent<Renderer>().material.color = color;
    }

    public bool isToggled()
    {
        return gameObject.GetComponent<NavMeshObstacle>().enabled;
    }

    public bool getIsActive()
    {
        return isActive;
    }
}
