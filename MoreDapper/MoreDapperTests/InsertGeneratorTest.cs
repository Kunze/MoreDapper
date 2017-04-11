using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoreDapper;
using System.Collections.Generic;
using System.Diagnostics;

namespace UnitTestProject1
{
    [TestClass]
    public class InsertGeneratorTest
    {
        public class Bar
        {
            public string Foo1 { get; set; }
            public string Foo2 { get; set; }
            public string Foo3 { get; set; }
            public bool Foo4 { get; set; }
            public int? Foo5 { get; set; }
            public decimal Foo6 { get; set; }
        }

        [TestMethod]
        public void GenerateInsertCommand()
        {
            var insertGenerator = new InsertGenerator();
            var command = insertGenerator.GenerateSingle(new Bar
            {
                Foo1 = "uma string",
                Foo2 = "outra string",
                Foo3 = "mais uma string",
                Foo4 = true,
                Foo5 = null,
                Foo6 = 15.1m
            });

            Assert.AreEqual("INSERT INTO Bar (Foo1, Foo2, Foo3, Foo4, Foo5, Foo6) VALUES (@Foo1, @Foo2, @Foo3, @Foo4, @Foo5, @Foo6);", command);
        }

        [TestMethod]
        public void GenerateMultipleInsertTyped()
        {
            var commandGenerator = new InsertGenerator();

            var commands = commandGenerator.GenerateMultiple(new List<Bar>
            {
                new Bar {
                    Foo1 = "uma string",
                    Foo2 = "outra string",
                    Foo3 = "mais uma string",
                    Foo4 = true,
                    Foo5 = null,
                    Foo6 = 15.1m
                },
                new Bar {
                    Foo1 = "1",
                    Foo2 = "2",
                    Foo3 = "3",
                    Foo4 = false,
                    Foo5 = null,
                    Foo6 = 20
                },
            });

            var value = "INSERT INTO Bar (Foo1, Foo2, Foo3, Foo4, Foo5, Foo6) VALUES ('uma string', 'outra string', 'mais uma string', 1, null, 15.1), ('1', '2', '3', 0, null, 20);";

            Assert.AreEqual(value, commands);
        }

        [TestMethod]
        public void GeneratedFullCommand()
        {
            var commandGenerator = new InsertGenerator();
            var insert = "Insert into table values";
            var values = "(@Foo1, @Foo2, 'murilo', 10, 11.12, @Foo3, @Foo4, @Foo5, @Foo6)";

            var command = commandGenerator.GenerateMultiple(insert, values, new List<Bar>
            {
                new Bar {
                    Foo1 = "uma string",
                    Foo2 = "outra string",
                    Foo3 = "mais uma string",
                    Foo4 = true,
                    Foo5 = null,
                    Foo6 = 15.1m
                },
                new Bar {
                    Foo1 = "1",
                    Foo2 = "2",
                    Foo3 = "3",
                    Foo4 = false,
                    Foo5 = null,
                    Foo6 = 20
                },
            });

            Assert.AreEqual("Insert into table values ('uma string', 'outra string', 'murilo', 10, 11.12, 'mais uma string', 1, null, 15.1), ('1', '2', 'murilo', 10, 11.12, '3', 0, null, 20);", command);
        }
    }
}
