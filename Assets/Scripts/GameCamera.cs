using UnityEngine;
using System.Collections;

public class GameCamera : MonoBehaviour
{

    private float speed = 2.0f;
    private float zoomSpeed = 1.0f;

    public float minX = -360.0f;
    public float maxX = 360.0f;

    public float minY = -45.0f;
    public float maxY = 45.0f;

    public float sensX = 100.0f;
    public float sensY = 100.0f;

    //float rotationY = 0.0f;
    //float rotationX = 0.0f;

    void Update()
    {
        //move cam
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            transform.position += Vector3.right * speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            transform.position += Vector3.left * speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
        {
            transform.position += Vector3.forward * speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
        {
            transform.position += Vector3.back * speed * Time.deltaTime;
        }

        //rotate cam
        /*
        if (Input.GetMouseButton(0))
        {
            rotationX += Input.GetAxis("Mouse X") * sensX * Time.deltaTime;
            rotationY += Input.GetAxis("Mouse Y") * sensY * Time.deltaTime;
            rotationY = Mathf.Clamp(rotationY, minY, maxY);
            transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);
        }
        */

        //zoom cam
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        float newY = scroll * zoomSpeed * -1;
        float newZ = scroll * zoomSpeed;
        if (newY != 0f)
        {
            //if (transform.localPosition.y > 0.39f)
            //{
                transform.Translate(0, newY, newZ, Space.World);
            //}
        }
    }


    // Use this for initialization
    void Start () {
	
	}
	
	
}
