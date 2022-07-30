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

    private PlayerStatus _playerStatus;
    private void Start()
    {
        _playerStatus = GameManager.instance.playerGameObject.GetComponent<PlayerStatus>();
    }

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
            && _playerStatus.landMarkEnable[landMarkNumber - 1] == false)
        {
            _playerStatus.landMarkEnable[landMarkNumber - 1] = true;
            PlayableDirector cutScene = landMarkCutScene.GetComponent<PlayableDirector>();
            cutScene.Play();
        }
        else if (landMarkCutScene == null) // 컷씬 없음: 4번 랜드마크
        {
            _playerStatus.landMarkEnable[landMarkNumber - 1] = true;
        }
    }
    
    public bool GetMapPieceable()
    {
        return mapPieceAble;
    }

}
