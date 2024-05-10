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

    // Update is called once per frame
    void Update()
    {
        
    }
    void setTiling(Material mat, float tileCount) {
        // calculate tiling dimensions so stays square
        float ratio = Screen.width*1.0f/Screen.height;
        Vector2 tiling = new Vector2(tileCount*ratio, tileCount);
        // set the tiling value of halftonepattern
        mat.SetTextureScale("_HalftonePattern", tiling);
    }
}
