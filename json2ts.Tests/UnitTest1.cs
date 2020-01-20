using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace json2ts.Tests
{
    using System.IO;

    using FluentAssertions;

    using Newtonsoft.Json.Linq;

    using Xamasoft.JsonClassGenerator;
    using Xamasoft.JsonClassGenerator.CodeWriters;

    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void JsonGenerator_With_NestedArray_With_Object_Should_Write_any()
        {
            string json = "{ \"test\":[[{}]] }";
            string expected = "declare module namespace {\r\n\r\n    export interface RootObject {\r\n        test: any[][];\r\n    }\r\n\r\n}\r\n\r\n";
            var result = Prepare(json, "namespace", "RootObject");

            result.Should().Be(expected);
        }

        [TestMethod]
        public void JsonGenerator_With_NestedArray_Should_Write_any()
        {
            string json = "{ \"test\":[[]] }";
            string expected = "declare module namespace {\r\n\r\n    export interface RootObject {\r\n        test: any[][];\r\n    }\r\n\r\n}\r\n\r\n";
            var result = Prepare(json, "namespace", "RootObject");

            result.Should().Be(expected);
        }  
        
        [TestMethod]
        public void JsonGenerator_With_Array_Should_Write_any()
        {
            string json = "{ \"test\":[] }";
            string expected = "declare module namespace {\r\n\r\n    export interface RootObject {\r\n        test: any[];\r\n    }\r\n\r\n}\r\n\r\n";
            var result = Prepare(json, "namespace", "RootObject");

            result.Should().Be(expected);
        }

        [TestMethod]
        public void GetTypeName_With_Array_Should_Write_any()
        {
            //arrange
            TypeScriptCodeWriter tscw = new TypeScriptCodeWriter();
            var jsonClassGenerator = new JsonClassGenerator();
            jsonClassGenerator.Namespace = "namespace";
            jsonClassGenerator.SingleFile = true;;
            jsonClassGenerator.MainClass = "RootObject";
            jsonClassGenerator.UseNestedClasses = true;
            //act
            JArray jObject = JArray.Parse("[]");
            var result = tscw.GetTypeName(new JsonType(jsonClassGenerator, jObject), jsonClassGenerator);

            //assert
            result.Should().Be("any[]");

        }  
        
        [TestMethod]
        public void GetTypeName_With_Array_of_Object_Should_Write_any()
        {
            //arrange
            TypeScriptCodeWriter tscw = new TypeScriptCodeWriter();
            var jsonClassGenerator = new JsonClassGenerator();
            jsonClassGenerator.Namespace = "namespace";
            jsonClassGenerator.SingleFile = true;;
            jsonClassGenerator.MainClass = "RootObject";
            jsonClassGenerator.UseNestedClasses = true;
            //act
            JArray jObject = JArray.Parse("[{}]");
            var result = tscw.GetTypeName(new JsonType(jsonClassGenerator, jObject), jsonClassGenerator);

            //assert
            result.Should().Be("any[]");

        }

        private string Prepare(string code, string ns, string mainClass)
        {
            var gen = new JsonClassGenerator();
            gen.Example = code;
            TextWriter txtStream = new StringWriter();
            gen.OutputStream = txtStream;
            //gen.InternalVisibility = this.radInternal.Checked;
            gen.CodeWriter = new TypeScriptCodeWriter();

            gen.Namespace = ns;
            //gen.NoHelperClass = this.chkNoHelper.Checked;
            //gen.SecondaryNamespace = this.radDifferentNamespace.Checked && !string.IsNullOrEmpty(this.edtSecondaryNamespace.Text) ? this.edtSecondaryNamespace.Text : null;
            //gen.TargetFolder = this.edtTargetFolder.Text;
            //gen.UseProperties = true;
            gen.MainClass = mainClass;
            //gen.UsePascalCase = false;
            gen.UseNestedClasses = true; //this.radNestedClasses.Checked;
            //gen.ApplyObfuscationAttributes = this.chkApplyObfuscationAttributes.Checked;
            gen.SingleFile = true; //this.chkSingleFile.Checked;
            //gen.ExamplesInDocumentation = true;
            gen.GenerateClasses();
            return txtStream.ToString();
        }
    }
}
