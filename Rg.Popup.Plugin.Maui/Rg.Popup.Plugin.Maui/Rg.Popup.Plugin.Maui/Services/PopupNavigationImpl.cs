using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Rg.Plugins.Popup.Contracts;
using Rg.Plugins.Popup.Events;
using Rg.Plugins.Popup.Pages;
using AsyncAwaitBestPractices;
using System.Diagnostics;

namespace Rg.Plugins.Popup.Services
{
    public class PopupNavigationImpl : IPopupNavigation
    {
        private readonly object _locker = new();

        public IReadOnlyList<PopupPage> PopupStack => _popupStack;
        private readonly List<PopupPage> _popupStack = new();

        public event EventHandler<PopupNavigationEventArgs>? Pushing;

        public event EventHandler<PopupNavigationEventArgs>? Pushed;

        public event EventHandler<PopupNavigationEventArgs>? Popping;

        public event EventHandler<PopupNavigationEventArgs>? Popped;


        private static readonly Lazy<IPopupPlatform> lazyImplementation = new(() => GeneratePopupPlatform(), System.Threading.LazyThreadSafetyMode.PublicationOnly);

        private readonly IPopupPlatform PopupPlatform = lazyImplementation.Value;

        private static IPopupPlatform GeneratePopupPlatform()
        {
            try
            {
                return PullPlatformImplementation();
            }
            catch (Exception)
            {
                throw new InvalidOperationException("You MUST install Rg.Plugins.Popup to each project and call " + Config.InitializationDescriptionUrl);
            }

            static IPopupPlatform PullPlatformImplementation()
            {
#if ANDROID
                return new Rg.Plugins.Popup.Droid.Impl.PopupPlatformDroid();
#endif

                throw new PlatformNotSupportedException();
            }
        }

        public PopupNavigationImpl()
        {
            PlatformSpecificInititalisation();
            PopupPlatform.OnInitialized += OnInitialized;

            static void PlatformSpecificInititalisation()
            {
#if ANDROID
                Rg.Plugins.Popup.Popup.Init(Microsoft.Maui.Essentials.Platform.AppContext);
#endif
            }
        }

        private void OnInitialized(object? sender, EventArgs e)
        {
            if (_popupStack.Count > 0)
            {
                PopAllAsync(false).SafeFireAndForget();
            }
        }

        public Task PushAsync(PopupPage page, bool animate = true)
        {
            lock (_locker)
            {
                if (_popupStack.Contains(page))
                {
                    throw new InvalidOperationException("The page has been pushed already. Pop or remove the page before to push it again");
                }

                Pushing?.Invoke(this, new PopupNavigationEventArgs(page, animate));

                _popupStack.Add(page);

                var pushPageTask = Microsoft.Maui.Essentials.MainThread.IsMainThread
                    ? PushPage()
                    : Microsoft.Maui.Essentials.MainThread.InvokeOnMainThreadAsync(PushPage);


                page.AppearingTransactionTask = pushPageTask;

                return pushPageTask;

                async Task PushPage()
                { 
                    animate = CanBeAnimated(animate);

                    if (animate)
                    {
                        page.PreparingAnimation();
                        await PopupPlatform.AddAsync(page);
                        await page.AppearingAnimation();
                    }
                    else
                    {
                        await PopupPlatform.AddAsync(page);
                    }

                    page.AppearingTransactionTask = null;

                    Pushed?.Invoke(this, new PopupNavigationEventArgs(page, animate));
                };
            }
        }

        public async Task PopAllAsync(bool animate = true)
        {
            while (PopupNavigation.Instance.PopupStack.Count > 0)
            {
                await PopAsync(CanBeAnimated(animate));
            }
        }

        public Task PopAsync(bool animate = true)
        {
            lock (_locker)
            {
                animate = CanBeAnimated(animate);
                return _popupStack.Count <= 0
                    ? throw new InvalidOperationException("PopupStack is empty")
                    : RemovePageAsync(PopupStack[PopupStack.Count - 1], animate);
            }
        }

        public Task RemovePageAsync(PopupPage page, bool animate = true)
        {
            lock (_locker)
            {
                if (page == null)
                    throw new InvalidOperationException("Page can not be null");

                if (!_popupStack.Contains(page))
                    throw new InvalidOperationException("The page has not been pushed yet or has been removed already");

                if (page.DisappearingTransactionTask != null)
                    return page.DisappearingTransactionTask;

                var RemovePageTask = Microsoft.Maui.Essentials.MainThread.IsMainThread
                    ? RemovePage()
                    : Microsoft.Maui.Essentials.MainThread.InvokeOnMainThreadAsync(RemovePage);

                //RemovePageTask.ContinueWith(_ => Debug.WriteLine("BINGBINGBING"));
                page.DisappearingTransactionTask = RemovePageTask;
                return RemovePageTask;


                async Task RemovePage()
                {
                    if (page.AppearingTransactionTask != null)
                    {
                        await page.AppearingTransactionTask;
                    }

                    lock (_locker)
                    {
                        if (!_popupStack.Contains(page))
                        {
                            return;
                        }
                    }

                    Popping?.Invoke(this, new PopupNavigationEventArgs(page, animate));

                    animate = CanBeAnimated(animate);

                    if (animate)
                    {
                        await page.DisappearingAnimation();
                    }

                    await PopupPlatform.RemoveAsync(page);

                    if (animate)
                    {
                        page.DisposingAnimation();
                    }

                    lock (_locker)
                    {
                        _popupStack.Remove(page);
                        page.DisappearingTransactionTask = null;

                        Popped?.Invoke(this, new PopupNavigationEventArgs(page, animate));
                    }
                }
            }
        }

        // Private

        // Internal 
        
        internal void RemovePopupFromStack(PopupPage page) // unused?
        {
            if (_popupStack.Contains(page))
                _popupStack.Remove(page);
        }

        #region Animation

        bool CanBeAnimated(bool animate)
        {
            return animate && PopupPlatform.IsSystemAnimationEnabled;
        }

        #endregion

        #region Helpers
        /*
        Task InvokeThreadSafe(Func<Task> action)
        {
            var tcs = new TaskCompletionSource<bool>();

            Device.BeginInvokeOnMainThread(async () =>
            {
                try
                {
                    await action.Invoke();
                    tcs.SetResult(true);
                }
                catch (Exception e)
                {
                    tcs.SetException(e);
                }
            });

            return tcs.Task;
        }
        */
        #endregion
    }
}
