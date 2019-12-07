
using UnityEngine;

public class CameraNavigation : MonoBehaviour
{
    private float maxZoom = 100f;
    private float minZoom = 1f;

    [SerializeField]
    private float dragSpeed = 40f;
    private Camera editorCamera;

    private void Start()
    {
        editorCamera = this.gameObject.GetComponent<Camera>();
    }

    private void Update()
    {
        Zoom();
        Drag();      
       
    }

    private void Drag()
    {
        if (Input.GetMouseButton(1))
        {
            //Drag camera around with Right Mouse
            transform.Translate(-Input.GetAxisRaw("Mouse X") * Time.deltaTime * dragSpeed, -Input.GetAxisRaw("Mouse Y") * Time.deltaTime * dragSpeed, 0);
        }
    }

    private void Zoom()
    {
        // Scroll forward
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            ZoomOrthoCamera(Camera.main.ScreenToWorldPoint(Input.mousePosition), 1);
        }

        // Scoll back
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            ZoomOrthoCamera(Camera.main.ScreenToWorldPoint(Input.mousePosition), -1);
        }
    }

    // Ortographic camera zoom towards a point (in world coordinates). Negative amount zooms in, positive zooms out
    // TODO: when reaching zoom limits, stop camera movement as well
    private void ZoomOrthoCamera(Vector3 zoomTowards, float amount)
    {
        // Calculate how much we will have to move towards the zoomTowards position
        float multiplier = (1.0f / this.editorCamera.orthographicSize * amount);

        // Move camera
        transform.position += (zoomTowards - transform.position) * multiplier;

        // Zoom camera
        this.editorCamera.orthographicSize -= amount;

        // Limit zoom
        this.editorCamera.orthographicSize = Mathf.Clamp(this.editorCamera.orthographicSize, minZoom, maxZoom);
    }

}
