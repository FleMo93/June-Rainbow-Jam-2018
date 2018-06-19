using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_FadeOutMovement : MonoBehaviour {

    public Vector3 movement;
    public float fadeSpeed = 1;
    public float curFade = 0;
    Vector3 oldpos;

    public TextMesh textComp;
    public SpriteRenderer SrComp;

    private void Start()
    {
        oldpos = transform.position;
    }

    // Update is called once per frame
    void Update () {

        if (textComp == null || SrComp == null)
        {
            Debug.LogWarning("Missing components");
            Destroy(gameObject);
        }


        if (curFade >= 1)
        {
            Destroy(gameObject);
        }


        if (curFade > 0.5f)
        {
            textComp.color = new Color(textComp.color.r, textComp.color.g, textComp.color.b, (1 - curFade) * 2.0f);
            SrComp.color = new Color(SrComp.color.r, SrComp.color.g, SrComp.color.b, (1 - curFade) * 2.0f);
        }



        curFade += fadeSpeed * Time.deltaTime;
        transform.position = Vector3.Lerp(oldpos, oldpos + movement , curFade);

	}
}
