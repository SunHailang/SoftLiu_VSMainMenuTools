﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ExcelTConfig {
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
    internal class CSharpTemplate {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal CSharpTemplate() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("ExcelTConfig.CSharpTemplate", typeof(CSharpTemplate).Assembly);
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
        
        /// <summary>
        ///   Looks up a localized string similar to ///Auto-generated file, do not modify
        ///
        ///namespace __namespace__
        ///{__extra_attrs__
        ///    public enum __className__ : int
        ///    {
        ///__fields__
        ///    }
        ///}.
        /// </summary>
        internal static string EnumTemplate {
            get {
                return ResourceManager.GetString("EnumTemplate", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ///Auto-generated file, do not modify
        //////excelName: __excelName__
        ///
        ///using System;
        ///using System.Runtime.InteropServices;
        ///using UnityEngine;
        ///
        ///namespace __namespace__
        ///{
        ///    public unsafe partial struct __className__ : IConfigType
        ///    {
        ///        [StructLayout(LayoutKind.Sequential)]
        ///        private unsafe struct __dataName__
        ///        {
        ///__dataFields__
        ///        }
        ///
        ///        private static byte* dataPointer { get { Table.TableIndexCheck(TableIndex.__className__); return Table.GetDataPointer(TableIndex.__className__); }  [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string InstanceTemplate {
            get {
                return ResourceManager.GetString("InstanceTemplate", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ///Auto-generated file, do not modify
        ///
        ///using System.Runtime.InteropServices;
        ///
        ///namespace __namespace__
        ///{
        ///    public static unsafe partial class __className__
        ///    {
        ///        [StructLayout(LayoutKind.Sequential)]
        ///        private unsafe struct __dataName__
        ///        {
        ///__dataFields__
        ///        }
        ///
        ///        private static byte* dataPointer { get { return Table.GetDataPointer(TableIndex.__className__); } }
        ///        private static __dataName__* p { get { return (__dataName__*)Table.ConfigByIndex(TableIndex.__className__, 0 [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string StaticTemplate {
            get {
                return ResourceManager.GetString("StaticTemplate", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ///Auto-generated file, do not modify
        ///
        ///using System.Runtime.InteropServices;
        ///
        ///namespace __namespace__
        ///{
        ///    public unsafe partial struct __className__ : IConfigStruct
        ///    {
        ///        [StructLayout(LayoutKind.Sequential)]
        ///        private struct __dataName__
        ///        {
        ///__dataFields__
        ///        }
        ///
        ///        public void SetPointer(byte* p, byte* dataPointer, int pVersion) { this.p = (__dataName__*)p; this.dataPointer = dataPointer; this.pVersion = pVersion; }
        ///        public __className__(byte* p, byte* dataPointer, in [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string StructTemplate {
            get {
                return ResourceManager.GetString("StructTemplate", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ///Auto-generated file, do not modify
        ///
        ///namespace __namespace__
        ///{
        ///    public unsafe partial class __className__
        ///    {
        ///__fields__
        ///
        ///        public static void SetData()
        ///        {
        ///__setDataContents__
        ///        }
        ///    }
        ///}.
        /// </summary>
        internal static string TableIndex {
            get {
                return ResourceManager.GetString("TableIndex", resourceCulture);
            }
        }
    }
}
