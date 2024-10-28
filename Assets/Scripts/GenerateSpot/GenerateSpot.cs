using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using System;
using UnityEngine;
using TMPro;
using RealityEditor;
using UnityEngine.UI;



public class GenerateSpot : MonoBehaviour
{
    public bool TestConfirmGeneration; 
    public bool isAcopy = false;
    // private NetworkRunner _runner;

    public int id;
    public string downloadURL = "http://34.106.250.143/upload/";

    public string URLID; 


    //Manager
    public RealityEditorManager manager;
    

    public bool isselsected = false;
    
    //public GameObject loadingIcon;
    
    // public ParticleSystem loadingParticles;
    // public Renderer SmoothCubeRenderer; 
    public string Prompt;

    private string oldPrompt; 
    // public DataSync dataSync; 
    
    // public TMP_Text URLIDText;
    
    // //Control Interface
    // public TMP_Text PromtText;
    // public Text LitseningText;
    
    // // UI panel;
    // public GameObject UiMenu;
    // public GameObject[] controlPanels;
    // public Toggle[] PanelsToggles;
    // public GameObject VoicePanel;
    // public GameObject EditMenu;
    // public GameObject ScanningPanel;

    // public GameObject DownloadPanel;
    // public GameObject selectMenu;

    // public GameObject ColorBtn;
    // public GameObject ErasingPanel;

    // public Toggle EraseBtn;
    // public Slider Britheness;


    // LoadObject
    public GameObject TargetObject;

    // public recordData RecordData;
    
    // public Projector erasingProjector;
    
    // public BoundBox Outlinebox;
    ModelDownloader modelDownloader;

    bool promptGenrated = false;
    bool  InstructGen = false,Inpainted=false;

    // public Grabbable _grabbable;
    
    public GenerateType SpotType;
    
    public Shader VertexColor,UnlitShader;

    public Material TargetMaterial;
    // public Material ProjectorMeterial;

    // public Texture WhiteTex,OriginTex;

    // public GameObject EraseQuad;

    // public RawImage PreviewWindow;
    
    // public Transform Player;

    // public Toggle sculptMode,PositionisLock;

    // public bool SculptingModeOn=false;
    
    //Networking
    // public string DataSyncTestNumber; 
    // public RealtimeTransform _realtimeTransform;
    // public RealtimeView _realtimeView;
    // private NetworkObject _networkObject;
    // private PhotonDataSync _photonDataSync;
    // private GenerateSpotRPC _generateSpotRPC; 
  

    
    
    void Start()
    {
        manager = FindObjectOfType<RealityEditorManager>();
        modelDownloader = FindObjectOfType<ModelDownloader>();
        // _runner = FindObjectOfType<NetworkRunner>();
        // _grabbable = GetComponent<Grabbable>();
        downloadURL=manager.ServerURL;
        // Player = manager.PlayerCamera; 
        SpotType = GenerateType.Add;
        // loadingIcon.SetActive(false);
        // loadingParticles.Stop();
        // grabInteractable=GetComponent<GrabInteractable>();
        
        // //NETWORKING
        // _networkObject = GetComponent<NetworkObject>();
        // _photonDataSync = GetComponent<PhotonDataSync>();
        // _generateSpotRPC = GetComponent<GenerateSpotRPC>(); 
      

        // RecordData = GetComponent<recordData>();
        
        // _grabbable.WhenPointerEventRaised += HandlePointerEventRaised;
        
        // //   externalController.GrabbedObjectDelegate += Grab;
        //    externalController.ReleasedObjectDelegate += Release;
        
        // loadingIcon.SetActive(false);
        
        // SpotType = GenerateType.Add;
        // initAdd();
        
        // if (selectMenu != null) selectMenu.SetActive(true);
        //  ControlPanels();

        if(manager.isFireScene){

            // VoicePanel.SetActive(false);
        }

        if (isAcopy)
        {
            // selectMenu.SetActive(false);

        }
    }
    
    bool hasMeshFilter=false;


    // public void changeSculptMode(){
    //     if(sculptMode.isOn){
    //          StartSculpting();
    //          manager.turnSculptingMenu(sculptMode.isOn);

