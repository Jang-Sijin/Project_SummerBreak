using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapPiecesController : MonoBehaviour
{

    [SerializeField] private List<GameObject> mapPiece = new List<GameObject>();
    
    public bool[] landMarkEnable = new bool[5];
    
    #region Singleton
        public static MapPiecesController instance; // PlayerEventSystem 싱글톤으로 관리
        private void Awake()
        {
            // Dialog System 싱글톤 설정
            if (instance == null)
            {
                instance = this;
            }
        }
    #endregion Singleton

    public void LoadMap(bool[] setLandMarkEnable)
    {
        landMarkEnable = setLandMarkEnable;

        MapInit();
    }

    public void MapInit()
    {
        for (int i = 0; i < mapPiece.Count; ++i)
        {
            if (landMarkEnable[i] && !mapPiece[i].activeSelf)
            {
                mapPiece[i].SetActive(true);
            }
        }
    }
}
