using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawPrint : MonoBehaviour
{
    public float DisappearanceRate = 0.2f;
    public Color color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
    public float StencilID = 1;
    private MeshRenderer[] renderers;
    // Start is called before the first frame update
    void Start()
    {
        renderers = gameObject.GetComponentsInChildren<MeshRenderer>(true);
        foreach(MeshRenderer renderer in renderers) {
            renderer.material.color = color;
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach(MeshRenderer renderer in renderers) {
            Color oldColor = renderer.material.color;
            renderer.material.color = new Color(oldColor.r, oldColor.g, oldColor.b, oldColor.a - DisappearanceRate*Time.deltaTime);
            renderer.material.SetFloat("_StencilID", StencilID);
        }
        if (renderers[0].material.color.a < 0.03) {
            /*
            foreach(MeshRenderer renderer in renderers) {
                renderer.material.color = color;
            }
            gameObject.SetActive(false);
            */
            Destroy(gameObject);
        }
    }
}
