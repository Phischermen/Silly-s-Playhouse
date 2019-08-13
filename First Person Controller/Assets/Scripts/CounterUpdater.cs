using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class CounterUpdater : MonoBehaviour
{
    [SerializeField]
    Transform M3Grid;
    Text counter;
    Text threshold;
    Image image;
    Sprite lockSprite;
    Sprite unlockSprite;
    [SerializeField]
    DoorInteraction door;
    [SerializeField]
    CounterUpdater parrot;
    [SerializeField]
    public int minimumForUnlock;
    int count;
    [SerializeField]
    bool on;
    

    public bool isOn { get; private set; }
    public bool isBelowThreshold { get; private set; }
    public bool isParrot { get; private set; }
    Coroutine coroutine;
    // Start is called before the first frame update
    private void Awake()
    {
        counter = transform.Find("Counter").GetComponent<Text>();
        threshold = transform.Find("Threshold").GetComponent<Text>();
        image = transform.Find("Lock").GetComponent<Image>();
        lockSprite = Resources.Load<Sprite>("Sprites/Lock");
        unlockSprite = Resources.Load<Sprite>("Sprites/Unlock");
    }
    void Start()
    {
        isParrot = (parrot!=null);
        if (MyCharacterController.firstSpawnInScene)
        {
            updateMyself();
            powerPanel(on);
        }
    }

    private void OnDestroy()
    {
        if(coroutine != null)
            StopCoroutine(coroutine);
    }
    public void powerPanel(bool on)
    {
        isOn = on;
        if(coroutine != null)
            StopCoroutine(coroutine);
        if (isOn)
        {
            coroutine = StartCoroutine("UpdateCounter");
            counter.canvasRenderer.SetAlpha(1f);
        }
        else
        {
            counter.canvasRenderer.SetAlpha(0.5f);
        }
    }
    private void LockDoor(bool locked)
    {
        door.twistLock(locked);
    }
    private void updateDisplay()
    {
        threshold.text = minimumForUnlock.ToString();
        counter.text = count.ToString();
        if (isBelowThreshold)
        {
            threshold.canvasRenderer.SetColor(Color.green);
            image.sprite = unlockSprite;
        }
        else
        {
            threshold.canvasRenderer.SetColor(Color.red);
            image.sprite = lockSprite;
        }
    }
    
    private void updateCount()
    {
        if (isParrot)
        {
            count = parrot.count;
            minimumForUnlock = parrot.minimumForUnlock;
            isBelowThreshold = parrot.isBelowThreshold;
        }
        else
        {
            count = M3Grid.childCount - 1;
            isBelowThreshold = (count <= minimumForUnlock);
        }
    }
    private IEnumerator UpdateCounter()
    {
        while (true)
        {
            updateMyself();
            if (!isParrot)
            {
                if (isBelowThreshold && door.isLocked)
                {
                    LockDoor(false);
                }
                else if (!isBelowThreshold && !door.isLocked)
                {
                    LockDoor(true);
                }
            }
            yield return new WaitForSeconds(.1f);
        }
    }
    private void updateMyself()
    {
        updateCount();
        updateDisplay();
    }
    [ContextMenu("UpdateCounter")]
    private void updateEditor()
    {
        count = M3Grid.childCount;
        isBelowThreshold = (count <= minimumForUnlock);

        Awake();
        threshold.text = minimumForUnlock.ToString();
        counter.text = count.ToString();
        if (isBelowThreshold)
        {
            threshold.canvasRenderer.SetColor(Color.green);
            image.sprite = unlockSprite;
        }
        else
        {
            threshold.canvasRenderer.SetColor(Color.red);
            image.sprite = lockSprite;
        }
    }
}
