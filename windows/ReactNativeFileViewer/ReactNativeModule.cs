using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.ReactNative;
using Microsoft.ReactNative.Managed;
using Windows.ApplicationModel.Core;
using Windows.Storage;
using Windows.System;
using Windows.UI.Core;

namespace ReactNativeFileViewer
{
    [ReactModule("RNFileViewer")]
    internal sealed class ReactNativeModule
    {
        [ReactMethod("open")]
        public async void Open(string filepath, IReactPromise<bool> promise)
        {
            try
            {
                var file = await StorageFile.GetFileFromPathAsync(filepath);

                if (file != null)
                {
                    await CoreApplication.MainView.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                    {
                        var success = await Launcher.LaunchFileAsync(file);

                        if (success)
                        {
                            promise.Resolve(true);
                        }
                        else
                        {
                            promise.Reject(new ReactError { Message = $"Error opening the file {filepath}" });
                        }
                    });
                }
                else
                {
                    promise.Reject(new ReactError { Message = $"Error loading the file {filepath}" });
                }
            }
            catch (Exception e)
            {
                promise.Reject(new ReactError { Exception = e });
            }
        }
    }
}
