using UnityEngine;

public class FootSteps : MonoBehaviour
{
    [SerializeField]
    private AudioClip[] grassClips;
    [SerializeField]
    private AudioClip[] SandClips;
    [SerializeField]
    private AudioClip[] StoneClips;
    [SerializeField]
    private AudioClip[] DirtClips;
    [SerializeField]
    private AudioClip[] SnowClips;
    [SerializeField]
    private AudioClip[] WoodClips;

    private AudioSource audioSource;
    private TerrainDetector terrainDetector;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        terrainDetector = new TerrainDetector();
    }
    private void Step()
    {
        AudioClip clip = GetRandomClip(terrainDetector);
        audioSource.PlayOneShot(clip);

    }

    private AudioClip GetRandomClip(TerrainDetector terrainDetector)
    {
        int terrainTextureIndex = terrainDetector.GetActiveTerrainTextureIdx(transform.position);

        switch(terrainTextureIndex)
        {
            case 0:
            case 1:
            case 6:
            case 8:
                return grassClips[UnityEngine.Random.Range(0, grassClips.Length)];
            case 2:
            case 10:
                return SandClips[UnityEngine.Random.Range(0, SandClips.Length)];
            case 3:
            case 4:
                return StoneClips[UnityEngine.Random.Range(0, StoneClips.Length)];
            case 5:
            case 7:
            default:
                return DirtClips[UnityEngine.Random.Range(0, DirtClips.Length)];
            case 9:
                return SnowClips[UnityEngine.Random.Range(0, SnowClips.Length)];
            case 11:
                return WoodClips[UnityEngine.Random.Range(0, WoodClips.Length)];
        }
        
    }
}