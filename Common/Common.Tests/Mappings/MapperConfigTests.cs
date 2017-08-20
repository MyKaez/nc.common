using System;
using System.Globalization;
using Ns.Common.FLINQ;
using Ns.Common.Mappings;
using NUnit.Framework;

namespace Ns.Common.Tests.Mappings
{
    [TestFixture]
    class MapperConfigTests
    {
        [TestCase]
        public void Map()
        {
            var config = new MapperConfig();

            CreateNameMapping(config);
            CreateBirthDateMapping(config);
            CreateCommentMapping(config);

            var mapper = new Mapper(config);

            var source = new SourceObject
            {
                PropNo1 = "Kenneth",
                PropNo2 = "19900913",
                PropNo3 = "Call him Kenny"
            };

            var target = mapper.MapObject(source).As<TargetObject>();

            Assert.AreEqual(source.PropNo1, target.Name);
            Assert.AreEqual(DateTime.ParseExact(source.PropNo2, "yyyyMMdd", CultureInfo.InvariantCulture),
                target.BirthDate.Value);
            Assert.AreEqual("Comment:=" + source.PropNo3, target.Comment);
        }

        private void CreateCommentMapping(MapperConfig config)
        {
            var source = new MappingEntity
            {
                ObjectType = typeof(SourceObject).AssemblyQualifiedName,
                PropertyName = nameof(SourceObject.PropNo3)
            };
            var target = new MappingEntity
            {
                ObjectType = typeof(TargetObject).AssemblyQualifiedName,
                PropertyName = nameof(TargetObject.Comment)
            };
            var nameMapping = new Mapping(source, target) {ConversionFormat = "Comment:=$VALUE$"};

            config.Add(nameMapping);
        }

        private void CreateNameMapping(MapperConfig config)
        {
            var source = new MappingEntity
            {
                ObjectType = typeof(SourceObject).AssemblyQualifiedName,
                PropertyName = nameof(SourceObject.PropNo1)
            };
            var target = new MappingEntity
            {
                ObjectType = typeof(TargetObject).AssemblyQualifiedName,
                PropertyName = nameof(TargetObject.Name)
            };
            var nameMapping = new Mapping(source, target);

            config.Add(nameMapping);
        }

        private void CreateBirthDateMapping(MapperConfig config)
        {
            var source = new MappingEntity
            {
                ObjectType = typeof(SourceObject).AssemblyQualifiedName,
                PropertyName = nameof(SourceObject.PropNo2)
            };
            var target = new MappingEntity
            {
                ObjectType = typeof(TargetObject).AssemblyQualifiedName,
                PropertyName = nameof(TargetObject.BirthDate)
            };
            var nameMapping = new Mapping(source, target) {ConversionFormat = "yyyyMMdd" };

            config.Add(nameMapping);
        }

        class SourceObject
        {
            public string PropNo1 { get; set; }

            public string PropNo2 { get; set; }

            public string PropNo3 { get; set; }
        }

        class TargetObject
        {
            public string Name { get; set; }

            public DateTime? BirthDate { get; set; }

            public string Comment { get; set; }
        }
    }
}
