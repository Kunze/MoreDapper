using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoreDapper.Converter.Types;
using System.Globalization;

namespace UnitTestProject1
{
    public class FakeModel
    {
        public bool Bool1 { get; set; }
        public bool? Bool2 { get; set; }
        public string String1 { get; set; }
        public string String2 { get; set; }
        public DateTime DateTime1 { get; set; }
        public DateTime? DateTime2 { get; set; }
        public double Double1 { get; set; }
        public double? NullableDouble { get; set; }
    }

    [TestClass]
    public class SqlTypeConverterTest
    {
        [TestMethod]
        public void BooleanConverterTest()
        {
            var model = new FakeModel()
            {
                Bool1 = true,
                Bool2 = true
            };
            var type = model.GetType();
            var property1 = model.GetType().GetProperty("Bool1");
            var property2 = model.GetType().GetProperty("Bool2");

            var converter = new BooleanConverter();

            Assert.IsTrue(converter.Match(property1));
            Assert.IsTrue(converter.Match(property2));

            var info1 = converter.GetValue(model, property1);

            Assert.AreEqual("1", info1);

            var info2 = converter.GetValue(model, property2);

            Assert.AreEqual("1", info2);

            model.Bool2 = null;

            var info3 = converter.GetValue(model, property2);
            Assert.AreEqual("null", info3);
        }

        [TestMethod]
        public void StringConverterTest()
        {
            var model = new FakeModel()
            {
                String1 = "murilo",
                String2 = null
            };
            var type = model.GetType();
            var property1 = model.GetType().GetProperty("String1");
            var property2 = model.GetType().GetProperty("String2");

            var converter = new StringConverter();

            Assert.IsTrue(converter.Match(property1));
            Assert.IsTrue(converter.Match(property2));

            var info1 = converter.GetValue(model, property1);

            Assert.AreEqual("'murilo'", info1);

            var info2 = converter.GetValue(model, property2);

            Assert.AreEqual("null", info2);
        }

        [TestMethod]
        public void DatetimeConverterTest()
        {
            var now = DateTime.Parse("01/02/2000 10:11:12", CultureInfo.InvariantCulture);

            var model = new FakeModel()
            {
                DateTime1 = now,
                DateTime2 = null
            };
            var type = model.GetType();
            var property1 = model.GetType().GetProperty("DateTime1");
            var property2 = model.GetType().GetProperty("DateTime2");

            var converter = new DateTimeConverter();

            Assert.IsTrue(converter.Match(property1));
            Assert.IsTrue(converter.Match(property2));

            var info1 = converter.GetValue(model, property1);

            Assert.AreEqual("01/02/2000 10:11:12", info1);

            var info2 = converter.GetValue(model, property2);

            Assert.AreEqual("null", info2);
        }

        [TestMethod]
        public void Double()
        {
            var model = new FakeModel()
            {
                Double1 = 10d
            };
            var type = model.GetType();
            var property1 = model.GetType().GetProperty("Double1");

            var converter = new DoubleConverter();

            Assert.IsTrue(converter.Match(property1));
            Assert.AreEqual("10", converter.GetValue(model, property1));
        }

        [TestMethod]
        public void NullableDouble()
        {
            var model = new FakeModel()
            {
                NullableDouble = 11.10d
            };
            var type = model.GetType();
            var property1 = model.GetType().GetProperty("NullableDouble");

            var converter = new DoubleConverter();

            Assert.IsTrue(converter.Match(property1));
            Assert.AreEqual("11.1", converter.GetValue(model, property1));
        }
    }
}
