using System;
using System.Reflection;


namespace Rescues
{
    public static class AssetsInjector
    {
        private static readonly Type _injectAssetAttributeType = typeof(InjectAudioInterfacesAttribute);

        public static T Inject<T>(this AudioControllerContext context, T target)
        {
            var targetType = target.GetType();
            var allFields = targetType.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            for (int i = 0; i < allFields.Length; i++)
            {
                var fieldInfo = allFields[i];
                var injectAssetAttribute =
                fieldInfo.GetCustomAttribute(_injectAssetAttributeType) as InjectAudioInterfacesAttribute;
                if (injectAssetAttribute == null)
                {
                    continue;
                }
                var objectToInject = context.GetObjectOfType(fieldInfo.FieldType);
                fieldInfo.SetValue(target, objectToInject);
            }
            return target;
        }
    }
}