using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapOpenTrigger : MonoBehaviour
{
    
    [SerializeField]
    private GameObject mapPiece;

    public void SetActiveMapPiece()
    {
        mapPiece.SetActive(true);
    }
    
}
