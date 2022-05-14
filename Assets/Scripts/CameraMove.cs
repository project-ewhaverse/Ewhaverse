using UnityEngine;
using System.Collections;
public class CameraMove : MonoBehaviour
{
    public float rot_speed = 100.0f;
    public GameObject Player;
    public GameObject MainCamera;
    private float camera_dist = 0f;
    public float camera_width = -10f;
    public float camera_height = 4f;
    public float camera_fix = 3f;
    Vector3 dir;
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        MainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        camera_dist = Mathf.Sqrt(camera_width * camera_width + camera_height * camera_height);
        dir = new Vector3(0, camera_height, camera_width).normalized;
    }
    void Update()
    {
        transform.Rotate(Vector3.up * Input.GetAxis("Mouse X") * Time.deltaTime * rot_speed, Space.World);
        transform.Rotate(Vector3.left * Input.GetAxis("Mouse Y") * Time.deltaTime * rot_speed, Space.Self);
        transform.position = Player.transform.position;
        Vector3 ray_target = transform.up * camera_height + transform.forward * camera_width;
        Debug.Log("ray_target : " + ray_target);
        RaycastHit hitinfo;
        Physics.Raycast(transform.position, ray_target, out hitinfo, camera_dist);
        if (hitinfo.point != Vector3.zero)
        {
            MainCamera.transform.position = hitinfo.point;
            MainCamera.transform.Translate(dir * -1 * camera_fix);
        }
        else
        {
            MainCamera.transform.localPosition = Vector3.zero;
            MainCamera.transform.Translate(dir * camera_dist);
            MainCamera.transform.Translate(dir * -1 * camera_fix);
        }
    }
}