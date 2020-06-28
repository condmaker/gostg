using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArayaBehaviour : MonoBehaviour
{
    private Vector3[] beamLocations;

    private DamageZone damageZone;
    private EnemyHealth arayaHp;
    private DamageZone cansZone;
    public GameObject beam;

    private Animator   arayaAnim;
    private Animator   cansAnim;
    private SpriteRenderer arayaShock;

    private float shockTimer = 10;
    private float beamTimer = 1;

    public bool isShock = false;

    // Start is called before the first frame update
    void Start()
    {
        damageZone = GetComponent<DamageZone>();
        arayaHp = GetComponent<EnemyHealth>();
        arayaAnim = GetComponent<Animator>();
        arayaShock = GetComponent<SpriteRenderer>();
        cansZone = transform.GetChild(0).GetComponent<DamageZone>();
        cansAnim = transform.GetChild(0).GetComponent<Animator>();

        beamLocations = new Vector3[11];

        beamLocations[0] = new Vector3(-7100, -4066, 0);
        beamLocations[1] = new Vector3(-7069, -4066, 0);
        beamLocations[2] = new Vector3(-7038, -4066, 0);
        beamLocations[3] = new Vector3(-7007, -4066, 0);
        beamLocations[4] = new Vector3(-6976, -4066, 0);
        beamLocations[5] = new Vector3(-6945, -4066, 0);
        beamLocations[6] = new Vector3(-6914, -4066, 0);
        beamLocations[7] = new Vector3(-6883, -4066, 0);

    }

    // Update is called once per frame
    void Update()
    {
        shockTimer -= Time.deltaTime;
        beamTimer -= Time.deltaTime;

        Debug.Log(arayaHp.enemyHealth);

        if (arayaHp.isInvul)
        {
            if (arayaShock.color.a == 1)
                arayaShock.color -= new Color(0, 0, 0, 1);
            else
                arayaShock.color += new Color(0, 0, 0, 1);
        }
        else
            if (arayaShock.color.a == 0) arayaShock.color += new Color(0, 0, 0, 1);

        if ((arayaHp.enemyHealth <= ((arayaHp.premadeHealth * 10) / 2)) && (arayaHp.enemyHealth != 0))
            AddBeam();

        if (shockTimer <= 0)
            StartCoroutine("ArayaShock");
        if (beamTimer <= 0)
            SpawnBeam();
    }

    IEnumerator ArayaShock()
    {
        shockTimer = 10;
        isShock = true;

        arayaAnim.SetBool("isShock", isShock);

        yield return new WaitForSeconds(0.31f);

        cansAnim.SetBool("isShock", isShock);

        damageZone.enabled = true;
        cansZone.enabled = true;
        
        yield return new WaitForSeconds(5);

        isShock = false;
        damageZone.enabled = false;
        cansZone.enabled = false;

        arayaAnim.SetBool("isShock", isShock);
        cansAnim.SetBool("isShock", isShock);
    }

    private void SpawnBeam()
    {
        beamTimer = Random.Range(0.5f, 1);

        int beamIndex = Random.Range(0, beamLocations.Length);

        if (beamIndex <= 7)
            Instantiate(beam, beamLocations[beamIndex], Quaternion.identity);
        else
            Instantiate(beam, beamLocations[beamIndex], Quaternion.Euler(0, 0, -90));
    }

    private void AddBeam()
    {
        beamLocations[8] = new Vector3(-6800, -4468, 0);
        beamLocations[9] = new Vector3(-6800, -4401, 0);
        beamLocations[10] = new Vector3(-6800, -4553, 0);
    }
}
