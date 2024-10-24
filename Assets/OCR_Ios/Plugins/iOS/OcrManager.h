
#import <Foundation/Foundation.h>
#import <UIKit/UIKit.h>
#import <CoreText/CoreText.h>
#import "UnityAppController.h"
#import <MediaPlayer/MediaPlayer.h>
#import <AVFoundation/AVFoundation.h>
#import <Vision/Vision.h>
#import <MobileCoreServices/MobileCoreServices.h>

typedef enum CallBackStatus {
    ImageCapturedByCameraOrLibrary,
    ImageCapturedCanceledByUser,
    ImageCapturedErrorByUser,
    ImageSavedToCameraRoll,
    ImageSavedToSpecificAlbum,
    ImageLoadedToUnity,
    ImageProcessingError
} CallBackStatus;
@interface OcrManager : NSObject<UIImagePickerControllerDelegate, UINavigationControllerDelegate>

@end
