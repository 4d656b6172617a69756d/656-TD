using UnityEngine;

public class Camera_Behavior : MonoBehaviour
{

    private bool doMovment = true;


    public float panSpeed = 30f;
    public float panBorderThickness = 10f;

    public float scrollSpeed = 5f;
    public float minY = 10f;
    public float maxY = 80f;

    public float minZ = -100f;
    public float maxZ = 100f;

    public float minX = -100f;
    public float maxX = 100f;


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            doMovment = !doMovment;

        if (!doMovment)
            return;

        if (Input.GetKey(KeyCode.W) || Input.mousePosition.y >= Screen.height - panBorderThickness)
        {
            transform.Translate(panSpeed * Time.deltaTime * Vector3.forward, Space.World);
        }

        if (Input.GetKey(KeyCode.S) || Input.mousePosition.y <= panBorderThickness)
        {
            transform.Translate(panSpeed * Time.deltaTime * Vector3.back, Space.World);
        }

        if (Input.GetKey(KeyCode.D) || Input.mousePosition.x >= Screen.width - panBorderThickness)
        {
            transform.Translate(panSpeed * Time.deltaTime * Vector3.right, Space.World);
        }

        if (Input.GetKey(KeyCode.A) || Input.mousePosition.x <= panBorderThickness)
        {
            transform.Translate(panSpeed * Time.deltaTime * Vector3.left, Space.World);
        }
        
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if ((transform.position.y <= minY && scroll >= 0) || (transform.position.y >= maxY && scroll <= 0))
            return;

        transform.Translate(scroll * scrollSpeed * Time.deltaTime * Vector3.forward);

    }
}

