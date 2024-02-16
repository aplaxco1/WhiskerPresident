using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Input;

public class ToolClass : MonoBehaviour
{
    [Header("Tool Class Properties")]
    public Texture2D cursorTexture;

    // COMMENTING OUT SOME STUFF SINCE WE ARENT USING IT RIGHT NOW

    // public string toolName = "New Item";
    // public string toolDescription = "New Description";
    // public enum Type {Default, Consumable, Permanent}
    // public Type type = Type.Default;

    // public GameObject tool;
    // public GameObject currentTool;
    // public int toolNumber = 0;
    // // Start is called before the first frame update
    // void Start()
    // {
    //     //set current tool to 0
    //     currentTool = tool;
    // }

    // // Update is called once per frame
    // void Update()
    // {
    //     //if (Input.GetAxis("Mouse ScrollWheel"))
    //     {
    //         if (Input.GetAxis("Mouse ScrollWheel") > 0)
    //         {
    //             toolNumber = (toolNumber + 1);
    //         }
    //         if (Input.GetAxis("Mouse ScrollWheel") < 0)
    //         {
    //             toolNumber = (toolNumber - 1);
    //         }
    //     }
    //     currentTool = tool;
    // }
}
