using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AdvancedCardType
{
    woodenComponent,
    stoneComponent,
    tileComponent,
    decorativeComponent,
}
public class AdvancedCard : MonoBehaviour
{
    public AdvancedCardType cardType;
}
