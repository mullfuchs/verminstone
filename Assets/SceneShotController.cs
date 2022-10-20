using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class SceneShotController : MonoBehaviour
{

    public SceneObject[] SceneObjects;
    public SceneShot[] CameraPositions;
    // Start is called before the first frame update
    public GameObject currentActiveSceneObject;
    public GameObject mainSceneCamera;
    public GameObject RealCamera;
    public bool skipInitialization = false;

    private Transform OriginalCameraTransform;
    void Start()
    {

        if(mainSceneCamera == null)
        {
            GameObject.FindGameObjectWithTag("MainCamera");
        }

        if (SceneObjects.Length != 0 && !skipInitialization)
        {
            SceneObjects[0].SceneGameObject.SetActive(true);
            currentActiveSceneObject = SceneObjects[0].SceneGameObject;
        }

        if (CameraPositions.Length != 0 && !skipInitialization)
        {
            mainSceneCamera.transform.position = CameraPositions[0].ShotTransform.transform.position;
            mainSceneCamera.transform.rotation = CameraPositions[0].ShotTransform.transform.rotation;
            mainSceneCamera.GetComponent<Camera>().fieldOfView = CameraPositions[0].FOV;
        }


    }

    // Update is called once per frame
    void Update()
    {

    }

    [Yarn.Unity.YarnCommand("changeSceneObject")]
    public void changeSceneObject(string shotName)
    {
        foreach(SceneObject scene in SceneObjects)
        {
            if(scene.SceneName == shotName)
            {
                currentActiveSceneObject.SetActive(false);
                scene.SceneGameObject.SetActive(true);
                currentActiveSceneObject = scene.SceneGameObject;
            }
        }
    }

    [Yarn.Unity.YarnCommand("changeSceneShot")]
    public void changeSceneShot(string shotName)
    {
        foreach (SceneShot shot in CameraPositions)
        {
            if (shot.ShotName == shotName)
            {
                mainSceneCamera.transform.position = shot.ShotTransform.transform.position;
                mainSceneCamera.transform.rotation = shot.ShotTransform.transform.rotation;
                mainSceneCamera.GetComponent<Camera>().fieldOfView = shot.FOV;
            }
        }
    }

    [Yarn.Unity.YarnCommand("disableAutoCam")]
    public void disableAutoCam()
    {
        RealCamera.SetActive(false);
        mainSceneCamera.SetActive(true);
        //mainSceneCamera.GetComponentInParent<UnityStandardAssets.Cameras.AutoCam>().enabled = false;
    }

    [Yarn.Unity.YarnCommand("enableAutoCam")]
    public void enableAutoCam()
    {
        mainSceneCamera.SetActive(false);
        RealCamera.SetActive(true);

        // mainSceneCamera.GetComponentInParent<UnityStandardAssets.Cameras.AutoCam>().enabled = true;
    }


    [Serializable]
    public struct SceneObject
    {
        public string SceneName;
        public GameObject SceneGameObject;
    }

    [Serializable]
    public struct SceneShot
    {
        public string ShotName;
        public GameObject ShotTransform;
        public float FOV;
    }
}
