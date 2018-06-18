using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_testAttachment : MonoBehaviour {

    public Transform AttachingObject;
    public Transform AttachmentPoint;

	// Use this for initialization
	void Start () {
        if (AttachingObject != null && AttachmentPoint != null)
        {
            AttachingObject.parent = AttachmentPoint;
            AttachingObject.localPosition = Vector3.zero;
            AttachingObject.localRotation = Quaternion.identity;
            AttachingObject.localScale = Vector3.one;
        }
	}
	
}
