using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenValise : MonoBehaviour
{

    public bool isOpen = false;

    private TouchManager touchManager;

    public float duration;

    private void Start()
    {
        touchManager = FindAnyObjectByType<TouchManager>();
    }

    private void Update()
    {
        if (isOpen)
        {
            if (touchManager.isSwipeDown)
            {
                if (Camera.main.GetComponent<CameraMovement>().onDownValise)
                {
                    Camera.main.GetComponent<CameraMovement>().MovementToValiseTop(duration / 2);
                }

            }
            if (touchManager.isSwipeUp) 
            {
                if (Camera.main.GetComponent<CameraMovement>().onDownValise)
                {
                    Camera.main.GetComponent<CameraMovement>().MovementToInitial(duration);
                    StartCoroutine(SmoothOpen(duration, new Vector3(0, 180, 0)));
                }

                if (Camera.main.GetComponent<CameraMovement>().onTopValise)
                {
                    Camera.main.GetComponent<CameraMovement>().MovementToValiseDown(duration / 2);
                }
            }
        }
    }
    public void OnOpenClick()
    {
        if (!isOpen)
        {
            StartCoroutine(SmoothOpen(duration, new Vector3(-90, 180, 0)));
            Camera.main.GetComponent<CameraMovement>().MovementToValiseDown(duration);
        }
       
    }

    

    IEnumerator SmoothOpen(float duration, Vector3 degres)
    {
        float timeStamp = 0f;
        Quaternion rotationActuelle = transform.rotation;

        while (timeStamp < duration)
        {

            transform.rotation = Quaternion.Lerp(
            rotationActuelle,
            Quaternion.Euler(degres),
            timeStamp / duration
            );

            timeStamp += Time.deltaTime;

            yield return null;

        }

        transform.rotation = Quaternion.Euler(degres);
        if (isOpen) isOpen = false;
        else isOpen = true;
    }

    IEnumerator SmoothClose(float duration, Vector3 degres)
    {
        float timeStamp = 0f;
        Quaternion rotationActuelle = transform.rotation;

        while (timeStamp < duration)
        {

            transform.rotation = Quaternion.Lerp(
            rotationActuelle,
            Quaternion.Euler(degres),
            timeStamp / duration
            );

            timeStamp += Time.deltaTime;

            yield return null;

        }

        transform.rotation = Quaternion.Euler(degres);
        isOpen = false;
    }
}
