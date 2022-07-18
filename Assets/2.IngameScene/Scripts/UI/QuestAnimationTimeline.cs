using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;

public class QuestAnimationTimeline : MonoBehaviour
{
    [SerializeField] private PlayableDirector _questAcceptDirector;
    [SerializeField] private PlayableDirector _questCompleteDirector;

    [SerializeField] private TextMeshProUGUI _questMainTitleName;

    public void OnStartQuestAcceptAnime(string questTitleName)
    {
        _questMainTitleName.text = questTitleName;
        _questAcceptDirector.Play();
    }

    public void OnStartQuestCompleteAnime(string questTitleName)
    {
        _questMainTitleName.text = questTitleName;
        _questCompleteDirector.Play();
    }
}
