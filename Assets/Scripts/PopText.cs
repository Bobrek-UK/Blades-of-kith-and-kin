using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopText : MonoBehaviour
{
    public void animatorOn(string damageNote, Vector3 damageLocation)
    {
        gameObject.transform.position = damageLocation;
        gameObject.transform.localScale = new Vector3(0.03f, 0.03f, 1);
        gameObject.GetComponent<UnityEngine.UI.Text>().text = damageNote;
        gameObject.GetComponent<Animator>().enabled=true;
        
    }

    public void animatorOff()
    {
        gameObject.transform.localScale = new Vector3(0, 1, 1);
        gameObject.GetComponent<Animator>().enabled = false;
    }
}
