using System.Collections;
using TMPro;
using UnityEngine;

public class Spin : MonoBehaviour
{
    [SerializeField] private LayerMask spinMask;
    [SerializeField] private float speedSpin;
    [SerializeField] private GameObject collection;
    [SerializeField] private Transform arrow;
    [SerializeField] private RandomItem randomItem;

    [SerializeField] private GameObject textResult;

    private float startSpin;
    private bool isSpinning;
    private bool isSpinningFollowResulItem;
    private Transform itemSelected;
    private int indexItemSelected;

    private float minDis = Mathf.Infinity;

    private void Awake()
    {
        startSpin = speedSpin;
        randomItem.OnRandomResultItem += RandomItem_OnRandomResultItem;
    }

    private void RandomItem_OnRandomResultItem(object sender, RandomItem.OnRandomResultItemEventArgs e)
    {
        itemSelected = e.itemSelected;
        indexItemSelected = collection.GetComponent<Collection>().items.IndexOf(itemSelected);
        isSpinningFollowResulItem = true;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isSpinning)
        {
            if (IsPressButtonSpin())
            {
                isSpinning = true;
                if (isSpinningFollowResulItem)
                {
                    StartSpinTowardsResult();
                }
                else
                {
                    StartRandomSpin();
                }
            }
        }
    }

    private void StartRandomSpin()
    {
        speedSpin = startSpin;
        Invoke("ExcuteSpin",0f);
    }

    private void StartSpinTowardsResult()
    {
        speedSpin = startSpin;
        Invoke("SpinTowardsResult", 0f);
    }

    private void ExcuteSpin()
    {
        if (speedSpin <= 0)
        {
            isSpinning = false;
            //CancelInvoke("ExcuteSpin");
            Debug.Log(collection.GetComponent<Collection>().CaculatePositionItemSelected(arrow));
            return;
        }
        collection.transform.Rotate(0f, speedSpin * Time.deltaTime, 0f);
        speedSpin -= Time.deltaTime * Random.Range(50f, 100f);
        InvokeRepeating("ExcuteSpin", 0f, 0f);
    }

    private void SpinTowardsResult()
    {
        float dis = Vector3.Distance(arrow.position, itemSelected.position);
        
        if (minDis > dis)
        {
            //Debug.Log(dis);
            minDis = dis;
        }
        if (speedSpin <= 30f)
        {
            if (Mathf.Abs(dis-minDis) < .001f)
            {
                isSpinning = false;
                minDis = Mathf.Infinity;
                isSpinningFollowResulItem = false;
                itemSelected.GetComponent<TextMeshProUGUI>().color = Color.white;
                Debug.Log(itemSelected.GetComponent<TextMeshProUGUI>().text);
                return;
            }
            else
            {
                speedSpin = 30f;
                collection.transform.Rotate(0f, speedSpin * Time.deltaTime, 0f);
            }
        }
        else
        {
            collection.transform.Rotate(0f, speedSpin * Time.deltaTime, 0f);
            speedSpin -= Time.deltaTime * Random.Range(50f, 100f);
        }
        InvokeRepeating("SpinTowardsResult", 0f, 0f);
    }

    private bool IsPressButtonSpin()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100f, spinMask))
        {
            if (hit.collider != null)
            {
                return true;
            }
        }
        return false;
    }
}
