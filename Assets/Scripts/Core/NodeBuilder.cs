using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class NodeBuilder : MonoBehaviour
{
    private GameObject turretToBuild;
    public GameObject BasicTurret;
    public GameObject RangeTurret;

    private GameObject builtTurret;
    private Renderer nodeRenderer;
    private int towerCost;

    public Color startColor;
    private Color prebuildColor;
    private Vector3 positionOffset;

    private const KeyCode Alpha1KeyCode = KeyCode.Alpha1;
    private const KeyCode Alpha2KeyCode = KeyCode.Alpha2;
    private const KeyCode ReturnKeyCode = KeyCode.Return;

    public bool BuildMode = false;
    private void Start()
    {
        nodeRenderer = GetComponent<Renderer>();
        startColor = nodeRenderer.material.color;
    }

    void Update()
    {
        if (BuildMode == false)
        {
            startColor = nodeRenderer.material.color;
        }

        KeyCode keyPressed = KeyCode.None;

        if (Input.GetKeyDown(Alpha1KeyCode))
        {
            keyPressed = Alpha1KeyCode;
        }
        else if (Input.GetKeyDown(Alpha2KeyCode))
        {
            keyPressed = Alpha2KeyCode;
        }
        else if (Input.GetKeyDown(ReturnKeyCode))
        {
            keyPressed = ReturnKeyCode;
        }

        switch (keyPressed)
        {
            case Alpha1KeyCode:
                turretToBuild = BasicTurret;
                break;

            case Alpha2KeyCode:
                turretToBuild = RangeTurret;
                break;

            case ReturnKeyCode:
                BuildMode = !BuildMode;
                break;
        }
    }

    private void OnMouseDown()
    {
        BuildTower();
    }

    private void OnMouseEnter()
    {
        if (BuildMode && turretToBuild != null)
        {
            prebuildColor = nodeRenderer.material.color;
            nodeRenderer.material.color = Color.blue;
        }
    }

    private void OnMouseExit()
    {
        if (turretToBuild != null)
        {
            nodeRenderer.material.color = startColor;
        }
    }

    void BuildTower()
    {
        if (BuildMode && turretToBuild != null)
        {
            if (builtTurret != null || Player_Currency.money < towerCost)
            {
                Debug.Log("Can't build");
                return;
            }
            Player_Currency.money -= 50;
            builtTurret = Instantiate(turretToBuild, transform.position + positionOffset, transform.rotation);
        }
    }

}

