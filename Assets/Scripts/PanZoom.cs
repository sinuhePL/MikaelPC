using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(Camera))]
public class PanZoom : MonoBehaviour
{
    private Vector3 touchStart;
    private Camera myCamera;
    private float smoothing = 50.0f;

    public float zoomOutMin = 3;
    public float zoomOutMax = 6;

    private IEnumerator ZoomAtDice(float duration, float zoom)
    {
        float t;
        for (t = 0; t < duration; t += Time.deltaTime)
        {
            myCamera.orthographicSize = Mathf.Lerp(myCamera.orthographicSize, zoom, t / duration);
            yield return 0;
        }
    }

    private void ZoomOut(ThrowResult r)
    {
        StartCoroutine(ZoomAtDice(0.5f, 4.0f));
    }

    private void OnEnable()
    {
        EventManager.onDiceThrow += ZoomOut;
    }

    // Start is called before the first frame update
    void Start()
    {
        myCamera = Camera.main;
    }

    private void OnDestroy()
    {
        EventManager.onDiceThrow -= ZoomOut;
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

    public void zoom(float increment)
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

    public void LookAtDice(Vector3 target)
    {
        int groundMask;
        Ray camRay;
        RaycastHit groundHit;
        Vector3 dif, newpos, campos;

        StartCoroutine(ZoomAtDice(0.5f, 2.0f));
        groundMask = LayerMask.GetMask("Ground");
        camRay = myCamera.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));
        if (Physics.Raycast(camRay, out groundHit, 100.0f, groundMask))
        {
            campos = myCamera.transform.position;
            dif = target - groundHit.point;
            newpos = campos + dif;
            myCamera.transform.DOMove(newpos - new Vector3(0.0f, 10.0f, 0.0f), 0.5f).SetEase(Ease.OutQuint);
        }

    }
}
