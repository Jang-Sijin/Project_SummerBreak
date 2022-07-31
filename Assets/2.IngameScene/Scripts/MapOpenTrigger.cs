using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;

public class MapOpenTrigger : MonoBehaviour
{

    public int landMarkNumber;
    
    [SerializeField]
    private GameObject mapPiece;

    [SerializeField] 
    private GameObject landMarkCutScene;
    
    [SerializeField] 
    private bool mapPieceAble;

    [SerializeField]
    public void SetActiveMapPiece()
    {
        if (mapPiece != null)
        {
            mapPiece.SetActive(true);
            mapPieceAble = true;
        }
    }


    public void StartCutScene()
    {
        if (landMarkCutScene != null 
            && GetMapPieceable()
            && MapPiecesController.instance.landMarkEnable[landMarkNumber - 1] == false)
        {
            if (landMarkNumber == 5)
            {
                for (int i = 0; i < landMarkNumber - 1; ++i)
                {
                    if (MapPiecesController.instance.landMarkEnable[i] == false)
                    {
                        Debug.Log("[이민호] 지도를 다 못채움");
                        return;
                    }
                }
            }
            MapPiecesController.instance.landMarkEnable[landMarkNumber - 1] = true;
            PlayableDirector cutScene = landMarkCutScene.GetComponent<PlayableDirector>();
            cutScene.Play();
        }
        else if (landMarkCutScene == null) // 컷씬 없음: 4번 랜드마크
        {
            MapPiecesController.instance.landMarkEnable[landMarkNumber - 1] = true;
        }
    }
    
    public bool GetMapPieceable()
    {
        return mapPieceAble;
    }

}
