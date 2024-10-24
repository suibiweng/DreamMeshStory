using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.UI;

namespace BrainCheck {
	public class UnityReceiveMessages : MonoBehaviour {
		public static UnityReceiveMessages Instance;
		public GameObject guiTex;
		public Texture2D texture2D;

		void Awake(){
			Instance = this;
			// guiTex.GetComponent<MeshRenderer>().material.mainTexture = texture2D;
		}

		// Use this for initialization
		void Start () {
		}

		// Update is called once per frame
		void Update () {
		}

		public void statusMessage(string message) {
			GetComponent<TextMesh>().text = message + "\n" + GetComponent<TextMesh>().text;
		}

		public void clearMessage() {
			GetComponent<TextMesh>().text = "";
		}

		public void ImageSavedStatus(string imageStatus){
			texture2D = new Texture2D (200, 200);
			string currentImageStatus = imageStatus;
			string imagePath = Application.persistentDataPath + "/Test.jpg";
			byte[] imageBytes = File.ReadAllBytes(imagePath);
			if(imageBytes == null)
				currentImageStatus = "Please Try Again To Laod Image";
			GetComponent<TextMesh>().text = currentImageStatus;
			texture2D.LoadImage(imageBytes);
			guiTex.GetComponent<MeshRenderer>().material.mainTexture = texture2D;
		}
	}
}
