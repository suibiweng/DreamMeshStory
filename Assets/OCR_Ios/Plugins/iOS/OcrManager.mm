//
//  MusicAppController.mm
//  MusicAppControllerPlugin
//
//  Created by Mayank Gupta on 19/07/17.
//  Copyright (c) 2017 Mayank Gupta. All rights reserved.
//

#import "OcrManager.h"

@interface OcrManager()<AVCaptureVideoDataOutputSampleBufferDelegate> {
    UnityAppController *objectUnityAppController;
    NSString *msgReceivingGameObjectNameGlobal;
    NSString *msgReceivingMethodtNameGlobal;
    AVCaptureSession *session;
    NSString *delimeterString;
    NSString *languageCode;
    dispatch_queue_t queue_imageframe_processing;
}
 
@end

@implementation OcrManager
#pragma mark Unity bridge
UIImage *chosenImage;
+ (OcrManager *)pluginSharedInstance {
    static OcrManager *sharedInstance = nil;
    static dispatch_once_t onceToken;
    dispatch_once(&onceToken, ^{
        sharedInstance = [[OcrManager alloc] init];
        sharedInstance->delimeterString = @"####";
        sharedInstance->queue_imageframe_processing = dispatch_queue_create("MyQueue", DISPATCH_QUEUE_CONCURRENT);
    });
    return sharedInstance;
}



#pragma mark Ios Methods

- (NSString*) getStatusInString:(CallBackStatus) type {
    NSString *result = nil;

    switch(type) {
        case ImageCapturedByCameraOrLibrary:
            result = @"Image Captured Successfully";
            break;
        case ImageCapturedCanceledByUser:
            result = @"User cancelled Image Capture";
            break;
        case ImageCapturedErrorByUser:
            result = @"Error in Image Capture";
            break;
        case ImageSavedToCameraRoll:
            result = @"Image Saved To CameraRoll";
            break;
        case ImageSavedToSpecificAlbum:
            result = @"Image Saved To Album";
            break;
        case ImageLoadedToUnity:
            result = @"Image Loaded To Unity";
            break;
        case ImageProcessingError:
            result = @"Error in Process";
            break;
        default:
            result = @"unknown";
    }

    return result;
}

-(void) readDataFromImage: (UIImage *)image {
    if (@available(iOS 13.0, *)) {
        VNRecognizeTextRequest *request = [[VNRecognizeTextRequest alloc] init];
        __weak OcrManager *weakSelf = self;
        request = [request initWithCompletionHandler:^(VNRequest *request1, NSError *error){
            if (request1.results != nil) {
                NSArray<VNRecognizedTextObservation*>* observations = request1.results;
                for (VNRecognizedTextObservation *i in observations) {
                    NSArray<VNRecognizedText*>*arr = [i topCandidates:1];
                    for (VNRecognizedText *j in arr) {
                        [weakSelf sendMessageToUnity:j.string];
                    }
                }
                [weakSelf sendMessageToUnity:@"OCR Completed"];
            }
        }];
        request.revision = 3;
        request.recognitionLevel = VNRequestTextRecognitionLevelAccurate;
        if(self->languageCode != NULL) {
            request.recognitionLanguages = @[self->languageCode];
        }
        
        
//        NSArray *arr = [VNRecognizeTextRequest supportedRecognitionLanguagesForTextRecognitionLevel:VNRequestTextRecognitionLevelAccurate
//                                                                            revision:3
//                                                                               error:NULL];
//        request.recognitionLanguages = arr;
        
        dispatch_async(dispatch_get_global_queue( DISPATCH_QUEUE_PRIORITY_LOW, 0), ^{
            CGImageRef cgImage = [image CGImage];
            VNImageRequestHandler *requestHandler = [VNImageRequestHandler alloc];
            requestHandler = [requestHandler initWithCGImage:cgImage options:@{}];
            NSError *err;// = [[NSError alloc];
            NSArray *requests = [NSArray arrayWithObject:request];
            [requestHandler performRequests:requests
                                      error: &err];
        });
    }
}

- (void) takeScreenshotAndReadCharaters {
    NSString *tempFileName = @"Screenshot.png";
    NSArray *pathForDirectoriesInDomains = NSSearchPathForDirectoriesInDomains(NSDocumentDirectory, NSUserDomainMask, YES);
    NSString *documentsDirectory = [pathForDirectoriesInDomains objectAtIndex:0];
    NSString *fileAbsolutePath = [documentsDirectory stringByAppendingPathComponent:tempFileName];
    NSData *fileData = [NSData dataWithContentsOfFile:fileAbsolutePath];
    UIImage *image = [UIImage imageWithData:fileData];
    [self readDataFromImage:image];
}

- (void) takeImageFromCameraAndReadCharaters {
    objectUnityAppController = GetAppController();
    UIImagePickerController *picker = [[UIImagePickerController alloc] init];
    picker.delegate = self;
    picker.allowsEditing = YES;
    picker.sourceType = UIImagePickerControllerSourceTypeCamera;
    picker.showsCameraControls = YES;
    picker.cameraFlashMode = UIImagePickerControllerCameraFlashModeAuto;
    [objectUnityAppController.rootViewController presentViewController:picker animated:YES completion:NULL];
}

