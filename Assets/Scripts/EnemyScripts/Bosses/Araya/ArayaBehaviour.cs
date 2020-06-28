using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArayaBehaviour : MonoBehaviour
{
    private DamageZone damageZone;
    private DamageZone cansZone;

    private Animator   arayaAnim;
    private Animator   cansAnim;

    private float shockTimer = 10;

    public bool isShock = false;

    // Start is called before the first frame update
    void Start()
    {
        damageZone = GetComponent<DamageZone>();
        arayaAnim = GetComponent<Animator>();
        cansZone = transform.GetChild(0).GetComponent<DamageZone>();
        cansAnim = transform.GetChild(0).GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(cansAnim);

        shockTimer -= Time.deltaTime;

        if (shockTimer <= 0)
            StartCoroutine("ArayaShock");
    }

    IEnumerator ArayaShock()
    {
        shockTimer = Random.Range(10, 20);
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
}
