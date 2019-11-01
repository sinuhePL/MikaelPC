using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LookArrowController : MonoBehaviour
{
    private Camera myCamera;

    public bool isActive;
    public Button otherButton;

    private IEnumerator SmoothRotate(Vector3 rotatePoint, float angle, float duration)
    {
        float t, lastAngle = 0;
        for(t=0; t<duration; t+= Time.deltaTime)
        {
            myCamera.transform.RotateAround(rotatePoint, new Vector3(0.0f, 1.0f, 0.0f), Mathf.Lerp(0.0f, angle, t / duration) - lastAngle);
            lastAngle = Mathf.Lerp(0.0f, angle, t / duration);
            yield return 0;
        }
    }

    // zmienia kąt patrzenia kamery obracajac ją wokół punktu na który aktualnie patrzy kamera
    public void ChangeViewAngle(string direction)
    {
        int groundMask;
        Ray camRay;
        RaycastHit groundHit;
        if (isActive)
        {
            groundMask = LayerMask.GetMask("Ground");
            // wyznacza p[unkt na mapie na który patrzy kamera
            camRay = myCamera.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));
            if (Physics.Raycast(camRay, out groundHit, 100.0f, groundMask))
            {
                //obraca kamerę wokół punktu na który patrzy, kierunekj zalezy od kierunku aktualnego
                if (direction == "left")
                {
                    StartCoroutine(SmoothRotate(groundHit.point, 90.0f, 0.5f));
                }
                else
                {
                    StartCoroutine(SmoothRotate(groundHit.point, -90.0f, 0.5f));
                }
                otherButton.GetComponent<LookArrowController>().isActive = true;
                isActive = false;
            }
        }
    }

    public void Start()
    {
        myCamera = Camera.main;
    }
}