- (void) takeImageFromLibraryAndReadCharaters {
    objectUnityAppController = GetAppController();
    UIImagePickerController *picker = [[UIImagePickerController alloc] init];
    picker.delegate = self;
    picker.allowsEditing = YES;
    picker.sourceType = UIImagePickerControllerSourceTypePhotoLibrary;
    [objectUnityAppController.rootViewController presentViewController:picker animated:YES completion:NULL];
}

- (void) takeVideoFromLibraryAndReadCharaters {
    objectUnityAppController = GetAppController();
    if ([UIImagePickerController isSourceTypeAvailable:UIImagePickerControllerSourceTypePhotoLibrary]) {
        UIImagePickerController *picker = [[UIImagePickerController alloc] init];
        picker.delegate = self;
        picker.allowsEditing = YES;
        picker.sourceType = UIImagePickerControllerSourceTypePhotoLibrary;
        picker.mediaTypes = [[NSArray alloc] initWithObjects: (NSString *) kUTTypeMovie, nil];            
        [objectUnityAppController.rootViewController presentViewController:picker animated:YES completion:NULL];
    }
}

- (void) readCharactersFromCameraFeed {
    objectUnityAppController = GetAppController();
//    AVCaptureVideoPreviewLayer *cameraLayer= [AVCaptureVideoPreviewLayer new];
//    [cameraLayer setSession:session];
//    cameraLayer.frame = objectUnityAppController.rootView.frame;
//    cameraLayer.videoGravity = AVLayerVideoGravityResizeAspectFill;
//    [objectUnityAppController.rootView.layer addSublayer:cameraLayer];
    [session startRunning];
}

- (void) setUpCaptureSession {
    session = [[AVCaptureSession alloc] init];
    session.sessionPreset = AVCaptureSessionPresetMedium;
    AVCaptureDevice *device = [AVCaptureDevice defaultDeviceWithMediaType:AVMediaTypeVideo];
    NSError *error = nil;
    AVCaptureDeviceInput *input = [AVCaptureDeviceInput deviceInputWithDevice:device error:&error];
    if (!input) {
        [self sendMessageToUnity:@"Issue In Setting Up Camera"];
        return;
    }
    [session addInput:input];
    AVCaptureVideoDataOutput *output = [[AVCaptureVideoDataOutput alloc] init];
    [session addOutput:output];
    output.videoSettings = @{ (NSString *)kCVPixelBufferPixelFormatTypeKey : @(kCVPixelFormatType_32BGRA) };
    [device lockForConfiguration:&error];
    [device setActiveVideoMaxFrameDuration:CMTimeMake(1, 15)];
    [device unlockForConfiguration];
    dispatch_queue_t queue = dispatch_queue_create("MyQueue", NULL);
    [output setSampleBufferDelegate:self queue:queue];
}

- (void) getImageTakenByCamera {
    if(chosenImage != nil)
    {
        NSData *jpgData = UIImageJPEGRepresentation(chosenImage,1.0 );
        NSArray *pathForDirectoriesInDomains = NSSearchPathForDirectoriesInDomains(NSDocumentDirectory, NSUserDomainMask, YES);
        NSString *documentsDirectory = [pathForDirectoriesInDomains objectAtIndex:0];
        NSString *getScreenshotPath = [documentsDirectory stringByAppendingPathComponent:@"Test.jpg"];
        [jpgData writeToFile:getScreenshotPath atomically:YES];
        NSString *imageStatus =[self getStatusInString:ImageLoadedToUnity];
        [self sendMessageToUnity:imageStatus];
    } else {
        NSString *imageStatus =@"Image Not available";
        [self sendMessageToUnity:imageStatus];
    }
}

- (void) setLanguage:(NSString *)languageCode {
    self->languageCode = languageCode;
}

- (void) sendImageToNativeForOCR:(NSString *)imagePath {
    UIImage*tempImage = [UIImage imageWithContentsOfFile:imagePath];
    [self readDataFromImage:tempImage];
}

- (void)captureOutput:(AVCaptureOutput *)captureOutput
         didOutputSampleBuffer:(CMSampleBufferRef)sampleBuffer
         fromConnection:(AVCaptureConnection *)connection {
//    NSLog(@"====captureOutput====");
//    __weak OcrManager *weakSelf = self;
//    dispatch_async(queue_imageframe_processing, ^{
        CVImageBufferRef imageBuffer = CMSampleBufferGetImageBuffer(sampleBuffer);
        CIImage *ciImage = [CIImage imageWithCVPixelBuffer:imageBuffer];
        UIImage *img = [self convertCIImageToUiImage:ciImage];
        [self readDataFromImage:img];
//    });

}

