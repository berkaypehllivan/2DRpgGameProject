using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] private float sfxMinimumDistance;
    [SerializeField] private AudioSource[] sfx;
    [SerializeField] private AudioSource[] bgm;

    public bool playBgm;
    public bool playRain;

    private int rainSfx;

    private int bgmIndex;
    private int sfxIndex;

    private bool canPlaySfx;

    [Header("Menu Music")]
    [SerializeField] private AudioClip menuMusic; // Inspector'dan atayaca��n�z men� m�zi�i
    [SerializeField] private int menuBGMIndex = -1; // Varsay�lan BGM dizisinden farkl� olsun

    private void Awake()
    {
        if (instance != null)
            Destroy(instance.gameObject);
        else
            instance = this;

        rainSfx = 23;

        Invoke("AllowSfx", 1);
    }

    private void Update()
    {
        if (!playBgm)
            StopAllBGM();
        else
        {
            if (!bgm[bgmIndex].isPlaying)
                PlayBGM(bgmIndex);
        }

        if (playRain)
        {
            if (!sfx[rainSfx].isPlaying)
                sfx[rainSfx].Play();
        }
        else
        {
            sfx[rainSfx].Stop();
        }
    }

    public void PlayMenuMusic()
    {
        if (menuMusic != null)
        {
            // Mevcut BGM'leri durdur
            StopAllBGM();

            // �zel men� m�zi�ini �al (mevcut BGM kaynaklar�ndan birini kullanarak)
            bgm[0].clip = menuMusic; // �lk BGM kayna��n� kullan�yoruz
            bgm[0].loop = true;
            bgm[0].Play();
        }
        else if (menuBGMIndex >= 0 && menuBGMIndex < bgm.Length)
        {
            // Veya BGM dizisinden belirli bir indexi �al
            PlayBGM(menuBGMIndex);
        }
        else
        {
            Debug.LogWarning("Menu music not assigned!");
        }
    }

    public void PlaySwitchSFX(AudioClip clip)
    {
        if (clip == null || !canPlaySfx) return;

        // Bo� bir AudioSource bul veya yeni olu�tur
        foreach (var source in sfx)
        {
            if (!source.isPlaying)
            {
                source.clip = clip;
                source.Play();
                return;
            }
        }

        // T�m kaynaklar doluysa, ilkini kullan
        sfx[0].clip = clip;
        sfx[0].Play();
    }

    public void PlaySFX(int _sfxIndex, Transform _source = null)
    {
        // Null ve dizi s�n�r kontrol�
        if (sfx == null || _sfxIndex < 0 || _sfxIndex >= sfx.Length || sfx[_sfxIndex] == null)
        {
            Debug.LogWarning("SFX not properly initialized!");
            return;
        }

        // Mevcut sfx dizisindeki kayna�� kullan
        if (_source != null)
        {
            float distance = Vector3.Distance(PlayerManager.instance.player.transform.position, _source.position);
            if (distance > sfxMinimumDistance)
                return;

            sfx[_sfxIndex].transform.position = _source.position;
            sfx[_sfxIndex].Play();
        }
        else
        {
            sfx[_sfxIndex].Play();
        }
    }

    public void StopSFX(int _sfxIndex) => sfx[_sfxIndex].Stop();

    public void PlayRandomBGM()
    {
        bgmIndex = Random.Range(0, bgm.Length);
        PlayBGM(bgmIndex);
    }

    public void StopSfxWithTime(int _index) => StartCoroutine(DecreaseVolume(sfx[_index]));

    private IEnumerator DecreaseVolume(AudioSource _audio)
    {
        float defaultvolume = _audio.volume;

        while (_audio.volume > .1f)
        {
            _audio.volume -= _audio.volume * .2f;
            yield return new WaitForSeconds(.6f);

            if (_audio.volume <= .1f)
            {
                _audio.volume = defaultvolume;
                break;
            }
        }
    }

    public void PlayBGM(int _bgmIndex)
    {
        bgmIndex = _bgmIndex;

        StopAllBGM();

        bgm[bgmIndex].Play();
    }

    public void PlayUISfx(int _fxIndex)
    {
        sfxIndex = _fxIndex;

        sfx[sfxIndex].Play();
    }

    public void StopAllBGM()
    {
        for (int i = 0; i < bgm.Length; i++)
        {
            bgm[i].Stop();
        }
    }

    private void AllowSfx() => canPlaySfx = true;
}
