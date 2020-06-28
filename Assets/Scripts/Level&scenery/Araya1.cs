using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class Araya1 : MonoBehaviour
{
    public AudioClip bossSong;
    public GameObject boss;

    private CinemachineVirtualCamera cam;
    private GameObject box;
    private SoundMng snd;
    private bool isOn;

    // Start is called before the first frame update
    void Start()
    {
        cam = GameObject.FindObjectOfType<CinemachineVirtualCamera>();
        snd = GameObject.FindObjectOfType<SoundMng>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            var composer = cam.GetCinemachineComponent<CinemachineFramingTransposer>();
            composer.m_SoftZoneWidth = 0;
            composer.m_SoftZoneHeight = 0;
            composer.m_DeadZoneWidth = 0;
            composer.m_DeadZoneHeight = 0;
            cam.m_Lens.OrthographicSize = 250;
            cam.LookAt = transform;
            cam.Follow = transform;

            box = transform.GetChild(0).gameObject;
            Debug.Log(box.name);
            box.SetActive(true);

            snd.PlayMusic(bossSong);

            Instantiate(boss, new Vector3(-7228.03f, -4281, 0), transform.rotation);

        }
    }
}
