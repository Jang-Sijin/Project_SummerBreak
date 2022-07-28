using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapOpenTrigger : MonoBehaviour
{
    
    [SerializeField]
    private GameObject mapPiece;

    [SerializeField] 
    private bool mapPieceable;

    public void SetActiveMapPiece()
    {
        if (mapPiece != null)
        {
            mapPiece.SetActive(true);
            mapPieceable = true;
        }
    }

    public GameObject GetMapPiece()
    {
        return mapPiece;
    }

    public bool GetMapPieceable()
    {
        return mapPieceable;
    }

}
