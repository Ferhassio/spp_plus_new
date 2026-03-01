using System;
using System.Reflection;
using EntityStates;
using RoR2;

namespace SkillsPlusPlus.Modifiers
{
    public abstract class ReflectedSkillModifier : BaseSkillModifier
    {
        protected T GetStaticField<T>(Type type, string fieldName)
        {
            FieldInfo field = type.GetField(fieldName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
            if (field == null)
            {
                Logger.Warn("Could not find static field {0} in type {1}", fieldName, type.FullName);
                return default(T);
            }
            return (T)field.GetValue(null);
        }

        protected void SetStaticField<T>(Type type, string fieldName, T value)
        {
            FieldInfo field = type.GetField(fieldName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
            if (field == null)
            {
                Logger.Warn("Could not find static field {0} in type {1}", fieldName, type.FullName);
                return;
            }
            field.SetValue(null, value);
        }

        protected float GetStaticFloat(Type type, string fieldName)
        {
            return GetStaticField<float>(type, fieldName);
        }

        protected void SetStaticFloat(Type type, string fieldName, float value)
        {
            SetStaticField(type, fieldName, value);
        }

        protected int GetStaticInt(Type type, string fieldName)
        {
            return GetStaticField<int>(type, fieldName);
        }

        protected void SetStaticInt(Type type, string fieldName, int value)
        {
            SetStaticField(type, fieldName, value);
        }
    }
}
