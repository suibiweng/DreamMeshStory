using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using System.IO;
using System.Collections.Generic;
using System;

namespace BrainCheck {

		public class OcrBridge {

			[DllImport("__Internal")]
			private static extern void _takeScreenshotAndReadCharaters();
			public static IEnumerator takeScreenshotAndReadCharaters(){
				yield return new WaitForEndOfFrame ();
				ScreenCapture.CaptureScreenshot ("Screenshot.png");
				yield return new WaitForSeconds (3);
				_takeScreenshotAndReadCharaters();
			}

			[DllImport("__Internal")]
			private static extern void _takeImageFromCameraAndReadCharaters();
			public static void takeImageFromCameraAndReadCharaters(){
				_takeImageFromCameraAndReadCharaters();
			}

			[DllImport("__Internal")]
			private static extern void _takeImageFromLibraryAndReadCharaters();
			public static void takeImageFromLibraryAndReadCharaters(){
				_takeImageFromLibraryAndReadCharaters();
			}

			[DllImport("__Internal")]
			private static extern void _getImageTakenByCamera();
			public static void getImageTakenByCamera(){
				_getImageTakenByCamera();
			}

			[DllImport("__Internal")]
			private static extern void _setLanguage(string language);
			public static void setLanguage(string language) {
				_setLanguage(language);
			}

			[DllImport("__Internal")]
			private static extern void _sendImageToNativeForOCR(string imagePath);
			public static void sendImageToNativeForOCR(string imagePath){
				_sendImageToNativeForOCR(imagePath);
			}

			public static string SaveTextutreToApplicationPathAndGetPath(Texture2D texture, string imageName) {
				string path = Application.persistentDataPath;
				string savePath = path + "/" + imageName;
				File.WriteAllBytes(savePath, texture.EncodeToPNG());
				return savePath;
			}

			[DllImport("__Internal")]
			private static extern void _setCallBackMethod(string msgReceivingGameObjectName,string msgReceivingMethodName);
			public static void setCallBackMethod(string msgReceivingGameObjectName,string msgReceivingMethodName)
			{
				if (Application.isEditor) {
					return;
				}
				#if UNITY_IPHONE
					_setCallBackMethod( msgReceivingGameObjectName, msgReceivingMethodName);
				#endif
			}
		}
}
