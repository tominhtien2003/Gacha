using TMPro;
using UnityEngine;

public class ActivatedMessageBoxResult : MonoBehaviour
{
    [SerializeField] private Spin spin;

    [SerializeField] private GameObject resultUI;

    private void Start()
    {
        spin.OnHandleResultWithoutScreen += Spin_OnHandleResultWithoutScreen;
    }

    private void Spin_OnHandleResultWithoutScreen(object sender, Spin.OnHandleResultWithoutScreenEventArg e)
    {
        resultUI.SetActive(true);

        resultUI.GetComponentInChildren<TextMeshProUGUI>().text = e.nameItem.text;
    }
}
