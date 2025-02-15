﻿global using System;
global using Community.VisualStudio.Toolkit;
global using Microsoft.VisualStudio.Shell;
global using Task = System.Threading.Tasks.Task;
using System.ComponentModel.Design;
using System.Runtime.InteropServices;
using System.Threading;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.Win32;

namespace MarkdownEditor2022
{
    [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
    [InstalledProductRegistration(Vsix.Name, Vsix.Description, Vsix.Version)]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [Guid(PackageGuids.MarkdownEditor2022String)]

    [ProvideLanguageService(typeof(MarkdownEditorV2), Constants.LanguageName, 0, ShowHotURLs = false, DefaultToNonHotURLs = true, EnableLineNumbers = true, EnableAsyncCompletion = true, ShowCompletion = true, ShowDropDownOptions = true)]
    [ProvideLanguageEditorOptionPage(typeof(OptionsProvider.AdvancedOptions), Constants.LanguageName, "", "Advanced", null, new[] { "mark", "md", "mdown" })]
    [ProvideLanguageExtension(typeof(MarkdownEditorV2), Constants.FileExtension)]

    [ProvideEditorFactory(typeof(MarkdownEditorV2), 0, false, CommonPhysicalViewAttributes = (int)__VSPHYSICALVIEWATTRIBUTES.PVA_SupportsPreview, TrustLevel = __VSEDITORTRUSTLEVEL.ETL_AlwaysTrusted)]
    [ProvideEditorLogicalView(typeof(MarkdownEditorV2), VSConstants.LOGVIEWID.TextView_string, IsTrusted = true)]
    [ProvideEditorExtension(typeof(MarkdownEditorV2), Constants.FileExtension, 1000)]

    [ProvideFileIcon(Constants.FileExtension, "KnownMonikers.RegistrationScript")]
    public sealed class MarkdownEditor2022Package : ToolkitPackage
    {
        protected override async Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
        {
            await JoinableTaskFactory.SwitchToMainThreadAsync();

            MarkdownEditorV2 language = new(this);
            RegisterEditorFactory(language);
            ((IServiceContainer)this).AddService(typeof(MarkdownEditorV2), language, true);

            //SetInternetExplorerRegistryKey();

            await this.RegisterCommandsAsync();
            await Commenting.InitializeAsync();
            await ToggleTaskCommand.InitializeAsync();
        }

        // TODO: Remove this method if WebView2 doesn't benefit from it
        // This is to enable DPI scaling in the preview browser instance
        //private static void SetInternetExplorerRegistryKey()
        //{
        //    try
        //    {
        //        using (RegistryKey featureControl = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Internet Explorer\\Main\\FeatureControl", true))
        //        using (RegistryKey pixel = featureControl.CreateSubKey("FEATURE_96DPI_PIXEL", true, RegistryOptions.Volatile))
        //        {
        //            pixel.SetValue("devenv.exe", 1, RegistryValueKind.DWord);
        //            //pixel.DeleteValue("devenv.exe");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ex.Log();
        //    }
        //}
    }
}