- (UIImage *) convertCIImageToUiImage:(CIImage*) img {
    CIContext *context = [CIContext new];
    UIImage *returnImage = [UIImage imageWithCGImage:[context
                                                      createCGImage:img
                                                      fromRect:img.extent]];
    return returnImage;
}

//-------------------------------------------------------------------------------------------------
- (void)imagePickerController:(UIImagePickerController *)picker didFinishPickingMediaWithInfo:(NSDictionary *)info {
    
    chosenImage = info[UIImagePickerControllerEditedImage];
    [picker dismissViewControllerAnimated:YES completion:NULL];
    [self readDataFromImage:chosenImage];
}
//-------------------------------------------------------------------------------------------------
- (void)imagePickerControllerDidCancel:(UIImagePickerController *)picker {
    
    [picker dismissViewControllerAnimated:YES completion:NULL];
    NSString *imageStatus =[self getStatusInString:ImageCapturedCanceledByUser];
    [self sendMessageToUnity:imageStatus];
}
//-------------------------------------------------------------------------------------------------

- (void)image:(UIImage *)image didFinishSavingWithError:(NSError *)error contextInfo:(void *)contextInfo {
    if (error) {
        NSString *imageStatus =[self getStatusInString:ImageCapturedErrorByUser];
        [self sendMessageToUnity:imageStatus];
        NSLog(@"--%@--" , error);
    }
}

//-------------------------------------------------------------------------------------------------

//-------------------------------------------------------------------------------------------------

-(void) sendMessageToUnity : (NSString *) msg {
    if (msg == NULL) {
        return;
    }
    const char *msgImageSaved = [msg cStringUsingEncoding:NSUTF8StringEncoding];
    const char *gameObjectName = [msgReceivingGameObjectNameGlobal cStringUsingEncoding:NSASCIIStringEncoding];
    const char *methodName = [msgReceivingMethodtNameGlobal cStringUsingEncoding:NSASCIIStringEncoding];
    if(msgImageSaved == nil) {
        return;
    }
    UnitySendMessage(gameObjectName,methodName, msgImageSaved);
}

- (NSString *)encodeNSDataToString:(NSData *)theData {
    return [theData base64EncodedStringWithOptions:NSDataBase64Encoding64CharacterLineLength];
}

- (NSData *)decodeNsStringToNsData:(NSString *)theData {
    NSLog(@"===image decodeNSDataToString==%@==", theData);
    NSData *nsdataFromBase64String = [[NSData alloc] initWithBase64EncodedString:theData options:NSDataBase64DecodingIgnoreUnknownCharacters];
    return nsdataFromBase64String;
}


-(void) setCallBackMethod: (NSString *)gameObject
                         : (NSString *)methodName {
    msgReceivingGameObjectNameGlobal = gameObject;
    msgReceivingMethodtNameGlobal = methodName;
}
@end

// Helper method used to convert NSStrings into C-style strings.
NSString *CreateStr(const char* string) {
    if (string) {
        return [NSString stringWithUTF8String:string];
    } else {
        return [NSString stringWithUTF8String:""];
    }
}


// Unity can only talk directly to C code so use these method calls as wrappers
// into the actual plugin logic.
extern "C" {
    void _takeScreenshotAndReadCharaters(){
        OcrManager *obj = [OcrManager pluginSharedInstance];
        [obj takeScreenshotAndReadCharaters];
    }

    void _takeImageFromCameraAndReadCharaters(){
        OcrManager *obj = [OcrManager pluginSharedInstance];
        [obj takeImageFromCameraAndReadCharaters];
    }

    void _takeImageFromLibraryAndReadCharaters(){
        OcrManager *obj = [OcrManager pluginSharedInstance];
        [obj takeImageFromLibraryAndReadCharaters];
    }

    void _takeVideoFromLibraryAndReadCharaters(){
        OcrManager *obj = [OcrManager pluginSharedInstance];
        [obj takeVideoFromLibraryAndReadCharaters];
    }

    void _getImageTakenByCamera(){
        OcrManager *obj = [OcrManager pluginSharedInstance];
        [obj getImageTakenByCamera];
    }

    void _setLanguage(const char *languageCode) {
        OcrManager *obj = [OcrManager pluginSharedInstance];
        [obj setLanguage:CreateStr(languageCode)];
    }

    void _sendImageToNativeForOCR(const char *imagePath){
        OcrManager *obj = [OcrManager pluginSharedInstance];
        [obj sendImageToNativeForOCR:CreateStr(imagePath)];
    }

    void _readCharactersFromCameraFeed(){
        OcrManager *obj = [OcrManager pluginSharedInstance];
        [obj setUpCaptureSession];
        [obj readCharactersFromCameraFeed];
    }

    void _setCallBackMethod(const char *msgReceivingGameObjectName, const char *msgReceivingMethodName) {
        OcrManager *obj = [OcrManager pluginSharedInstance];
        [obj setCallBackMethod:CreateStr(msgReceivingGameObjectName)
                              :CreateStr(msgReceivingMethodName)];
    }
}
