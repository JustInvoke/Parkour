using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpItem : Item
{
    [SerializeField] private BoxCollider2D boxCollider;

    private protected override void ItemAction(CharacterControl characterControl) {
        characterControl.JumpPowerupInitialize();
    }
}
