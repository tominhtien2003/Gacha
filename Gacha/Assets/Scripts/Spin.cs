using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Spin : MonoBehaviour
{
    public event EventHandler<OnHandleResultWithoutScreenEventArg> OnHandleResultWithoutScreen;

    public class OnHandleResultWithoutScreenEventArg : EventArgs
    {
        public TextMeshProUGUI nameItem;
    }

    [SerializeField] private LayerMask spinMask;
    [SerializeField] private float speedSpin;
    [SerializeField] private GameObject collection;
    [SerializeField] private Transform arrow;
    [SerializeField] private RandomItem randomItem;

    private float startSpinSpeed;
    private bool isSpinning;
    private bool isSpinningToResult;
    private Transform itemSelected;
    private float minDistance = Mathf.Infinity;
    private Vector3 posArrow;

    private void Awake()
    {
        startSpinSpeed = speedSpin;

        posArrow = arrow.position;

        randomItem.OnRandomResultItem += RandomItem_OnRandomResultItem;
    }

    private void RandomItem_OnRandomResultItem(object sender, RandomItem.OnRandomResultItemEventArgs e)
    {
        itemSelected = e.itemSelected;

        isSpinningToResult = true;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isSpinning && IsPressButtonSpin())
        {
            isSpinning = true;

            if (isSpinningToResult)
            {
                StartSpinTowardsResult();
            }
            else
            {
                StartRandomSpin();
            }
        }
    }

    private void StartRandomSpin()
    {
        speedSpin = startSpinSpeed;

        StartCoroutine(ExecuteSpinCoroutine());
    }

    private void StartSpinTowardsResult()
    {
        speedSpin = startSpinSpeed;

        StartCoroutine(SpinTowardsResultCoroutine());
    }

    private IEnumerator ExecuteSpinCoroutine()
    {
        while (speedSpin > 1.5f)
        {
            collection.transform.Rotate(0f, speedSpin * Time.deltaTime, 0f);

            speedSpin = Mathf.Lerp(speedSpin, 0f, Time.deltaTime * 0.3f);

            yield return null;
        }
        isSpinning = false;

        TextMeshProUGUI selectedText = collection.GetComponent<Collection>().CaculatePositionItemSelected(posArrow);

        ActivateResultWithoutScreen(selectedText);
    }

    private IEnumerator SpinTowardsResultCoroutine()
    {
        while (isSpinning)
        {
            float distance = Vector3.Distance(posArrow, itemSelected.position);

            if (minDistance > distance)
            {
                minDistance = distance;
            }

            if (speedSpin <= 50f)
            {
                if (Mathf.Abs(distance - minDistance) < 0.01f)
                {
                    StopSpinning();
                    ActivateResultWithoutScreen(itemSelected.GetComponent<TextMeshProUGUI>());
                    yield break;
                }
                speedSpin = Mathf.Lerp(speedSpin, 0f, Time.deltaTime * 0.1f);
            }
            else
            {
                speedSpin = Mathf.Lerp(speedSpin, 0f, Time.deltaTime * 0.3f);
            }

            collection.transform.Rotate(0f, speedSpin * Time.deltaTime, 0f);
            yield return null;
        }
        
    }

    private bool IsPressButtonSpin()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        return Physics.Raycast(ray, out RaycastHit hit, 100f, spinMask) && hit.collider != null;
    }

    private void ActivateResultWithoutScreen(TextMeshProUGUI nameItem)
    {
        OnHandleResultWithoutScreen?.Invoke(this, new OnHandleResultWithoutScreenEventArg { nameItem = nameItem });
    }

    private void StopSpinning()
    {
        isSpinning = false;

        isSpinningToResult = false;

        minDistance = Mathf.Infinity;

        itemSelected.GetComponent<TextMeshProUGUI>().color = Color.white;
    }
}
