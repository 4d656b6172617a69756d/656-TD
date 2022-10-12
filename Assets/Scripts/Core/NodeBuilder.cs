using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeBuilder : MonoBehaviour
{
    public Color startcol;
    public Color prebuild;
    public Vector3 offset;
    public Vector3 size;
    
    private GameObject turretToBuild;
    // public GameObject getTurret;
    public GameObject BasicTurret;
    public GameObject RangeTurret;

    private GameObject tower;
    private Renderer map;

    private bool BuildMode = false;
    private void Start()
    {
        map = GetComponent<Renderer>();
        startcol = map.material.color;
        size = map.bounds.size;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            turretToBuild = BasicTurret;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            turretToBuild = RangeTurret;
        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            BuildMode = true;
        }
    }
    // placeholder for selection + money system later

    private void OnMouseDown()
    {
        if (BuildMode && turretToBuild != null)
        {
            if (tower != null)
            {
                Debug.Log("Can't build");
                return;
            }
            tower = Instantiate(turretToBuild, transform.position, transform.rotation);
        }
        BuildMode = false;
    }

    private void OnMouseEnter()
    {
        if (BuildMode && turretToBuild != null)
        {
            map.material.color = prebuild;
        }
    }

    private void OnMouseExit()
    {
        map.material.color = startcol;
    }

}
