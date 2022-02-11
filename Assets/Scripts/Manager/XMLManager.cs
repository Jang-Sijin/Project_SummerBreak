using System;
using System.Data;
using System.IO;
using System.Xml;
using UnityEngine;

public class XMLManager : MonoBehaviour
{
    #region XML Manager 싱글톤 설정
    public static XMLManager instance; // XML Manager을 싱글톤으로 관리
    private void Awake()
    {
        // XML Manager 싱글톤 설정
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        } 
        else
        {
            // 이미 XML Manager가 존재할 때 오브젝트 파괴 
            Destroy(this.gameObject);  
        }
    }
    #endregion
    void Start()
    {
    }
    
    void Update()
    {
        
    }

    private SavePlayerInfo CreateSaveGameObject()
    {
        // 현재 시점에서 오브젝트 이름이 Player를 찾고 할당한다.
        GameObject player = GameObject.Find("Player");
        SavePlayerInfo savePlayerInfo = new SavePlayerInfo
        {
            positionX = player.transform.position.x,
            positionY = player.transform.position.y,
            positionZ = player.transform.position.z,
        
            rotationX = player.transform.rotation.x,
            rotationY = player.transform.rotation.y,
            rotationZ = player.transform.rotation.z
        };
        
        return savePlayerInfo;
    }
    
    public void SaveByMXL()
    {
        // SaveByMXL함수는 xml 파일에 현재 데이터를 저장합니다.
        
        // 현재 캐릭터의 정보가 저장되어 있는 변수입니다.
        SavePlayerInfo savePlayerInfo = CreateSaveGameObject();

        XmlDocument xmlDocument = new XmlDocument();
        string saveFileName = "SaveFile_01";

        XmlElement root = xmlDocument.CreateElement("Save");
        root.SetAttribute("FileName", saveFileName);

        #region XML elements 설정
        
        // XmlElement saveSlotNumElement = xmlDocument.CreateElement("SaveSlotNum");
        // saveSlotNumElement.InnerText = playerInfo.saveSlotNum.ToString(); //XmlNode.InnerText 노드와 모든 자식 노드의 연결된 값을 가져오거나 설정합니다.
        // root.AppendChild(saveSlotNumElement);
        
        XmlElement saveRealTimeElement = xmlDocument.CreateElement("SaveRealTime");
        saveRealTimeElement.InnerText = DateTime.Now.ToString(("yyyy-MM-dd HH:mm:ss tt"));
        root.AppendChild(saveRealTimeElement);

        // XmlElement playTimeElement = xmlDocument.CreateElement("PlayTime");
        // playTimeElement.InnerText = playerInfo.playTime.ToString();
        // root.AppendChild(playTimeElement);
        //
        // XmlElement playerNameElement = xmlDocument.CreateElement("PlayerName");
        // playerNameElement.InnerText = playerInfo.playerName.ToString();
        // root.AppendChild(playerNameElement);
        //
        // XmlElement mapLocationElement = xmlDocument.CreateElement("MapLocation");
        // mapLocationElement.InnerText = playerInfo.mapLocation.ToString();
        // root.AppendChild(mapLocationElement);
        
        XmlElement positionXElement = xmlDocument.CreateElement("PositionX");
        positionXElement.InnerText = savePlayerInfo.positionX.ToString();
        root.AppendChild(positionXElement);
        
        XmlElement positionYElement = xmlDocument.CreateElement("PositionY");
        positionYElement.InnerText = savePlayerInfo.positionY.ToString();
        root.AppendChild(positionYElement);
        
        XmlElement positionZElement = xmlDocument.CreateElement("PositionZ");
        positionZElement.InnerText = savePlayerInfo.positionZ.ToString();
        root.AppendChild(positionZElement);
        
        XmlElement rotationXElement = xmlDocument.CreateElement("RotationX");
        rotationXElement.InnerText = savePlayerInfo.rotationX.ToString();
        root.AppendChild(rotationXElement);
        
        XmlElement rotationYElement = xmlDocument.CreateElement("RotationY");
        rotationYElement.InnerText = savePlayerInfo.rotationY.ToString();
        root.AppendChild(rotationYElement);
        
        XmlElement rotationZElement = xmlDocument.CreateElement("RotationZ");
        rotationZElement.InnerText = savePlayerInfo.rotationZ.ToString();
        root.AppendChild(rotationZElement);
        
        // XmlElement playerHpElement = xmlDocument.CreateElement("Hp");
        // playerHpElement.InnerText = playerInfo.hp.ToString();
        // root.AppendChild(playerHpElement);
        //
        // XmlElement playerStamina = xmlDocument.CreateElement("Stamina");
        // playerStamina.InnerText = playerInfo.stamina.ToString();
        // root.AppendChild(playerStamina);
        
        #endregion

        xmlDocument.AppendChild(root);
        
        // 세이브 파일 위치 설정 및 저장
        string saveDataPathName = "/SaveFile/" + saveFileName + ".xml";
        xmlDocument.Save(Application.dataPath + saveDataPathName);
        
        // 디버그 체크
        if (File.Exists(Application.dataPath + saveDataPathName))
        {
            Debug.Log($"[장시진] savePlayerInfo = x:{savePlayerInfo.positionX}, y:{savePlayerInfo.positionY}, z:{savePlayerInfo.positionZ}");
            Debug.Log($"[장시진] XML 파일 저장 완료!");
        }
        else
        {
            Debug.Log($"[장시진] XML 파일 저장 실패!!!");
        }
    }

    public void LoadByXML()
    {
        // 저장된 XML 파일을 읽어 인게임에 세팅합니다.
        string saveDataPathName = "/SaveFile/SaveFile_01.xml";
        if (File.Exists(Application.dataPath + saveDataPathName))
        {
            SavePlayerInfo savePlayerInfo = new SavePlayerInfo();

            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(Application.dataPath + saveDataPathName);

            XmlNodeList positionX = xmlDocument.GetElementsByTagName("PositionX");
            float positionXNum = float.Parse(positionX[0].InnerText);
            savePlayerInfo.positionX = positionXNum;

            XmlNodeList positionY = xmlDocument.GetElementsByTagName("PositionY");
            float positionYNum = float.Parse(positionY[0].InnerText);
            savePlayerInfo.positionY = positionYNum;

            XmlNodeList positionZ = xmlDocument.GetElementsByTagName("PositionZ");
            float positionZNum = float.Parse(positionZ[0].InnerText);
            savePlayerInfo.positionZ = positionZNum;

            XmlNodeList rotationX = xmlDocument.GetElementsByTagName("RotationX");
            float rotationXNum = float.Parse(rotationX[0].InnerText);
            savePlayerInfo.rotationX = rotationXNum;

            XmlNodeList rotationY = xmlDocument.GetElementsByTagName("RotationY");
            float rotationYNum = float.Parse(rotationY[0].InnerText);
            savePlayerInfo.rotationY = rotationYNum;

            XmlNodeList rotationZ = xmlDocument.GetElementsByTagName("RotationZ");
            float rotationZNum = float.Parse(rotationZ[0].InnerText);
            savePlayerInfo.rotationZ = rotationZNum;
            
            // 게임 실제 데이터에 데이터를 저장합니다.
            GameManager.instance.loadPlayerTransform = transform; // (필수!!!) loadPlayerTransform 초기화 
            GameManager.instance.loadPlayerTransform.position = new Vector3(savePlayerInfo.positionX, savePlayerInfo.positionY, savePlayerInfo.positionZ);
            GameManager.instance.loadPlayerTransform.rotation = Quaternion.Euler(savePlayerInfo.rotationX, savePlayerInfo.rotationY, savePlayerInfo.rotationZ);
        }
        else
        {
            Debug.Log($"[장시진] 세이브 XML파일을 찾을 수 없습니다. 파일 위치:{saveDataPathName}");
        }
    }
}
