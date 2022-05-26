using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicUI : MonoBehaviour
{
    void OnGUI()
    {
        int posX = 10;
        int posY = 10;
        int width = 100;
        int height = 30;
        int spacing = 10;

        List<string> itemList = Managers.Inventory.GetItemList();
        if (itemList.Count == 0)
        {
            GUI.Box(new Rect(posX, posY, width, height), "No items");
        }
        foreach (string item in itemList)
        {
            int count = Managers.Inventory.GetItemCount(item);
            Texture2D image = Resources.Load<Texture2D>("Icons/" + item);
            if (GUI.Button(new Rect(posX, posY, width, height), new GUIContent(" (" + count + ") ", image)))
            {
                Managers.Inventory.EquipItem(item);
            }
            posX += width + spacing;
        }

        string equipped = Managers.Inventory.equippedItem;
        if (equipped != null)
        {
            posX = Screen.width - (width + spacing);
            Texture2D image = Resources.Load("Icons/" + equipped) as Texture2D;
            GUI.Box(new Rect(posX, posY, width, height), new GUIContent("Equipped", image));
        }
    }
}
