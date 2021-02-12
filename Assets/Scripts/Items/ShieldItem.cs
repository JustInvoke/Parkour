using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldItem : Item
{
    [SerializeField] private BoxCollider2D boxCollider;

    private protected override void ItemAction(CharacterControl characterControl) {
        //characterControl.ShieldPowerupInitialize();
    }
}
