using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickItem : MonoBehaviour
{
    public static List<ClickItem> moveableObjects = new List<ClickItem>();
    public float speed = 5f;

    private Vector3 target;
    private bool selected;

    void Start()
    {
        moveableObjects.Add(this);
        target = transform.position;
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(1) && selected)
        {
            target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            target.z = transform.position.z;
        }

        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
    }

    private void OnMouseDown()
    {
        selected = true;
        gameObject.GetComponent<MeshRenderer>().material.color = Color.black;

        foreach(ClickItem obj in moveableObjects)
        {
            obj.selected = false;
            obj.gameObject.GetComponent<MeshRenderer>().material.color = Color.white;
        }
    }
    /* private bool started;

    private Vector3 startPoint;

    private Vector3 endPoint;

    private float startTime;
    private float beginX;
    private float beginY;

    void Start()
    {

        startPoint = transform.position;
        endPoint = new Vector3(15.0f, 3.0f, 0.0f);
        startTime = Time.time;
        started = false;

    }

    void Update()
    {

        if (started)
        {
            transform.position = Vector3.Lerp(startPoint, endPoint, Time.deltaTime);
        }
    }

    void OnMouseUp()
    {

        if (!started)
        {
            RaycastHit hitInfo1;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hitInfo1))
            {
                if(hitInfo1.collider == GetComponent<BoxCollider>().GetComponent<Collider>())
                {
                    startPoint = transform.position;
                    endPoint = new Vector3(beginX, beginY, 0);
                    started = true;
                    
                }
                
            }
        } */

    /* Vector3 newPosition;
    // Start is called before the first frame update
    void Start()
    {
        newPosition = transform.position;
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                newPosition = hit.point;
                transform.position = newPosition;
            }
        }
    } */

}


