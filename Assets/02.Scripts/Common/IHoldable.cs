using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EItemType
{
    Extinguisher,
    Food,
    Dish,
    Pan,
    Coke,
    Basket,
}
public abstract class IHoldable : MonoBehaviour
{
    public EItemType ItemType;

    protected Character _holdCharacter;
    
    public bool IsHold => _holdCharacter != null;

    //public bool Placeable = false;

    
    public abstract void Hold(Character character, Transform handTransform);
    public abstract void UnHold(Vector3 dropPosition, Quaternion dropRotation);
    public abstract Vector3 DropOffset { get; }

    //public abstract Quaternion DropOffset_Rotation { get; }

    public virtual bool IsProcessed => true;

    public abstract void Place(Transform place);

    public void Destroy()
    {
        // 음식 오브젝트 삭제
        Destroy(gameObject);
    }

    internal void Hold(Character nearbyCharacter, object handTransform)
    {
        throw new NotImplementedException();
    }
}
