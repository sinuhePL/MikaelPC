using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(Camera))]
public class PanZoom : MonoBehaviour
{
    private Vector3 touchStart;
    private Vector3 prevCamPosition;
    private float prevEulerY;
    private Camera myCamera;
    private float smoothing = 50.0f;
    private int lookDirection; // army 1: left 1, right 2 army 2: left 3 right 4
    private bool stopRotate;

    public float zoomOutMin = 3;
    public float zoomOutMax = 6;
    public float zoomOutMinPerspective = 3.0f;
    public float zoomOutMaxPerspective = 9.0f;
    public float zoomMinAngle = 15.0f;
    public float zoomMaxAngle = 55.0f;
    public float rotateTime = 2.0f;

    private IEnumerator ZoomAtDice(float duration, float zoom, float delay)
    {
        float t;
        yield return new WaitForSeconds(delay);
        for (t = 0; t < duration; t += Time.deltaTime)
        {
            myCamera.orthographicSize = Mathf.Lerp(myCamera.orthographicSize, zoom, t / duration);
            yield return 0;
        }
    }

    private IEnumerator SmoothRotate(Vector3 rotatePoint, float angle, float duration)
    {
        float t, lastAngle = 0;
        for (t = 0; Mathf.Abs(lastAngle) < Mathf.Abs(angle); t += Time.deltaTime)
        {
            if (stopRotate)
            {
                stopRotate = false;
                yield break;
            }
            myCamera.transform.RotateAround(rotatePoint, new Vector3(0.0f, 1.0f, 0.0f), Mathf.Lerp(0.0f, angle, ((float)t )/ duration) - lastAngle);
            lastAngle = Mathf.Lerp(0.0f, angle, ((float)t) / duration);
            yield return 0;
        }
    }

    private void ZoomOut(StateChange r)
    {
        if (BattleManager.viewType == "isometric") StartCoroutine(ZoomAtDice(0.5f, 4.0f, 0.0f));
        else
        {
            myCamera.transform.DOMove(prevCamPosition, 0.5f).SetEase(Ease.OutQuint);
            myCamera.transform.DORotateQuaternion(Quaternion.Euler(Mathf.Lerp(zoomMinAngle, zoomMaxAngle, (prevCamPosition.y - zoomOutMinPerspective) / (zoomOutMaxPerspective - zoomOutMinPerspective)), prevEulerY, 0.0f), 0.5f);
        }
    }

    private void ZoomOutAfterRoutTest(string resultDescription, int result, int morale)
    {
        ZoomOut(new StateChange());
    }

    private void TurnEnd()
    {
        if(BattleManager.isPlayer1Human && BattleManager.isPlayer2Human) ChangeViewAngle("button");
    }

    private void OnEnable()
    {
        EventManager.onDiceResult += ZoomOut;
        EventManager.onTurnEnd += TurnEnd;
        EventManager.onAttackOrdered += LookAtDice;
        EventManager.onRouteTestOver += ZoomOutAfterRoutTest;
    }

    // Start is called before the first frame update
    void Start()
    {
        myCamera = Camera.main;
        lookDirection = 1;
        stopRotate = false;
    }

    private void OnDestroy()
    {
        EventManager.onDiceResult -= ZoomOut;
        EventManager.onTurnEnd -= TurnEnd;
        EventManager.onAttackOrdered -= LookAtDice;
        EventManager.onRouteTestOver -= ZoomOutAfterRoutTest;
    }

    public void RoutTest(Vector3 testSpot)
    {
        ReallyLookAtDice(testSpot, 1.5f);
    }

