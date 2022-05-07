using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapOpenTrigger : MonoBehaviour
{
    
    [SerializeField]
    private GameObject mapPiece;

    public void SetActiveMapPiece()
    {
        if (mapPiece != null)
        {
            mapPiece.SetActive(true);
        }
    }

    public GameObject GetMapPiece()
    {
        return mapPiece;
    }
    
}