    //     }else{
    //         manager.turnSculptingMenu(sculptMode.isOn);
    //          StopScupting();
             
    //     }

    // }

    // public GrabInteractable grabInteractable;

    public void toLockthePosition(bool toLock)
    {
        if(toLock){

            // grabInteractable.Disable();

           // grabFreeTransformer.enabled=false;
// _grabbable.enabled = false;
            //
        }else{

            
          //  grabFreeTransformer.enabled=true;

            // grabInteractable.Enable();
            // //


        }





    }



    void StopScupting(){

        // SculptingModeOn=false;
        
    }
    
    // public void StartSculpting(){

    //     SculptingModeOn=true;

    //     if(!hasMeshFilter ){
    //         hasMeshFilter = true;
    //         TargetObject.GetComponentInChildren<MeshFilter>().gameObject.AddComponent<SculptingPro_Model>();
    //     }
    // }
    
    public void ChangeID(string ID){
        manager.ChangeID(URLID,ID,this.gameObject);
        URLID=ID;

    }
    
    
     float minDistance = 0.1f; // Minimum distance for full opacity
    float maxDistance = 1f; // 

    void BoundingBoxColorAlhpaDinstance(){
        
        // float distanceToPlayer = Vector3.Distance(transform.position, Player.position);

        // Calculate the alpha value based on the distance
        // float alpha = Mathf.InverseLerp(maxDistance, minDistance, distanceToPlayer);

        // Interpolate the alpha value of the material's color

        // Outlinebox.lineColor=new Color(Outlinebox.lineColor.r, Outlinebox.lineColor.g, Outlinebox.lineColor.b, alpha);
        
    }


    public void setTheType(int select)
    {
        switch (select)
        {
            case 0:
                SpotType = GenerateType.Add;
                initAdd();

                break;

            case 1:
                SpotType = GenerateType.Reconstruction;
                initReconstruction();
                break;

        }
        
    }

    Coroutine ChecktheFile;


    public void initAdd()
    {
      ChecktheFile=  StartCoroutine(CheckURLPeriodically(downloadURL + URLID + "_generated.zip"));
        // loadingParticles.Play();
        isMaterialChanging = false;
        // VoicePanel.SetActive(true);
    }

    void initReconstruction()
    {
        // ScanningPanel.SetActive(true);
    }


    void OpenEditMenu()
    {
       // EditMenu.SetActive(true);
    }

    void CloseEditMenu()
    {
        // EditMenu.SetActive(false);
    }

  bool isErasing=false;
    public void Erasing(){
        
    }

        public bool isGrabing =false;


    




    
    public void OnSelect()
    {
        if (!manager.GenCubesDic.ContainsKey(URLID))
        {
            manager.GenCubesDic.Add(URLID,this.gameObject); //This should be adding it to other peoples dictionaries once the URLID is synced
            manager.IDs++; 
            manager.selectedIDUrl = URLID;
        }
        
        manager.updateSelected(id, URLID);
        // if (_networkObject.HasStateAuthority == false)
        // {
        //     _networkObject.RequestStateAuthority();
        //     _networkObject.AssignInputAuthority(_runner.LocalPlayer);
        // }        
        
        isselsected = true;
        if (SpotType != GenerateType.None)
        {
          //  if (TargetObject.transform.childCount != 0) OpenEditMenu();

        }
    }


    public void Release()
    {
        // Debug.Log("The grabbable script.enabled is: " + GetComponent<Grabbable>().enabled);

        // Debug.Log("should be releasing the cube");

        // URLIDText.text = "should be releasing the cube";
        // Outlinebox.line_renderer=false;

    }

    bool originTex=false;

    public bool setMaterialforGenrated(Transform obj,Shader shader)
    {

        if (obj.childCount == 0)
        {
            return false;

        }
        else
        {
            Renderer renderer;

            renderer = obj.gameObject.GetComponentInChildren<Renderer>();

            renderer.materials[0].shader = shader;
            
            TargetMaterial=renderer.materials[0];
            if(SpotType!=GenerateType.Add)
            {
                if(!originTex){
                    originTex=true;


                    //  OriginTex=TargetMaterial.GetTexture("_MainTex");
                     
                }

            }
            
            return true;
            
        }
        
    }


