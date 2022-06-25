using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class SkipCutScene : MonoBehaviour
{
    [Header("[스킵 버튼 오브젝트 프리팹]")]
    [SerializeField] private Button skipButton;
    [Header("[스킵 버튼을 눌렀을 때 이동할 TimeLine 시간]")]
    [SerializeField] private float setSkipTime;
    private PlayableDirector _currentDirector;
    private bool _isSkip = true; // 스킵 유무

    private void Start()
    {
        skipButton.onClick.AddListener(OnSkip);
    }

    private void OnSkip()
    {
        // 스킵 버튼이 눌렸을 때
        if (!_isSkip)
        {
            // TimeLine이 설정한 setSkipTime 시간으로 이동한다.  
            _currentDirector.time = setSkipTime;
            _isSkip = true;
        }
    }
    
    // Skip Signal Emitter가 지정된 시간에 PlayableDirector를 받아온다.
    public void GetDirector(PlayableDirector director)
    {
        _isSkip = false;
        _currentDirector = director;
    }
}
