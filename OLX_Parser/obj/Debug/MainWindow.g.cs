﻿#pragma checksum "..\..\MainWindow.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "1B3F2A31D98B7FDFA5DDB10A350D631FAB7FD2228CC250CC31A667C93223C320"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using OLX_Parser;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace OLX_Parser {
    
    
    /// <summary>
    /// MainWindow
    /// </summary>
    public partial class MainWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector, System.Windows.Markup.IStyleConnector {
        
        
        #line 177 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox WebsiteInput;
        
        #line default
        #line hidden
        
        
        #line 179 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox PhoneInput;
        
        #line default
        #line hidden
        
        
        #line 181 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button RegisterButton;
        
        #line default
        #line hidden
        
        
        #line 184 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TabControl Tabs;
        
        #line default
        #line hidden
        
        
        #line 254 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid NewOffers;
        
        #line default
        #line hidden
        
        
        #line 333 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TabItem SettingsTab;
        
        #line default
        #line hidden
        
        
        #line 337 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button RestorePerson;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/Notify OLX;component/mainwindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\MainWindow.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            
            #line 10 "..\..\MainWindow.xaml"
            ((OLX_Parser.MainWindow)(target)).Closing += new System.ComponentModel.CancelEventHandler(this.Window_Closing);
            
            #line default
            #line hidden
            
            #line 10 "..\..\MainWindow.xaml"
            ((OLX_Parser.MainWindow)(target)).MouseMove += new System.Windows.Input.MouseEventHandler(this.Window_MouseMove);
            
            #line default
            #line hidden
            
            #line 10 "..\..\MainWindow.xaml"
            ((OLX_Parser.MainWindow)(target)).MouseDown += new System.Windows.Input.MouseButtonEventHandler(this.Drag);
            
            #line default
            #line hidden
            return;
            case 4:
            this.WebsiteInput = ((System.Windows.Controls.TextBox)(target));
            
            #line 177 "..\..\MainWindow.xaml"
            this.WebsiteInput.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.EnableDisableAddButton);
            
            #line default
            #line hidden
            
            #line 177 "..\..\MainWindow.xaml"
            this.WebsiteInput.Loaded += new System.Windows.RoutedEventHandler(this.WebsiteInput_Loaded);
            
            #line default
            #line hidden
            
            #line 177 "..\..\MainWindow.xaml"
            this.WebsiteInput.LostFocus += new System.Windows.RoutedEventHandler(this.WebsiteInput_LostFocus);
            
            #line default
            #line hidden
            
            #line 177 "..\..\MainWindow.xaml"
            this.WebsiteInput.GotFocus += new System.Windows.RoutedEventHandler(this.WebsiteInput_GotFocus);
            
            #line default
            #line hidden
            
            #line 177 "..\..\MainWindow.xaml"
            this.WebsiteInput.PreviewDragEnter += new System.Windows.DragEventHandler(this.WebsiteInput_GotFocus);
            
            #line default
            #line hidden
            
            #line 177 "..\..\MainWindow.xaml"
            this.WebsiteInput.PreviewDragLeave += new System.Windows.DragEventHandler(this.WebsiteInput_LostFocus);
            
            #line default
            #line hidden
            return;
            case 5:
            this.PhoneInput = ((System.Windows.Controls.TextBox)(target));
            
            #line 179 "..\..\MainWindow.xaml"
            this.PhoneInput.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.EnableDisableAddButton);
            
            #line default
            #line hidden
            
            #line 179 "..\..\MainWindow.xaml"
            this.PhoneInput.Loaded += new System.Windows.RoutedEventHandler(this.PhoneInput_Loaded);
            
            #line default
            #line hidden
            
            #line 179 "..\..\MainWindow.xaml"
            this.PhoneInput.LostFocus += new System.Windows.RoutedEventHandler(this.PhoneInput_LostFocus);
            
            #line default
            #line hidden
            
            #line 179 "..\..\MainWindow.xaml"
            this.PhoneInput.GotFocus += new System.Windows.RoutedEventHandler(this.PhoneInput_GotFocus);
            
            #line default
            #line hidden
            
            #line 179 "..\..\MainWindow.xaml"
            this.PhoneInput.PreviewDragEnter += new System.Windows.DragEventHandler(this.PhoneInput_GotFocus);
            
            #line default
            #line hidden
            
            #line 179 "..\..\MainWindow.xaml"
            this.PhoneInput.PreviewDragLeave += new System.Windows.DragEventHandler(this.PhoneInput_LostFocus);
            
            #line default
            #line hidden
            return;
            case 6:
            this.RegisterButton = ((System.Windows.Controls.Button)(target));
            
            #line 181 "..\..\MainWindow.xaml"
            this.RegisterButton.Click += new System.Windows.RoutedEventHandler(this.RegisterWebsiteFromInput);
            
            #line default
            #line hidden
            return;
            case 7:
            this.Tabs = ((System.Windows.Controls.TabControl)(target));
            return;
            case 9:
            this.NewOffers = ((System.Windows.Controls.DataGrid)(target));
            
            #line 254 "..\..\MainWindow.xaml"
            this.NewOffers.MouseWheel += new System.Windows.Input.MouseWheelEventHandler(this.ScrollViewer_PreviewMouseWheel);
            
            #line default
            #line hidden
            return;
            case 14:
            this.SettingsTab = ((System.Windows.Controls.TabItem)(target));
            return;
            case 15:
            this.RestorePerson = ((System.Windows.Controls.Button)(target));
            
            #line 337 "..\..\MainWindow.xaml"
            this.RestorePerson.Click += new System.Windows.RoutedEventHandler(this.RestoreWebsite);
            
            #line default
            #line hidden
            return;
            case 16:
            
            #line 351 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.CreateBackup);
            
            #line default
            #line hidden
            return;
            case 17:
            
            #line 352 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.RestoreBackup);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        void System.Windows.Markup.IStyleConnector.Connect(int connectionId, object target) {
            System.Windows.EventSetter eventSetter;
            switch (connectionId)
            {
            case 2:
            eventSetter = new System.Windows.EventSetter();
            eventSetter.Event = System.Windows.UIElement.PreviewMouseWheelEvent;
            
            #line 35 "..\..\MainWindow.xaml"
            eventSetter.Handler = new System.Windows.Input.MouseWheelEventHandler(this.ScrollViewer_PreviewMouseWheel);
            
            #line default
            #line hidden
            ((System.Windows.Style)(target)).Setters.Add(eventSetter);
            break;
            case 3:
            eventSetter = new System.Windows.EventSetter();
            eventSetter.Event = System.Windows.UIElement.MouseDownEvent;
            
            #line 52 "..\..\MainWindow.xaml"
            eventSetter.Handler = new System.Windows.Input.MouseButtonEventHandler(this.Drag);
            
            #line default
            #line hidden
            ((System.Windows.Style)(target)).Setters.Add(eventSetter);
            break;
            case 8:
            
            #line 242 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.UnregisterWebsite);
            
            #line default
            #line hidden
            break;
            case 10:
            
            #line 304 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.IgnoreNewOffer);
            
            #line default
            #line hidden
            break;
            case 11:
            
            #line 305 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.CopyAddress);
            
            #line default
            #line hidden
            break;
            case 12:
            
            #line 313 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.Image)(target)).MouseDown += new System.Windows.Input.MouseButtonEventHandler(this.EnlargeImageCommand);
            
            #line default
            #line hidden
            break;
            case 13:
            
            #line 319 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.EnlargeImageCommand);
            
            #line default
            #line hidden
            break;
            }
        }
    }
}

