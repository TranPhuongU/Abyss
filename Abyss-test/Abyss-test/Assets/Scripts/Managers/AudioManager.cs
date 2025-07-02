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
    private int bgmIndex;

    private bool canPlaySFX;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        Invoke("AllowSFX", 1f);
    }
    private void Update()
    {
        if(!playBgm)
            StopAllBGM();
        else
        {
            if (!bgm[bgmIndex].isPlaying)
                PlayBGM(bgmIndex);
        }
    }
    public void PlaySFX(int _sfxIndex, Transform _source)
    {
        //if (sfx[_sfxIndex].isPlaying)
           // return;
        if(canPlaySFX == false)
            return;

        if (_source != null && Vector2.Distance(PlayerManager.instance.player.transform.position, _source.position) > sfxMinimumDistance) 
            return;

        if(_sfxIndex < sfx.Length)
        {
            //random pitch để âm thanh khác nhau 1 xíu mỗi lần được gọi
            sfx[_sfxIndex].pitch = Random.Range(0.85f, 1.1f);
            sfx[_sfxIndex].Play();
        }
    }

    public void PlaySFX2(int _sfxIndex, Transform _source)
    {
        if (!canPlaySFX || _sfxIndex >= sfx.Length || _source == null)
            return;

        float distance = Vector2.Distance(PlayerManager.instance.player.transform.position, _source.position);
        float maxDistance = sfxMinimumDistance;

        // Nếu vượt quá khoảng giới hạn, không phát
        if (distance > maxDistance)
            return;

        float volumeScale = Mathf.Clamp01(1f - (distance / maxDistance)); // Tỉ lệ âm giảm dần

        sfx[_sfxIndex].volume = volumeScale;

        sfx[_sfxIndex].Play();
    }


    public void StopSFX(int _index) => sfx[_index].Stop();

    public void StopSFXWithTime(int _index) => StartCoroutine(DecreaseVolume(sfx[_index]));

    private IEnumerator DecreaseVolume(AudioSource _audio)
    {
        float defaultVolume = _audio.volume;

        while (_audio.volume > 0.1f)
        {
            _audio.volume -= _audio.volume * 0.2f;
            yield return new WaitForSeconds(.6f);

            if(_audio.volume <= 0.1f)
            {
                _audio.Stop();
                _audio.volume = defaultVolume;
                break;
            }
        }
    }

    public void PlayRandomBGM()
    {
        bgmIndex = Random.Range(0, bgm.Length);
        PlayBGM(bgmIndex);
    }

    public void PlayBGM(int _bgmIndex)
    {
        bgmIndex = _bgmIndex;

        StopAllBGM();

        bgm[_bgmIndex].Play();
    }

    public void StopAllBGM()
    {
        for (int i = 0; i < bgm.Length; i++)
        {
            bgm[i].Stop();
        }
    }

    private void AllowSFX() => canPlaySFX = true;
}
