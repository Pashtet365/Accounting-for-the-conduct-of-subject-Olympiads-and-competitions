﻿#pragma checksum "..\..\ПрохождениеЭтапов.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "5435436199AF2F1CD80D2C0D793CCD62A3BCFF346F8A610AB8367A700F39B17B"
//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

using MaterialDesignThemes.Wpf;
using MaterialDesignThemes.Wpf.Converters;
using MaterialDesignThemes.Wpf.Transitions;
using Olympiads;
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


namespace Olympiads {
    
    
    /// <summary>
    /// ПрохождениеЭтапов
    /// </summary>
    public partial class ПрохождениеЭтапов : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 11 "..\..\ПрохождениеЭтапов.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label mainLabel;
        
        #line default
        #line hidden
        
        
        #line 12 "..\..\ПрохождениеЭтапов.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtBall;
        
        #line default
        #line hidden
        
        
        #line 13 "..\..\ПрохождениеЭтапов.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox comboStudent;
        
        #line default
        #line hidden
        
        
        #line 14 "..\..\ПрохождениеЭтапов.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox comboStatus;
        
        #line default
        #line hidden
        
        
        #line 15 "..\..\ПрохождениеЭтапов.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox comboApl;
        
        #line default
        #line hidden
        
        
        #line 16 "..\..\ПрохождениеЭтапов.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button mainButton;
        
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
            System.Uri resourceLocater = new System.Uri("/Olympiads;component/%d0%9f%d1%80%d0%be%d1%85%d0%be%d0%b6%d0%b4%d0%b5%d0%bd%d0%b8" +
                    "%d0%b5%d0%ad%d1%82%d0%b0%d0%bf%d0%be%d0%b2.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\ПрохождениеЭтапов.xaml"
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
            
            #line 9 "..\..\ПрохождениеЭтапов.xaml"
            ((Olympiads.ПрохождениеЭтапов)(target)).Closed += new System.EventHandler(this.Window_Closed);
            
            #line default
            #line hidden
            return;
            case 2:
            this.mainLabel = ((System.Windows.Controls.Label)(target));
            return;
            case 3:
            this.txtBall = ((System.Windows.Controls.TextBox)(target));
            
            #line 12 "..\..\ПрохождениеЭтапов.xaml"
            this.txtBall.PreviewTextInput += new System.Windows.Input.TextCompositionEventHandler(this.txtName_PreviewTextInput);
            
            #line default
            #line hidden
            return;
            case 4:
            this.comboStudent = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 5:
            this.comboStatus = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 6:
            this.comboApl = ((System.Windows.Controls.ComboBox)(target));
            
            #line 15 "..\..\ПрохождениеЭтапов.xaml"
            this.comboApl.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.comboApl_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 7:
            this.mainButton = ((System.Windows.Controls.Button)(target));
            
            #line 16 "..\..\ПрохождениеЭтапов.xaml"
            this.mainButton.Click += new System.Windows.RoutedEventHandler(this.mainButton_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

