# MoreDapper
Extension methods for Dapper

```cs
InsertSingle<T>(this IDbConnection connection, T param, string table = null, int? commandTimeout = null, IDbTransaction transaction = null)

///More than 10.000 inserts per second
InsertMultiple<T>(this IDbConnection connection, IList<T> list, string table = null, int maxItens = 1000, int maxPacketSize = 4194304, int? commandTimeout = null, IDbTransaction transaction = null)
InsertMultiple<T>(this IDbConnection connection, string insert, string values, IList<T> list, int maxItens = 1000, int maxPacketSize = 4194304, int? commandTimeout = null, IDbTransaction transaction = null)

UpdateSingle<T>(this IDbConnection connection, T param, string table = null, int? commandTimeout = null, IDbTransaction transaction = null)

DeleteSingle<T>(this IDbConnection connection, T param, string table = null, int? commandTimeout = null, IDbTransaction transaction = null)
```

```cs
using MoreDapper;

public class Bar
{
    public int Id { get; set; }
    public string Foo1 { get; set; }
    public int Foo2 { get; set; }
    public bool Foo3 { get; set; }
    public float Foo4 { get; set; }
    public decimal Foo5 { get; set; }
}

class Program
{
    static void Main(string[] args)
    {
        using (var connection = new SqlConnection("your_connectionstring"))
        {
            var totalInserts = connection.InsertSingle(new Bar
            {
                Foo1 = "foo1",
                Foo2 = 5,
                Foo3 = true,
                Foo4 = 10.12f,
                Foo5 = 13.458m
            });

            var totalUpdates = connection.UpdateSingle(new Bar
            {
                Id = 3,
                Foo1 = "foo1 updated",
                Foo2 = 15,
                Foo3 = false,
                Foo4 = 10.99f,
                Foo5 = 13.999m
            });

            var totalDeletes = connection.DeleteSingle(new Bar
            {
                Id = 1
            });

            var list = new List<Bar>();
            for (int i = 0; i < 100000; i++)
            {
                list.Add(new Bar
                {
                    Foo1 = $"{i}",
                    Foo2 = i,
                    Foo3 = false,
                    Foo4 = i,
                    Foo5 = i
                });
            }

            var totalMultipleInserts1 = connection.InsertMultiple("INSERT INTO Bar (Foo1, Foo2, Foo3, Foo4, Foo5) VALUES", "(@Foo1, @Foo2, @Foo3, @Foo4, @Foo5)", list);

            //or

            var totalMultipleInserts2 = connection.InsertMultiple(list);
        }
    }
}
```