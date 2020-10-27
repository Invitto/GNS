using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SlotScript : MonoBehaviour, IPointerClickHandler, IClickable
{
    private ObservableStack<Item> items = new ObservableStack<Item>();
    
    [SerializeField]
    private Image icon;

    [SerializeField]
    private Text stackSize;

    private void Awake()
    {
        items.OnPop += new UpdateStackEvent(UpdateSlot);    
        items.OnPush += new UpdateStackEvent(UpdateSlot);    
        items.OnClear += new UpdateStackEvent(UpdateSlot);    
    }

    public bool IsEmpty
    {
        get 
        {
            return items.Count == 0;
        }
    }

    public Item MyItem
    {

        get
        {
            if (!IsEmpty)
            {
                return items.Peek();
            }
            return null;
        }
       
    }

    public Image MyIcon { get => icon; set => icon = value; }

    public int MyCount => items.Count;

    public Text StackSize { get => stackSize; }

    public Text MyStackText => stackSize;

    public bool AddItem(Item item)
    {
        items.Push(item);
        icon.sprite = item.MyIcon;
        icon.color = Color.white;
        item.MySlot = this;
        return true;
    }

    public void RemoveItem(Item item)
    {
        if (!IsEmpty)
        {
            items.Pop();
            
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            UseItem();
        }
    }

    public void UseItem()
    {
        if (MyItem is IUseable)
        {
            (MyItem as IUseable).Use();
        }
        
    }

    public bool StackItem(Item item)
    {
        if (!IsEmpty && item.name == MyItem.name && items.Count < MyItem.MyStackSize)
        {
            items.Push(item);
            item.MySlot = this;
            return true;
        }
        return false;
    }

    private void UpdateSlot()
    {
        UIManager.MyInstance.UpdateStackSize(this);
    }
}
