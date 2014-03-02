using Caliburn.Micro;
using OrgPortal.Common;
using OrgPortal.Views;
using System;
using System.Collections.Generic;
using System.Composition;
using System.Composition.Hosting;
using System.Reflection;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml.Controls;

namespace OrgPortal
{
    sealed partial class App : CaliburnApplication
    {
        private CompositionHost Container { get; set; }

        public App()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override async void OnLaunched(LaunchActivatedEventArgs e)
        {
//#if DEBUG
//            if (System.Diagnostics.Debugger.IsAttached)
//            {
//                this.DebugSettings.EnableFrameRateCounter = true;
//            }
//#endif
            
            DisplayRootView<MainPage>();


            //    //Associate the frame with a SuspensionManager key                                
            //    SuspensionManager.RegisterFrame(rootFrame, "AppFrame");
            //    // Set the default language
            //    rootFrame.Language = Windows.Globalization.ApplicationLanguages.Languages[0];

            
            //    if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
            //    {
            //        // Restore the saved session state only when appropriate
            //        try
            //        {
            //            await SuspensionManager.RestoreAsync();
            //        }
            //        catch (SuspensionManagerException)
            //        {
            //            //Something went wrong restoring state.
            //            //Assume there is no state and continue
            //        }
            //    }
        }

        protected override void Configure()
        {
            var configuration = new ContainerConfiguration()
                .WithAssembly(typeof(App).GetTypeInfo().Assembly);

            Container = configuration.CreateContainer();
        }

        protected override void PrepareViewFirst(Frame rootFrame)
        {
            var nav = Container.GetExport<INavigation>();
            nav.Initialize(new FrameAdapter(rootFrame));
        }

        protected override IEnumerable<Assembly> SelectAssemblies()
        {
            return new[] { typeof(App).GetTypeInfo().Assembly };
        }

        protected override object GetInstance(Type service, string key)
        {
            object instance = null;
            
            if (!string.IsNullOrEmpty(key))
                instance = Container.GetExport(service, key);
            else
                instance = Container.GetExport(service);

            if (instance != null)
                return instance;

            throw new Exception("Could not locate any instances.");
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return Container.GetExports(service);
        }

        protected override void BuildUp(object instance)
        {
            Container.SatisfyImports(instance);
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        protected override async void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            await SuspensionManager.SaveAsync();
            deferral.Complete();
        }

    }
}