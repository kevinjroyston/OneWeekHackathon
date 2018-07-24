using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileHelper : MonoBehaviour {

    float colorSpeed = .04f;
    // Use this for initialization
    void Start () {
		
	}
    float colorTimer = 1000f;
    void Update()
    {
        colorTimer += Time.deltaTime * colorSpeed;
        //SetColor(Color.Lerp(a,b, .5f + .5f*Mathf.Sin(colorTimer + transform.position.x )));
        SetColor(Color.HSVToRGB((colorTimer + transform.localPosition.x * .1f + transform.localPosition.z * .1f) % 1f, 1f, 1f));
    }
    public void SetColor(Color col)
    {
        Color oldColor = GetComponent<Renderer>().material.color;
        Color newColor = new Color(col.r, col.g, col.b, oldColor.a);
        GetComponent<Renderer>().material.color = newColor;
    }
}
