﻿#pragma checksum "..\..\Педагоги.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "80CB5C2A00367EA489873F33BDC8CB817ABAB8D42D6A185AC38749B70584F909"
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
    /// Педагоги
    /// </summary>
    public partial class Педагоги : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 11 "..\..\Педагоги.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label mainLabel;
        
        #line default
        #line hidden
        
        
        #line 12 "..\..\Педагоги.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtFio;
        
        #line default
        #line hidden
        
        
        #line 13 "..\..\Педагоги.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtStaf;
        
        #line default
        #line hidden
        
        
        #line 14 "..\..\Педагоги.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox comboOrg;
        
        #line default
        #line hidden
        
        
        #line 15 "..\..\Педагоги.xaml"
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
            System.Uri resourceLocater = new System.Uri("/Olympiads;component/%d0%9f%d0%b5%d0%b4%d0%b0%d0%b3%d0%be%d0%b3%d0%b8.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\Педагоги.xaml"
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
            
            #line 9 "..\..\Педагоги.xaml"
            ((Olympiads.Педагоги)(target)).Closed += new System.EventHandler(this.Window_Closed);
            
            #line default
            #line hidden
            return;
            case 2:
            this.mainLabel = ((System.Windows.Controls.Label)(target));
            return;
            case 3:
            this.txtFio = ((System.Windows.Controls.TextBox)(target));
            
            #line 12 "..\..\Педагоги.xaml"
            this.txtFio.PreviewTextInput += new System.Windows.Input.TextCompositionEventHandler(this.txtFio_PreviewTextInput);
            
            #line default
            #line hidden
            return;
            case 4:
            this.txtStaf = ((System.Windows.Controls.TextBox)(target));
            
            #line 13 "..\..\Педагоги.xaml"
            this.txtStaf.PreviewTextInput += new System.Windows.Input.TextCompositionEventHandler(this.txtStaf_PreviewTextInput);
            
            #line default
            #line hidden
            return;
            case 5:
            this.comboOrg = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 6:
            this.mainButton = ((System.Windows.Controls.Button)(target));
            
            #line 15 "..\..\Педагоги.xaml"
            this.mainButton.Click += new System.Windows.RoutedEventHandler(this.mainButton_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

