using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class CarnetObject : MonoBehaviour
{

    public bool isTake = false;
    public float duration;

    public Vector3 initialPosition;
    public Vector3 outPosition;
    public Vector3 finalPosition;

    public Vector3 initialRotation;
    public Vector3 finalRotation;

    public GameObject physicsCarnet;
    private GameObject actualPhysicsCarnet;

    private TouchManager touchManager;

    private void Start()
    {
        touchManager = FindAnyObjectByType<TouchManager>();
    }

    public void takeCarnet()
    {
        if (isTake)
        {

        }
        else
        {
            if (touchManager.lastTouchIsClick())
            {
                isTake = true;
                StartCoroutine(smoothTakeCarnet(duration, outPosition, finalPosition, finalRotation));
                Camera.main.GetComponent<CameraMovement>().TakeCarnet();
            }
        }
    }

    IEnumerator smoothTakeCarnet(float duration, Vector3 firstPosition, Vector3 finalPosition, Vector3 finalRotation)
    {
        float timeStamp = 0f;
        Vector3 positionActuelle = transform.position;
        Quaternion rotationActuelle = transform.rotation;

        while (timeStamp < duration)
        {

            transform.position = Vector3.Lerp(
            positionActuelle,
            firstPosition,
            timeStamp / duration
            );

            transform.rotation = Quaternion.Lerp(
            rotationActuelle,
            Quaternion.Euler(finalRotation),
            timeStamp / (duration)
            );

            timeStamp += Time.deltaTime;

            yield return null;

        }
        transform.position = firstPosition;
        timeStamp = 0f;

        while (timeStamp < duration)
        {

            transform.position = Vector3.Lerp(
            firstPosition,
            finalPosition,
            timeStamp / duration
            );

            timeStamp += Time.deltaTime;

            yield return null;

        }

        transform.position = finalPosition;

        actualPhysicsCarnet = Instantiate(physicsCarnet);

    }

}