    // Update is called once per frame
    void Update()
    {
        if (!BattleManager.isInputBlocked)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if(BattleManager.viewType == "isometric") touchStart = myCamera.ScreenToWorldPoint(Input.mousePosition);
                else if (BattleManager.viewType == "perspective") touchStart = GetCursorWorldPosition();
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

                if (BattleManager.viewType == "isometric") zoom(difference * 0.01f);
                else if (BattleManager.viewType == "perspective") zoomPerspective(difference * 0.1f);
            }
            else if (Input.GetMouseButton(0))
            {
                Vector3 direction, tempPosition;
                if (BattleManager.viewType == "isometric") direction = touchStart - myCamera.ScreenToWorldPoint(Input.mousePosition);
                else if (BattleManager.viewType == "perspective") direction = touchStart - GetCursorWorldPosition();
                else direction = new Vector3(0.0f, 0.0f, 0.0f);
                Vector3 previousCamPosition = myCamera.transform.position;
                // limits camera movement
                if (BattleManager.viewType == "perspective")
                {
                    direction = new Vector3(direction.x, 0.0f, direction.z + direction.y);
                    tempPosition = myCamera.transform.position + direction * 0.1f;
                    if (tempPosition.x > 4.0f 
                        && tempPosition.z < -8.0f 
                        && tempPosition.x < BattleManager.boardFieldWitdth * BattleManager.boardWidth + 8.0f 
                        && tempPosition.z > -1.0f * BattleManager.boardFieldHeight * BattleManager.boardHeight - 8.0f)
                    {
                        myCamera.transform.position += direction * 0.1f;
                    }
                }
                else
                {
                    myCamera.transform.position = Vector3.Lerp(myCamera.transform.position, myCamera.transform.position + direction, smoothing * Time.deltaTime);
                    /*  Ray lbRay = myCamera.ScreenPointToRay(new Vector2(0.0f, 0.0f));
                      Ray ltRay = myCamera.ScreenPointToRay(new Vector2(0.0f, Screen.height));
                      Ray rbRay = myCamera.ScreenPointToRay(new Vector2(Screen.width, 0.0f));
                      Ray rtRay = myCamera.ScreenPointToRay(new Vector2(Screen.width, Screen.height));
                      if (!Physics.Raycast(lbRay) || !Physics.Raycast(ltRay) || !Physics.Raycast(rbRay) || !Physics.Raycast(rtRay))*/
                    if (lookDirection == 1 && (myCamera.transform.position.x < -2.0f
                        || myCamera.transform.position.z > -16.0f
                        || myCamera.transform.position.x > BattleManager.boardFieldWitdth * BattleManager.boardWidth - 4.0f
                        || myCamera.transform.position.z < -1.0f * BattleManager.boardFieldHeight * BattleManager.boardHeight - 18.0f))
                    {
                        myCamera.transform.position = previousCamPosition;
                    }
                    else if (lookDirection == 2 && (myCamera.transform.position.x < 22.0f
                        || myCamera.transform.position.z > -22.0f
                        || myCamera.transform.position.x > BattleManager.boardFieldWitdth * BattleManager.boardWidth + 16.0f
                        || myCamera.transform.position.z < -1.0f * BattleManager.boardFieldHeight * BattleManager.boardHeight - 20.0f))
                    {
                        myCamera.transform.position = previousCamPosition;
                    }
                }
            }
            float mouseScrollWheel = Input.GetAxis("Mouse ScrollWheel");
            if (mouseScrollWheel != 0)
            {
                if (BattleManager.viewType == "isometric") zoom(mouseScrollWheel);
                else if (BattleManager.viewType == "perspective") zoomPerspective(mouseScrollWheel * 80.0f);
            }
        }
    }

    private Vector3 GetCursorWorldPosition()
    {
        Vector2 tempMousePos = Input.mousePosition;
        Ray tempRay = myCamera.ScreenPointToRay(tempMousePos);
        Plane ground = new Plane(myCamera.transform.forward, new Vector3(0, 0, 1.0f));
        float distance;
        ground.Raycast(tempRay, out distance);
        tempRay.GetPoint(distance);
        return tempRay.GetPoint(distance);
    }

    public void zoomPerspective(float increment)
    {
        Vector3 newPosition;
        newPosition = new Vector3(myCamera.transform.position.x, myCamera.transform.position.y - increment * Time.deltaTime, myCamera.transform.position.z);
        if (newPosition.y >= zoomOutMinPerspective && newPosition.y <= zoomOutMaxPerspective)
        {
            myCamera.transform.position = newPosition;
            myCamera.transform.rotation = Quaternion.Euler(Mathf.Lerp(zoomMinAngle, zoomMaxAngle, (newPosition.y - zoomOutMinPerspective) / (zoomOutMaxPerspective - zoomOutMinPerspective)), myCamera.transform.localEulerAngles.y, 0.0f );
        }
    }

    public void zoom(float increment)
    {
        myCamera.orthographicSize = Mathf.Clamp(Mathf.Lerp(myCamera.orthographicSize, myCamera.orthographicSize - increment, smoothing * Time.deltaTime), zoomOutMin, zoomOutMax);
        Ray lbRay = myCamera.ScreenPointToRay(new Vector2(0.0f, 0.0f));
        Ray ltRay = myCamera.ScreenPointToRay(new Vector2(0.0f, Screen.height));
        Ray rbRay = myCamera.ScreenPointToRay(new Vector2(Screen.width, 0.0f));
        Ray rtRay = myCamera.ScreenPointToRay(new Vector2(Screen.width, Screen.height));
        if (!Physics.Raycast(lbRay)) myCamera.transform.position = Vector3.Lerp(myCamera.transform.position, myCamera.transform.position + new Vector3(0.2f, 0.0f, 0.2f), 6 * smoothing * Time.deltaTime);
        if (!Physics.Raycast(ltRay)) myCamera.transform.position = Vector3.Lerp(myCamera.transform.position, myCamera.transform.position + new Vector3(0.2f, 0.0f, -0.2f), 6 * smoothing * Time.deltaTime);
        if (!Physics.Raycast(rbRay)) myCamera.transform.position = Vector3.Lerp(myCamera.transform.position, myCamera.transform.position + new Vector3(-0.2f, 0.0f, 0.2f), 6 * smoothing * Time.deltaTime);
        if (!Physics.Raycast(rtRay)) myCamera.transform.position = Vector3.Lerp(myCamera.transform.position, myCamera.transform.position + new Vector3(-0.2f, 0.0f, -0.2f), 6 * smoothing * Time.deltaTime);
    }

    private void ReallyLookAtDice(Vector3 target, float delay)
    {
        int groundMask;
        Ray camRay;
        RaycastHit groundHit;
        Vector3 dif, newpos, campos;
        Sequence cameraSequence = DOTween.Sequence();
        if (BattleManager.viewType == "isometric")
        {
            StartCoroutine(ZoomAtDice(0.5f, 3.0f, delay));
            groundMask = LayerMask.GetMask("Ground");
            camRay = myCamera.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));
            if (Physics.Raycast(camRay, out groundHit, 100.0f, groundMask))
            {
                campos = myCamera.transform.position;
                dif = target - groundHit.point;
                newpos = campos + dif;
                cameraSequence.AppendInterval(delay);
                cameraSequence.Append(myCamera.transform.DOMove(newpos + new Vector3(2.0f, 0.0f, -2.0f), 0.5f).SetEase(Ease.OutQuint));
            }
        }
        else
        {
            prevCamPosition = myCamera.transform.position;
            prevEulerY = myCamera.transform.localEulerAngles.y;
            target.y = 4.0f;
            cameraSequence.AppendInterval(delay);
            cameraSequence.Append(myCamera.transform.DOMove(target - new Vector3(3.0f, 0.0f, 5.0f), 0.5f).SetEase(Ease.OutQuint));
            cameraSequence.Insert(delay, myCamera.transform.DORotateQuaternion(Quaternion.Euler(Mathf.Lerp(zoomMinAngle, zoomMaxAngle, (4.0f - zoomOutMinPerspective) / (zoomOutMaxPerspective - zoomOutMinPerspective)), 30.0f, 0.0f), 0.5f));
        }
    }

    public void LookAtDice(int attackId)
    {
        Vector3 target;

        target = BattleManager.Instance.GetAttack(attackId).GetPosition();
        ReallyLookAtDice(target + new Vector3(2.0f, 0.0f, -1.0f), 0.0f);
    }

    public void StopRotate()
    {
        stopRotate = true;
    }

    public void ArrowPressed(string type)
    {
        int groundMask;
        Ray camRay;
        RaycastHit groundHit;
        stopRotate = false;
        groundMask = LayerMask.GetMask("Ground");
        // wyznacza punkt na mapie na który patrzy kamera
        camRay = myCamera.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));
        if (Physics.Raycast(camRay, out groundHit, 100.0f, groundMask))
        {
            if(type == "right")
                if(myCamera.transform.localEulerAngles.y <= 30.1f && myCamera.transform.localEulerAngles.y > 0.0f) StartCoroutine(SmoothRotate(groundHit.point, -(30.0f + myCamera.transform.localEulerAngles.y), rotateTime*(30.0f + myCamera.transform.localEulerAngles.y)/60.0f));
                else StartCoroutine(SmoothRotate(groundHit.point, -(myCamera.transform.localEulerAngles.y - 330.0f), rotateTime*(myCamera.transform.localEulerAngles.y - 330.0f)/60.0f));
            if (type == "left")
                if (myCamera.transform.localEulerAngles.y <= 30.1f && myCamera.transform.localEulerAngles.y > 0.0f) StartCoroutine(SmoothRotate(groundHit.point, 30.0f - myCamera.transform.localEulerAngles.y, rotateTime*(30.0f - myCamera.transform.localEulerAngles.y)/60.0f));
                else StartCoroutine(SmoothRotate(groundHit.point, 360.0f - myCamera.transform.localEulerAngles.y + 30.0f, rotateTime*(360.0f - myCamera.transform.localEulerAngles.y + 30.0f)/60.0f));
        }
    }

    // zmienia kąt patrzenia kamery obracajac ją wokół punktu na który aktualnie patrzy kamera
    public void ChangeViewAngle(string caller)
    {
        int groundMask;
        Ray camRay;
        RaycastHit groundHit;
        groundMask = LayerMask.GetMask("Ground");
        // wyznacza punkt na mapie na który patrzy kamera
        camRay = myCamera.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));
        if (Physics.Raycast(camRay, out groundHit, 100.0f, groundMask))
        {
            //obraca kamerę wokół punktu na który patrzy, kierunekj zalezy od kierunku aktualnego
            if (BattleManager.turnOwnerId == 1)
            {
                if(lookDirection == 1)
                {
                    if(caller == "arrow")
                    {
                        StartCoroutine(SmoothRotate(groundHit.point, -90.0f, 0.5f));
                        lookDirection = 2;
                    }
                    else
                    {
                        StartCoroutine(SmoothRotate(groundHit.point, 90.0f, 0.5f));
                        lookDirection = 4;
                    }
                }
                else if(lookDirection == 2)
                {
                    if (caller == "arrow")
                    {
                        StartCoroutine(SmoothRotate(groundHit.point, 90.0f, 0.5f));
                        lookDirection = 1;
                    }
                    else
                    {
                        StartCoroutine(SmoothRotate(groundHit.point, -90.0f, 0.5f));
                        lookDirection = 3;
                    }
                }
            }
            else
            {
                if (lookDirection == 3)
                {
                    if (caller == "arrow")
                    {
                        StartCoroutine(SmoothRotate(groundHit.point, -90.0f, 0.5f));
                        lookDirection = 4;
                    }
                    else
                    {
                        StartCoroutine(SmoothRotate(groundHit.point, 90.0f, 0.5f));
                        lookDirection = 2;
                    }
                }
                else if (lookDirection == 4)
                {
                    if (caller == "arrow")
                    {
                        StartCoroutine(SmoothRotate(groundHit.point, 90.0f, 0.5f));
                        lookDirection = 3;
                    }
                    else
                    {
                        StartCoroutine(SmoothRotate(groundHit.point, -90.0f, 0.5f));
                        lookDirection = 1;
                    }
                }
            }
        }
    }
}
