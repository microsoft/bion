using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;

namespace ScaleDemo.Serializers
{
    public class RegionContractResolver : DefaultContractResolver
    {
        public override JsonContract ResolveContract(Type type)
        {
            JsonContract contract = base.CreateContract(type);

            if (type == typeof(List<Region2>))
            {
                contract.Converter = Region2ListConverter.Instance;
            }
            else if (type == typeof(Region2))
            {
                contract.Converter = Region2Converter.Instance;
            }
            else if (type == typeof(RegionTable))
            {
                contract.Converter = RegionTableConverter.Instance;
            }

            return contract;
        }
    }
}
