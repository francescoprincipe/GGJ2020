using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerImage : MonoBehaviour
{
    private Image image;
    private bool isVisible = false;
    public bool FollowCamera;

    private void Start()
    {
        image = GetComponent<Image>();
    }

    private void LateUpdate()
    {
        if (FollowCamera)
        {
            transform.LookAt(Camera.main.transform.position);
        }
        else
        {
            transform.forward = Camera.main.transform.forward;
        }
    }

    public void ChangeImageSprite(Sprite newSprite)
    {
        image.sprite = newSprite;
    }

    public void ChangeImageState()
    {
        if (isVisible)
        {
            image.enabled = false;
        }
        else
        {
            image.enabled = true;
        }
        isVisible = !isVisible;
    }
}