    public void ControlPanels()
    {
    //     for (int i = 0; i < PanelsToggles.Length; i++)
    //     {

    //         controlPanels[i].SetActive(PanelsToggles[i].isOn);

    //     }

    //     if (controlPanels[0].activeInHierarchy == true || controlPanels[1].activeInHierarchy == true)
    //     {

    //         //AimStart.GetComponent<MeshRenderer>().enabled=false;
    //   //      VoicePanel.SetActive(true);
    //     }
    //     else
    //     {

    //         // RecordData.campoints.Seton(true);
    //   //    VoicePanel.SetActive(false);
    //         //AimStart.GetComponent<MeshRenderer>().enabled=true;


    //     }

    }

    public void onLitsenClick()
    {
        // Prompt = LitseningText.text;

    }

    bool PanelLock;





    // Update is called once per frame
    void Update()
    {
        if (TestConfirmGeneration)
        {
            ConfirmGeneration();
            // _generateSpotRPC.CallConfirmGenerationRPC();
            TestConfirmGeneration = false; 
        }
        
        if (Prompt != oldPrompt)
        {
            // _photonDataSync.UpdatePrompt(Prompt);
        }
        oldPrompt = Prompt; 
        
        // if (_realtimeView.isOwnedLocallySelf)
        // {
        //     dataSync.SetURLID(URLID); 
        //     dataSync.Setprompt(Prompt);
        // }
        
       // toLockthePosition();

        
        // if (manager == null)
        // {
        //     FindObjectOfType<RealityEditorManager2>();  //this shouldnt be necessary
        // }
        
        // URLIDText.text = URLID; //commented this out while trying to figure out data syncing
        
        if (isAcopy)
        {
          //  UiMenu.SetActive(false);
            return;

        }
        
        updateTheTransform();

        switch (SpotType)
        {

            case GenerateType.Add:
                setMaterialforGenrated(TargetObject.transform,VertexColor);
                // EraseBtn.gameObject.SetActive(false);
                // ColorBtn.SetActive(true);

                break;

            case GenerateType.Instruction:
                setMaterialforGenrated(TargetObject.transform,UnlitShader);
                // TargetObject.transform.localEulerAngles=new Vector3(0,-90,90);
                // TargetObject.transform.localScale=new Vector3(5,5,5);

            break;

            case GenerateType.Reconstruction:
                setMaterialforGenrated(TargetObject.transform,UnlitShader);
                TargetObject.transform.localEulerAngles=new Vector3(0,-90,90);
                TargetObject.transform.localScale=new Vector3(8,8,8);
                 //EraseBtn.SetActive(true);
                //  ColorBtn.SetActive(false);
                //  EraseBtn.gameObject.SetActive(true);

                //  isErasing=EraseBtn.isOn;

                //  PreviewWindow.gameObject.SetActive(isselsected);

                //  erasingProjector.gameObject.SetActive(isErasing);

                // EraseQuad.SetActive(false); 
             if(isErasing){


                if(!PanelLock){
                    PanelLock=true;
                    // ErasingPanel.SetActive(true);
                }

                //    TargetMaterial.SetTexture("_MainTex", WhiteTex);
                //    ProjectorMeterial.SetFloat("_Amt",Britheness.value);

             }else{
                    PanelLock=false;
                    // ErasingPanel.SetActive(false);
                //  TargetMaterial.SetTexture("_MainTex", OriginTex);
             }

                



                break;

            case GenerateType.None:

                CloseEditMenu();
                break;






        }


        //  BoundingBoxColorAlhpaDinstance();
         




        // if (isselsected) PromtText.text = Prompt;
        // PromtText.text = Prompt; 


        if (Input.GetKeyDown(KeyCode.X))
        {
                DebugLoadModel();


        }



        if (Input.GetKeyDown(KeyCode.D))
        {

            //_inpainting.jpg

          ChecktheFile=   StartCoroutine(CheckURLPeriodically(downloadURL + "20240325024513_inpainting.jpg"));

        }


        if (Input.GetKeyDown(KeyCode.S))
        {

            InstructGen = false;
            StartCoroutine(CleartheObjinTarget());

        }

        //Text_Instruction.text = RecordData.instruction;



    }

