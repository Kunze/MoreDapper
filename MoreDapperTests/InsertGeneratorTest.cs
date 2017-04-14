using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoreDapper.Generators;
using MoreDapper.Exceptions;
using System.Collections.Generic;

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
            public double? Foo7 { get; set; }
        }

        [TestMethod]
        public void GenerateInsertCommand()
        {
            var command = InsertGenerator.GenerateSingle(new Bar
            {
                Foo1 = "uma string",
                Foo2 = "outra string",
                Foo3 = "mais uma string",
                Foo4 = true,
                Foo5 = null,
                Foo6 = 15.1m
            });

            Assert.AreEqual("INSERT INTO Bar (Foo1, Foo2, Foo3, Foo4, Foo5, Foo6, Foo7) VALUES (@Foo1, @Foo2, @Foo3, @Foo4, @Foo5, @Foo6, @Foo7);", command);
        }

        [TestMethod]
        public void GenerateMultipleInsertTyped()
        {
            var commands = InsertGenerator.GenerateMultiple(new List<Bar>
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
            }, 1000, 4194304);

            var value = "INSERT INTO Bar (Foo1, Foo2, Foo3, Foo4, Foo5, Foo6, Foo7) VALUES ('uma string', 'outra string', 'mais uma string', 1, null, 15.1, null), ('1', '2', '3', 0, null, 20, null);";

            Assert.AreEqual(value, commands[0]);
        }

        [TestMethod]
        public void GeneratedFullCommand()
        {
            var insert = "Insert into table values";
            var values = "(@Foo1, @Foo2, 'murilo', 10, 11.12, @Foo3, @Foo4, @Foo5, @Foo6, @Foo7)";

            var commands = InsertGenerator.GenerateMultiple(insert, values, new List<Bar>
            {
                new Bar {
                    Foo1 = "uma string",
                    Foo2 = "outra string",
                    Foo3 = "mais uma string",
                    Foo4 = true,
                    Foo5 = null,
                    Foo6 = 15.1m,
                    Foo7 = 10.1d
                },
                new Bar {
                    Foo1 = "1",
                    Foo2 = "2",
                    Foo3 = "3",
                    Foo4 = false,
                    Foo5 = null,
                    Foo6 = 20,
                    Foo7 = 10.1d
                },
            }, 1000, 4194304);

            Assert.AreEqual("Insert into table values ('uma string', 'outra string', 'murilo', 10, 11.12, 'mais uma string', 1, null, 15.1, 10.1), ('1', '2', 'murilo', 10, 11.12, '3', 0, null, 20, 10.1);", commands[0]);
        }

        [TestMethod]
        public void TestMaxItens()
        {
            var barList = new List<Bar>();
            for (int i = 0; i < 101; i++)
            {
                barList.Add(new Bar
                {
                    Foo1 = "uma string",
                    Foo2 = "outra string",
                    Foo3 = "mais uma string",
                    Foo4 = true,
                    Foo5 = null,
                    Foo6 = 15.1m
                });
            }

            var commands = InsertGenerator.GenerateMultiple(barList, 10, 4194304);

            Assert.AreEqual(11, commands.Count);
        }

        [TestMethod]
        public void TestMaxPacketSize()
        {
            var barList = new List<Bar>();
            for (int i = 0; i < 101; i++)
            {
                barList.Add(new Bar
                {
                    Foo1 = $"uma string {i}",
                    Foo2 = "outra string",
                    Foo3 = "mais uma string",
                    Foo4 = true,
                    Foo5 = null,
                    Foo6 = 15.1m
                });
            }

            var commands = InsertGenerator.GenerateMultiple(barList, 1000, 250);

            Assert.AreEqual(51, commands.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidSqlParameterException))]
        public void InvalidProperty()
        {
            var insert = "Insert into table values";
            var values = "(@Foo1, @FooX)";

            var commands = InsertGenerator.GenerateMultiple(insert, values, new List<Bar>
            {
                new Bar()
            }, 1000, 4194304);

            Assert.AreEqual("Insert into table values ('uma string', 'outra string', 'murilo', 10, 11.12, 'mais uma string', 1, null, 15.1, 10.1), ('1', '2', 'murilo', 10, 11.12, '3', 0, null, 20, 10.1);", commands[0]);
        }
        //[TestMethod]
        //public void TestPerformance()
        //{
        //    var barList = new List<Bar>();
        //    for (int i = 0; i < 100000; i++)
        //    {
        //        barList.Add(new Bar
        //        {
        //            Foo1 = "uma string",
        //            Foo2 = "outra string",
        //            Foo3 = "mais uma string",
        //            Foo4 = true,
        //            Foo5 = null,
        //            Foo6 = 15.1m
        //        });
        //    }
        //    var commandGenerator = new InsertGenerator();

        //    var stopwatch = new Stopwatch();
        //    stopwatch.Start();
        //    var commands = commandGenerator.GenerateMultiple(barList, 10, 4194304);
        //    var time = stopwatch.ElapsedMilliseconds;
        //}
    }
}
