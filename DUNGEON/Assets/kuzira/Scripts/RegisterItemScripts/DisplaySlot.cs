using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplaySlot : MonoBehaviour
{
    private Item item;
    private Image image;
    private void Awake()
    {
        image = GetComponent<Image>();
    }
    public void Display(Item displayItem)
    {
        item = displayItem;
        image.sprite = item.GetIcon();
    }

    public void Clear()
    {
        item = null;

        //Awakeより先に呼ばれるっぽいので、nullチェックでimageが取得されていなければ、ここで取得
        if(image == null)
        {
            image = GetComponent<Image>();
        }

        image.sprite = null;
    }

    private void OnDestroy()
    {
        Debug.Log($"DisplaySlot {gameObject.name} was destroyed.");
    }
}