    public void updateTheTransform()
    {



    }

    public void InturuptProcess()
    {
        manager.sendStop();

    }

    public void materialModify()
    {
        isMaterialChanging = true;
        // VoicePanel.SetActive(true);


    }





    bool isMaterialChanging;



    public void ConfirmGeneration(string Prompt){









    }



    public void ConfirmGeneration() 
    {
        
        if (isMaterialChanging)
        {
            Debug.Log("Running ModifyModelinstruction inside confirm generation");

            ModifyModelinstruction();

        }
        else
        {
            Debug.Log("Running generate model inside confirm generation");
            GenrateModel();
            Debug.Log("Finished Running generate model inside confirm generation");

        }
       // OpenEditMenu();

    }









    public string DremmeshPrompt;


    public void GenrateModel()
    {
        manager.promtGenerateModel(id, Prompt, URLID);
        // loadingParticles.Play();
        // SmoothCubeRenderer.enabled = false;
        // Outlinebox.wire_renderer = false;

        DremmeshPrompt=Prompt;

        // PreViewQuad.SetActive(true);
        // loadingIcon.SetActive(true);
        Prompt = "";

    }



    public void GenrateModelPrompt(string prompt)
    {
        manager.promtGenerateModel(id, prompt, URLID);
        // loadingParticles.Play();
        // SmoothCubeRenderer.enabled = false;
        // Outlinebox.wire_renderer = false;

        DremmeshPrompt=Prompt;

        // _generateSpotRPC.CallConfirmGenerationRPC();

        // PreViewQuad.SetActive(true);
        // loadingIcon.SetActive(true);
        Prompt = "";

    }


    void ModifyModelinstruction()
    {
        manager.InstructModify(id, Prompt, URLID);
        // loadingParticles.Play();
        // SmoothCubeRenderer.enabled = false;

        // loadingIcon.SetActive(true);
        Prompt = "";
        InstructGen = false;
        SpotType=GenerateType.Instruction;
        ChecktheFile=StartCoroutine(CheckURLPeriodically(downloadURL + URLID + "_Instruction.zip"));
        StartCoroutine(CleartheObjinTarget());
        //  UiMenu.SetActive(false);

    }


    IEnumerator CleartheObjinTarget()
    {
        yield return new WaitForSeconds(1f);



        int childCount = TargetObject.transform.childCount;

        for (int i = 0; i < childCount; i++)
        {

            Destroy(TargetObject.transform.GetChild(0).gameObject);
        }


    }
    
    
    

    bool StartScanning = false;
    // public TMP_Text Text_Scanning_Btn;
    // public TMP_Text Text_Instruction;

//     public void ScanObject()
//     {

//         if (!StartScanning)
//         {
//             StartScanning = true;
//             RecordData.StartRecording();
//             Text_Scanning_Btn.text = "Stop Scanning";

//         }
//         else
//         {
//             StartScanning = false;
//             Text_Scanning_Btn.text = "Start Scanning";
//             RecordData.StopRecording();

//         }


//     }

//    public void StartToinpainting(){


//          RecordData.StartInpainting();

//         Inpainted=false;
//       ChecktheFile=   StartCoroutine(CheckURLPeriodically(downloadURL + URLID + "_inpainting.jpg"));






//     }

    



    public void DebugLoadModel()
    {
        modelDownloader.LoadModel(
            new ModelIformation()
            {
                ModelURL = "http://192.168.0.139:8000/20240328215957_Instruction.zip",
                gameobjectWarp = TargetObject

            });
        modelDownloader.startDownload();
    }

    public void downloadModel(string url, GameObject warp)
    {
        modelDownloader.AddTask(
            new ModelIformation()
            {
                ModelURL = url,
                gameobjectWarp = warp
            }
        );

        // loadingIcon.SetActive(false);
        // loadingParticles.Stop();
        // SmoothCubeRenderer.enabled = false;

        modelDownloader.startDownload();
    }


