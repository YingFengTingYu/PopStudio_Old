using Foundation;
using UIKit;
using AppKit;
using MobileCoreServices;

namespace PopStudio.MAUI
{
    internal static partial class PickFile
    {
        public static partial async Task<string> ChooseSaveFile(this ContentPage page) => (await FilePicker.PickAsync()).FullPath;

		public static partial async Task<string> ChooseOpenFile(this ContentPage page) => (await FilePicker.PickAsync()).FullPath;

		public static partial async Task<string> ChooseFolder(this ContentPage page) => await PickerHelper.PickFolder();

		static class PickerHelper
        {
			class PickerDelegate : UIDocumentPickerDelegate
			{
				public Action<NSUrl[]> PickHandler { get; set; }

				public override void WasCancelled(UIDocumentPickerViewController controller)
					=> PickHandler?.Invoke(null);

				public override void DidPickDocument(UIDocumentPickerViewController controller, NSUrl[] urls)
					=> PickHandler?.Invoke(urls);

				public override void DidPickDocument(UIDocumentPickerViewController controller, NSUrl url)
					=> PickHandler?.Invoke(new NSUrl[] { url });
			}

			static void GetFileResults(NSUrl[] urls, TaskCompletionSource<string> tcs)
			{
				try
				{
					tcs.TrySetResult(urls?[0]?.ToString() ?? "");
				}
				catch (Exception ex)
				{
					tcs.TrySetException(ex);
				}
			}

			static string filesHead = "file://";
            static string filesEnd = "/";

            public static async Task<string> PickFolder()
			{
				var allowedTypes = new string[]
				{
			        "public.folder"
				};

				var picker = new UIDocumentPickerViewController(allowedTypes, UIDocumentPickerMode.Open);
				var tcs = new TaskCompletionSource<string>();

				picker.Delegate = new PickerDelegate
				{
					PickHandler = urls => GetFileResults(urls, tcs)
				};

				if (picker.PresentationController != null)
				{
					picker.PresentationController.Delegate =
						new UIPresentationControllerDelegate(() => GetFileResults(null, tcs));
				}

				var parentController = Platform.GetCurrentUIViewController();

				parentController.PresentViewController(picker, true, null);

				string ans = await tcs.Task;
				if (ans.StartsWith(filesHead))
                {
					ans = ans[filesHead.Length..];
                }
				if (ans.EndsWith(filesEnd))
                {
					ans = ans[..^1];
                }

                return ans;
			}

			internal class UIPresentationControllerDelegate : UIAdaptivePresentationControllerDelegate
			{
				Action dismissHandler;

				internal UIPresentationControllerDelegate(Action dismissHandler)
					=> this.dismissHandler = dismissHandler;

				public override void DidDismiss(UIPresentationController presentationController)
				{
					dismissHandler?.Invoke();
					dismissHandler = null;
				}

				protected override void Dispose(bool disposing)
				{
					dismissHandler?.Invoke();
					base.Dispose(disposing);
				}
			}
		}
    }
}