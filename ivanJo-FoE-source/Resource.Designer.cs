﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.239
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ForgeBot {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resource {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resource() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("ForgeBot.Resource", typeof(Resource).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        internal static System.Drawing.Bitmap DonateButton {
            get {
                object obj = ResourceManager.GetObject("DonateButton", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Trying to load last used world....
        /// </summary>
        internal static string MainForm_AutoLogin_LoadingLastUsedWorld {
            get {
                return ResourceManager.GetString("MainForm_AutoLogin_LoadingLastUsedWorld", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Waiting for home page to load took more than {0} seconds. Aborting auto login..
        /// </summary>
        internal static string MainForm_AutoLogin_PageLoadTooSlow {
            get {
                return ResourceManager.GetString("MainForm_AutoLogin_PageLoadTooSlow", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Document not ready. Aborting auto login..
        /// </summary>
        internal static string MainForm_AutoLogin_PageNotReady {
            get {
                return ResourceManager.GetString("MainForm_AutoLogin_PageNotReady", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Sleeping a bit to allow page render to finish....
        /// </summary>
        internal static string MainForm_AutoLogin_WaitPageToCompleteLoading {
            get {
                return ResourceManager.GetString("MainForm_AutoLogin_WaitPageToCompleteLoading", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Application auto refresh triggered....
        /// </summary>
        internal static string MainForm_AutoRefreshTick_Reload {
            get {
                return ResourceManager.GetString("MainForm_AutoRefreshTick_Reload", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Awesomium CRASHED! Status: {0}.
        /// </summary>
        internal static string MainForm_AwesomiumCrashed {
            get {
                return ResourceManager.GetString("MainForm_AwesomiumCrashed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Wait {0} seconds....
        /// </summary>
        internal static string MainForm_BotDoActionsThreaded_BotIdleBeforeProduction {
            get {
                return ResourceManager.GetString("MainForm_BotDoActionsThreaded_BotIdleBeforeProduction", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Trying to resurrect from dead....
        /// </summary>
        internal static string MainForm_BotDoActionsThreaded_RestartApplication {
            get {
                return ResourceManager.GetString("MainForm_BotDoActionsThreaded_RestartApplication", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Pause Timer.
        /// </summary>
        internal static string MainForm_botStart_Pause_Timer {
            get {
                return ResourceManager.GetString("MainForm_botStart_Pause_Timer", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Auto reload: OFF.
        /// </summary>
        internal static string MainForm_buttonAutoReload_Auto_reload_OFF {
            get {
                return ResourceManager.GetString("MainForm_buttonAutoReload_Auto_reload_OFF", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Auto reload: ON.
        /// </summary>
        internal static string MainForm_buttonAutoReload_Auto_reload_ON {
            get {
                return ResourceManager.GetString("MainForm_buttonAutoReload_Auto_reload_ON", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to View mode can be changed only when bot is idle..
        /// </summary>
        internal static string MainForm_buttonChangeView_Click_You_can_only_change_the_view_mode_when_the_bot_is_idle_ {
            get {
                return ResourceManager.GetString("MainForm_buttonChangeView_Click_You_can_only_change_the_view_mode_when_the_bot_is" +
                        "_idle_", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &apos;userPicture.png&apos; saved..
        /// </summary>
        internal static string MainForm_buttonSavePicture_Click_Picture_saved {
            get {
                return ResourceManager.GetString("MainForm_buttonSavePicture_Click_Picture_saved", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Restart Bot.
        /// </summary>
        internal static string MainForm_buttonStartBot_Restart_Bot {
            get {
                return ResourceManager.GetString("MainForm_buttonStartBot_Restart_Bot", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Resume Timer.
        /// </summary>
        internal static string MainForm_buttonStartBot_Resume_Timer {
            get {
                return ResourceManager.GetString("MainForm_buttonStartBot_Resume_Timer", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Running....
        /// </summary>
        internal static string MainForm_buttonStartBot_Running {
            get {
                return ResourceManager.GetString("MainForm_buttonStartBot_Running", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Selected military item #{0}.
        /// </summary>
        internal static string MainForm_ClickMilitaryItems_MilitaryItemSelected {
            get {
                return ResourceManager.GetString("MainForm_ClickMilitaryItems_MilitaryItemSelected", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Error: Blue Screen Of Death.
        /// </summary>
        internal static string MainForm_closePopup_Error_Blue_Screen_Of_Death {
            get {
                return ResourceManager.GetString("MainForm_closePopup_Error_Blue_Screen_Of_Death", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Error: Can&apos;t recognize game window.
        /// </summary>
        internal static string MainForm_closePopup_Error_Game_window_not_recognized {
            get {
                return ResourceManager.GetString("MainForm_closePopup_Error_Game_window_not_recognized", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Error: Popup still there after close..
        /// </summary>
        internal static string MainForm_closePopup_Error_Popup_still_there_after_close_click {
            get {
                return ResourceManager.GetString("MainForm_closePopup_Error_Popup_still_there_after_close_click", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Stealth: Clicking first {0} items if any found..
        /// </summary>
        internal static string MainForm_DoCollectClicks_Stealth_ClickingFirstXItems {
            get {
                return ResourceManager.GetString("MainForm_DoCollectClicks_Stealth_ClickingFirstXItems", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Error: Popup true after click. Maybe not enough supplies. Selecting goods!.
        /// </summary>
        internal static string MainForm_DoSupplyClicks_Error_PopupRemained_SelectingGoods {
            get {
                return ResourceManager.GetString("MainForm_DoSupplyClicks_Error_PopupRemained_SelectingGoods", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Error: Popup true after click. Maybe not enough supplies. Selecting military!.
        /// </summary>
        internal static string MainForm_DoSupplyClicks_Error_PopupRemained_SelectingMilitary {
            get {
                return ResourceManager.GetString("MainForm_DoSupplyClicks_Error_PopupRemained_SelectingMilitary", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Error: Popup true after click. Selecting supplies!.
        /// </summary>
        internal static string MainForm_DoSupplyClicks_Error_PopupRemained_SelectingSupplies {
            get {
                return ResourceManager.GetString("MainForm_DoSupplyClicks_Error_PopupRemained_SelectingSupplies", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Goods autostart disabled, closing popup....
        /// </summary>
        internal static string MainForm_DoSupplyClicks_GoodsClick_Disabled {
            get {
                return ResourceManager.GetString("MainForm_DoSupplyClicks_GoodsClick_Disabled", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Military autostart disabled, closing popup....
        /// </summary>
        internal static string MainForm_DoSupplyClicks_MilitaryClick_Disabled {
            get {
                return ResourceManager.GetString("MainForm_DoSupplyClicks_MilitaryClick_Disabled", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to #{0} not a production building..
        /// </summary>
        internal static string MainForm_DoSupplyClicks_NonProductionBuilding {
            get {
                return ResourceManager.GetString("MainForm_DoSupplyClicks_NonProductionBuilding", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Page finished loading..
        /// </summary>
        internal static string MainForm_OnAddressChanged_PageLoaded {
            get {
                return ResourceManager.GetString("MainForm_OnAddressChanged_PageLoaded", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to BOT thread stopped....
        /// </summary>
        internal static string MainForm_OnFormClosing_BOT_thread_stopped {
            get {
                return ResourceManager.GetString("MainForm_OnFormClosing_BOT_thread_stopped", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Stopping running BOT thread....
        /// </summary>
        internal static string MainForm_OnFormClosing_Stopping_running_BOT_thread {
            get {
                return ResourceManager.GetString("MainForm_OnFormClosing_Stopping_running_BOT_thread", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to BOT loaded.
        /// </summary>
        internal static string MainForm_OnLoad_Bot_loaded_succesfully {
            get {
                return ResourceManager.GetString("MainForm_OnLoad_Bot_loaded_succesfully", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Entered bot friendly view mode.
        /// </summary>
        internal static string MainForm_resizeBrowser_Entered_bot_friendly_view_mode {
            get {
                return ResourceManager.GetString("MainForm_resizeBrowser_Entered_bot_friendly_view_mode", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Entered user friendly view mode.
        /// </summary>
        internal static string MainForm_resizeBrowser_Entered_user_friendly_view_mode {
            get {
                return ResourceManager.GetString("MainForm_resizeBrowser_Entered_user_friendly_view_mode", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to View: Bot friendly.
        /// </summary>
        internal static string MainForm_resizeBrowser_ViewMode_Bot_friendly {
            get {
                return ResourceManager.GetString("MainForm_resizeBrowser_ViewMode_Bot_friendly", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to View: User friendly.
        /// </summary>
        internal static string MainForm_resizeBrowser_ViewMode_User_friendly {
            get {
                return ResourceManager.GetString("MainForm_resizeBrowser_ViewMode_User_friendly", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Saving scan results to file: {0}.
        /// </summary>
        internal static string MainForm_saveScanResultToFile_Saving_scan_results_to_file {
            get {
                return ResourceManager.GetString("MainForm_saveScanResultToFile_Saving_scan_results_to_file", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Scan results saved to &quot;{0}&quot;..
        /// </summary>
        internal static string MainForm_SaveScanResultToFile_ScanResultsSaved {
            get {
                return ResourceManager.GetString("MainForm_SaveScanResultToFile_ScanResultsSaved", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Bot: Clicking to collect....
        /// </summary>
        internal static string MainForm_updateStatus_Bot_Clicking {
            get {
                return ResourceManager.GetString("MainForm_updateStatus_Bot_Clicking", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Bot: Clicking production....
        /// </summary>
        internal static string MainForm_updateStatus_Bot_Clicking_production {
            get {
                return ResourceManager.GetString("MainForm_updateStatus_Bot_Clicking_production", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Bot: Done clicking!.
        /// </summary>
        internal static string MainForm_updateStatus_Bot_Done_clicking {
            get {
                return ResourceManager.GetString("MainForm_updateStatus_Bot_Done_clicking", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Bot: Idle.
        /// </summary>
        internal static string MainForm_updateStatus_Bot_Idle {
            get {
                return ResourceManager.GetString("MainForm_updateStatus_Bot_Idle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Stealth: Bot idle for approx. {0} min..
        /// </summary>
        internal static string MainForm_updateStatus_Bot_Idle_for_X_min {
            get {
                return ResourceManager.GetString("MainForm_updateStatus_Bot_Idle_for_X_min", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Bot: Paused for {0} seconds..
        /// </summary>
        internal static string MainForm_updateStatus_Bot_Paused {
            get {
                return ResourceManager.GetString("MainForm_updateStatus_Bot_Paused", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Bot is in pause mode for {0} seconds due to user activity..
        /// </summary>
        internal static string MainForm_updateStatus_Bot_Paused_for_X_min {
            get {
                return ResourceManager.GetString("MainForm_updateStatus_Bot_Paused_for_X_min", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Bot: Production scanning done!.
        /// </summary>
        internal static string MainForm_updateStatus_Bot_Production_scanning_done {
            get {
                return ResourceManager.GetString("MainForm_updateStatus_Bot_Production_scanning_done", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Bot: Scanning....
        /// </summary>
        internal static string MainForm_updateStatus_Bot_Scanning {
            get {
                return ResourceManager.GetString("MainForm_updateStatus_Bot_Scanning", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Bot: Scanning done!.
        /// </summary>
        internal static string MainForm_updateStatus_Bot_Scanning_done {
            get {
                return ResourceManager.GetString("MainForm_updateStatus_Bot_Scanning_done", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Bot: Scanning production....
        /// </summary>
        internal static string MainForm_updateStatus_Bot_Scanning_production {
            get {
                return ResourceManager.GetString("MainForm_updateStatus_Bot_Scanning_production", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Bot: Zooming screen....
        /// </summary>
        internal static string MainForm_updateStatus_Bot_Zooming {
            get {
                return ResourceManager.GetString("MainForm_updateStatus_Bot_Zooming", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Check whether screen is zoomed....
        /// </summary>
        internal static string MainForm_updateStatus_Check_for_zoom {
            get {
                return ResourceManager.GetString("MainForm_updateStatus_Check_for_zoom", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Clicked item #{0}.
        /// </summary>
        internal static string MainForm_updateStatus_Clicked_item_X {
            get {
                return ResourceManager.GetString("MainForm_updateStatus_Clicked_item_X", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Item #{0} found.
        /// </summary>
        internal static string MainForm_updateStatus_Item_X_found {
            get {
                return ResourceManager.GetString("MainForm_updateStatus_Item_X_found", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Plunged item #{0} found.
        /// </summary>
        internal static string MainForm_updateStatus_Plunged_item_X_found {
            get {
                return ResourceManager.GetString("MainForm_updateStatus_Plunged_item_X_found", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Production item #{0} found.
        /// </summary>
        internal static string MainForm_updateStatus_Production_item_X_found {
            get {
                return ResourceManager.GetString("MainForm_updateStatus_Production_item_X_found", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Production scanning done in {0} secs..
        /// </summary>
        internal static string MainForm_updateStatus_Production_scanning_done {
            get {
                return ResourceManager.GetString("MainForm_updateStatus_Production_scanning_done", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Rotted supply item #{0} found.
        /// </summary>
        internal static string MainForm_updateStatus_RottedSupply_item_X_found {
            get {
                return ResourceManager.GetString("MainForm_updateStatus_RottedSupply_item_X_found", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Scanning done in {0} secs..
        /// </summary>
        internal static string MainForm_updateStatus_Scanning_done {
            get {
                return ResourceManager.GetString("MainForm_updateStatus_Scanning_done", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Selected production item #{0}.
        /// </summary>
        internal static string MainForm_updateStatus_Selected_production_item_X {
            get {
                return ResourceManager.GetString("MainForm_updateStatus_Selected_production_item_X", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Selected production type..
        /// </summary>
        internal static string MainForm_updateStatus_Selected_production_type {
            get {
                return ResourceManager.GetString("MainForm_updateStatus_Selected_production_type", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Started supplies clicking....
        /// </summary>
        internal static string MainForm_updateStatus_Started_clicking {
            get {
                return ResourceManager.GetString("MainForm_updateStatus_Started_clicking", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Started production items clicking....
        /// </summary>
        internal static string MainForm_updateStatus_Started_clicking_production_items {
            get {
                return ResourceManager.GetString("MainForm_updateStatus_Started_clicking_production_items", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Collect scanning started....
        /// </summary>
        internal static string MainForm_updateStatus_Started_scanning {
            get {
                return ResourceManager.GetString("MainForm_updateStatus_Started_scanning", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Production scanning started....
        /// </summary>
        internal static string MainForm_updateStatus_Started_scanning_for_production {
            get {
                return ResourceManager.GetString("MainForm_updateStatus_Started_scanning_for_production", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Trying to zoom in....
        /// </summary>
        internal static string MainForm_updateStatus_Trying_to_zoom_in {
            get {
                return ResourceManager.GetString("MainForm_updateStatus_Trying_to_zoom_in", resourceCulture);
            }
        }
        
        internal static System.Drawing.Bitmap panelBckgr {
            get {
                object obj = ResourceManager.GetObject("panelBckgr", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
    }
}