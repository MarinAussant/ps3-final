using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.UIElements;

public class LevelInventaireManager : MonoBehaviour
{

    [SerializeField] public GameObject invObjectPrefab;

    public ScriptableInv[] inventaireList;
    public List<ScriptableInvDrag> inventaire;
    public List<ScriptableInvDrag> inventairePlace;

    private TouchManager touchManager;

    private GameObject isSelecting;
    private bool is3D;

    public GameObject infoUiArea;
    public TextMeshProUGUI infoText;
    public Image infoImage;
    public TextMeshProUGUI infoTitre;

    public GameObject SlideLeftButton;
    public GameObject SlideRightButton;


    // Start is called before the first frame update
    void Start()
    {
        is3D = false;

        infoUiArea.SetActive(false);
        //SlideLeftButton.SetActive(false);
        //SlideRightButton.SetActive(false);

        inventairePlace = new List<ScriptableInvDrag>();
        inventaire = new List<ScriptableInvDrag>();
        foreach (ScriptableInvDrag invObject in inventaireList[1].objectList)
        {
            inventaire.Add(invObject);
        }

        touchManager = FindAnyObjectByType<TouchManager>();

        loadInventaire(1);
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(isSelecting);

        if (Input.touchCount == 1)
        {
            if (Input.touches[0].phase == TouchPhase.Began)
            {
                if (Physics.Raycast(Camera.main.ScreenPointToRay(new Vector3(Input.touches[0].position.x, Input.touches[0].position.y, Camera.main.farClipPlane)), out var info))
                {
                    if (info.collider.gameObject.tag == "CaseInventaire")
                    {
                        isSelecting = info.collider.gameObject;
                        //StartDrag();

                    }
                }
            }
        }

        if (Input.touchCount == 1)
        {
            if (Input.touches[0].phase == TouchPhase.Ended)
            {
                if (isSelecting)
                {
                    if (touchManager.lastTouchIsClick())
                    {
                        infoText.text = isSelecting.GetComponent<individualInvElement>().scriptableData.infoText;
                        infoImage.sprite = isSelecting.GetComponent<individualInvElement>().scriptableData.imageRef;
                        infoTitre.text = isSelecting.GetComponent<individualInvElement>().scriptableData.objectName;
                        infoUiArea.SetActive(true);
                    }
                    if(is3D) {
                        is3D = false;
                    }
                    else
                    {
                        isSelecting = null;
                    }

                }
            }
        }

        if (isSelecting && !is3D)
        {
            if (TouchPosition().y > -5.15)
            {
                StartDrag();
                is3D = true;

            }
        }

    }

    public void loadInventaire(int numNiveau)
    {

        float offset = 0;

        foreach (ScriptableInvDrag invObject in inventaire)
        {
            GameObject tempAffichage = Instantiate(invObjectPrefab, transform);
            tempAffichage.transform.position += new Vector3(offset, 0, 0);
            SpriteRenderer spriteRenderer = tempAffichage.GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = invObject.imageRef;
            tempAffichage.GetComponent<BoxCollider>().size = new Vector3(spriteRenderer.sprite.bounds.size.x, spriteRenderer.sprite.bounds.size.y, 1);
            tempAffichage.GetComponent<individualInvElement>().invElementNum = (int) offset;
            tempAffichage.GetComponent<individualInvElement>().scriptableData = invObject;
            offset += 1;
        }
    }

    public void SlideRight()
    {
        StartCoroutine(SmoothSlideRight());
    }

    public void SlideLeft()
    {
        StartCoroutine(SmoothSlideLeft());
    }

    IEnumerator SmoothSlideRight()
    {
        float timeStamp = 0f;
        Vector3 actualPosition = transform.position;
        Vector3 destination = new Vector3(transform.position.x - 3, transform.position.y, transform.position.z);

        while (timeStamp < 0.2)
        {

            transform.position = Vector3.Lerp(
            actualPosition,
            destination,
            timeStamp / 0.2f
            );

            timeStamp += Time.deltaTime;

            yield return null;

        }

        transform.position = destination;
    }

    IEnumerator SmoothSlideLeft()
    {
        float timeStamp = 0f;
        Vector3 actualPosition = transform.position;
        Vector3 destination = new Vector3(transform.position.x + 3, transform.position.y, transform.position.z);

        while (timeStamp < 0.2)
        {

            transform.position = Vector3.Lerp(
            actualPosition,
            destination,
            timeStamp / 0.2f
            );

            timeStamp += Time.deltaTime;

            yield return null;

        }

        transform.position = destination;
    }

    public void StartDrag()
    {
        if (isSelecting)
        {
            inventaire.Remove(isSelecting.GetComponent<individualInvElement>().scriptableData);
            inventairePlace.Add(isSelecting.GetComponent<individualInvElement>().scriptableData);

            ReloadInv();
            
            GameObject draggingObject = Instantiate(isSelecting.GetComponent<individualInvElement>().scriptableData.objectDraggable, transform.parent);
            draggingObject.GetComponentInChildren<Draggable>().isDragging = true;
            draggingObject.GetComponentInChildren<Draggable>().attachScriptableDrag = isSelecting.GetComponent<individualInvElement>().scriptableData;
        }
    }

    public void EndDrag(bool isOnReceptacle, GameObject leDraggable)
    {
        if (!isOnReceptacle && !leDraggable.GetComponent<Draggable>().isPlaced)
        {
            
            inventairePlace.Remove(leDraggable.GetComponent<Draggable>().attachScriptableDrag);
            inventaire.Add(leDraggable.GetComponent<Draggable>().attachScriptableDrag);
            Destroy(leDraggable);
            isSelecting = null;

            ReloadInv();
        }
        if (is3D)
        {
            is3D = false;
        }

    }

    public void ReloadInv()
    {
        //GameObject tempIsSelected = isSelecting;
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        //isSelecting = tempIsSelected;

        if (inventaire.Count > 0)
        {
            float offset = 0;
            foreach (ScriptableInvDrag invObject in inventaire)
            {
                GameObject tempAffichage = Instantiate(invObjectPrefab, transform);
                tempAffichage.transform.position += new Vector3(offset, 0, 0);
                SpriteRenderer spriteRenderer = tempAffichage.GetComponent<SpriteRenderer>();
                spriteRenderer.sprite = invObject.imageRef;
                tempAffichage.GetComponent<BoxCollider>().size = new Vector3(spriteRenderer.sprite.bounds.size.x, spriteRenderer.sprite.bounds.size.y, 1);
                tempAffichage.GetComponent<individualInvElement>().invElementNum = (int)offset;
                tempAffichage.GetComponent<individualInvElement>().scriptableData = invObject;
                offset += 1;
            }
        }

    }

    public void desactivateInformation()
    {
        infoUiArea.SetActive(false);
    }

    public Vector3 TouchPosition()
    {
        return Camera.main.ScreenToWorldPoint(new Vector3(Input.touches[0].position.x, Input.touches[0].position.y, Camera.main.nearClipPlane + 7.5f));
    }
}
