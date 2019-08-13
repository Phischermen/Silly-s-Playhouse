using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoSave : MonoBehaviour
{
    [SerializeField]
    private bool once;
    [SerializeField]
    private float recharge;
    private bool charged = true;
    private Coroutine coroutine;
    private void OnTriggerEnter(Collider other)
    {
        if (charged)
        {
            SaveContainer.Save("autosave.save");
            if (once)
            {
                Destroy(gameObject);
            }
            charged = false;
            coroutine = StartCoroutine("Recharge");
        }
    }
    IEnumerator Recharge()
    {
        yield return(new WaitForSeconds(recharge));
        charged = true;
    }
    private void OnDestroy()
    {
        if(coroutine != null)
            StopCoroutine(coroutine);
    }
}
