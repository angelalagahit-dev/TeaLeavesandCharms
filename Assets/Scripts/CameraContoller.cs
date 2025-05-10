using UnityEngine;

public class CameraContoller : MonoBehaviour
{
    private Transform target;


    private Camera cam;
    private float halfWidth, halfHeight;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //target = FindAnyObjectByType<PlayerMovement>().transform;
        target = PlayerMovement.instance.transform;

        cam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(target.position.x, target.position.y, transform.position.z);
    }
}