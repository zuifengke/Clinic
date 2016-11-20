// ***********************************************************
// 封装与系统对象反射有关的方法集合
// Creator:YangMingkun  Date:2013-7-24
// Copyright:supconhealth
// ***********************************************************
using System;
using System.Text;
using System.Reflection;
using System.Collections;
using System.ComponentModel;
using System.Globalization;

namespace Windy.WebMVC.Web2.Utility
{
    public partial struct GlobalMethods
    {
        /// <summary>
        /// 封装与系统反射有关的方法
        /// </summary>
        public struct Reflect
        {
            /// <summary>
            /// 创建指定类型元素对象的实例
            /// </summary>
            /// <param name="type">对象类型</param>
            /// <param name="args">构造参数</param>
            /// <returns>对象实例</returns>
            public static object CreateInstance(Type type, object[] args)
            {
                try
                {
                    Type[] types = new Type[0];
                    return type.GetConstructor(types).Invoke(args);
                }
                catch (Exception ex)
                {
                    return null;
                }
            }

            /// <summary>
            /// 获取指定对象的指定属性的属性值
            /// </summary>
            /// <param name="instance">指定对象</param>
            /// <param name="property">指定属性</param>
            /// <returns>属性值</returns>
            public static object GetPropertyValue(object instance, PropertyInfo property)
            {
                if (property == null || instance == null || !property.CanRead)
                    return null;
                try
                {
                    return property.GetValue(instance, null);
                }
                catch (Exception ex)
                {
                    return null;
                }
            }

            /// <summary>
            /// 设置指定对象的指定属性的属性值
            /// </summary>
            /// <param name="instance">指定对象</param>
            /// <param name="property">指定属性</param>
            /// <param name="value">属性值</param>
            /// <returns>是否成功</returns>
            public static bool SetPropertyValue(object instance, PropertyInfo property, object value)
            {
                if (property == null || instance == null || !property.CanWrite)
                    return false;

                try
                {
                    if (value == null || value.GetType() == property.PropertyType)
                    {
                        property.SetValue(instance, value, null);
                        return true;
                    }

                    TypeConverter converter = TypeDescriptor.GetConverter(property.PropertyType);
                    if (converter.CanConvertFrom(value.GetType()))
                    {
                        value = converter.ConvertFrom(null, CultureInfo.InvariantCulture, value);
                    }
                    property.SetValue(instance, value, null);
                    return true;
                }
                catch { return false; }
            }

            /// <summary>
            /// 拷贝同类型的两个对象的属性
            /// </summary>
            /// <param name="source">源对象</param>
            /// <param name="target">目标对象</param>
            /// <returns>是否成功</returns>
            public static bool CopyProperties(object source, object target)
            {
                if (source == null || target == null || source.GetType() != target.GetType())
                    return false;
                PropertyInfo[] elementProperties = source.GetType().GetProperties();
                foreach (PropertyInfo property in elementProperties)
                {
                    MethodInfo method = property.GetSetMethod();
                    if (method == null || !method.IsPublic)
                        continue;
                    if (!property.CanRead || !property.CanWrite)
                        continue;

                    Type propertyType = property.PropertyType;
                    object propertyValue = GetPropertyValue(source, property);
                    ICloneable cloneValue = propertyValue as ICloneable;
                    if (cloneValue != null)
                    {
                        SetPropertyValue(target, property, cloneValue.Clone());
                        continue;
                    }

                    //支持IList或IDictionary接口来迭代集合
                    IDictionary dictionary = propertyValue as IDictionary;
                    if (dictionary != null)
                    {
                        IDictionary instance = null;
                        if (!property.CanWrite)
                            instance = GetPropertyValue(target, property) as IDictionary;
                        else
                            instance = CreateInstance(propertyType, null) as IDictionary;
                        if (instance == null)
                            continue;
                        foreach (DictionaryEntry entry in dictionary)
                        {
                            ICloneable clone = entry.Key as ICloneable;
                            object key = (clone == null) ? entry.Key : clone.Clone();
                            if (key == null)
                                continue;
                            object value = null;
                            if (entry.Value != null)
                            {
                                clone = entry.Value as ICloneable;
                                value = (clone == null) ? entry.Value : clone.Clone();
                            }
                            instance.Add(key, value);
                        }
                        GlobalMethods.Reflect.SetPropertyValue(target, property, instance);
                        continue;
                    }

                    //支持IList或IDictionary接口来迭代集合
                    IList list = propertyValue as IList;
                    if (list != null)
                    {
                        IList instance = null;
                        if (!property.CanWrite)
                            instance = GetPropertyValue(target, property) as IList;
                        else
                            instance = CreateInstance(propertyType, null) as IList;
                        if (instance == null)
                            continue;
                        foreach (object item in list)
                        {
                            ICloneable clone = item as ICloneable;
                            if (clone == null)
                                instance.Add(item);
                            else
                                instance.Add(clone.Clone());
                        }
                        GlobalMethods.Reflect.SetPropertyValue(target, property, instance);
                        continue;
                    }
                    GlobalMethods.Reflect.SetPropertyValue(target, property, propertyValue);
                }
                return true;
            }
        }
    }
}
