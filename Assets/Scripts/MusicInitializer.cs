using UnityEngine;

// Responsible for bootstrapping the background music in the initial scene.
public class MusicInitializer : MonoBehaviour
{
    public AudioClip backgroundTrack;
    public float targetVolume = 0.5f;

    private void Start()
    {
        if (BackgroundMusicManager.Instance == null)
        {
            GameObject musicManager = new GameObject("BackgroundMusicManager");
            BackgroundMusicManager managerScript = musicManager.AddComponent<BackgroundMusicManager>();
            
            managerScript.musicClip = backgroundTrack;
            managerScript.maxVolume = targetVolume;
            
            managerScript.BeginPlayback();
        }
    }
}