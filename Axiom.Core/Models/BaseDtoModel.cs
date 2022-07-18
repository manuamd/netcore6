using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Reflection;
using System.Text;

namespace Axiom.Application.Models
{
    public class BaseDtoModel
    {
        public string UserName { get; set; }
        public string ClientId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CreatedDate { get; set; }
        public string CreateUser { get; set; }
        public string UpdateDate { get; set; }
        public string UpdateUser { get; set; }

        //Fill Model Property if Dynamic Obj Contains
        public void FillModelValue(dynamic obj, object mod, string Key)
        {
            var obj2 = (IDictionary<string, object>)obj;
            if (((IDictionary<String, object>)obj).ContainsKey(Key))
                mod.GetType().GetProperty(Key).SetValue(mod, obj2[Key], null);
        }

        public object ChangeType(object value, Type conversion)
        {
            var t = conversion;

            if (t.IsGenericType && t.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                if (value == null)
                {
                    return null;
                }

                t = Nullable.GetUnderlyingType(t);
            }

            if (t.FullName == "System.Boolean" && (value.ToString() == "1" || value.ToString() == "0"))
            {
                if (value.ToString() == "1")
                {
                    value = "true";
                }
                else if (value.ToString() == "0")
                {
                    value = "false";
                }
            }

            return Convert.ChangeType(value, t);
        }

        protected void FillAllProperties(dynamic obj, object mod)
        {
            if (obj != null)
            {
                PropertyInfo[] props = mod.GetType().GetProperties();
                List<string> keys = new List<string>();
                foreach (PropertyInfo pI in props)
                {
                    try
                    {
                        keys.Add(pI.Name);
                    }
                    catch (Exception ex)
                    {
                        //LogManager.GetCurrentClassLogger().Error(ex, ex.Message);

                    }
                }
                foreach (string key in keys)
                {
                    try
                    {
                        var obj2 = (IDictionary<string, object>)obj;
                        if (((IDictionary<string, object>)obj).ContainsKey(key))
                        {

                            var value = obj2[key];
                            if (value is ExpandoObject)
                            {
                                // Value is the dynamic object
                                // Property contains the type of the property.

                                var property = mod.GetType().GetProperty(key);
                                Type type = property.PropertyType;

                                // Get the nested dynamic object's dynamic constructor.
                                ConstructorInfo ctor = type.GetConstructor(new[] { typeof(DynamicObject) });
                                if (ctor != null)
                                {
                                    object instance = ctor.Invoke(new object[] { value });
                                    mod.GetType().GetProperty(key).SetValue(mod, instance, null);
                                }
                            }
                            else
                            {
                                object newValue = ChangeType(obj2[key], mod.GetType().GetProperty(key).PropertyType);
                                mod.GetType().GetProperty(key).SetValue(mod, newValue, null);
                            }


                        }
                    }
                    catch (Exception ex)
                    {
                        //Disabling this because alot of converserions are failing BUT it is not breaking anything
                        //LogManager.GetCurrentClassLogger().Error(ex, ex.Message);
                    }
                }

            }
        }

    }
}
