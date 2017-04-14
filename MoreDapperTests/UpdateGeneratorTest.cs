using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoreDapper.Generators;

namespace MoreDapperTests
{
    [TestClass]
    public class UpdateGeneratorTest
    {
        private class Foo
        {
            public int Id { get; set; }
            public string Foo1 { get; set; }
            public string Foo2 { get; set; }
        }

        [TestMethod]
        public void Update()
        {
            var command = UpdateGenerator.Generate(new Foo
            {
                Id = 5,
                Foo1 = "foo1",
                Foo2 = "foo2"
            });

            Assert.AreEqual("UPDATE Foo SET Foo1 = @Foo1, Foo2 = @Foo2 WHERE Id = @Id;", command);
        }
    }
}
