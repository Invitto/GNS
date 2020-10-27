using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : ScriptableObject
{
    [SerializeField]
    private Sprite icon;

    [SerializeField]
    private int stackSize;

    public SlotScript MySlot;

    public Sprite MyIcon { get => icon; }
    public int MyStackSize { get => stackSize; }
    protected SlotScript Slot { get => MySlot; set => MySlot = value; }

    public void Remove()
    {
        if (MySlot != null)
        {
            MySlot.RemoveItem(this);
        }

    }
}
