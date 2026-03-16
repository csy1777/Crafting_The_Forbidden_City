using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AdvancedCardType
{
    woodenComponent,
    stoneComponent,
    tileComponent,
    decorativeComponent,
    none
}
public class AdvancedCard : Card
{
    public AdvancedCardType advancedCardType;
}
