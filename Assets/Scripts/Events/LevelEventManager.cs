using System.Collections.Generic;
using UnityEngine;

public class LevelEventManager : MonoBehaviour
{
    [System.Serializable]
    public class EventEntry
    {
        public string name;
        public float timeToTrigger; 
        public bool isRepeating;    
        public float repeatInterval = 5f;
        public GameObject eventPrefab;

        [HideInInspector] public bool hasTriggered;
    }

    [SerializeField] private List<EventEntry> timeline = new List<EventEntry>();
    private float timer;

    void Update()
    {
        if (Time.timeScale == 0) return;

        timer += Time.deltaTime;

        foreach (var entry in timeline)
        {
            if (timer >= entry.timeToTrigger)
            {
                if (!entry.hasTriggered)
                    Trigger(entry);
            }
        }
    }

    private void Trigger(EventEntry entry)
    {
        GameObject go = Instantiate(entry.eventPrefab);

        if (go.TryGetComponent<ILevelEvent>(out ILevelEvent evt))
        {
            evt.Execute(() =>
            {
                if (entry.isRepeating)
                {
                    entry.timeToTrigger = timer + entry.repeatInterval;
                    entry.hasTriggered = false; 
                }
            });

            entry.hasTriggered = true; 
        }

        if (entry.isRepeating)
            entry.timeToTrigger = timer + entry.repeatInterval;
        else
            entry.hasTriggered = true;
    }
}
