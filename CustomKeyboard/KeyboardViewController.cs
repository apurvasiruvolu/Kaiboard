using System;

using ObjCRuntime;
using Foundation;
using UIKit;
using Plugin.Media;
using Plugin.Media.Abstractions;
using CoreGraphics;

namespace CustomKeyboard
{
    public partial class KeyboardViewController : UIInputViewController
    {
        UIButton cameraButton, galleryButton;
        UIImagePickerController picker;
        UIImageView imageView;

        protected KeyboardViewController(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        public override void DidReceiveMemoryWarning()
        {
            // Releases the view if it doesn't have a superview.
            base.DidReceiveMemoryWarning();

            // Release any cached data, images, etc that aren't in use.
        }

        public override void UpdateViewConstraints()
        {
            base.UpdateViewConstraints();

            // Add custom view sizing constraints here
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            // Perform custom UI setup here
            cameraButton = new UIButton(UIButtonType.System);
            galleryButton = new UIButton(UIButtonType.System);

            cameraButton.SetTitle("Camera", UIControlState.Normal);
            cameraButton.SizeToFit();
            cameraButton.TranslatesAutoresizingMaskIntoConstraints = false;
            cameraButton.AddTarget(cameraButton_click, UIControlEvent.TouchDown);
            View.AddSubview(cameraButton);
            var cameraAlignment = NSLayoutConstraint.Create(cameraButton, NSLayoutAttribute.CenterXWithinMargins, NSLayoutRelation.Equal, View, NSLayoutAttribute.CenterXWithinMargins, 1.0f, 0.0f);
            View.AddConstraints(new[] { cameraAlignment });

            imageView = new UIImageView(new CGRect(10, 150, 300, 300));
            Add(imageView);

            galleryButton.SetTitle("Gallery", UIControlState.Normal);
            galleryButton.SizeToFit();
            galleryButton.TranslatesAutoresizingMaskIntoConstraints = false;
            galleryButton.AddTarget(galleryButton_click, UIControlEvent.TouchDown);
            View.AddSubview(galleryButton);
            View.Add(galleryButton);
            var galleryAlignment = NSLayoutConstraint.Create(galleryButton, NSLayoutAttribute.LeftMargin, NSLayoutRelation.Equal, View, NSLayoutAttribute.LeftMargin, 1.0f, 0.0f);
            var gallerycenterAlignment = NSLayoutConstraint.Create(cameraButton, NSLayoutAttribute.CenterXWithinMargins, NSLayoutRelation.Equal, View, NSLayoutAttribute.CenterXWithinMargins, 1.0f, 0.0f);
            View.AddConstraints(new[] { galleryAlignment, gallerycenterAlignment});

        }

        public void cameraButton_click(object sender, EventArgs e)
        {
            picker = new UIImagePickerController();
            picker.SourceType = UIImagePickerControllerSourceType.Camera;
            picker.MediaTypes = UIImagePickerController.AvailableMediaTypes(UIImagePickerControllerSourceType.Camera);
            picker.FinishedPickingMedia += Finished;
            picker.Canceled += Canceled;
            PresentViewController(picker, animated: true, completionHandler: null);
            //await CrossMedia.Current.Initialize();
            //if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            //{
            //    Console.Write("No Camera");
            //    return;
            //}

            //var photo = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions()
            //{

            //});
            //if (photo != null)
            //    return;
            ////    image.Image = UIImage.FromBundle("picture.png");
        }


        public void galleryButton_click(object sender, EventArgs e)
        {
            picker = new UIImagePickerController();
            picker.SourceType = UIImagePickerControllerSourceType.PhotoLibrary;
            picker.MediaTypes = UIImagePickerController.AvailableMediaTypes(UIImagePickerControllerSourceType.PhotoLibrary);
            picker.FinishedPickingMedia += Finished;
            picker.Canceled += Canceled;
            PresentViewController(picker, animated: true, completionHandler: null);

        }

        public void Finished(object sender, UIImagePickerMediaPickedEventArgs e)
        {
            bool isImage = false;
            switch (e.Info[UIImagePickerController.MediaType].ToString())
            {
                case "public.image":
                    isImage = true;
                    break;
                case "public.video":
                    break;
            }
            NSUrl referenceURL = e.Info[new NSString("UIImagePickerControllerReferenceUrl")] as NSUrl;
            if (referenceURL != null) Console.WriteLine("Url:" + referenceURL.ToString());
            if (isImage)
            {
                UIImage originalImage = e.Info[UIImagePickerController.OriginalImage] as UIImage;
                if (originalImage != null)
                {
                    imageView.Image = originalImage;
                }
            }
            else
            {
                NSUrl mediaURL = e.Info[UIImagePickerController.MediaURL] as NSUrl;
                if (mediaURL != null)
                {
                    Console.WriteLine(mediaURL.ToString());
                }
            }
            picker.DismissModalViewController(true);
        }

        void Canceled(object sender, EventArgs e)
        {
            picker.DismissModalViewController(true);
        }

        public override void TextWillChange(IUITextInput textInput)
        {
            // The app is about to change the document's contents. Perform any preparation here.
        }

        public override void TextDidChange(IUITextInput textInput)
        {
            // The app has just changed the document's contents, the document context has been updated.
            UIColor textColor = null;

            if (TextDocumentProxy.KeyboardAppearance == UIKeyboardAppearance.Dark)
            {
                textColor = UIColor.White;
            }
            else
            {
                textColor = UIColor.Black;
            }

            cameraButton.SetTitleColor(textColor, UIControlState.Normal);
            galleryButton.SetTitleColor(textColor, UIControlState.Normal);
        }
    }
}
