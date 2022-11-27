using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitCardView : MonoBehaviour
{
    public Image fillImage;
    public Image icon;

    public void Inititialization(Sprite iconSprite, Color color)
    {
        icon.sprite = iconSprite;
        fillImage.color = color;
    }
}
