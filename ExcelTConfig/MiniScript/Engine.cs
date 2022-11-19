using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniScript
{
    public abstract class Engine
    {
        protected string[] arguments;
        protected string[] globalValues;
        protected List<string> localValues = new List<string>();
        protected HashSet<string> allScopeLocalValues = new HashSet<string>();
        protected int maxLocalValuess = 0;

        protected string content;
        protected byte[] contentBytes;

        public Engine(string[] globalValues, string[] arguments)
        {
            this.globalValues = globalValues;
            this.arguments = arguments;
        }

        public int IndexArgument(string name)
        {
            return Array.IndexOf(arguments, name);
        }

        public int IndexGlobal(string name)
        {
            return Array.IndexOf(globalValues, name);
        }

        public int IndexLocal(string name)
        {
            return localValues.IndexOf(name);
        }

        public string GetCodePositionInfo(int codeIndex)
        {
            int line = 1;
            for (int it = 0; it <= codeIndex; it++)
            {
                var ch = contentBytes[it];
                if (ch == '\n') line++;
            }
            return $"line:{line}, index:{codeIndex}";
        }

        public void AddLocal(string name, int codePosition)
        {
            if (IndexArgument(name) >= 0) throw new Exception($"local var name [{name}] conflict with argument value, {GetCodePositionInfo(codePosition)}");
            if (IndexLocal(name) >= 0) throw new Exception($"local var name [{name}] conflict, {GetCodePositionInfo(codePosition)}");
            if (localValues.Count > byte.MaxValue) throw new Exception($"too many local vars, {GetCodePositionInfo(codePosition)}");
            localValues.Add(name);
            if (localValues.Count > maxLocalValuess) maxLocalValuess = localValues.Count;
        }

        protected void SetContent(string content)
        {
            this.content = content;
            this.contentBytes = content == null ? null : Encoding.UTF8.GetBytes(content);
        }

        public Scope PushScope()
        {
            var scope = new Scope(localValues.Count, this);
            return scope;
        }

        private void PopScope(int fromIndex)
        {
            localValues.RemoveRange(fromIndex, localValues.Count - fromIndex);
        }

        public struct Scope : IDisposable
        {
            private int fromIndex;
            private Engine engine;

            public Scope(int fromIndex, Engine engine)
            {
                this.fromIndex = fromIndex;
                this.engine = engine;
            }

            public void Dispose()
            {
                engine.PopScope(fromIndex);
            }
        }
    }
}
