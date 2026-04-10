using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CardType
{
    BaseType,
    AdvancedType,
}
public class Card : MonoBehaviour
{
    public CardType cardType;
    public Cell currentCell;
}
