﻿#pragma checksum "C:\Users\Daniel\Desktop\Windows App Development\Film-Spot\Film Spot\Film Spot\MainPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "7A372AF16E2FEBDD62993A9DCD1BF96E"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18033
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
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


namespace Film_Spot {
    
    
    public partial class MainPage : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal Microsoft.Phone.Shell.ApplicationBarIconButton pin;
        
        internal Microsoft.Phone.Shell.ApplicationBarIconButton faq;
        
        internal Microsoft.Phone.Shell.ApplicationBarIconButton rate;
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.Grid ContentPanel;
        
        internal System.Windows.Controls.Button search;
        
        internal System.Windows.Controls.Button browse;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/Film%20Spot;component/MainPage.xaml", System.UriKind.Relative));
            this.pin = ((Microsoft.Phone.Shell.ApplicationBarIconButton)(this.FindName("pin")));
            this.faq = ((Microsoft.Phone.Shell.ApplicationBarIconButton)(this.FindName("faq")));
            this.rate = ((Microsoft.Phone.Shell.ApplicationBarIconButton)(this.FindName("rate")));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.ContentPanel = ((System.Windows.Controls.Grid)(this.FindName("ContentPanel")));
            this.search = ((System.Windows.Controls.Button)(this.FindName("search")));
            this.browse = ((System.Windows.Controls.Button)(this.FindName("browse")));
        }
    }
}
