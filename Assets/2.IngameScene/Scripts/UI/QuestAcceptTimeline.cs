using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;

public class QuestAcceptTimeline : MonoBehaviour
{
    [SerializeField] private PlayableDirector _questAcceptDirector;
    [SerializeField] private PlayableDirector _questCompleteDirector;

    [SerializeField] private TextMeshProUGUI _questMainTitleName;

    public void OnStartQuestAcceptAnime(TMP_Text questTitleName)
    {
        _questMainTitleName.text = questTitleName.text;
        _questAcceptDirector.Play();
    }

    public void OnStartQuestCompleteAnime(TMP_Text questTitleName)
    {
        _questMainTitleName.text = questTitleName.text;
        _questCompleteDirector.Play();
    }
}
