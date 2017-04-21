using UnityEngine;
using System;
using System.Collections;

public class ScreenshotMovie : MonoBehaviour
{

//	Create a new javascript file in your Unity Project. (It does not have to be in an "Editor" folder.)
//	Name it "ScreenshotMovie".
//	Attach it to the camera you wish to record from.
//	Adjust settings to your liking.
//	Hit play and it should dump out image files. They will be in your project's root folder, so they will not appear in the Unity Editor.

	// The folder we place all screenshots inside.
	// If the folder exists we will append numbers to create an empty folder.
	public string folder = "ScreenshotMovieOutput";
	public int frameRate = 25;
	public int sizeMultiplier = 1;

	private string name = "";
	private string realFolder = "";
	private int suffix = 0;
	private int origFrameRate = 0;


	private bool record = false;
	
	void Start()
	{
		// Set the playback framerate!
		// (real time doesn't influence time anymore)
		//Time.captureFramerate = frameRate;
		
		// Find a folder that doesn't exist yet by appending numbers!
		realFolder = folder;
		int count = 1;
		while (System.IO.Directory.Exists(realFolder))
		{
			realFolder = folder + count;
			count++;
		}
		// Create the folder
		System.IO.Directory.CreateDirectory(realFolder);
	}
	
	void Update()
	{
		if (Input.GetKeyDown ("p")) {
			record = !record;
			if(record){
				name = string.Format("{0}/_{1:yyyy-MM-dd_HH-mm-ss-fff}_", realFolder, DateTime.Now);
				suffix = 0;
				origFrameRate = Time.captureFramerate;
				Time.captureFramerate = frameRate;
			} else{
				Time.captureFramerate = origFrameRate;
			}
		}

		if (record) {
			// name is "realFolder/shot 0005.png"
			//string name = string.Format ("{0}/shot_{1:D04}.png", realFolder, Time.frameCount);
			//string name = string.Format("{0}/{1:yyyy-MM-dd_HH-mm-ss-fff}.png", realFolder, DateTime.Now);
		
			// Capture the screenshot
			Application.CaptureScreenshot (name + (suffix++).ToString().PadLeft(6, '0') + ".png", sizeMultiplier);
		}
	}
}
