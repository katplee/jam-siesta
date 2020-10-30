using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bed : MonoBehaviour
{    
    public bool IsAvailable { get; set; }
    public bool IsDirty { get; set; }

    [SerializeField]
    private float timeToClean = 2f;
    public float timeFromClean;

    private void Update()
    {
        IsAvailable = (transform.childCount > 0) ? false : true;

        foreach(Transform tr in transform)
        {
            if (tr.GetComponent<DirtySheets>() != null)
            {
                IsDirty = true;
            }
            else
            {
                IsDirty = false;
            }
        }
    }

    private void LateUpdate()
    {
        if (Vector3.Distance(PlayerLocation.Instance.gameObject.transform.position, transform.position) <= 1.5)
        {
            if (IsDirty)
            {
                if(GameManager.Instance.clickRecord[GameManager.Instance.clickRecord.Count-2].name == "Clean Sheets Cabinet")
                {
                    CleanSheets();
                }
            }
        }
        else
        {
            timeFromClean = 0f;
        }
    }

    private void CleanSheets()
    {
        timeFromClean += Time.deltaTime;

        if (Mathf.Clamp(timeFromClean, 0f, timeToClean) == timeToClean)
        {
            timeFromClean = 0f;

            GameObject dirtySheets;
            if(Utilities.HasChildWithComponent<DirtySheets>(gameObject, out dirtySheets)) { }
            dirtySheets.transform.SetParent(PlayerLocation.Instance.transform);
        }        
    }
}
