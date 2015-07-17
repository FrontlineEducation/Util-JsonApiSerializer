﻿using System;
using System.Linq;
using FakeItEasy;
using FakeItEasy.ExtensionSyntax.Full;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NJsonApi.Serialization;
using SoftwareApproach.TestingExtensions;

namespace NJsonApi.Test.Serialization.JsonApiTransformerTest
{
    [TestClass]
    public class TestLinks
    {
        private const string appUrl = @"http://localhost/";

        private JsonApiTransformer transformer;
        private readonly TransformationHelper transformationHelper = new TransformationHelper();

        [TestInitialize]
        public void Initialize()
        {
            transformer = new JsonApiTransformer(){TransformationHelper = transformationHelper};
        }

        [TestMethod]
        public void Creates_one_to_one_relation_links()
        {
            // Arrange
            var configuration = CreateConfiguration();
            var objectToTransform = CreateObject();

            // Act
            var result = transformer.Transform(objectToTransform, configuration, appUrl);

            // Assert
            result.Links.ShouldNotBeEmpty();
            result.Links.First().Key.ShouldEqual("sampleClasses.nestedValues");
            result.Links.First().Value.Href = "http://nested/{id}";
            result.Links.First().Value.Type = "nestedClasses";
        }

        [TestMethod]
        public void Creates_one_to_one_null_relation_links()
        {
            // Arrange
            var configuration = CreateConfiguration();
            var objectToTransform = CreateNullNestedObject();

            // Act
            var result = transformer.Transform(objectToTransform, configuration, appUrl);

            // Assert
            result.Links.ShouldNotBeEmpty();
            result.Links.First().Key.ShouldEqual("sampleClasses.nestedValues");
            result.Links.First().Value.Href = "http://nested/{id}";
            result.Links.First().Value.Type = "nestedClasses";
        }


        private SampleClass CreateObject()
        {
            var sampleClass = new SampleClass()
            {
                Id = 1,
                SomeValue = "Some string value",
                NestedValue = new NestedClass()
                {
                    Id = 1000,
                    SomeNestedValue = "Nested text value"
                }
            };

            return sampleClass;
        }

        private SampleClass CreateNullNestedObject()
        {
            var sampleClass = new SampleClass()
            {
                Id = 1,
                SomeValue = "Some string value",
                NestedValue = null
            };

            return sampleClass;
        }

        private Configuration CreateConfiguration()
        {
            var conf = new Configuration();
            var sampleClassMapping = new ResourceMapping<SampleClass>(c => c.Id, "http://sampleClass/{id}");
            sampleClassMapping.ResourceType = "sampleClasses";
            sampleClassMapping.AddPropertyGetter("someValue", c => c.SomeValue);
            sampleClassMapping.AddPropertyGetter("nestedValue", c => c.NestedValue);


            var nestedClassMapping = new ResourceMapping<NestedClass>(c => c.Id, "nested/{id}");
            nestedClassMapping.ResourceType = "nestedClasses";
            nestedClassMapping.AddPropertyGetter("someNestedValue", c => c.SomeNestedValue);

            var linkMapping = new LinkMapping<SampleClass, NestedClass>()
            {
                LinkName = "nestedValues",
                ResourceMapping = nestedClassMapping,
                ResourceGetter = c => c.NestedValue,
                ResourceIdGetter = c => c.NestedValueId
            };

            sampleClassMapping.Links.Add(linkMapping);
            
            conf.AddMapping(sampleClassMapping);
            conf.AddMapping(nestedClassMapping);

            return conf;

        }

        class SampleClass
        {
            public int Id { get; set; }
            public string SomeValue { get; set; }
            public int? NestedValueId { get; set; }
            public NestedClass NestedValue { get; set; }
        }

        class NestedClass
        {
            public int Id { get; set; }
            public string SomeNestedValue { get; set; }
        }
    }


}