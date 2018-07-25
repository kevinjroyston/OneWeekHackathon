﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WallHelper : MonoBehaviour {
    public bool isActive = false;
    float colorSpeed = .04f;
	// Use this for initialization
	void Start () {
        ToggleWall(isActive);
    }

    // Update is called once per frame
    float colorTimer = 1000f;
	void Update () {
        colorTimer += Time.deltaTime * colorSpeed;
        //SetColor(Color.Lerp(a,b, .5f + .5f*Mathf.Sin(colorTimer + transform.position.x )));
        SetColor(Color.HSVToRGB((colorTimer+transform.localPosition.x*.1f+transform.localPosition.z*.1f) % 1f, 1f, 1f));
    }

    public void SetColor(Color col)
    {
        col.a = GetComponent<Renderer>().material.color.a;
        GetComponent<Renderer>().material.color = col;
    }

    public void ToggleWall(bool isToggle)
    {
        isActive = isToggle;

        Color color = gameObject.GetComponent<Renderer>().material.color;
        if (isToggle)
        {
            gameObject.layer = 9;
            gameObject.GetComponent<NavMeshObstacle>().enabled = true;
            color.a = 1.0f;
        } else
        {
            gameObject.layer = 10;
            gameObject.GetComponent<NavMeshObstacle>().enabled = false;
            color.a = 0.1f;
        }
        gameObject.GetComponent<Renderer>().material.color = color;

    }

    public void ChangeToggle()
    {
        ToggleWall(!isActive);
    }

    public bool IsToggled()
    {
        return gameObject.GetComponent<NavMeshObstacle>().enabled;
    }

}
