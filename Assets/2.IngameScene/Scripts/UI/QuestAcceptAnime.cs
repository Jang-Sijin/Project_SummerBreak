using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;

public class QuestAcceptAnime : MonoBehaviour
{
    [Header("[애니메이션 시간 설정]")]
    [SerializeField] private float _fadeTime;

    [Header("[애니메이션 시간 설정]")]
    [SerializeField] private Image _mainPanelImage;
    [SerializeField] private TextMeshProUGUI _mainQuestTitleText;
    [SerializeField] private Image _questTitleImage;
    [SerializeField] private TextMeshProUGUI _questTitleText;
    [SerializeField] private Image _questCompleteCheckImage;
    [SerializeField] private Image _stampSpaceImage;
    [SerializeField] private Image _stampImage;

    private bool _isCompleteQuest = false; // 퀘스트를 클리어 유(true)/무(false)

    void Start()
    {
        DOTween.PlayAll();
    }

    #region EventFunc
    // 특정 조건에 해당되는 함수를 불러오세요.
    
    // 1.퀘스트를 수락 했을 때 출력되는 애니메이션
    public void QuestStartAnime()
    {
        _isCompleteQuest = false;
        this.gameObject.SetActive(true);
    }
    
    // 2.퀘스트를 완료 했을 때 출력되는 애니메이션
    public void QuestCompleteAnime()
    {
        _isCompleteQuest = true;
        _stampImage.gameObject.SetActive(true);
        this.gameObject.SetActive(true);
    }
    #endregion

    private void OnEnable()
    {
        _mainPanelImage.color = StartSetAlpha(_mainPanelImage.color);
        _mainQuestTitleText.color = StartSetAlpha(_mainQuestTitleText.color);
        _questTitleImage.color = StartSetAlpha(_questTitleImage.color);
        _questTitleText.color = StartSetAlpha(_questTitleText.color);
        _questCompleteCheckImage.color = StartSetAlpha(_questCompleteCheckImage.color);
        _stampSpaceImage.color = StartSetAlpha(_stampSpaceImage.color);
        _stampImage.color = StartSetAlpha(_stampImage.color);

        if (_isCompleteQuest)
            QuestCompleteAnimation();
        else
            QuestStartAnimation();
    }

    // 퀘스트 수락 애니메이션
    private void QuestStartAnimation()
    {
        int alphaValue = 1; // Alpha == 255
        int fadeOverShoot = 2; // FadeIn 1회, FadeOut 1회

        _mainPanelImage.DOFade(alphaValue, _fadeTime).SetEase(Ease.OutFlash, fadeOverShoot).OnComplete(CompleteAnime);
        _mainQuestTitleText.DOFade(alphaValue, _fadeTime).SetEase(Ease.OutFlash, fadeOverShoot);
        _questTitleImage.DOFade(alphaValue, _fadeTime).SetEase(Ease.OutFlash, fadeOverShoot);
        _questTitleText.DOFade(alphaValue, _fadeTime).SetEase(Ease.OutFlash, fadeOverShoot);
        _questCompleteCheckImage.DOFade(alphaValue, _fadeTime).SetEase(Ease.OutFlash, fadeOverShoot);
        _stampSpaceImage.DOFade(alphaValue, _fadeTime).SetEase(Ease.OutFlash, fadeOverShoot);
        // _stampImage.DOFade(alphaValue, _fadeTime).SetEase(Ease.OutFlash, fadeOverShoot);
    }

    // 퀘스트 완료 애니메이션
    private void QuestCompleteAnimation()
    {
        int alphaValue = 1; // Alpha == 255
        int fadeOverShoot = 2; // FadeIn 1회, FadeOut 1회

        _mainPanelImage.DOFade(alphaValue, _fadeTime).SetEase(Ease.OutFlash, fadeOverShoot).OnComplete(CompleteAnime);
        _mainQuestTitleText.DOFade(alphaValue, _fadeTime).SetEase(Ease.OutFlash, fadeOverShoot);
        _questTitleImage.DOFade(alphaValue, _fadeTime).SetEase(Ease.OutFlash, fadeOverShoot);
        _questTitleText.DOFade(alphaValue, _fadeTime).SetEase(Ease.OutFlash, fadeOverShoot);
        _questCompleteCheckImage.DOFade(alphaValue, _fadeTime).SetEase(Ease.OutFlash, fadeOverShoot);
        _stampSpaceImage.DOFade(alphaValue, _fadeTime).SetEase(Ease.OutFlash, fadeOverShoot);
        _stampImage.DOFade(alphaValue, _fadeTime).SetEase(Ease.OutFlash, fadeOverShoot);
    }

    private Color StartSetAlpha(Color paramColor) // 애니메이션 시작 전 Alpha값을 0으로 설정한다. (패널이 안보이게 시작)
    {
        Color setAlpha = new Color(paramColor.r, paramColor.g, paramColor.b, 0);

        return setAlpha;
    }

    private void CompleteAnime() // 애니메이션이 끝나면 자동으로 Canvas 비활성화
    {
        this.gameObject.SetActive(false);
        
        // 스템프 이미지
        if (_isCompleteQuest)
        {
            _stampImage.gameObject.SetActive(false);
            _isCompleteQuest = false;
        }
    }
}
