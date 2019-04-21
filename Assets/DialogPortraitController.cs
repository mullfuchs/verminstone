using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class DialogPortraitController : MonoBehaviour
{
    /*
    General dialog setup
    portraits containing the main talking characters
    focusLeft and focusRight do a little tween and fade in animation to imply they're talking
    unfocusLeft and unfocus right undo these
    setLeft and setRight change the sprites
    */
    public GameObject leftPortrait;
    public GameObject rightPortrait;
    
    //array or list for left images here
    //array or list for right images
	public Sprite[] leftImages;
	public Sprite[] rightImages;


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    //some kind of fucking, I dunno, populate image function
    //when a dialog starts it calls a function that gets both dialog members and pulls their dialog portraits in, which are stored on the NPC themselves
    //
	public void populateDialogPortraits(Sprite[] leftPortraits, Sprite[] rightPortraits)
    {
		leftImages = leftPortraits;
		rightImages = rightPortraits;
    }

    [Yarn.Unity.YarnCommand("setLeftPortrait")]
    public void ChangeLeftImage(string imageName)
    {
        print("setting left portrait");
		Sprite portrait = GetImage (imageName, leftImages);
		if (portrait != null) {
			leftPortrait.GetComponent<Image>().sprite = portrait;
		}
       // leftPortrait.GetComponent<UnityEngine.UI.Image>().sprite = ?
    }

    [Yarn.Unity.YarnCommand("setRightPortrait")]
    public void ChangeRightImage(string imageName)
    {
        print("setting right portrait");
		Sprite portrait = GetImage (imageName, rightImages);
		if (portrait != null) {
			rightPortrait.GetComponent<Image> ().sprite = portrait;
		}
    }

	public Sprite GetImage(string imageName, Sprite[] images)
    {

		Sprite s = null;
		foreach (Sprite info in images)
        {
            print("sprint named " + info.name);
			if (info.name == imageName)
            {
                s = info;
                break;
            }
        }
        if (s == null)
        {
			Debug.LogErrorFormat("Can't find sprite named {0}!", imageName);
            return null;
        }
		return s;
    }


}
