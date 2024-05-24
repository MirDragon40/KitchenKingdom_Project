using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EItemType
{
    Extinguisher,
    Food,
    Dish,
}
public abstract class IHoldable : MonoBehaviour
{
    public EItemType ItemType;

    protected Character _holdCharacter;
    
    public bool IsHold => _holdCharacter != null;

    public bool Placeable = false;

    
    public abstract void Hold(Character character, Transform handTransform);
    public abstract void UnHold(Vector3 dropPosition, Quaternion dropRotation);

    public void Place(Makefood makeFood)
    {

    }

    internal void Hold(Character nearbyCharacter, object handTransform)
    {
        throw new NotImplementedException();
    }
}
