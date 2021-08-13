using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NMHSoundMng : NMHSingleton<NMHSoundMng>
{
    public enum BGMList
    {
        MAIN,
        IN_GAME
    }

    public enum EffectList
    {
        ATTACK,
        ATTACK_ENEMY,
        BUTTON,
        MISS
    }

    public AudioClip[] bgms;
    public AudioClip[] fx;

    public AudioSource bgmSource;
    public AudioSource fxSource;



	void Start ()
    {
        DontDestroyOnLoad(this);	
	}

    

    public void RunBGM(BGMList _list)
    {
        bgmSource.clip = bgms[(int)_list];
        bgmSource.Play();
    }

    public void RunBGM(string _key)
    {
        switch (_key)
        {
            case "MAIN":
                bgmSource.clip = bgms[(int)BGMList.MAIN];
                bgmSource.Play();
                break;
            case "IN_GAME":
                bgmSource.clip = bgms[(int)BGMList.IN_GAME];
                bgmSource.Play();
                break;
        }
    }

    public void RunFX(EffectList _list)
    {
        fxSource.clip = fx[(int)_list];
        fxSource.PlayOneShot(fx[(int)_list]);
    }

    public void RunFX(string _key)
    {
        switch(_key)
        {
            case "ATTACK":
                fxSource.clip = fx[(int)EffectList.ATTACK];
                fxSource.PlayOneShot(fx[(int)EffectList.ATTACK]);
                break;
            case "ATTACK_ENEMY":
                fxSource.clip = fx[(int)EffectList.ATTACK_ENEMY];
                fxSource.PlayOneShot(fx[(int)EffectList.ATTACK_ENEMY]);
                break;
            case "BUTTON":
                fxSource.clip = fx[(int)EffectList.BUTTON];
                fxSource.PlayOneShot(fx[(int)EffectList.BUTTON]);
                break;
            case "MISS":
                fxSource.clip = fx[(int)EffectList.MISS];
                fxSource.PlayOneShot(fx[(int)EffectList.MISS]);
                break;
        }
    }
}
