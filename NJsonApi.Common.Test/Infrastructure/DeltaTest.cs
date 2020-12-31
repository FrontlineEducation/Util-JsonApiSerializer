﻿using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using UtilJsonApiSerializer.Common.Infrastructure;

namespace UtilJsonApiSerializer.Common.Test.Infrastructure
{
    public class DeltaTest
    {
        [Theory]
        public void SimpleTestOfFunction()
        {
            //Arange
            var simpleObject = new SimpleTestClass();
            var classUnderTest = new Delta<SimpleTestClass>();

            classUnderTest.AddFilter(t => t.Prop1NotIncluded);
            classUnderTest.ObjectPropertyValues = new Dictionary<string, object>()
                                         {
                                           {"Prop2","b"}
                                         };
            //Act
            classUnderTest.Apply(simpleObject);
            //Assert
            simpleObject.Prop2.Should().NotBeNull();
            simpleObject.Prop2.Should().Be("b");
            simpleObject.Prop1NotIncluded.Should().BeNull();
        }

        [Theory]
        public void TestNotIncludedProperties()
        {
            //Arrange
            var simpleObject = new SimpleTestClass();
            var objectUnderTest = new Delta<SimpleTestClass>();

            objectUnderTest.AddFilter(t => t.Prop1NotIncluded);
            objectUnderTest.ObjectPropertyValues = new Dictionary<string, object>()
                                         {
                                           {"Prop2","b"},
                                           {"Prop1NotIncluded",5} 
                                         };
            //Act
            objectUnderTest.Apply(simpleObject);
            //Assert
            simpleObject.Prop2.Should().NotBeNull();
            simpleObject.Prop2.Should().Be("b");
            simpleObject.Prop1NotIncluded.Should().BeNull();
        }

        [Theory]
        public void TestEmptyPropertiesValues()
        {
            //Arrange
            var simpleObject = new SimpleTestClass();
            var objectUnderTest = new Delta<SimpleTestClass>();
            //Act
            objectUnderTest.AddFilter(t => t.Prop1NotIncluded);
            objectUnderTest.Apply(simpleObject);
            //Assert
            simpleObject.Prop1NotIncluded.Should().BeNull();
            simpleObject.Prop1.Should().BeNull();
            simpleObject.Prop2.Should().BeNull();
        }
    }

    public class SimpleTestClass
    {
        public string Prop1 { get; set; }
        public string Prop2 { get; set; }
        public int? Prop1NotIncluded { get; set; }
    }
    public class SecondSimpleTestClass
    {
        public string Prop1 { get; set; }
        public string Prop2 { get; set; }
        public int Prop1NotIncluded { get; set; }
    }
}
