﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace ExcelTConfig {
    using System;
    
    
    /// <summary>
    ///   一个强类型的资源类，用于查找本地化的字符串等。
    /// </summary>
    // 此类是由 StronglyTypedResourceBuilder
    // 类通过类似于 ResGen 或 Visual Studio 的工具自动生成的。
    // 若要添加或移除成员，请编辑 .ResX 文件，然后重新运行 ResGen
    // (以 /str 作为命令选项)，或重新生成 VS 项目。
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class JavaTemplate {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal JavaTemplate() {
        }
        
        /// <summary>
        ///   返回此类使用的缓存的 ResourceManager 实例。
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("ExcelTConfig.JavaTemplate", typeof(JavaTemplate).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   重写当前线程的 CurrentUICulture 属性，对
        ///   使用此强类型资源类的所有资源查找执行重写。
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
        ///   查找类似 package config;
        ///
        ///public enum __EnumName__
        ///{
        ///    /*values*/;
        ///
        ///    private int value;
        ///    public int value() { return value; }
        ///    private __EnumName__(int value) { this.value = value; }
        ///
        ///    public static __EnumName__ valueOf(int value)
        ///    {
        ///        switch(value)
        ///        {
        ////*switchContent*/
        ///            default: return null;
        ///        }
        ///    }
        ///
        ///    public static __EnumName__ read(byte[] bytes, int index)
        ///    {
        ///        return valueOf(Config.readInt32(bytes, index));
        ///    }
        ///} 的本地化字符串。
        /// </summary>
        internal static string @__EnumName__ {
            get {
                return ResourceManager.GetString("__EnumName__", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 package config;
        ///
        ///import java.util.ArrayList;
        ///import java.util.HashMap;
        ///
        ///public class __KlassName__ {
        ///	
        ////*properties*/
        ///	
        ///	private __KlassName__() {}
        ///	
        ///	private static __KlassName__[] configs;
        ///	public static int count(){ return configs.length; }
        ///	public static __KlassName__ byIndex(int index) { return index &gt;= 0 &amp;&amp; index &lt; configs.length ? configs[index] : null; }
        ////*map*/
        ///
        ///	public static void load()
        ///	{
        ///		if(configs != null) return;
        ///
        ///		byte[] bytes = Config.readFile(&quot;__KlassName__&quot;);
        ///		Hea [字符串的其余部分被截断]&quot;; 的本地化字符串。
        /// </summary>
        internal static string @__KlassName__ {
            get {
                return ResourceManager.GetString("__KlassName__", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 package config;
        ///
        ///public class __Loader__
        ///{
        ///    public static void loadAll()
        ///    {
        ////*content*/
        ///    }
        ///} 的本地化字符串。
        /// </summary>
        internal static string @__Loader__ {
            get {
                return ResourceManager.GetString("__Loader__", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 package config;
        ///
        ///import java.util.ArrayList;
        ///import java.util.HashMap;
        ///
        ///public class __StaticName__
        ///{
        ////*properties*/
        ///    public static void load()
        ///    {
        ///        byte[] bytes = Config.readFile(&quot;__StaticName__&quot;);
        ///        Header header = Config.readHeader(bytes);
        ///        int offset = header.dataOffset;
        ///        
        ////*readProperties*/
        ///    }
        ///} 的本地化字符串。
        /// </summary>
        internal static string @__StaticName__ {
            get {
                return ResourceManager.GetString("__StaticName__", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 package config;
        ///
        ///import java.util.ArrayList;
        ///import java.util.HashMap;
        ///
        ///public class __StructName__
        ///{
        ////*properties*/
        ///
        ///    private __StructName__(){}
        ///
        ///    public static __StructName__ read(byte[] bytes, int offset)
        ///    {
        ///        __StructName__ __instance__ = new __StructName__();
        ////*content*/
        ///        return __instance__;
        ///    }
        ///} 的本地化字符串。
        /// </summary>
        internal static string @__StructName__ {
            get {
                return ResourceManager.GetString("__StructName__", resourceCulture);
            }
        }
    }
}
