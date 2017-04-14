using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoreDapper.Generators;

namespace MoreDapperTests
{
    [TestClass]
    public class DeleteGeneratorTest
    {
        private class Foo
        {
            public int Id { get; set; }
            public string Foo1 { get; set; }
            public string Foo2 { get; set; }
        }

        [TestMethod]
        public void Delete()
        {
            var command = DeleteGenerator.Generate(new Foo
            {
                Id = 5,
                Foo1 = "foo1",
                Foo2 = "foo2"
            });

            Assert.AreEqual("DELETE FROM Foo WHERE Id = @Id;", command);
        }
    }
}
