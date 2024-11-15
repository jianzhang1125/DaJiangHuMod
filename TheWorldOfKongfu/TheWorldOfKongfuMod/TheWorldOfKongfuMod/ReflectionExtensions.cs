using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TheWorldOfKongfuMod
{
    public static class ReflectionExtensions
    {
        public static T GetFieldValue<T>(this object instance, string fieldname)
        {
            try
            {
                return (T)(AccessTools.Field(instance.GetType(), fieldname)?.GetValue(instance));
            }
            catch (Exception arg)
            {
                PluginMain.LogError($"Error getting field {fieldname} from {instance.GetType().Name}: {arg}");
                return default(T);
            }
        }

        public static T GetClassFieldValue<T>(this Type type, string fieldname)
        {
            return (T)(AccessTools.Field(type, fieldname)?.GetValue(null));
        }

        public static void SetPrivateField(this object instance, string fieldname, object value)
        {
            AccessTools.Field(instance.GetType(), fieldname)?.SetValue(instance, value);
        }

        public static T CallPrivateMethod<T>(this object instance, string methodname, params object[] param)
        {
            return (T)AccessTools.Method(instance.GetType(), methodname, (Type[])null, (Type[])null).Invoke(instance, param);
        }

        public static void CallPrivateMethod(this object instance, string methodname, params object[] param)
        {
            AccessTools.Method(instance.GetType(), methodname, (Type[])null, (Type[])null)?.Invoke(instance, param);
        }

        public static void SetPrivateProperty(this object instance, string propertyname, object value)
        {
            AccessTools.Property(instance.GetType(), propertyname)?.SetValue(instance, value);
        }

        public static T GetPrivateProperty<T>(this object instance, string propertyname)
        {
            return (T)(AccessTools.Property(instance.GetType(), propertyname)?.GetValue(instance));
        }

        public static T ShallowCopy<T>(this T source) where T : class, new()
        {
            if (source == null)
            {
                return null;
            }
            Type type = source.GetType();
            T val = new T();
            FieldInfo[] fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            foreach (FieldInfo obj in fields)
            {
                object value = obj.GetValue(source);
                obj.SetValue(val, value);
            }
            PropertyInfo[] properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            foreach (PropertyInfo propertyInfo in properties)
            {
                if (propertyInfo.CanRead && propertyInfo.CanWrite)
                {
                    object value2 = propertyInfo.GetValue(source);
                    propertyInfo.SetValue(val, value2);
                }
            }
            return val;
        }
    }

}
