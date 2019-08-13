using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YellowBoxes : SullyTrigger
{
    public CounterUpdater counter;
    protected override IEnumerator PlayAndDelete()
    {
        yield return new WaitUntil(() => !source.isPlaying);
        counter.minimumForUnlock += 1;
        yield return new WaitForSeconds(1f);
        counter.minimumForUnlock += 1;
        source.clip = Resources.Load<AudioClip>("SullyVOs/Ryan-AlmostDone");
        source.Play();
        yield return new WaitUntil(() => !source.isPlaying);
        Destroy(gameObject);
    }
}
