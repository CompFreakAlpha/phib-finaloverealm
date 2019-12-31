using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;


    public Texture2D defaultCursor;
    public Vector2 defaultCursorHotspot;

    public GameObject HUDCanvas;


    public AudioSource PlayClipAt(AudioClip clip, Vector2 pos) { 
        GameObject tempGO = new GameObject("TempAudio"); // create the temp object
        tempGO.transform.position = pos; // set its position
        AudioSource aSource = tempGO.AddComponent<AudioSource>(); // add an audio source
        aSource.clip = clip; // define the clip
                             // set other aSource properties here, if desired
        aSource.Play(); // start the sound
        Destroy(tempGO, clip.length); // destroy object after clip duration
        return aSource; // return the AudioSource reference
    }


    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else
        {
            Destroy(gameObject);
        }
    }

    public IEnumerator IFadeSlowGame(Vignette v, ChromaticAberration c)
    {
        while (c.intensity.value > 0 || v.intensity.value > 0)
        {
            c.intensity.value -= 0.01f;
            v.intensity.value -= 0.01f;
            yield return null;
        }
        
    }

    public IEnumerator ISlowGame(float _duration, float _timeScale, float _waitDur = 0)
    {
        PostProcessVolume PPV = GameObject.FindGameObjectWithTag("Post").GetComponent<PostProcessVolume>();
        Vignette vOrig = PPV.profile.GetSetting<Vignette>();
        ChromaticAberration cOrig = PPV.profile.GetSetting<ChromaticAberration>();
        PPV.profile.TryGetSettings<Vignette>(out Vignette v);
        v.intensity.value = 0.5f;
        v.smoothness.value = 0.5f;
        PPV.profile.TryGetSettings<ChromaticAberration>(out ChromaticAberration c);
        c.intensity.value = 0.75f;

        yield return new WaitForSeconds(_waitDur);
        float originalTimeScale = Time.timeScale; // store original time scale in case it was not 1
        Time.timeScale = _timeScale; // pause
        StartCoroutine(IFadeSlowGame(v, c));
        float t = 0;
        while (t < _duration)
        {
            yield return null; // don't use WaitForSeconds() if Time.timeScale is 0!
            t += Time.unscaledDeltaTime; // returns deltaTime without being multiplied by Time.timeScale
            
        }
        Time.timeScale = originalTimeScale; // restore time scale from before pause
        v = vOrig;
        c = cOrig;
    }


    public void SlowGame(float _duration, float _timeScale = 0)
    {
        StartCoroutine(ISlowGame(_duration, _timeScale));
    }


    public void FadeToScene(string _sceneID)
    {
        GameObject.FindGameObjectWithTag("Transitions").transform.GetChild(0).GetComponent<Animator>().SetTrigger("FadeOut");
    }


    void Start()
    {
        Cursor.SetCursor(defaultCursor, defaultCursorHotspot, CursorMode.Auto);
    }

    void Update()
    {
        UpdateHUD();
        DebugControls();
    }

    public void DebugControls()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            PlayerManager.instance.ChangeHealth(1);
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            PlayerManager.instance.ChangeHealth(-1);
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            PlayerManager.maxHealth++;
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            PlayerManager.maxHealth--;
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            ItemDatabase.SpawnDrop(new ItemStack(ItemDatabase.GetItem("leaf_autumn"), 1), PlayerManager.instance.transform.position);
        }
    }

    public void UpdateHUD()
    {
        //((Mathf.CeilToInt((Player.health - 1) / 5)) * 9) + 16
        HUDCanvas.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2((9 * PlayerManager.maxHealth) + 7, 16);
        HUDCanvas.transform.GetChild(1).GetComponent<RectTransform>().sizeDelta = new Vector2((9 * PlayerManager.health) + 7, 16);
    }
}
