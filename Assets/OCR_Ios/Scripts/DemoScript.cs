using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;  

namespace BrainCheck {

	public enum OCR_Options 
	{
	  readFromScreenshot,
	  takeImageFromCameraAndReadCharaters,
	  takeImageFromLibraryAndReadCharaters,
	  getImageTakenByCamera,
	  sendImageToNativeForOCR,
	  setLanguage
	}

	public class DemoScript : MonoBehaviour {
		public OCR_Options myOption;
		private string callbackGameObjectName = "UnityReceiveMessage";
	  	private string callbackMethodName = "statusMessage";
	  	public Texture2D sampleImage;
  		private string imageName = "OcrImage.png";
  		private string languageCode = "ja-JP";


		void OnMouseUp() {
	    	StartCoroutine(BtnAnimation());
	 	}

	 	private IEnumerator BtnAnimation()
	    {
	    	Vector3 originalScale = gameObject.transform.localScale;
	        gameObject.transform.localScale = 0.9f * gameObject.transform.localScale;
	        yield return new WaitForSeconds(0.2f);
	        gameObject.transform.localScale = originalScale;
	        ButtonAction();
	    }

	    private void ButtonAction() {
	    	OcrBridge.setCallBackMethod(callbackGameObjectName, callbackMethodName);
	    	UnityReceiveMessages.Instance.clearMessage();
				switch(myOption) {
			    case OCR_Options.readFromScreenshot:
			      StartCoroutine(OcrBridge.takeScreenshotAndReadCharaters());
			      break;
			    case OCR_Options.takeImageFromCameraAndReadCharaters:
			    	OcrBridge.takeImageFromCameraAndReadCharaters();
			      break;
			    case OCR_Options.takeImageFromLibraryAndReadCharaters:
			    	OcrBridge.takeImageFromLibraryAndReadCharaters();
			      break;
			    case OCR_Options.getImageTakenByCamera:
			    	OcrBridge.setCallBackMethod(callbackGameObjectName, "ImageSavedStatus");
			    	OcrBridge.getImageTakenByCamera();
			      break;
			    case OCR_Options.sendImageToNativeForOCR:
			    	string imagePathTemp = OcrBridge.SaveTextutreToApplicationPathAndGetPath(sampleImage, imageName);
		      		OcrBridge.sendImageToNativeForOCR(imagePathTemp);
			      break;
			    case OCR_Options.setLanguage:
		      		OcrBridge.setLanguage(languageCode);
			      break;
				}
	    }
	}
	
}
