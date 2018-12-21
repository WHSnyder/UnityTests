using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetTest : MonoBehaviour {

    public RenderTexture tex;
    private Camera cam;

    Texture2D buffer;
    Rect frame = new Rect(0, 0, 128, 128);

    private byte[] data;

    float curr;


    void Update() {

        if (Input.GetKeyDown("w")) {
        
            cam.targetTexture = tex;

            RenderTexture.active = tex;
            cam.Render();

            buffer.ReadPixels(frame, 0, 0, false);
            buffer.Apply(false);

            RenderTexture.active = null;
            cam.targetTexture = null;

            data = buffer.GetRawTextureData();

            StartCoroutine(Upload(data));
        }
    }





    private void Awake(){

        cam = GetComponent<Camera>();
        cam.targetTexture = null;

        buffer = new Texture2D(128, 128, TextureFormat.BGRA32, false);
    }





    IEnumerator Upload(byte[] myData) {

        UnityWebRequest www = UnityWebRequest.Put("http://127.0.0.1:5000/", myData);
        www.SetRequestHeader("height", 128.ToString());
        www.SetRequestHeader("width", 128.ToString());

        curr = Time.fixedTime;

        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError) {

            Debug.Log(www.error);
        }
        else {
            Debug.Log("Took " + (Time.fixedTime - curr).ToString() + " seconds.");
        }
    }
}