using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject target; // the target game object that the camera will follow
    public new Camera camera;
    public float speed = 2.0f; // the speed at which the camera will move towards the target

    void LateUpdate()
    {
        // calculate the new position for the camera based on the target's position
        Vector3 newPos = new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z);

        // move the camera towards the target using a lerp function
        camera.transform.position = Vector3.Lerp(transform.position, newPos, speed * Time.deltaTime);

    }
}
