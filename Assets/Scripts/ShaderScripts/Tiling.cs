using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tiling : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        TileChildren();
    }
    public void TileChildren()
    {
        Renderer[] renderers = this.gameObject.GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers) {
            foreach (Material material in renderer.materials) {
                setTiling(material, 50.0f);
            }
        }
        //setTiling(this.gameObject.GetComponent<Renderer>().material, 50.0f);

    }
    void setTiling(Material mat, float tileCount) {
        // calculate tiling dimensions so stays square
        float ratio = Screen.width*1.0f/Screen.height;
        float tileCount2 = tileCount * Screen.width/1500;
        //Debug.Log(Screen.width);
        Vector2 tiling = new Vector2(tileCount2*ratio, tileCount2);
        // set the tiling value of halftonepattern
        if (mat.HasProperty("_HalftonePattern")) {mat.SetTextureScale("_HalftonePattern", tiling);}
        
        mat.SetFloat("_OutlineWidth",0.0015f);
        mat.SetColor("_AmbientColor", new Color(0.8f,0.8f,0.8f,1.0f));
    }
}
