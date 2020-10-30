using System;
using System.Threading.Tasks;
using Sharpie.Writer;

namespace Sharpie
{
    public readonly struct ForEachLoop
    {
        public readonly string Item;
        public readonly string Collection;
        public readonly Func<BodyWriter, Task> Body;

        public ForEachLoop(string item, string collection, Func<BodyWriter, Task> body)
        {
            Item = item;
            Collection = collection;
            Body = body;
        }

        public ForEachLoop(string item, string collection, string body) : this(item, collection, StringHelper.StringToBodyWriter(body)) { }

        public ForEachLoop(string collection, Func<BodyWriter, Task> body) : this("var item", collection, body) { }
        public ForEachLoop(string collection, string body) : this(collection, StringHelper.StringToBodyWriter(body)) { }
    }
}
