using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallHelper : MonoBehaviour {
    private bool isActive = false;
    private bool locked = false;
    float colorSpeed = .04f;
	// Use this for initialization
	void Start () {
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
        GetComponent<Renderer>().material.color = col;
    }

    public void ToggleWall()
    {

    }

    public void SetActive(bool isActive)
    {
        isActive = false;

    }

    public void SetLock(bool locked)
    {
        this.locked = locked;
    }

    public bool getIsActive()
    {
        return isActive;
    }
}
