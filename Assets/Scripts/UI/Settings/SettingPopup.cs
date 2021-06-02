using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using DG.Tweening;

public class SettingPopup : MonoBehaviour
{
    public AudioMixer mixer;

    public Slider masterSlider;
    public Slider musicSlider;
    public Slider masterSFXSlider;

    public LoadoutState loadoutState;
    public DataDeleteConfirmation confirmationPopup;

    protected float m_MasterVolume;
    protected float m_MusicVolume;
    protected float m_MasterSFXVolume;

    protected const float k_MinVolume = -80f;
    protected const string k_MasterVolumeFloatName = "MasterVolume";
    protected const string k_MusicVolumeFloatName = "MusicVolume";
    protected const string k_MasterSFXVolumeFloatName = "MasterSFXVolume";
    protected const float endY=0;
    protected float currentY=1280;
    protected Image m_Image;
    public void Open()
    {
        if(m_Image==null)
            m_Image=GetComponent<Image>();
        m_Image.enabled=false;
        gameObject.SetActive(true);
        transform.DOLocalMoveY(endY,0.5f).SetEase(Ease.Linear).OnComplete(()=>{ m_Image.enabled=true;});
        UpdateUI();
    }

    public void Close()
    {
        if(m_Image==null)
            m_Image=GetComponent<Image>();
		PlayerData.instance.Save ();
        m_Image.enabled=false;
        transform.DOLocalMoveY(currentY,0.5f).SetEase(Ease.Linear).OnComplete(()=>{
            gameObject.SetActive(false);
        });
    }

    void UpdateUI()
    {
        mixer.GetFloat(k_MasterVolumeFloatName, out m_MasterVolume);
        mixer.GetFloat(k_MusicVolumeFloatName, out m_MusicVolume);
        mixer.GetFloat(k_MasterSFXVolumeFloatName, out m_MasterSFXVolume);

        masterSlider.value = 1.0f - (m_MasterVolume / k_MinVolume);
        musicSlider.value = 1.0f - (m_MusicVolume / k_MinVolume);
        masterSFXSlider.value = 1.0f - (m_MasterSFXVolume / k_MinVolume);
    }

    public void DeleteData()
    {
        confirmationPopup.Open(loadoutState);
    }


    public void MasterVolumeChangeValue(float value)
    {
        m_MasterVolume = k_MinVolume * (1.0f - value);
        mixer.SetFloat(k_MasterVolumeFloatName, m_MasterVolume);
		PlayerData.instance.masterVolume = m_MasterVolume;
    }

    public void MusicVolumeChangeValue(float value)
    {
        m_MusicVolume = k_MinVolume * (1.0f - value);
        mixer.SetFloat(k_MusicVolumeFloatName, m_MusicVolume);
		PlayerData.instance.musicVolume = m_MusicVolume;
    }

    public void MasterSFXVolumeChangeValue(float value)
    {
        m_MasterSFXVolume = k_MinVolume * (1.0f - value);
        mixer.SetFloat(k_MasterSFXVolumeFloatName, m_MasterSFXVolume);
		PlayerData.instance.masterSFXVolume = m_MasterSFXVolume;
    }
    int numCheatEnable=5;
    public void EnableCheat(){
        numCheatEnable--;
        if(numCheatEnable<=0)
        {
            numCheatEnable=5;
            PlayerData.instance.coins+=900000;
            PlayerData.instance.premium+=900000;
        }
    }
}
