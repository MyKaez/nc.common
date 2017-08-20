using System;
using System.Collections.Generic;
using System.Linq;
using Ns.Common.FLINQ;

namespace Ns.Common.Mappings
{
    public class Mapper
    {
        private readonly MapperConfig _config;
        private readonly Dictionary<string, Type> _sourceTypes;
        private readonly Dictionary<string, Type> _targetTypes;

        public Mapper(MapperConfig config)
        {
            var mappings = config.Mappings.ToArray();

            _config = config;
            _sourceTypes = mappings.Select(m => m.Source.ObjectType).Distinct().ToDictionary(m => m, Type.GetType);
            _targetTypes = mappings.Select(m => m.Target.ObjectType).Distinct().ToDictionary(m => m, Type.GetType);
        }

        public object MapObject(object source)
        {
            var returnType = _targetTypes.First().Value;
            var valueHandler = Activator.CreateInstanceFrom(returnType.Assembly.Location, returnType.FullName);
            var targetValue = valueHandler.Unwrap();

            foreach (var m in _config.Mappings)
            {
                var sourceType = _sourceTypes[m.Source.ObjectType];
                var sourceProperty = sourceType.GetProperty(m.Source.PropertyName);
                var sourceValue = sourceProperty.GetValue(source);

                var targetType = _targetTypes[m.Target.ObjectType];
                var targetProperty = targetType.GetProperty(m.Target.PropertyName);
                var targetPropertyType = targetProperty.PropertyType;
                var targetPropertyValue = sourceValue.Convert(targetPropertyType, m.ConversionFormat);

                targetProperty.SetValue(targetValue, targetPropertyValue);
            }

            return targetValue;
        }
    }
}