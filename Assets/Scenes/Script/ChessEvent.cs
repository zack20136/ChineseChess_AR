using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Common;
using Lean.Touch;

public class ChessEvent : MonoBehaviour
{
    public void ChessOnSelect()
    {
        GameObject.Find("Chesses").SendMessage("ChessOnSelect", transform.parent.gameObject.name);
    }

    public void ChessOffSelect()
    {
        GameObject.Find("Chesses").SendMessage("ChessOffSelect");
    }
}
