using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoreDapper.Converter.Types;
using System.Globalization;
using MoreDapper;

namespace UnitTestProject1
{
    public class FakeModel
    {
        public bool BoolProperty { get; set; }
        public bool? NullableBool { get; set; }

        public string StringProperty { get; set; }
        public string NullableString { get; set; }

        public DateTime DateTimeProperty { get; set; }
        public DateTime? NullableDateTime { get; set; }

        public double DoubleProperty { get; set; }
        public double? NullableDouble { get; set; }

        public Int16 Int16Property { get; set; }
        public Int16? NullableInt16 { get; set; }
        public Int32 Int32Property { get; set; }
        public Int32? NullableInt32 { get; set; }
        public Int64 Int64Property { get; set; }
        public Int64? NullableInt64 { get; set; }

        public UInt16 UInt16Property { get; set; }
        public UInt16? NullableUInt16 { get; set; }

        public UInt32 UInt32Property { get; set; }
        public UInt32? NullableUInt32 { get; set; }

        public UInt64 UInt64Property { get; set; }
        public UInt64? NullableUInt64 { get; set; }
    }

    [TestClass]
    public class SqlTypeConverterTest
    {
        [TestMethod]
        public void SqlTypeConverter()
        {
            var now = DateTime.Parse("01/02/2000 10:11:12", CultureInfo.InvariantCulture);

            var model = new FakeModel()
            {
                BoolProperty = true,
                DateTimeProperty = now,
                DoubleProperty = 10.12d,
                StringProperty = "murilo",
                Int16Property = 16,
                Int32Property = 32,
                Int64Property = 64,
                UInt16Property = 16,
                UInt32Property = 32,
                UInt64Property = 64
            };
            var type = model.GetType();

            var converter = new SqlTypeConverter();

            Assert.AreEqual("1", converter.GetValue(model, type.GetProperty("BoolProperty")));

            Assert.AreEqual("01/02/2000 10:11:12", converter.GetValue(model, type.GetProperty("DateTimeProperty")));

            Assert.AreEqual("10.12", converter.GetValue(model, type.GetProperty("DoubleProperty")));

            Assert.AreEqual("'murilo'", converter.GetValue(model, type.GetProperty("StringProperty")));

            Assert.AreEqual("16", converter.GetValue(model, type.GetProperty("Int16Property")));
            Assert.AreEqual("32", converter.GetValue(model, type.GetProperty("Int32Property")));
            Assert.AreEqual("64", converter.GetValue(model, type.GetProperty("Int64Property")));

            Assert.AreEqual("16", converter.GetValue(model, type.GetProperty("UInt16Property")));
            Assert.AreEqual("32", converter.GetValue(model, type.GetProperty("UInt32Property")));
            Assert.AreEqual("64", converter.GetValue(model, type.GetProperty("UInt64Property")));

            Assert.AreEqual("null", converter.GetValue(model, type.GetProperty("NullableBool")));
            Assert.AreEqual("null", converter.GetValue(model, type.GetProperty("NullableDateTime")));
            Assert.AreEqual("null", converter.GetValue(model, type.GetProperty("NullableDouble")));
            Assert.AreEqual("null", converter.GetValue(model, type.GetProperty("NullableString")));
            Assert.AreEqual("null", converter.GetValue(model, type.GetProperty("NullableInt64")));
            Assert.AreEqual("null", converter.GetValue(model, type.GetProperty("NullableUInt16")));
            Assert.AreEqual("null", converter.GetValue(model, type.GetProperty("NullableUInt32")));
            Assert.AreEqual("null", converter.GetValue(model, type.GetProperty("NullableUInt64")));

            //Assert.IsTrue(converter.Match(property1));
        }

        [TestMethod]
        public void SimpleQuoteConversion()
        {
            var model = new FakeModel
            {
                StringProperty = "'murilo"
            };
            var type = typeof(FakeModel);
            var property = type.GetProperty("StringProperty");
            var converter = new StringConverter();
            var value = converter.GetValue(model, property);

            Assert.AreEqual("'''murilo'", value);
        }

        [TestMethod]
        public void BooleanConverter()
        {
            var model = new FakeModel()
            {
                BoolProperty = true,
                NullableBool = true
            };
            var type = model.GetType();
            var property1 = model.GetType().GetProperty("BoolProperty");
            var property2 = model.GetType().GetProperty("NullableBool");

            var converter = new BooleanConverter();

            Assert.IsTrue(converter.Match(property1));
            Assert.IsTrue(converter.Match(property2));

            var info1 = converter.GetValue(model, property1);

            Assert.AreEqual("1", info1);

            var info2 = converter.GetValue(model, property2);

            Assert.AreEqual("1", info2);

            model.NullableBool = null;

            var info3 = converter.GetValue(model, property2);
            Assert.AreEqual("null", info3);
        }

        [TestMethod]
        public void StringConverter()
        {
            var model = new FakeModel()
            {
                StringProperty = "murilo",
                NullableString = null
            };
            var type = model.GetType();
            var property1 = model.GetType().GetProperty("StringProperty");
            var property2 = model.GetType().GetProperty("NullableString");

            var converter = new StringConverter();

            Assert.IsTrue(converter.Match(property1));
            Assert.IsTrue(converter.Match(property2));

            var info1 = converter.GetValue(model, property1);

            Assert.AreEqual("'murilo'", info1);

            var info2 = converter.GetValue(model, property2);

            Assert.AreEqual("null", info2);
        }

        [TestMethod]
        public void DatetimeConverter()
        {
            var now = DateTime.Parse("01/02/2000 10:11:12", CultureInfo.InvariantCulture);

            var model = new FakeModel()
            {
                DateTimeProperty = now,
                NullableDateTime = null
            };
            var type = model.GetType();
            var property1 = model.GetType().GetProperty("DateTimeProperty");
            var property2 = model.GetType().GetProperty("NullableDateTime");

            var converter = new DateTimeConverter();

            Assert.IsTrue(converter.Match(property1));
            Assert.IsTrue(converter.Match(property2));

            var info1 = converter.GetValue(model, property1);

            Assert.AreEqual("01/02/2000 10:11:12", info1);

            var info2 = converter.GetValue(model, property2);

            Assert.AreEqual("null", info2);
        }

        [TestMethod]
        public void DoubleConverter()
        {
            var model = new FakeModel()
            {
                DoubleProperty = 10.12d,
                NullableDouble = null
            };
            var type = model.GetType();
            var property1 = model.GetType().GetProperty("DoubleProperty");
            var property2 = model.GetType().GetProperty("NullableDouble");

            var converter = new DoubleConverter();

            Assert.IsTrue(converter.Match(property1));
            Assert.AreEqual("10.12", converter.GetValue(model, property1));
            Assert.AreEqual("null", converter.GetValue(model, property2));
        }
    }
}
