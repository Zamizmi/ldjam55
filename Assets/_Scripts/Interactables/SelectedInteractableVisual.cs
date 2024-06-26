using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedInteractableVisual : MonoBehaviour
{
    private BaseInteractable interactable;
    [SerializeField] private GameObject[] visualGameObjectArray;
    private bool isActive = true;

    private void Start()
    {
        Player.Instance.OnSelectedInteractableChanged += Player_OnSelectedInteractableChanged;
        interactable = GetComponent<BaseInteractable>();
    }

    public void SetIsActive(bool newBool)
    {
        isActive = newBool;
        if (!newBool)
        {
            Hide();
        }
    }

    private void Player_OnSelectedInteractableChanged(object sender, Player.OnSelectedInteractableChangedEventArgs e)
    {
        if (isActive)
        {

            if (e.selectedInteractable == interactable)
            {
                Show();
            }
            else
            {
                Hide();
            }
        }
    }

    private void Show()
    {
        foreach (GameObject visualGameObject in visualGameObjectArray)
        {
            visualGameObject.SetActive(true);
        }
    }

    private void Hide()
    {
        foreach (GameObject visualGameObject in visualGameObjectArray)
        {
            visualGameObject.SetActive(false);
        }
    }
}
