using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class FrameCaptureController : MonoBehaviour
{
    public int frameRate = 30;
    public int totalFrameToCapture = 32;
    public bool startRecording = false;    

    private string frameFolderPath = string.Empty;

    //public List<Texture2D> capturedScreenshots = new List<Texture2D>();

    // Start is called before the first frame update
    void Start()
    {
        // Set the playback framerate (real time will not relate to game time after this).
        Time.captureFramerate = frameRate;
    }

    // Update is called once per frame
    void Update()
    {
        //if (startRecording)
        //{
        //    if (Time.frameCount <= 33)
        //    {
        //        //Append filename to folder name (format is '0005 shot.png"')
        //        string name = string.Format("{0}/frame{1}.png", folder, Time.frameCount);

        //        //Capture the screenshot to the specified file.
        //        ScreenCapture.CaptureScreenshot(name);
        //    }
        //    //if (capturedScreenshots.Count < 34)
        //    //{
        //    //    capturedScreenshots.Add(ScreenCapture.CaptureScreenshotAsTexture());
        //    //}
        //    else
        //    {
        //        startRecording = false;
        //        StartCoroutine(maker.MakeGif());
        //        //capturedScreenshots.RemoveRange(0, 2); //remove the first 2 frames, they are empty            
        //        //CreateGif();
        //        //StartCoroutine(CreateGIF());
        //    }
        //}
    }

    int currFrameCount = 0;
    private void OnPostRender()
    {
        if (startRecording)
        {
            //VERSION 1
            if (++currFrameCount <= totalFrameToCapture)
            {                
                if(!string.IsNullOrEmpty(frameFolderPath))
                {                    
                    string name = string.Format("{0}frame{1:D02}.png", frameFolderPath, currFrameCount);                    
                    ScreenCapture.CaptureScreenshot(name);
                }                
            }
            else
            {
                currFrameCount = 0;
                startRecording = false;                
            }

            //VERSION 2
            //if (Camera.main.targetTexture == null)
            //{
            //    Camera.main.forceIntoRenderTexture = true;
            //    Camera.main.targetTexture = renderTexture;
            //}
            //if (currFrameCount < 33)
            //{
            //    currFrameCount++;
            //    ScreenCaptureFunc(currFrameCount);
            //}
            //else
            //{
            //    startRecording = false;
            //    //StartCoroutine(gifMaker.MakeGif2());
            //}
        }
    }

    IEnumerator StartGif()
    {
        yield return new WaitForSeconds(0.1f);

        Debug.Log(Environment.CurrentDirectory);
    }

    //private void ScreenCaptureFunc(int frameCount)
    //{
    //    Camera mainCam = Camera.main;

    //    RenderTexture.active = mainCam.targetTexture;

    //    // create new texture based on render texture width and height (need to change in the future according to preferred size)
    //    Texture2D imageOverview = new Texture2D(mainCam.targetTexture.width, mainCam.targetTexture.height, TextureFormat.RGBA32, false);
    //    imageOverview.ReadPixels(new Rect(0, 0, mainCam.targetTexture.width, mainCam.targetTexture.height), 0, 0);
    //    imageOverview.Apply();

    //    // encode to png
    //    byte[] bytes = imageOverview.EncodeToPNG();
    //    string name = string.Format("{0}/frame{1}.png", folder, frameCount);
    //    File.WriteAllBytes(name, bytes);
    //}

    public void SetFrameName(MapData _currMapData)
    {
        int slotCount = _currMapData._artifactSlots;
        string parcelFilename = string.Format(Helper.PARCEL_FILENAME, Helper.GetParcelNumber(_currMapData._parcelName));
        //construct the path
        frameFolderPath = string.Format(Helper.OUTPUT_DIST_FOLDER_FRAMES, slotCount, parcelFilename);
        if (!Directory.Exists(frameFolderPath))
            Directory.CreateDirectory(frameFolderPath);
    }

    public void SetSingleName()
    {
        frameFolderPath = Helper.OUTPUT_DIST_FOLDER_SINGLE;
        if (!Directory.Exists(frameFolderPath))
            Directory.CreateDirectory(frameFolderPath);
    }
}