    //  public string urlToCheck = "https://www.example.com";
    public float checkInterval = 5f; // Check the URL every 5 seconds
    public event Action<bool> OnURLResponse = delegate { };




    IEnumerator CheckURLPeriodically(string urltocheck)
    {
        yield return new WaitForSeconds(10f);
        while (true)
        {
            yield return CheckURL(urltocheck);
            yield return new WaitForSeconds(checkInterval);
        }
    }

   // string ShapeEDownloadURL, BackGroundURL, TargetURL, instructionURL;
    IEnumerator CheckURL(string url)
    {
        UnityWebRequest www = UnityWebRequest.Get(url);
        UnityWebRequestAsyncOperation requestAsyncOperation = www.SendWebRequest();

        while (!requestAsyncOperation.isDone)
        {
            yield return null;
        }

        if (www.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("URL is responding!");


            if (url.Contains("Instruction"))
            {
          
                if (!InstructGen)
                { //for shape E
                    InstructGen = true;
                    downloadModel(url, TargetObject);
                }

            }

        
            if (url.Contains("generated"))
            {


                if (!promptGenrated)
                { //for shape E
                    promptGenrated = true;
                    downloadModel(url, TargetObject);
                }

            }

            if (url.Contains("inpainting")){

                if(!Inpainted)
                {
                    Inpainted=true;
                   // CreateMaterialAndSetTexture(url, ProjectorMeterial);

                }

                


            }

            StopCoroutine(ChecktheFile);

            OnURLResponse(true);
        }
        else
        {
            //  Debug.LogError("Error checking URL: " + www.error);
            OnURLResponse(false);
        }

        www.Dispose();
    }


    private void OnEnable()
    {
        // OnURLResponse += HandleURLResponse;
    }

    private void OnDisable()
    {
        // OnURLResponse -= HandleURLResponse;
        // _grabbable.WhenPointerEventRaised -= HandlePointerEventRaised;
    }





    public void Remove()
    {

        manager.RemoveSpot(URLID);


    }




   public void CreateMaterialAndSetTexture(string imageUrl,Material sourceMaterial)
    {
        // Check if the source material is assigned
        if(sourceMaterial == null)
        {
            Debug.LogError("Source material is not assigned.");
            return;
        }

        // Create a new material with the same shader as the source material
        Material newMaterial = new Material(sourceMaterial.shader);

        // Optionally copy properties from the source material to the new material
        // newMaterial.CopyPropertiesFromMaterial(sourceMaterial);

        // Start the coroutine to download and apply the texture
        StartCoroutine(DownloadImageCoroutine(imageUrl, newMaterial));
    }
    // public Texture2D erasingTexture;
    [ColorUsage(true, true)]
    public Color Bright,Dark;

    public bool GenerationisComplete=false;
    private IEnumerator DownloadImageCoroutine(string imageUrl, Material material)
    {
        using (UnityWebRequest webRequest = UnityWebRequestTexture.GetTexture(imageUrl))
        {
            // Send the request
            yield return webRequest.SendWebRequest();

            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error downloading image: " + webRequest.error);
            }
            else
            {
                // Get the downloaded texture
                Texture2D downloadedTexture = DownloadHandlerTexture.GetContent(webRequest);
                // erasingTexture=downloadedTexture;
             
                // Apply the texture to the provided material
                material.SetTexture("_ShadowTex",downloadedTexture);
                // erasingProjector.gameObject.SetActive(true);
                // erasingProjector.material=material;
                // // erasingProjector.GetComponent<EraseProjectorControl>().mat=material;
                // erasingProjector.gameObject.SetActive(false);
                // ProjectorMeterial=material;


                
                Debug.Log("Image applied to new material successfully.");
                // loadingIcon.SetActive(false);
                // loadingParticles.Stop();
                GenerationisComplete=true;

                // DownloadPanel.SetActive(true);
            }
        }
    }

    public void deletespot()
    {
        

        Destroy(gameObject);
    }

}

