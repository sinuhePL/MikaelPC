using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class PanZoom : MonoBehaviour
{
    private Vector3 touchStart;
    private Camera myCamera;
    private float smoothing = 50.0f;

    public float zoomOutMin = 3;
    public float zoomOutMax = 6;
    // Start is called before the first frame update
    void Start()
    {
        myCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            touchStart = myCamera.ScreenToWorldPoint(Input.mousePosition);
        }
        if (Input.touchCount == 2)
        {
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            float prevMagnitude = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float currentMagnitude = (touchZero.position - touchOne.position).magnitude;

            float difference = currentMagnitude - prevMagnitude;

            zoom(difference * 0.01f);
        }
        else if (Input.GetMouseButton(0))
        {
            Vector3 direction = touchStart - myCamera.ScreenToWorldPoint(Input.mousePosition);
            Vector3 tempPos = myCamera.transform.position;
            myCamera.transform.position = Vector3.Lerp(myCamera.transform.position, myCamera.transform.position + direction, smoothing * Time.deltaTime);
            Ray lbRay = myCamera.ScreenPointToRay(new Vector2(0.0f,0.0f));
            Ray ltRay = myCamera.ScreenPointToRay(new Vector2(0.0f, Screen.height));
            Ray rbRay = myCamera.ScreenPointToRay(new Vector2(Screen.width, 0.0f));
            Ray rtRay = myCamera.ScreenPointToRay(new Vector2(Screen.width, Screen.height));
            if (!Physics.Raycast(lbRay) || !Physics.Raycast(ltRay) || !Physics.Raycast(rbRay) || !Physics.Raycast(rtRay))
            {
                myCamera.transform.position = tempPos;
            }
        }
        zoom(Input.GetAxis("Mouse ScrollWheel"));
    }

    void zoom(float increment)
    {
        Camera.main.orthographicSize = Mathf.Clamp(Mathf.Lerp(myCamera.orthographicSize, myCamera.orthographicSize - increment, smoothing * Time.deltaTime), zoomOutMin, zoomOutMax);
        Ray lbRay = myCamera.ScreenPointToRay(new Vector2(0.0f, 0.0f));
        Ray ltRay = myCamera.ScreenPointToRay(new Vector2(0.0f, Screen.height));
        Ray rbRay = myCamera.ScreenPointToRay(new Vector2(Screen.width, 0.0f));
        Ray rtRay = myCamera.ScreenPointToRay(new Vector2(Screen.width, Screen.height));
        if (!Physics.Raycast(lbRay)) myCamera.transform.position = Vector3.Lerp(myCamera.transform.position, myCamera.transform.position + new Vector3(0.2f, 0.0f, 0.2f), 6 * smoothing * Time.deltaTime);
        if (!Physics.Raycast(ltRay)) myCamera.transform.position = Vector3.Lerp(myCamera.transform.position, myCamera.transform.position + new Vector3(0.2f, 0.0f, -0.2f), 6 * smoothing * Time.deltaTime);
        if (!Physics.Raycast(rbRay)) myCamera.transform.position = Vector3.Lerp(myCamera.transform.position, myCamera.transform.position + new Vector3(-0.2f, 0.0f, 0.2f), 6 * smoothing * Time.deltaTime);
        if (!Physics.Raycast(rtRay)) myCamera.transform.position = Vector3.Lerp(myCamera.transform.position, myCamera.transform.position + new Vector3(-0.2f, 0.0f, -0.2f), 6 * smoothing * Time.deltaTime);
    }
}
