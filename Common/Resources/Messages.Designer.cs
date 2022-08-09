﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Common.Resources {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class Messages {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Messages() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Common.Resources.Messages", typeof(Messages).Assembly);
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
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Content length over limit..
        /// </summary>
        public static string ContentLengthLimit {
            get {
                return ResourceManager.GetString("ContentLengthLimit", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Please enter a valid api key in the format - ApiKey keyValue.
        /// </summary>
        public static string EnterApiKey {
            get {
                return ResourceManager.GetString("EnterApiKey", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Please enter a valid JWT.
        /// </summary>
        public static string EnterJwt {
            get {
                return ResourceManager.GetString("EnterJwt", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to An error has occured. Please try again later..
        /// </summary>
        public static string GeneralErrorMessage {
            get {
                return ResourceManager.GetString("GeneralErrorMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Invalid deployment type.
        /// </summary>
        public static string InvalidDeploymentType {
            get {
                return ResourceManager.GetString("InvalidDeploymentType", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Invalid trace.
        /// </summary>
        public static string InvalidTrace {
            get {
                return ResourceManager.GetString("InvalidTrace", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Resource not found.
        /// </summary>
        public static string ResourceNotFound {
            get {
                return ResourceManager.GetString("ResourceNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Invalid authentication.
        /// </summary>
        public static string UnauthenticatedRequest {
            get {
                return ResourceManager.GetString("UnauthenticatedRequest", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Invalid authorization.
        /// </summary>
        public static string UnauthorizedRequest {
            get {
                return ResourceManager.GetString("UnauthorizedRequest", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unsupported Api Version ({0}). The default version is ({1})..
        /// </summary>
        public static string UnsupportedApiVersion {
            get {
                return ResourceManager.GetString("UnsupportedApiVersion", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The version of the API being called. For this operation ({0})..
        /// </summary>
        public static string VersionHeaderDescription {
            get {
                return ResourceManager.GetString("VersionHeaderDescription", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Version not selected.
        /// </summary>
        public static string VersionNotSelected {
            get {
                return ResourceManager.GetString("VersionNotSelected", resourceCulture);
            }
        }
    }
}
