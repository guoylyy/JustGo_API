﻿#pragma checksum "C:\Users\Jian\Documents\GitHub\JustGo_Project\JustGo_WP\Archive\Archive\Pages\UserProfilePage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "38E5CB1C55EDF36DEFACB79AF8EC25AB"
//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.34014
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

using CircleImage;
using Microsoft.Phone.Controls;
using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace Archive.Pages {
    
    
    public partial class UserProfilePage : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.Grid ProfileGrid;
        
        internal System.Windows.Controls.Image BigImage;
        
        internal CircleImage.CircleImage CircleImage;
        
        internal System.Windows.Controls.TextBlock UserNameTextBlock;
        
        internal System.Windows.Controls.Button EncourageButton;
        
        internal System.Windows.Controls.Grid FollowersGrid;
        
        internal System.Windows.Controls.Grid FollowingsGrid;
        
        internal System.Windows.Controls.Grid FightingCenterGrid;
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Windows.Application.LoadComponent(this, new System.Uri("/Archive;component/Pages/UserProfilePage.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.ProfileGrid = ((System.Windows.Controls.Grid)(this.FindName("ProfileGrid")));
            this.BigImage = ((System.Windows.Controls.Image)(this.FindName("BigImage")));
            this.CircleImage = ((CircleImage.CircleImage)(this.FindName("CircleImage")));
            this.UserNameTextBlock = ((System.Windows.Controls.TextBlock)(this.FindName("UserNameTextBlock")));
            this.EncourageButton = ((System.Windows.Controls.Button)(this.FindName("EncourageButton")));
            this.FollowersGrid = ((System.Windows.Controls.Grid)(this.FindName("FollowersGrid")));
            this.FollowingsGrid = ((System.Windows.Controls.Grid)(this.FindName("FollowingsGrid")));
            this.FightingCenterGrid = ((System.Windows.Controls.Grid)(this.FindName("FightingCenterGrid")));
        }
    }
}

