using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EItemType
{
    Extinguisher,
    Food,
}
public abstract class IHoldable : MonoBehaviour
{
    public EItemType ItemType;

    protected Character _holdCharacter;
    
    public bool IsHold => _holdCharacter != null;

    
    public abstract void Hold(Character character, Transform handTransform);
    public abstract void UnHold(Vector3 dropPosition, Quaternion dropRotation);

    internal void Hold(Character nearbyCharacter, object handTransform)
    {
        throw new NotImplementedException();
    }
}
