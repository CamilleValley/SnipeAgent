﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.239
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace UltimateSniper_DAL {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "10.0.0.0")]
    internal sealed partial class DALSettings : global::System.Configuration.ApplicationSettingsBase {
        
        private static DALSettings defaultInstance = ((DALSettings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new DALSettings())));
        
        public static DALSettings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=D:\\UltimateSniper\\UltimateSniperDB." +
            "accdb;User Id=admin;Password=;")]
        public string DBConnexionStringDev {
            get {
                return ((string)(this["DBConnexionStringDev"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Provider=sqloledb;Data Source=db2.3essentials.com;User Id=snipeagent_user;Passwor" +
            "d=CAms1982##; ")]
        public string DBConnexionStringProd {
            get {
                return ((string)(this["DBConnexionStringProd"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool Production {
            get {
                return ((bool)(this["Production"]));
            }
        }
    }
}
