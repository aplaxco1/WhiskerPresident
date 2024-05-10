using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tiling : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        setTiling(this.gameObject.GetComponent<Renderer>().material, 50.0f);
    }
    void setTiling(Material mat, float tileCount) {
        // calculate tiling dimensions so stays square
        float ratio = Screen.width*1.0f/Screen.height;
        float tileCount2 = tileCount * Screen.width/1500;
        Debug.Log(Screen.width);
        //if (Screen.width < 1000) {tileCount2 = tileCount2 / 2;}
        Vector2 tiling = new Vector2(tileCount2*ratio, tileCount2);
        // set the tiling value of halftonepattern
        mat.SetTextureScale("_HalftonePattern", tiling);
    }
}
