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
    private bool mapPieceable;

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
            mapPieceable = true;
        }
    }


    public void StartCutScene()
    {
        if (landMarkCutScene != null && GetMapPieceable()
            && _playerStatus.landMarkEnable[landMarkNumber - 1] == false)
        {
            _playerStatus.landMarkEnable[landMarkNumber - 1] = true;
            PlayableDirector _cutScene;
            _cutScene = landMarkCutScene.GetComponent<PlayableDirector>();
            _cutScene.Play();
        }
    }
    
    public bool GetMapPieceable()
    {
        return mapPieceable;
    }

}
