using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace PEG
{
    /// <summary>
    /// types of trees
    /// </summary>
    public enum Tag : byte
    {
        TChar = 0,  /* 'n' = char */
        TSet,  /* the set is stored in next CHARSETSIZE bytes */
        TAny,
        TTrue,
        TFalse,
        TRep,  /* 'sib1'* */
        TSeq,  /* 'sib1' 'sib2' */
        TChoice,  /* 'sib1' / 'sib2' */
        TNot,  /* !'sib1' */
        TAnd,  /* &'sib1' */
        TCall,  /* ktable[key] is rule's key; 'sib2' is rule being called */
        TOpenCall,  /* ktable[key] is rule's key */
        TRule,  /* ktable[key] is rule's key (but key == 0 for unused rules);
             'sib1' is rule's pattern;
             'sib2' is next rule; 'cap' is rule's sequential number */
        TGrammar,  /* 'sib1' is initial (and first) rule */
        TBehind,  /* 'sib1' is pattern, 'n' is how much to go back */
        TCapture,  /* captures: 'cap' is kind of capture (enum 'CapKind');
                ktable[key] is Lua value associated with capture;
                'sib1' is capture body */
        TRunTime  /* run-time capture: 'key' is Lua function;
               'sib1' is capture body */
    }

    public unsafe struct TreeNode
    {
        public Tag tag;
        public CapKind cap;  /* kind of capture (if it is a capture) */
        public ushort key;  /* key in ktable for Lua data (0 if no key) */
        public int u;

        public int ps { get => u; set => u = value; } /* occasional second child */
        public int n { get => u; set => u = value; }  /* occasional counter */

        public static TreeNode* sib1(TreeNode* t)
        {
            return t + 1;
        }

        public static TreeNode* sib2(TreeNode* t)
        {
            return t + t->ps;
        }

        public static byte* treebuffer(TreeNode* t)
        {
            return (byte*)(t + 1);
        }
    }

    public unsafe class Pattern
    {
        internal Instruction[] code;
        internal int codesize => code.Length;
        internal TreeNode[] treeNodes;
        internal object[] kTable;

        public static Pattern operator+ (Pattern pattern1, Pattern pattern2)=> LPEG.lp_choice(pattern1, pattern2);
        public static Pattern operator- (Pattern pattern1, Pattern pattern2)=> LPEG.lp_sub(pattern1, pattern2);
        public static Pattern operator-(Pattern pattern1)=> LPEG.lp_not(pattern1);
        public static Pattern operator *(Pattern pattern1, Pattern pattern2) => LPEG.lp_seq(pattern1, pattern2);
        public static Pattern operator/ (Pattern pattern1, CaptureFunction captureFunction)=> LPEG.lp_functioncapture(pattern1, captureFunction);

        public Pattern Repeat(int n)=> LPEG.lp_star(this, n);
        public Pattern SimpleCapture()=> LPEG.lp_simplecapture(this);
        public Pattern TableCapture()=> LPEG.lp_tablecapture(this);
        public Pattern GroupCapture(object groupName)=> LPEG.lp_groupcapture(this, groupName);
        public Pattern MatchTime(MatchTimeFunction matchTimeFunction)=> LPEG.lp_matchtime(this, matchTimeFunction);

        public object[] Match(string content)=> LPEG.lp_match(this, content);
    }

    public class Grammar
    {
        public string firstRuler;
        public Dictionary<string, Pattern> grammars; 
    }

    public struct Inst
    {
        public Opcode code;
        public byte aux;
        public short key;

        public override string ToString()
        {
            return $"{code}, aux: {(char)aux}, key: {key}";
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    unsafe struct Charset
    {
        public const int CharsetSize = 32;
        public static readonly int CharsetInstSize = ((CharsetSize + sizeof(Instruction) - 1) / sizeof(Instruction) + 1);

        private ulong L0, L1, L2, L3;

        public static Charset fullSet = new Charset
        {
            L0 = ulong.MaxValue, L1 = ulong.MaxValue, L2 = ulong.MaxValue, L3 = ulong.MaxValue,
        };

        public byte this[int index]
        {
            get
            {
                int rShift = (index % 8) * 8;

                ulong ret;
                switch(index / 8)
                {
                    case 0: ret = L0 >> rShift; break;
                    case 1: ret = L1 >> rShift; break;
                    case 2: ret = L2 >> rShift; break;
                    case 3: ret = L3 >> rShift; break;
                    default: throw new Exception("should not reach here");
                }
                return (byte)(ret & 0xff);
            }

            set
            {
                int lShift = (index % 8) * 8;
                ulong bitOrValue = ((ulong)value) << lShift;
                ulong bitAndValue = ~(0xffuL << lShift);

                switch(index / 8)
                {
                    case 0: L0 &= bitAndValue; L0 |= bitOrValue; break;
                    case 1: L1 &= bitAndValue; L1 |= bitOrValue; break;
                    case 2: L2 &= bitAndValue; L2 |= bitOrValue; break;
                    case 3: L3 &= bitAndValue; L3 |= bitOrValue; break;
                    default: throw new Exception("should not reach here");
                }
            }
        }
    }

    [StructLayout(LayoutKind.Explicit)]
    public unsafe struct Instruction
    {
        [FieldOffset(0)] public Inst i;
        [FieldOffset(0)] public int offset;
        [FieldOffset(0)] public int buff;

        public byte this[int index]
        {
            get
            {
                return (byte)((buff >> (8 * index)) & 0xff);
            }
        }

        public override string ToString()
        {
            return i.ToString();
        }

        public const int NOINST = -1;
    }

    public enum CapKind : byte
    {
        Cclose,  /* not used in trees */
        Cposition,
        Cconst,  /* ktable[key] is Lua constant */
        Cbackref,  /* ktable[key] is "name" of group to get capture */
        Carg,  /* 'key' is arg's number */
        Csimple,  /* next node is pattern */
        Ctable,  /* next node is pattern */
        Cfunction,  /* ktable[key] is function; next node is pattern */
        Cquery,  /* ktable[key] is table; next node is pattern */
        Cstring,  /* ktable[key] is string; next node is pattern */
        Cnum,  /* numbered capture; 'key' is number of value to return */
        Csubst,  /* substitution capture; next node is pattern */
        Cfold,  /* ktable[key] is function; next node is pattern */
        Cruntime,  /* not used in trees (is uses another type for tree) */
        Cgroup  /* ktable[key] is group's "name" */
    }

    public class CompileState
    {
        public Pattern pattern;
        public int ncode; /* next position in p->code to be filled */
    }

    /* Virtual Machine's instructions */
    public enum Opcode : byte
    {
        IAny, /* if no char, fail */
        IChar,  /* if char != aux, fail */
        ISet,  /* if char not in buff, fail */
        ITestAny,  /* in no char, jump to 'offset' */
        ITestChar,  /* if char != aux, jump to 'offset' */
        ITestSet,  /* if char not in buff, jump to 'offset' */
        ISpan,  /* read a span of chars in buff */
        IBehind,  /* walk back 'aux' characters (fail if not possible) */
        IRet,  /* return from a rule */
        IEnd,  /* end of pattern */
        IChoice,  /* stack a choice; next fail will jump to 'offset' */
        IJmp,  /* jump to 'offset' */
        ICall,  /* call rule at 'offset' */
        IOpenCall,  /* call rule number 'key' (must be closed to a ICall) */
        ICommit,  /* pop choice and jump to 'offset' */
        IPartialCommit,  /* update top choice to current position and jump */
        IBackCommit,  /* "fails" but jump to its own 'offset' */
        IFailTwice,  /* pop one choice and then fail */
        IFail,  /* go back to saved state on choice and jump to saved offset */
        IGiveup,  /* internal use */
        IFullCapture,  /* complete capture of last 'off' chars */
        IOpenCapture,  /* start a capture */
        ICloseCapture,
        ICloseRunTime
    }

    public static unsafe partial class LPEG
    {
        private static void assert(bool v)
        {
            if (!v) throw new Exception();
        }

        private static Pattern NewTree(int nodeCount)
        {
            return new Pattern { treeNodes = new TreeNode[nodeCount] };
        }

        private static Pattern NewLeaf(Tag tag)
        {
            var p = NewTree(1);
            p.treeNodes[0].tag = tag;
            return p;
        }

        private static int bytes2slots(int n) => ((n) - 1) / sizeof(TreeNode) + 1;
        private static Pattern newcharset()
        {
            var p = NewTree(bytes2slots(Charset.CharsetSize) + 1);
            fixed(TreeNode* tree = p.treeNodes)
            {
                tree->tag = Tag.TSet;
                for (int i = 0; i < Charset.CharsetSize; i++) TreeNode.treebuffer(tree)[i] = 0;
            }
            return p;
        }

        /*
        ** When joining 'ktables', constants from one of the subpatterns must
        ** be renumbered; 'correctkeys' corrects their indices (adding 'n'
        ** to each of them)
        */
        static void correctkeys(TreeNode* tree, int n)
        {
            if (n == 0) return;  /* no correction? */
            tailcall:
            switch (tree->tag)
            {
                case Tag.TOpenCall:
                case Tag.TCall:
                case Tag.TRunTime:
                case Tag.TRule:
                    {
                        if (tree->key > 0)
                            tree->key = (ushort)(tree->key + n);
                        break;
                    }
                case Tag.TCapture:
                    {
                        if (tree->key > 0 && tree->cap != CapKind.Carg && tree->cap != CapKind.Cnum)
                            tree->key = (ushort)(tree->key + n);
                        break;
                    }
                default: break;
            }
            switch (numsiblings[(int)tree->tag])
            {
                case 1:  /* correctkeys(sib1(tree), n); */
                    tree = TreeNode.sib1(tree); goto tailcall;
                case 2:
                    correctkeys(TreeNode.sib1(tree), n);
                    tree = TreeNode.sib2(tree); goto tailcall;  /* correctkeys(sib2(tree), n); */
                default: assert(numsiblings[(int)tree->tag] == 0); break;
            }
        }

        static void mergektable(Pattern pattern1, Pattern pattern2, TreeNode* stree)
        {
            int n1 = pattern1.kTable == null ? 0 : pattern1.kTable.Length;
            if (n1 == 0) return;
            int n2 = pattern2.kTable == null ? 0 : pattern2.kTable.Length;

            if (pattern2.kTable == null) pattern2.kTable = new object[n1 + n2];
            else Array.Resize(ref pattern2.kTable, n1 + n2);
            Array.Copy(pattern1.kTable, 0, pattern2.kTable, n2, n1);

            correctkeys(stree, n2);
        }

        static int addtoktable(Pattern pattern, object kValue)
        {
            if (pattern.kTable == null) pattern.kTable = new object[] { kValue };
            else
            {
                Array.Resize(ref pattern.kTable, pattern.kTable.Length + 1);
                pattern.kTable[pattern.kTable.Length - 1] = kValue;
            }

            return pattern.kTable.Length;
        }

        /*
        ** Create a new 'ktable' to the pattern at the top of the stack, adding
        ** all elements from pattern 'p' (if not 0) plus element 'idx' to it.
        ** Return index of new element.
        */
        static int addtonewktable(Pattern pattern, Pattern pattern1, object kValue)
        {
            if (pattern1 != null)
                mergektable(pattern1, pattern, null);
            return addtoktable(pattern, kValue);
        }

        /*
        ** copy 'ktable' of element 'idx' to new tree (on top of stack)
        */
        static void copyktable(Pattern pattern, Pattern pattern1)
        {
            pattern.kTable = pattern1.kTable;
        }

        static void joinktables(Pattern pattern, Pattern pattern1, Pattern pattern2, TreeNode* tree2)
        {
            int n1 = pattern1.kTable == null ? 0 : pattern1.kTable.Length;
            int n2 = pattern2.kTable == null ? 0 : pattern2.kTable.Length;

            if (n1 == 0 && n2 == 0) return;
            else if (n2 == 0 || pattern1.kTable == pattern2.kTable) pattern.kTable = pattern1.kTable;
            else if(n1 == 0) pattern.kTable = pattern2.kTable;
            else
            {
                var ktable = new object[n1 + n2];
                if (n1 > 0) Array.Copy(pattern1.kTable, ktable, n1);
                if (n2 > 0) Array.Copy(pattern2.kTable, 0, ktable, n1, n2);
                pattern.kTable = ktable;
                correctkeys(tree2, n1);  /* correction for indices from p2 */
            }
        }

        /*
        ** create a new tree, whith a new root and one sibling.
        ** Sibling must be on the Lua stack, at index 1.
        */
        static Pattern newroot1sib(Pattern pattern1, Tag tag)
        {
            fixed(TreeNode* tree1 = pattern1.treeNodes)
            {
                var pattern = NewTree(1 + pattern1.treeNodes.Length);
                fixed(TreeNode* tree = pattern.treeNodes)
                {
                    tree->tag = tag;
                    for (int it = 0; it < pattern1.treeNodes.Length; it++)
                    {
                        *(TreeNode.sib1(tree) + it) = *(tree1 + it);
                    }
                }
                return pattern;
            }
        }

        /*
        ** create a new tree, whith a new root and 2 siblings.
        ** Siblings must be on the Lua stack, first one at index 1.
        */
        static Pattern newroot2sib(Pattern pattern1, Pattern pattern2, Tag tag)
        {
            fixed(TreeNode* tree1 = pattern1.treeNodes)
            {
                fixed(TreeNode* tree2 = pattern2.treeNodes)
                {
                    var pattern = NewTree(1 + pattern1.treeNodes.Length + pattern2.treeNodes.Length);
                    fixed(TreeNode* tree = pattern.treeNodes)
                    {
                        tree->tag = tag;
                        tree->ps = 1 + pattern1.treeNodes.Length;
                        for(int it = 0; it < pattern1.treeNodes.Length; it++)
                        {
                            *(TreeNode.sib1(tree) + it) = *(tree1 + it);
                        }
                        for (int it = 0; it < pattern2.treeNodes.Length; it++)
                        {
                            *(TreeNode.sib2(tree) + it) = *(tree2 + it);
                        }
                        joinktables(pattern, pattern1, pattern2, TreeNode.sib2(tree));
                        return pattern;
                    }
                }
            }
        }

        /*
        ** sequence operator; optimizations:
        ** false x => false, x true => x, true x => x
        ** (cannot do x . false => false because x may have runtime captures)
        */
        internal static Pattern lp_seq(Pattern pattern1, Pattern pattern2)
        {
            var tree1 = pattern1.treeNodes[0];
            var tree2 = pattern2.treeNodes[0];
            if (tree1.tag == Tag.TFalse || tree2.tag == Tag.TTrue) return pattern1;
            else if (tree1.tag == Tag.TTrue) return pattern2;
            else return newroot2sib(pattern1, pattern2, Tag.TSeq);
        }

        /*
        ** choice operator; optimizations:
        ** charset / charset => charset
        ** true / x => true, x / false => x, false / x => x
        ** (x / true is not equivalent to true)
        */
        internal static Pattern lp_choice(Pattern pattern1, Pattern pattern2)
        {
            Charset st1 = default, st2 = default;
            fixed(TreeNode* t1 = pattern1.treeNodes)
            {
                fixed(TreeNode* t2 = pattern2.treeNodes)
                {
                    if (tocharset(t1, ref st1) != 0 && tocharset(t2, ref st2) != 0)
                    {
                        var pattern = newcharset();
                        fixed(TreeNode* t = pattern.treeNodes)
                        {
                            for (int i = 0; i < Charset.CharsetSize; i++) TreeNode.treebuffer(t)[i] = (byte)(st1[i] | st2[i]);
                        }
                        return pattern;
                    }
                    else if (nofail(t1) != 0 || t2->tag == Tag.TFalse)
                        return pattern1;  /* true / x => true, x / false => x */
                    else if (t1->tag == Tag.TFalse)
                        return pattern2;  /* false / x => x */
                    else
                        return newroot2sib(pattern1, pattern2, Tag.TChoice);
                }
            }
        }

        /*
        ** add to tree a sequence where first sibling is 'sib' (with size
        ** 'sibsize'); returns position for second sibling
        */
        static TreeNode* seqaux(TreeNode* tree, TreeNode* sib, int sibsize)
        {
            tree->tag = Tag.TSeq; tree->ps = sibsize + 1;
            for(int it = 0; it < sibsize; it++)
            {
                *(TreeNode.sib1(tree) + it) = *(sib + it);
            }
            return TreeNode.sib2(tree);
        }

        /*
        ** p^n
        */
        internal static Pattern lp_star(Pattern pattern1, int n)
        {
            Pattern pattern;
            fixed(TreeNode* tree1 = pattern1.treeNodes)
            {
                int size1 = pattern1.treeNodes.Length;

                if (n >= 0)
                {  /* seq tree1 (seq tree1 ... (seq tree1 (rep tree1))) */
                    pattern = NewTree((n + 1) * (size1 + 1));
                    fixed(TreeNode* tree = pattern.treeNodes)
                    {
                        if (nullable(tree1) != 0)
                            throw new Exception("loop body may accept empty string");

                        var tTree = tree;

                        while (n-- != 0)  /* repeat 'n' times */
                            tTree = seqaux(tTree, tree1, size1);
                        tTree->tag = Tag.TRep;
                        for(int it = 0; it < size1; it++)
                        {
                            *(TreeNode.sib1(tTree) + it) = *(tree1 + it);
                        }
                    }
                }
                else
                {  /* choice (seq tree1 ... choice tree1 true ...) true */
                    n = -n;
                    pattern = NewTree(n * (size1 + 3) - 1);
                    /* size = (choice + seq + tree1 + true) * n, but the last has no seq */
                    fixed(TreeNode* tree = pattern.treeNodes)
                    {
                        var tTree = tree;
                        for (; n > 1; n--)
                        {  /* repeat (n - 1) times */
                            tTree->tag = Tag.TChoice; tTree->ps = n * (size1 + 3) - 2;
                            TreeNode.sib2(tTree)->tag = Tag.TTrue;
                            tTree = TreeNode.sib1(tTree);
                            tTree = seqaux(tTree, tree1, size1);
                        }
                        tTree->tag = Tag.TChoice; tTree->ps = size1 + 1;
                        TreeNode.sib2(tTree)->tag = Tag.TTrue;
                        for(int it =0; it < size1; it++)
                        {
                            *(TreeNode.sib1(tTree) + it) = *(tree1 + it);
                        }
                    }
                }
                pattern.kTable = pattern1.kTable;
                return pattern;
            }
        }

        public static Pattern P(bool v)
        {
            return v ? NewLeaf(Tag.TTrue) : NewLeaf(Tag.TFalse);
        }

        public static Pattern P(int n)
        {
            if (n == 0) return NewLeaf(Tag.TTrue);
            else
            {
                TreeNode* nd;
                Pattern pattern = n > 0 ? NewTree(2 * n - 1) : NewTree(-2 * n);
                fixed (TreeNode* tree = pattern.treeNodes)
                {
                    if (n > 0)
                    {
                        nd = tree;
                    }
                    else
                    {  /* negative: code it as !(-n) */
                        n = -n;
                        tree->tag = Tag.TNot;
                        nd = TreeNode.sib1(tree);
                    }
                    fillseq(nd, Tag.TAny, n, null);  /* sequence of 'n' any's */
                }
                return pattern;
            }
        }

        public static Pattern P(string text)
        {
            assert(text != null);
            Pattern pattern;
            if(text.Length == 0)
            {
                pattern = NewLeaf(Tag.TTrue);
            }
            else
            {
                var bytes = Encoding.UTF8.GetBytes(text);
                fixed (byte* s = bytes)
                {
                    int sLen = bytes.Length;
                    pattern = NewTree(2 * (sLen - 1) + 1);
                    fixed (TreeNode* tree = pattern.treeNodes)
                    {
                        fillseq(tree, Tag.TChar, sLen, s);
                    }
                }
            }
            return pattern;
        }

        /*
        ** Build a sequence of 'n' nodes, each with tag 'tag' and 'u.n' got
        ** from the array 's' (or 0 if array is NULL). (TSeq is binary, so it
        ** must build a sequence of sequence of sequence...)
        */
        public static void fillseq(TreeNode* tree, Tag tag, int n, byte* s)
        {
            int i;
            for (i = 0; i < n - 1; i++)
            {  /* initial n-1 copies of Seq tag; Seq ... */
                tree->tag = Tag.TSeq;
                tree->u = 2;
                TreeNode.sib1(tree)->tag = tag;
                TreeNode.sib1(tree)->n = s != null ? s[i] : 0;
                tree = TreeNode.sib2(tree);
            }
            tree->tag = tag;  /* last one does not need TSeq */
            tree->n = s != null ? s[i] : 0;
        }

        public static Pattern P(Grammar grammer)
        {
            return newgrammar(grammer);
        }

        public static Pattern P(MatchTimeFunction matchTimeFunction)
        {
            var pattern = NewTree(2);

            fixed (TreeNode* tree = pattern.treeNodes)
            {
                tree->tag = Tag.TRunTime;
                tree->key = (ushort)addtonewktable(pattern, null, matchTimeFunction);
                TreeNode.sib1(tree)->tag = Tag.TTrue;
            }

            return pattern;
        }

        public static Pattern R(string text)
        {
            int arg;
            Pattern pattern = newcharset();
            assert(!string.IsNullOrEmpty(text) && text.Length % 2 == 0);
            fixed(TreeNode* tree = pattern.treeNodes)
            {
                int c;
                for (arg = 0; arg < text.Length; arg += 2)
                {
                    for (c = (byte)text[arg]; c <= (byte)text[arg + 1]; c++)
                        setchar(TreeNode.treebuffer(tree), c);
                }
            }
            return pattern;
        }

        public static Pattern S(string text)
        {
            assert(text != null);

            var pattern = newcharset();
            fixed (TreeNode* tree = pattern.treeNodes)
            {
                foreach (var ch in text) setchar(TreeNode.treebuffer(tree), ch);
            }
                
            return pattern;
        }

        /*
        ** Fix a TOpenCall into a TCall node, using table 'postable' to
        ** translate a key to its rule address in the tree. Raises an
        ** error if key does not exist.
        */
        static void fixonecall(Pattern pattern, Dictionary<string, int> postable, TreeNode* g, TreeNode* t)
        {
            var key = pattern.kTable[t->key - 1];
            int n;
            postable.TryGetValue((string)key, out n);

            if (n == 0)
            {  /* no position? */
                throw new Exception($"rule '{key}' undefined in given grammar");
            }
            t->tag = Tag.TCall;
            t->ps = n - (int)(t - g);  /* position relative to node */
            assert(TreeNode.sib2(t)->tag == Tag.TRule);
            TreeNode.sib2(t)->key = t->key;  /* fix rule's key */
        }

        /*
        ** Make final adjustments in a tree. Fix open calls in tree 't',
        ** making them refer to their respective rules or raising appropriate
        ** errors (if not inside a grammar). Correct associativity of associative
        ** constructions (making them right associative). Assume that tree's
        ** ktable is at the top of the stack (for error messages).
        */
        static void finalfix(Pattern pattern, Dictionary<string, int> postable, TreeNode* g, TreeNode* t)
        {
            tailcall:
            switch (t->tag)
            {
                case Tag.TGrammar:  /* subgrammars were already fixed */
                    return;
                case Tag.TOpenCall:
                    {
                        if (g != null)  /* inside a grammar? */
                            fixonecall(pattern, postable, g, t);
                        else
                        {  /* open call outside grammar */

                            throw new Exception($"rule '{pattern.kTable[t->key - 1]}' used outside a grammar");
                        }
                        break;
                    }
                case Tag.TSeq:
                case Tag.TChoice:
                    correctassociativity(t);
                    break;
            }
            switch (numsiblings[(int)t->tag])
            {
                case 1: /* finalfix(L, postable, g, sib1(t)); */
                    t = TreeNode.sib1(t); goto tailcall;
                case 2:
                    finalfix(pattern, postable, g, TreeNode.sib1(t));
                    t = TreeNode.sib2(t); goto tailcall;  /* finalfix(L, postable, g, sib2(t)); */
                default: assert(numsiblings[(int)t->tag] == 0); break;
            }
        }

        /*
        ** Compile a pattern
        */
        public static Instruction[] prepcompile(Pattern p)
        {
            fixed(TreeNode* tree  = p.treeNodes)
            {
                finalfix(p, null, null, tree);
                return compile(p, tree);
            }
            
        }

        private static Instruction[] compile(Pattern p, TreeNode* tree)
        {
            CompileState compst = new CompileState();
            compst.pattern = p; compst.ncode = 0; ;
            realloccode(p, 2);  /* minimum initial size */
            codegen(compst, tree, 0, Instruction.NOINST, ref Charset.fullSet);
            addinstruction(compst, Opcode.IEnd, 0);
            realloccode(p, compst.ncode);  /* set final size */
            peephole(compst);
            return p.code;
        }

        private static void codegen(CompileState compst, TreeNode* tree, int opt, int tt, ref Charset fl)
        {
            tailcall:
            switch(tree->tag)
            {
                case Tag.TChar: codechar(compst, tree->n, tt); break;
                case Tag.TAny: addinstruction(compst, Opcode.IAny, 0); break;
                case Tag.TSet:
                    {
                        var buffer = TreeNode.treebuffer(tree);
                        var charset = *(Charset*)buffer;
                        codecharset(compst, ref charset, tt);
                    }
                    break;
                case Tag.TTrue: break;
                case Tag.TFalse: addinstruction(compst, Opcode.IFail, 0); break;
                case Tag.TChoice: codechoice(compst, TreeNode.sib1(tree), TreeNode.sib2(tree), opt, ref fl); break;
                case Tag.TRep: coderep(compst, TreeNode.sib1(tree), opt, ref fl); break;
                case Tag.TBehind: codebehind(compst, tree); break;
                case Tag.TNot: codenot(compst, TreeNode.sib1(tree)); break;
                case Tag.TAnd: codeand(compst, TreeNode.sib1(tree), tt); break;
                case Tag.TCapture: codecapture(compst, tree, tt, ref fl); break;
                case Tag.TRunTime: coderuntime(compst, tree, tt); break;
                case Tag.TGrammar: codegrammar(compst, tree); break;
                case Tag.TCall: codecall(compst, tree); break;
                case Tag.TSeq:
                    {
                        tt = codeseq1(compst, TreeNode.sib1(tree), TreeNode.sib2(tree), tt, ref fl);  /* code 'p1' */
                                                                                /* codegen(compst, p2, opt, tt, fl); */
                        tree = TreeNode.sib2(tree); goto tailcall;
                    }
                default: throw new Exception("should not reach here");
            }
        }

        private static ref Instruction getinstr(CompileState cs, int i)
        {
             return ref cs.pattern.code[i];
        }

        private static void realloccode(Pattern p, int nsize)
        {
            Array.Resize(ref p.code, nsize);
        }

        private static int nextinstruction(CompileState compst)
        {
            int size = compst.pattern.codesize;
            if (compst.ncode >= size)
                realloccode(compst.pattern, size * 2);
            return compst.ncode++;
        }

        private static int addinstruction(CompileState compst, Opcode op, int aux)
        {
            int i = nextinstruction(compst);
            getinstr(compst, i).i.code = op;
            getinstr(compst, i).i.aux = (byte)aux;
            return i;
        }

        /*
        ** Code an IChar instruction, or IAny if there is an equivalent
        ** test dominating it
        */
        private static void codechar(CompileState compst, int c, int tt)
        {
            if (tt >= 0 && getinstr(compst, tt).i.code == Opcode.ITestChar &&
                           getinstr(compst, tt).i.aux == c)
                addinstruction(compst, Opcode.IAny, 0);
            else
                addinstruction(compst, Opcode.IChar, c);
        }

        /*
        ** Code first child of a sequence
        ** (second child is called in-place to allow tail call)
        ** Return 'tt' for second child
        */
        private static int codeseq1(CompileState compst, TreeNode* p1, TreeNode* p2, int tt, ref Charset fl)
        {
          if (needfollow(p1) != 0) {
                Charset fl1 = default;
                getfirst(p2, ref fl, ref fl1);  /* p1 follow is p2 first */
                codegen(compst, p1, 0, tt, ref fl1);
            }
          else  /* use 'fullset' as follow */
            codegen(compst, p1, 0, tt, ref Charset.fullSet);
          if (fixedlen(p1) != 0)  /* can 'p1' consume anything? */
            return  Instruction.NOINST;  /* invalidate test */
          else return tt;  /* else 'tt' still protects sib2 */
        }

        /*
        ** Check whether the code generation for the given tree can benefit
        ** from a follow set (to avoid computing the follow set when it is
        ** not needed)
        */
        static int needfollow(TreeNode* tree)
        {
            tailcall:
            switch (tree->tag)
            {
                case Tag.TChar:
                case Tag.TSet:
                case Tag.TAny:
                case Tag.TFalse:
                case Tag.TTrue:
                case Tag.TAnd:
                case Tag.TNot:
                case Tag.TRunTime:
                case Tag.TGrammar:
                case Tag.TCall:
                case Tag.TBehind:
                    return 0;
                case Tag.TChoice:
                case Tag.TRep:
                    return 1;
                case Tag.TCapture:
                    tree = TreeNode.sib1(tree); goto tailcall;
                case Tag.TSeq:
                    tree = TreeNode.sib2(tree); goto tailcall;
                default: throw new Exception("should not reach here");
            }
        }

        /*
        ** Computes the 'first set' of a pattern.
        ** The result is a conservative aproximation:
        **   match p ax -> x (for some x) ==> a belongs to first(p)
        ** or
        **   a not in first(p) ==> match p ax -> fail (for all x)
        **
        ** The set 'follow' is the first set of what follows the
        ** pattern (full set if nothing follows it).
        **
        ** The function returns 0 when this resulting set can be used for
        ** test instructions that avoid the pattern altogether.
        ** A non-zero return can happen for two reasons:
        ** 1) match p '' -> ''            ==> return has bit 1 set
        ** (tests cannot be used because they would always fail for an empty input);
        ** 2) there is a match-time capture ==> return has bit 2 set
        ** (optimizations should not bypass match-time captures).
        */
        static int getfirst(TreeNode* tree, ref Charset follow, ref Charset firstset)
        {
            tailcall:
            switch (tree->tag)
            {
                case Tag.TChar:
                case Tag.TSet:
                case Tag.TAny:
                    {
                        tocharset(tree, ref firstset);
                        return 0;
                    }
                case Tag.TTrue:
                    {
                        for (int i = 0; i < Charset.CharsetSize; i++) firstset[i] = follow[i];

                        return 1;  /* accepts the empty string */
                    }
                case Tag.TFalse:
                    {
                        for (int i = 0; i < Charset.CharsetSize; i++) firstset[i] = 0;
                        return 0;
                    }
                case Tag.TChoice:
                    {
                        Charset csaux = default;
                        int e1 = getfirst(TreeNode.sib1(tree), ref follow, ref firstset);
                        int e2 = getfirst(TreeNode.sib2(tree), ref follow, ref csaux);
                        for (int i = 0; i < Charset.CharsetSize; i++) firstset[i] |= csaux[i];
                        return e1 | e2;
                    }
                case Tag.TSeq:
                    {
                        if (nullable(TreeNode.sib1(tree)) == 0)
                        {
                            /* when p1 is not nullable, p2 has nothing to contribute;
                               return getfirst(sib1(tree), fullset, firstset); */
                            tree = TreeNode.sib1(tree); follow = Charset.fullSet; goto tailcall;
                        }
                        else
                        {  /* FIRST(p1 p2, fl) = FIRST(p1, FIRST(p2, fl)) */
                            Charset csaux = default;
                            int e2 = getfirst(TreeNode.sib2(tree), ref follow, ref csaux);
                            int e1 = getfirst(TreeNode.sib1(tree), ref csaux, ref firstset);
                            if (e1 == 0) return 0;  /* 'e1' ensures that first can be used */
                            else if (((e1 | e2) & 2) != 0)  /* one of the children has a matchtime? */
                                return 2;  /* pattern has a matchtime capture */
                            else return e2;  /* else depends on 'e2' */
                        }
                    }
                case Tag.TRep:
                    {
                        getfirst(TreeNode.sib1(tree), ref follow, ref firstset);
                        for (int i = 0; i < Charset.CharsetSize; i++) firstset[i] |= follow[i];
                        return 1;  /* accept the empty string */
                    }
                case Tag.TCapture:
                case Tag.TGrammar:
                case Tag.TRule:
                    {
                        /* return getfirst(sib1(tree), follow, firstset); */
                        tree = TreeNode.sib1(tree); goto tailcall;
                    }
                case Tag.TRunTime:
                    {  /* function invalidates any follow info. */
                        int e = getfirst(TreeNode.sib1(tree), ref Charset.fullSet, ref firstset);
                        if (e != 0) return 2;  /* function is not "protected"? */
                        else return 0;  /* pattern inside capture ensures first can be used */
                    }
                case Tag.TCall:
                    {
                        /* return getfirst(sib2(tree), follow, firstset); */
                        tree = TreeNode.sib2(tree); goto tailcall;
                    }
                case Tag.TAnd:
                    {
                        int e = getfirst(TreeNode.sib1(tree), ref follow, ref firstset);
                        for (int i = 0; i < Charset.CharsetSize; i++) firstset[i] &= follow[i];
                        return e;
                    }
                case Tag.TNot:
                    {
                        if (tocharset(TreeNode.sib1(tree), ref firstset) != 0)
                        {
                            cs_complement(ref firstset);
                            return 1;
                        }
                        /* else go through */
                        goto Tag_TBehind;
                    }
                case Tag.TBehind:
                    Tag_TBehind:
                    {  /* instruction gives no new information */

                        /* call 'getfirst' only to check for math-time captures */
                        int e = getfirst(TreeNode.sib1(tree), ref follow, ref firstset);
                        for (int i = 0; i < Charset.CharsetSize; i++) firstset[i] = follow[i];  /* uses follow */
                        return e | 1;  /* always can accept the empty string */
                    }
                default: throw new Exception("should not reach here");
            }
        }

        /*
        ** A few basic operations on Charsets
        */
        private static void cs_complement(ref Charset cs)
        {
            for (int i = 0; i < Charset.CharsetSize; i++) cs[i] = (byte)~cs[i];
        }

        static int cs_equal(ref Charset cs1, CompileState compst, int instructionIndex)
        {
            fixed (Instruction* instruction = compst.pattern.code)
            {
                var buffer = (byte*)(instruction + instructionIndex);
                return cs_equal(ref cs1, buffer);
            }
        }

        static int cs_equal(byte[] cs1, byte[] cs2)
        {
            for (int i = 0; i < Charset.CharsetSize; i++) if (cs1[i] != cs2[i]) return 0;
            return 1;
        }

        static int cs_equal(ref Charset cs1, byte* cs2)
        {
            for (int i = 0; i < Charset.CharsetSize; i++) if (cs1[i] != cs2[i]) return 0;
            return 1;
        }

        static int cs_equal(byte[] cs1, byte* cs2)
        {
            for (int i = 0; i < Charset.CharsetSize; i++) if (cs1[i] != cs2[i]) return 0;
            return 1;
        }

        static int cs_equal(byte* cs1, byte* cs2)
        {
            for (int i = 0; i < Charset.CharsetSize; i++) if (cs1[i] != cs2[i]) return 0;
            return 1;
        }

        static int cs_disjoint(ref Charset cs1, ref Charset cs2)
        {
            for (int i = 0; i < Charset.CharsetSize; i++) if ((cs1[i] & cs2[i]) != 0) return 0;
            return 1;
        }

        private static void setchar(ref Charset cs, int b)
        {
            cs[(b) >> 3] |= (byte)(1 << ((b) & 7));
        }

        private static void setchar(byte* cs, int b)
        {
            cs[(b) >> 3] |= (byte)(1 << ((b) & 7));
        }

        /*
        ** If 'tree' is a 'char' pattern (TSet, TChar, TAny), convert it into a
        ** charset and return 1; else return 0.
        */
        static int tocharset(TreeNode* tree, ref Charset cs)
        {
            switch (tree->tag)
            {
                case Tag.TSet:
                    {  /* copy set */
                        for (int i = 0; i < Charset.CharsetSize; i++) cs[i] = TreeNode.treebuffer(tree)[i];
                        return 1;
                    }
                case Tag.TChar:
                    {  /* only one char */
                        assert(0 <= tree->n && tree->n <= byte.MaxValue);
                        for (int i = 0; i < Charset.CharsetSize; i++) cs[i] = 0;  /* erase all chars */
                        setchar(ref cs, tree->n);  /* add that one */
                        return 1;
                    }
                case Tag.TAny:
                    {
                        for (int i = 0; i < Charset.CharsetSize; i++) cs[i] = 0xFF;  /* add all characters to the set */
                        return 1;
                    }
                default: return 0;
            }
        }

        /*
        ** Add a charset posfix to an instruction
        */
        static void addcharset(CompileState compst, ref Charset cs)
        {
            int p = gethere(compst);
            int i;
            for (i = 0; i < Charset.CharsetInstSize - 1; i++)
                nextinstruction(compst);  /* space for buffer */
                                          /* fill buffer with charset */

            fixed(Instruction* instruction = compst.pattern.code)
            {
                var buffer = (byte*)(instruction + p);
                for (int j = 0; j < Charset.CharsetSize; j++)
                {
                    buffer[j] = cs[j];
                }
            }

            
        }

        private static int nullable(TreeNode* tree)
        {
            return checkaux(tree, PEnullable);
        }

        private  static int nofail(TreeNode* tree)
        {
            return checkaux(tree, PEnofail);
        }

        public const int PEnullable = 0;
        public const int PEnofail = 1;

        /*
        ** Checks how a pattern behaves regarding the empty string,
        ** in one of two different ways:
        ** A pattern is *nullable* if it can match without consuming any character;
        ** A pattern is *nofail* if it never fails for any string
        ** (including the empty string).
        ** The difference is only for predicates and run-time captures;
        ** for other patterns, the two properties are equivalent.
        ** (With predicates, &'a' is nullable but not nofail. Of course,
        ** nofail => nullable.)
        ** These functions are all convervative in the following way:
        **    p is nullable => nullable(p)
        **    nofail(p) => p cannot fail
        ** The function assumes that TOpenCall is not nullable;
        ** this will be checked again when the grammar is fixed.
        ** Run-time captures can do whatever they want, so the result
        ** is conservative.
        */
        static int checkaux(TreeNode* tree, int pred)
        {
            tailcall:
            switch (tree->tag)
            {
                case Tag.TChar:
                case Tag.TSet:
                case Tag.TAny:
                case Tag.TFalse:
                case Tag.TOpenCall:
                    return 0;  /* not nullable */
                case Tag.TRep:
                case Tag.TTrue:
                    return 1;  /* no fail */
                case Tag.TNot:
                case Tag.TBehind:  /* can match empty, but can fail */
                    if (pred == PEnofail) return 0;
                    else return 1;  /* PEnullable */
                case Tag.TAnd:  /* can match empty; fail iff body does */
                    if (pred == PEnullable) return 1;
                    /* else return checkaux(sib1(tree), pred); */
                    tree = TreeNode.sib1(tree); goto tailcall;
                case Tag.TRunTime:  /* can fail; match empty iff body does */
                    if (pred == PEnofail) return 0;
                    /* else return checkaux(sib1(tree), pred); */
                    tree = TreeNode.sib1(tree); goto tailcall;
                case Tag.TSeq:
                    if (checkaux(TreeNode.sib1(tree), pred) == 0) return 0;
                    /* else return checkaux(sib2(tree), pred); */
                    tree = TreeNode.sib2(tree); goto tailcall;
                case Tag.TChoice:
                    if (checkaux(TreeNode.sib2(tree), pred) != 0) return 1;
                    /* else return checkaux(sib1(tree), pred); */
                    tree = TreeNode.sib1(tree); goto tailcall;
                case Tag.TCapture:
                case Tag.TGrammar:
                case Tag.TRule:
                    /* return checkaux(sib1(tree), pred); */
                    tree = TreeNode.sib1(tree); goto tailcall;
                case Tag.TCall:  /* return checkaux(sib2(tree), pred); */
                    tree = TreeNode.sib2(tree); goto tailcall;
                default: throw new Exception("should not reach here");
            }
        }


        /*
        ** number of characters to match a pattern (or -1 if variable)
        */
        static int fixedlen(TreeNode* tree)
        {
            int len = 0;  /* to accumulate in tail calls */
            tailcall:
            switch (tree->tag)
            {
                case Tag.TChar:
                case Tag.TSet:
                case Tag.TAny:
                    return len + 1;
                case Tag.TFalse:
                case Tag.TTrue:
                case Tag.TNot:
                case Tag.TAnd:
                case Tag.TBehind:
                    return len;
                case Tag.TRep:
                case Tag.TRunTime:
                case Tag.TOpenCall:
                    return -1;
                case Tag.TCapture:
                case Tag.TRule:
                case Tag.TGrammar:
                    /* return fixedlen(sib1(tree)); */
                    tree = TreeNode.sib1(tree); goto tailcall;
                case Tag.TCall:
                    {
                        int n1 = callrecursive(tree, ptr => fixedlen((TreeNode*)ptr.ToPointer()), -1);
                        if (n1 < 0)
                            return -1;
                        else
                            return len + n1;
                    }
                case Tag.TSeq:
                    {
                        int n1 = fixedlen(TreeNode.sib1(tree));
                        if (n1 < 0)
                            return -1;
                        /* else return fixedlen(sib2(tree)) + len; */
                        len += n1; tree = TreeNode.sib2(tree); goto tailcall;
                    }
                case Tag.TChoice:
                    {
                        int n1 = fixedlen(TreeNode.sib1(tree));
                        int n2 = fixedlen(TreeNode.sib2(tree));
                        if (n1 != n2 || n1 < 0)
                            return -1;
                        else
                            return len + n1;
                    }
                default: throw new Exception("should not reach here");
            };
        }

        /*
        ** Visit a TCall node taking care to stop recursion. If node not yet
        ** visited, return 'f(sib2(tree))', otherwise return 'def' (default
        ** value)
        */
        static int callrecursive(TreeNode* tree, Func<IntPtr, int> f, int def)
        {
            int key = tree->key;
            assert(tree->tag == Tag.TCall);
            assert(TreeNode.sib2(tree)->tag == Tag.TRule);
            if (key == 0)  /* node already visited? */
                return def;  /* return default value */
            else
            {  /* first visit */
                int result;
                tree->key = 0;  /* mark call as already visited */
                result = f((IntPtr)TreeNode.sib2(tree));  /* go to called rule */
                tree->key = (ushort)key;  /* restore tree */
                return result;
            }
        }

        /*
        ** Repetion; optimizations:
        ** When pattern is a charset, can use special instruction ISpan.
        ** When pattern is head fail, or if it starts with characters that
        ** are disjoint from what follows the repetions, a simple test
        ** is enough (a fail inside the repetition would backtrack to fail
        ** again in the following pattern, so there is no need for a choice).
        ** When 'opt' is true, the repetion can reuse the Choice already
        ** active in the stack.
        */
        static void coderep(CompileState compst, TreeNode* tree, int opt, ref Charset fl)
        {
            Charset st = default;
            if (tocharset(tree, ref st) != 0)
            {
                addinstruction(compst, Opcode.ISpan, 0);
                addcharset(compst, ref st);
            }
            else
            {
                int e1 = getfirst(tree, ref Charset.fullSet, ref st);
                if (headfail(tree) != 0 || (e1 != 0 && cs_disjoint(ref st, ref fl) != 0))
                {
                    /* L1: test (fail(p1)) -> L2; <p>; jmp L1; L2: */
                    int jmp;
                    int test = codetestset(compst, ref st, 0);
                    codegen(compst, tree, 0, test, ref Charset.fullSet);
                    jmp = addoffsetinst(compst, Opcode.IJmp);
                    jumptohere(compst, test);
                    jumptothere(compst, jmp, test);
                }
                else
                {
                    /* test(fail(p1)) -> L2; choice L2; L1: <p>; partialcommit L1; L2: */
                    /* or (if 'opt'): partialcommit L1; L1: <p>; partialcommit L1; */
                    int commit, l2;
                    int test = codetestset(compst, ref st, e1);
                    int pchoice = Instruction.NOINST;
                    if (opt != 0)
                        jumptohere(compst, addoffsetinst(compst, Opcode.IPartialCommit));
                    else
                        pchoice = addoffsetinst(compst, Opcode.IChoice);
                    l2 = gethere(compst);
                    codegen(compst, tree, 0, Instruction.NOINST, ref Charset.fullSet);
                    commit = addoffsetinst(compst, Opcode.IPartialCommit);
                    jumptothere(compst, commit, l2);
                    jumptohere(compst, pchoice);
                    jumptohere(compst, test);
                }
            }
        }

        /*
        ** Patch 'instruction' to jump to current position
        */
        static void jumptohere(CompileState compst, int instruction)
        {
            jumptothere(compst, instruction, gethere(compst));
        }

        static int gethere(CompileState compst) => compst.ncode;

        /*
        ** Patch 'instruction' to jump to 'target'
        */
        static void jumptothere(CompileState compst, int instruction, int target)
        {
            if (instruction >= 0)
                setoffset(compst, instruction, target - instruction);
        }

        /*
        ** Set the offset of an instruction
        */
        static void setoffset(CompileState compst, int instruction, int offset)
        {
            getinstr(compst, instruction + 1).offset = offset;
        }

        /*
        ** Add an instruction followed by space for an offset (to be set later)
        */
        static int addoffsetinst(CompileState compst, Opcode op)
        {
            int i = addinstruction(compst, op, 0);  /* instruction */
            addinstruction(compst, (Opcode)0, 0);  /* open space for offset */
            assert(op == Opcode.ITestSet || sizei(ref getinstr(compst, i)) == 2);
            return i;
        }

        /*
        ** size of an instruction
        */
        static int sizei(ref Instruction i) {
          switch(i.i.code) {
            case Opcode.ISet: case Opcode.ISpan: return Charset.CharsetInstSize;
            case Opcode.ITestSet: return Charset.CharsetInstSize + 1;
            case Opcode.ITestChar: case Opcode.ITestAny: case Opcode.IChoice: case Opcode.IJmp: case Opcode.ICall:
            case Opcode.IOpenCall: case Opcode.ICommit: case Opcode.IPartialCommit: case Opcode.IBackCommit:
              return 2;
            default: return 1;
          }
        }

        /*
        ** code a test set, optimizing unit sets for ITestChar, "complete"
        ** sets for ITestAny, and empty sets for IJmp (always fails).
        ** 'e' is true iff test should accept the empty string. (Test
        ** instructions in the current VM never accept the empty string.)
        */
        static int codetestset(CompileState compst, ref Charset cs, int e)
        {
            if (e != 0) return Instruction.NOINST;  /* no test */
            else
            {
                int c = 0;
                Opcode op = charsettype(ref cs, ref c);
                switch (op)
                {
                    case Opcode.IFail: return addoffsetinst(compst, Opcode.IJmp);  /* always jump */
                    case Opcode.IAny: return addoffsetinst(compst, Opcode.ITestAny);
                    case Opcode.IChar:
                        {
                            int i = addoffsetinst(compst, Opcode.ITestChar);
                            getinstr(compst, i).i.aux = (byte)c;
                            return i;
                        }
                    case Opcode.ISet:
                        {
                            int i = addoffsetinst(compst, Opcode.ITestSet);
                            addcharset(compst, ref cs);
                            return i;
                        }
                    default: throw new Exception("should not reach here");
                }
            }
        }

        private const int BITSPERCHAR = 8;

        /*
        ** Check whether a charset is empty (returns IFail), singleton (IChar),
        ** full (IAny), or none of those (ISet). When singleton, '*c' returns
        ** which character it is. (When generic set, the set was the input,
        ** so there is no need to return it.)
        */
        static Opcode charsettype(ref Charset cs, ref int c)
        {
            int count = 0;  /* number of characters in the set */
            int i;
            int candidate = -1;  /* candidate position for the singleton char */
            for (i = 0; i < Charset.CharsetSize; i++)
            {  /* for each byte */
                int b = cs[i];
                if (b == 0)
                {  /* is byte empty? */
                    if (count > 1)  /* was set neither empty nor singleton? */
                        return Opcode.ISet;  /* neither full nor empty nor singleton */
                                             /* else set is still empty or singleton */
                }
                else if (b == 0xFF)
                {  /* is byte full? */
                    if (count < (i * BITSPERCHAR))  /* was set not full? */
                        return Opcode.ISet;  /* neither full nor empty nor singleton */
                    else count += BITSPERCHAR;  /* set is still full */
                }
                else if ((b & (b - 1)) == 0)
                {  /* has byte only one bit? */
                    if (count > 0)  /* was set not empty? */
                        return Opcode.ISet;  /* neither full nor empty nor singleton */
                    else
                    {  /* set has only one char till now; track it */
                        count++;
                        candidate = i;
                    }
                }
                else return Opcode.ISet;  /* byte is neither empty, full, nor singleton */
            }
            switch (count)
            {
                case 0: return Opcode.IFail;  /* empty set */
                case 1:
                    {  /* singleton; find character bit inside byte */
                        int b = cs[candidate];
                        c = candidate * BITSPERCHAR;
                        if ((b & 0xF0) != 0) { c += 4; b >>= 4; }
                        if ((b & 0x0C) != 0) { c += 2; b >>= 2; }
                        if ((b & 0x02) != 0) { c += 1; }
                        return Opcode.IChar;
                    }
                default:
                    {
                        assert(count == Charset.CharsetSize * BITSPERCHAR);  /* full set */
                        return Opcode.IAny;
                    }
            }
        }

        /*
** If 'headfail(tree)' true, then 'tree' can fail only depending on the
** next character of the subject.
*/
        static int headfail(TreeNode* tree)
        {
            tailcall:
            switch (tree->tag)
            {
                case Tag.TChar:
                case Tag.TSet:
                case Tag.TAny:
                case Tag.TFalse:
                    return 1;
                case Tag.TTrue:
                case Tag.TRep:
                case Tag.TRunTime:
                case Tag.TNot:
                case Tag.TBehind:
                    return 0;
                case Tag.TCapture:
                case Tag.TGrammar:
                case Tag.TRule:
                case Tag.TAnd:
                    tree = TreeNode.sib1(tree); goto tailcall;  /* return headfail(sib1(tree)); */
                case Tag.TCall:
                    tree = TreeNode.sib2(tree); goto tailcall;  /* return headfail(sib2(tree)); */
                case Tag.TSeq:
                    if (nofail(TreeNode.sib2(tree)) == 0) return 0;
                    /* else return headfail(sib1(tree)); */
                    tree = TreeNode.sib1(tree); goto tailcall;
                case Tag.TChoice:
                    if (headfail(TreeNode.sib1(tree)) == 0) return 0;
                    /* else return headfail(sib2(tree)); */
                    tree = TreeNode.sib2(tree); goto tailcall;
                default: throw new Exception("should not reach here");
            }
        }

        private static bool CustomAlwaysFalse(int t) => false;

        /*
        ** Choice; optimizations:
        ** - when p1 is headfail or
        ** when first(p1) and first(p2) are disjoint, than
        ** a character not in first(p1) cannot go to p1, and a character
        ** in first(p1) cannot go to p2 (at it is not in first(p2)).
        ** (The optimization is not valid if p1 accepts the empty string,
        ** as then there is no character at all...)
        ** - when p2 is empty and opt is true; a IPartialCommit can reuse
        ** the Choice already active in the stack.
        */
        static void codechoice(CompileState compst, TreeNode* p1, TreeNode* p2, int opt,
                        ref Charset fl)
        {
            int emptyp2 = (p2->tag == Tag.TTrue) ? 1 : 0;
            Charset cs1 = default, cs2 = default;
            int e1 = getfirst(p1, ref Charset.fullSet, ref cs1);
            if (headfail(p1) != 0 ||
                (e1 == 0 && (CustomAlwaysFalse(getfirst(p2, ref fl, ref cs2))&&0!=cs_disjoint(ref cs1, ref cs2))))
            {
                /* <p1 / p2> == test (fail(p1)) -> L1 ; p1 ; jmp L2; L1: p2; L2: */
                int test = codetestset(compst, ref cs1, 0);
                int jmp = Instruction.NOINST;
                codegen(compst, p1, 0, test, ref fl);
                if (emptyp2 == 0)
                    jmp = addoffsetinst(compst, Opcode.IJmp);
                jumptohere(compst, test);
                codegen(compst, p2, opt, Instruction.NOINST, ref fl);
                jumptohere(compst, jmp);
            }
            else if (opt != 0 && emptyp2 != 0)
            {
                /* p1? == IPartialCommit; p1 */
                jumptohere(compst, addoffsetinst(compst, Opcode.IPartialCommit));
                codegen(compst, p1, 1, Instruction.NOINST, ref Charset.fullSet);
            }
            else
            {
                /* <p1 / p2> == 
                    test(first(p1)) -> L1; choice L1; <p1>; commit L2; L1: <p2>; L2: */
                int pcommit;
                int test = codetestset(compst, ref cs1, e1);
                int pchoice = addoffsetinst(compst, Opcode.IChoice);
                codegen(compst, p1, emptyp2, test, ref Charset.fullSet);
                pcommit = addoffsetinst(compst, Opcode.ICommit);
                jumptohere(compst, pchoice);
                jumptohere(compst, test);
                codegen(compst, p2, opt, Instruction.NOINST, ref fl);
                jumptohere(compst, pcommit);
            }
        }

        /*
        ** Not predicate; optimizations:
        ** In any case, if first test fails, 'not' succeeds, so it can jump to
        ** the end. If pattern is headfail, that is all (it cannot fail
        ** in other parts); this case includes 'not' of simple sets. Otherwise,
        ** use the default code (a choice plus a failtwice).
        */
        static void codenot(CompileState compst, TreeNode* tree)
        {
            Charset st = default;
            int e = getfirst(tree, ref Charset.fullSet, ref st);
            int test = codetestset(compst, ref st, e);
            if (headfail(tree) != 0)  /* test (fail(p1)) -> L1; fail; L1:  */
                addinstruction(compst, Opcode.IFail, 0);
            else
            {
                /* test(fail(p))-> L1; choice L1; <p>; failtwice; L1:  */
                int pchoice = addoffsetinst(compst, Opcode.IChoice);
                codegen(compst, tree, 0, Instruction.NOINST, ref Charset.fullSet);
                addinstruction(compst, Opcode.IFailTwice, 0);
                jumptohere(compst, pchoice);
            }
            jumptohere(compst, test);
        }

        public const int MAXOFF = 0xF;
        public const int MAXAUX = 0xFF;

        /* maximum number of bytes to look behind */
        public const int MAXBEHIND = MAXAUX;

        /*
        ** And predicate
        ** optimization: fixedlen(p) = n ==> <&p> == <p>; behind n
        ** (valid only when 'p' has no captures)
        */
        static void codeand(CompileState compst, TreeNode* tree, int tt)
        {
            int n = fixedlen(tree);
            if (n >= 0 && n <= MAXBEHIND && hascaptures(tree) == 0)
            {
                codegen(compst, tree, 0, tt, ref Charset.fullSet);
                if (n > 0)
                    addinstruction(compst, Opcode.IBehind, n);
            }
            else
            {  /* default: Choice L1; p1; BackCommit L2; L1: Fail; L2: */
                int pcommit;
                int pchoice = addoffsetinst(compst, Opcode.IChoice);
                codegen(compst, tree, 0, tt, ref Charset.fullSet);
                pcommit = addoffsetinst(compst, Opcode.IBackCommit);
                jumptohere(compst, pchoice);
                addinstruction(compst, Opcode.IFail, 0);
                jumptohere(compst, pcommit);
            }
        }


        /* number of siblings for each tree */
        static readonly byte[] numsiblings = {
          0, 0, 0,	/* char, set, any */
          0, 0,		/* true, false */	
          1,		/* rep */
          2, 2,		/* seq, choice */
          1, 1,		/* not, and */
          0, 0, 2, 1,  /* call, opencall, rule, grammar */
          1,  /* behind */
          1, 1  /* capture, runtime capture */
        };

        /*
        ** Check whether a pattern tree has captures
        */
        static int hascaptures(TreeNode* tree)
        {
            tailcall:
            switch (tree->tag)
            {
                case Tag.TCapture:
                case Tag.TRunTime:
                    return 1;
                case Tag.TCall:
                    return callrecursive(tree, ptr=> hascaptures((TreeNode*)ptr), 0);
                case Tag.TRule:  /* do not follow siblings */
                    tree = TreeNode.sib1(tree); goto tailcall;
                case Tag.TOpenCall: throw new Exception("should not reach here");
                default:
                    {
                        switch (numsiblings[(int)tree->tag])
                        {
                            case 1:  /* return hascaptures(sib1(tree)); */
                                tree = TreeNode.sib1(tree); goto tailcall;
                            case 2:
                                if (hascaptures(TreeNode.sib1(tree)) != 0)
                                    return 1;
                                /* else return hascaptures(sib2(tree)); */
                                tree = TreeNode.sib2(tree); goto tailcall;
                            default: assert(numsiblings[(int)tree->tag] == 0); return 0;
                        }
                    }
            }
        }

        /*
        ** <behind(p)> == behind n; <p>   (where n = fixedlen(p))
        */
        static void codebehind(CompileState compst, TreeNode* tree)
        {
            if (tree->n > 0)
                addinstruction(compst, Opcode.IBehind, tree->n);
            codegen(compst, TreeNode.sib1(tree), 0, Instruction.NOINST, ref Charset.fullSet);
        }

        static void codecall(CompileState compst, TreeNode* call)
        {
            int c = addoffsetinst(compst, Opcode.IOpenCall);  /* to be corrected later */
            getinstr(compst, c).i.key = (byte)TreeNode.sib2(call)->cap;  /* rule number */
            assert(TreeNode.sib2(call)->tag == Tag.TRule);
        }

        /*
        ** Captures: if pattern has fixed (and not too big) length, and it
        ** has no nested captures, use a single IFullCapture instruction
        ** after the match; otherwise, enclose the pattern with OpenCapture -
        ** CloseCapture.
        */
        static void codecapture(CompileState compst, TreeNode* tree, int tt,
                         ref Charset fl)
        {
            int len = fixedlen(TreeNode.sib1(tree));
            if (len >= 0 && len <= MAXOFF && hascaptures(TreeNode.sib1(tree)) == 0)
            {
                codegen(compst, TreeNode.sib1(tree), 0, tt, ref fl);
                addinstcap(compst, Opcode.IFullCapture, (int)tree->cap, tree->key, len);
            }
            else
            {
                addinstcap(compst, Opcode.IOpenCapture, (int)tree->cap, tree->key, 0);
                codegen(compst, TreeNode.sib1(tree), 0, tt, ref fl);
                addinstcap(compst, Opcode.ICloseCapture, (int)CapKind.Cclose, 0, 0);
            }
        }

        /*
        ** in capture instructions, 'kind' of capture and its offset are
        ** packed in field 'aux', 4 bits for each
        */
        static int joinkindoff(int k, int o) => ((k) | ((o) << 4));

        /*
        ** Add a capture instruction:
        ** 'op' is the capture instruction; 'cap' the capture kind;
        ** 'key' the key into ktable; 'aux' is the optional capture offset
        **
        */
        static int addinstcap(CompileState compst, Opcode op, int cap, int key,
                               int aux)
        {
            int i = addinstruction(compst, op, joinkindoff(cap, aux));
            getinstr(compst, i).i.key = (short)key;
            return i;
        }

        static void coderuntime(CompileState compst, TreeNode* tree, int tt)
        {
            addinstcap(compst, Opcode.IOpenCapture, (int)CapKind.Cgroup, tree->key, 0);
            codegen(compst, TreeNode.sib1(tree), 0, tt, ref Charset.fullSet);
            addinstcap(compst, Opcode.ICloseRunTime, (int)CapKind.Cclose, 0, 0);
        }

        private const int MAXRULES = 250;

        /*
        ** Code for a grammar:
        ** call L1; jmp L2; L1: rule 1; ret; rule 2; ret; ...; L2:
        */
        static void codegrammar(CompileState compst, TreeNode* grammar)
        {
            int[] positions = new int[MAXRULES];
            int rulenumber = 0;
            TreeNode* rule;
            int firstcall = addoffsetinst(compst, Opcode.ICall);  /* call initial rule */
            int jumptoend = addoffsetinst(compst, Opcode.IJmp);  /* jump to the end */
            int start = gethere(compst);  /* here starts the initial rule */
            jumptohere(compst, firstcall);
            for (rule = TreeNode.sib1(grammar); rule->tag == Tag.TRule; rule = TreeNode.sib2(rule))
            {
                positions[rulenumber++] = gethere(compst);  /* save rule position */
                codegen(compst, TreeNode.sib1(rule), 0, Instruction.NOINST, ref Charset.fullSet);  /* code rule */
                addinstruction(compst, Opcode.IRet, 0);
            }
            assert(rule->tag == Tag.TTrue);
            jumptohere(compst, jumptoend);
            correctcalls(compst, positions, start, gethere(compst));
        }

        /*
        ** change open calls to calls, using list 'positions' to find
        ** correct offsets; also optimize tail calls
        */
        static void correctcalls(CompileState compst, int[] positions,
                                  int from, int to)
        {
            int i;
            fixed(Instruction* code = compst.pattern.code)
            {
                for (i = from; i < to; i += sizei(ref code[i]))
                {
                    if (code[i].i.code == Opcode.IOpenCall)
                    {
                        int n = code[i].i.key;  /* rule number */
                        int rule = positions[n];  /* rule position */
                        assert(rule == from || code[rule - 1].i.code == Opcode.IRet);
                        if (code[finaltarget(code, i + 2)].i.code == Opcode.IRet)  /* call; ret ? */
                            code[i].i.code = Opcode.IJmp;  /* tail call */
                        else
                            code[i].i.code = Opcode.ICall;
                        jumptothere(compst, i, rule);  /* call jumps to respective rule */
                    }
                }
            }
            assert(i == to);
        }

        private static int target(Instruction* code, int i) => ((i) + code[i + 1].offset);

        /*
        ** Find the final destination of a sequence of jumps
        */
        static int finaltarget(Instruction* code, int i)
        {
            while (code[i].i.code == Opcode.IJmp)
                i = target(code, i);
            return i;
        }

        /*
        ** code a char set, optimizing unit sets for IChar, "complete"
        ** sets for IAny, and empty sets for IFail; also use an IAny
        ** when instruction is dominated by an equivalent test.
        */
        static void codecharset(CompileState compst, ref Charset cs, int tt)
        {
            int c = 0;  /* (=) to avoid warnings */
            Opcode op = charsettype(ref cs, ref c);
            switch (op)
            {
                case Opcode.IChar: codechar(compst, c, tt); break;
                case Opcode.ISet:
                    {
                        /* non-trivial set? */
                        if (tt >= 0 && getinstr(compst, tt).i.code == Opcode.ITestSet &&
                            cs_equal(ref cs, compst, tt + 2) != 0)
                        addinstruction(compst, Opcode.IAny, 0);
                        else
                        {
                            addinstruction(compst, Opcode.ISet, 0);
                            addcharset(compst, ref cs);
                        }
                        break;
                    }
                default: addinstruction(compst, op, c); break;
            }
        }

        internal static Pattern lp_simplecapture(Pattern pattern)
        {
            return capture_aux(pattern, CapKind.Csimple, null);
        }

        internal static Pattern lp_tablecapture(Pattern pattern)
        {
            return capture_aux(pattern, CapKind.Ctable, null);
        }

        internal static Pattern lp_functioncapture(Pattern pattern, CaptureFunction func)
        {
            return capture_aux(pattern, CapKind.Cfunction, func);
        }

        /*
        ** Create a tree for a non-empty capture, with a body and
        ** optionally with an associated Lua value (at index 'labelidx' in the
        ** stack)
        */
        static Pattern capture_aux(Pattern pattern, CapKind cap, object kValue)
        {
            var capturePattern = newroot1sib(pattern, Tag.TCapture);
            fixed (TreeNode* tree = capturePattern.treeNodes)
            {
                tree->cap = cap;
                tree->key = (ushort)((kValue == null) ? 0 : addtonewktable(capturePattern, pattern, kValue));
            }
            return capturePattern;
        }


        //TC TC ...... ...... ......
        //TC ...... TC ...... ......

        /*
        ** Transform left associative constructions into right
        ** associative ones, for sequence and choice; that is:
        ** (t11 + t12) + t2  =>  t11 + (t12 + t2)
        ** (t11 * t12) * t2  =>  t11 * (t12 * t2)
        ** (that is, Op (Op t11 t12) t2 => Op t11 (Op t12 t2))
        */
        static void correctassociativity(TreeNode* tree)
        {
            TreeNode* t1 = TreeNode.sib1(tree);
            assert(tree->tag == Tag.TChoice || tree->tag == Tag.TSeq);
            while (t1->tag == tree->tag)
            {
                int n1size = tree->ps - 1;  /* t1 == Op t11 t12 */
                int n11size = t1->ps - 1;
                int n12size = n1size - n11size - 1;

                var from = TreeNode.sib1(t1);
                var to = TreeNode.sib1(tree);
                for(int it = 0; it < n11size; it++)
                {
                    to[it] = from[it];
                }
                tree->ps = n11size + 1;
                TreeNode.sib2(tree)->tag = tree->tag;
                TreeNode.sib2(tree)->ps = n12size + 1;
            }
        }

        /*
        ** final label (after traversing any jumps)
        */
        static int finallabel(Instruction* code, int i)
        {
            return finaltarget(code, target(code, i));
        }

        /*
        ** Optimize jumps and other jump-like instructions.
        ** * Update labels of instructions with labels to their final
        ** destinations (e.g., choice L1; ... L1: jmp L2: becomes
        ** choice L2)
        ** * Jumps to other instructions that do jumps become those
        ** instructions (e.g., jump to return becomes a return; jump
        ** to commit becomes a commit)
        */
        static void peephole(CompileState compst)
        {
            fixed(Instruction* code = compst.pattern.code)
            {
                int i;
                for (i = 0; i < compst.ncode; i += sizei(ref code[i]))
                {
                    redo:
                    switch (code[i].i.code)
                    {
                        case Opcode.IChoice:
                        case Opcode.ICall:
                        case Opcode.ICommit:
                        case Opcode.IPartialCommit:
                        case Opcode.IBackCommit:
                        case Opcode.ITestChar:
                        case Opcode.ITestSet:
                        case Opcode.ITestAny:
                            {  /* instructions with labels */
                                jumptothere(compst, i, finallabel(code, i));  /* optimize label */
                                break;
                            }
                        case Opcode.IJmp:
                            {
                                int ft = finaltarget(code, i);
                                switch (code[ft].i.code)
                                {  /* jumping to what? */
                                    case Opcode.IRet:
                                    case Opcode.IFail:
                                    case Opcode.IFailTwice:
                                    case Opcode.IEnd:
                                        {  /* instructions with unconditional implicit jumps */
                                            code[i] = code[ft];  /* jump becomes that instruction */
                                            code[i + 1].i.code = Opcode.IAny;  /* 'no-op' for target position */
                                            break;
                                        }
                                    case Opcode.ICommit:
                                    case Opcode.IPartialCommit:
                                    case Opcode.IBackCommit:
                                        {  /* inst. with unconditional explicit jumps */
                                            int fft = finallabel(code, ft);
                                            code[i] = code[ft];  /* jump becomes that instruction... */
                                            jumptothere(compst, i, fft);  /* but must correct its offset */
                                            goto redo;  /* reoptimize its label */
                                        }
                                    default:
                                        {
                                            jumptothere(compst, i, ft);  /* optimize label */
                                            break;
                                        }
                                }
                                break;
                            }
                        default: break;
                    }
                }
                assert(code[i - 1].i.code == Opcode.IEnd);
            }
        }

        /*
        ** Create a non-terminal
        */
        public static Pattern V(object key)
        {
            if (key == null) throw new Exception("non-null value expected");
            var pattern = NewLeaf(Tag.TOpenCall);
            pattern.treeNodes[0].key = (ushort)addtonewktable(pattern, null, key);
            return pattern;
        }

        /*
        ** -p == !p
        */
        internal static Pattern lp_not(Pattern pattern1)
        {
            return newroot1sib(pattern1, Tag.TNot);
        }

        /*
        ** [t1 - t2] == Seq (Not t2) t1
        ** If t1 and t2 are charsets, make their difference.
        */
        internal static Pattern lp_sub(Pattern pattern1, Pattern pattern2)
        {
            Pattern pattern;
            Charset st1 = default, st2 = default;
            int s1 = pattern1.treeNodes.Length, s2 = pattern2.treeNodes.Length;

            fixed(TreeNode* t1 = pattern1.treeNodes)
            {
                fixed(TreeNode* t2 = pattern2.treeNodes)
                {
                    if (tocharset(t1, ref st1) != 0 && tocharset(t2, ref st2) != 0)
                    {
                        pattern = newcharset();
                        fixed(TreeNode* t = pattern.treeNodes)
                        {
                            for(int i = 0; i < Charset.CharsetSize; i++) TreeNode.treebuffer(t)[i] = (byte)(st1[i] & ~st2[i]);
                        }
                    }
                    else
                    {
                        pattern = NewTree(2 + s1 + s2);
                        fixed(TreeNode* tree = pattern.treeNodes)
                        {
                            tree->tag = Tag.TSeq;  /* sequence of... */
                            tree->ps = 2 + s2;
                            TreeNode.sib1(tree)->tag = Tag.TNot;  /* ...not... */

                            /* ...t2 */
                            for (int it = 0; it < s2; it++) *(TreeNode.sib1(TreeNode.sib1(tree)) + it) = *(t2 + it);

                            for (int it = 0; it < s1; it++) *(TreeNode.sib2(tree) + it) = *(t1 + it);/* ... and t1 */

                            joinktables(pattern, pattern1, pattern2, TreeNode.sib1(tree));
                        }
                    }
                }
            }

            return pattern;
        }

        internal static Pattern lp_groupcapture(Pattern pattern, object groupName)
        {
            return capture_aux(pattern, CapKind.Cgroup, groupName);
        }

        internal static Pattern lp_foldcapture(Pattern pattern, FoldFunction foldFunction)
        {
            return capture_aux(pattern, CapKind.Cfold, foldFunction);
        }

        internal static Pattern lp_matchtime(Pattern pattern1, MatchTimeFunction matchTimeFunction)
        {
            if (matchTimeFunction == null) throw new NullReferenceException();

            var pattern = newroot1sib(pattern1, Tag.TRunTime);
            pattern.treeNodes[0].key = (ushort)addtonewktable(pattern, pattern1, matchTimeFunction);
            return pattern;
        }

        internal static Pattern newgrammar(Grammar arg)
        {
            int treesize;
            Dictionary<string, int> positionTable;
            var ruleList = collectrules(arg, out treesize, out positionTable);
            int n = ruleList.Count;
            var pattern = NewTree(treesize);
            fixed(TreeNode* g = pattern.treeNodes)
            {
                pattern.kTable = Array.Empty<object>();
                g->tag = Tag.TGrammar; g->n = n;
                buildgrammar(pattern, g, ruleList);
                finalfix(pattern, positionTable, g, TreeNode.sib1(g));
                initialrulename(pattern, g, ruleList[0].key);
                verifygrammar(pattern, g);
                return pattern;  /* new table at the top of the stack */
            }
        }

        /*
        ** traverse grammar at index 'arg', pushing all its keys and patterns
        ** into the stack. Create a new table (before all pairs key-pattern) to
        ** collect all keys and their associated positions in the final tree
        ** (the "position table").
        ** Return the number of rules and (in 'totalsize') the total size
        ** for the new tree.
        */
        static List<(string key, Pattern rule)> collectrules(Grammar arg, out int totalsize, out Dictionary<string,int> positionTable)
        {
            var list = new List<(string, Pattern)>();  /* to count number of rules */
            int size;  /* accumulator for total size */
            positionTable = new Dictionary<string, int>();  /* create position table */
            var firstRuler = getfirstrule(arg, positionTable);
            list.Add((arg.firstRuler, firstRuler));
            size = 2 + firstRuler.treeNodes.Length;  /* TGrammar + TRule + rule */

            foreach(var kv in arg.grammars)
            {
                if (kv.Value == firstRuler) continue;
                if (kv.Value == null) throw new NullReferenceException();

                positionTable[kv.Key] = size;
                size += 1 + kv.Value.treeNodes.Length;
                list.Add((kv.Key, kv.Value));
            }

            totalsize = size + 1;  /* TTrue to finish list of rules */
            return list;
        }

        /*
        ** push on the stack the index and the pattern for the
        ** initial rule of grammar at index 'arg' in the stack;
        ** also add that index into position table.
        */
        static Pattern getfirstrule(Grammar arg, Dictionary<string, int> postab)
        {
            var firstRulerName = arg.firstRuler;  /* access first element */

            Pattern firstRuler;
            if(!arg.grammars.TryGetValue(firstRulerName, out firstRuler))
            {
                throw new Exception("grammar has no initial rule");
            }

            if (firstRuler == null) throw new NullReferenceException();

            postab[firstRulerName] = 1;

            return firstRuler;
        }

        static void buildgrammar(Pattern grammerPattern, TreeNode* grammar, List<(string key, Pattern rule)> ruleList)
        {
            TreeNode* nd = TreeNode.sib1(grammar);  /* auxiliary pointer to traverse the tree */

            for(int it = 0; it < ruleList.Count; it++)
            {
                var rule = ruleList[it];

                int rulesize = rule.rule.treeNodes.Length;

                fixed (TreeNode* rn = rule.rule.treeNodes)
                {
                    nd->tag = Tag.TRule;
                    nd->key = 0;  /* will be fixed when rule is used */
                    nd->cap = (CapKind)it;  /* rule number */
                    nd->ps = rulesize + 1;  /* point to next rule */

                    for (int j = 0; j < rulesize; j++) *(TreeNode.sib1(nd) + j) = *(rn + j); /* copy rule */

                    mergektable(rule.rule, grammerPattern, TreeNode.sib1(nd));  /* merge its ktable into new one */
                    nd = TreeNode.sib2(nd);  /* move to next rule */
                }
            }
            nd->tag = Tag.TTrue;  /* finish list of rules */
        }

        /*
        ** Give a name for the initial rule if it is not referenced
        */
        static void initialrulename(Pattern grammerPattern, TreeNode* grammar, string firstRuleName)
        {
            if (TreeNode.sib1(grammar)->key == 0)
            {  /* initial rule is not referenced? */
                int n = grammerPattern.kTable.Length + 1;
                Array.Resize(ref grammerPattern.kTable, n);
                grammerPattern.kTable[n - 1] = firstRuleName;
                TreeNode.sib1(grammar)->key = (ushort)n;
            }
        }

        static void verifygrammar(Pattern grammarPattern, TreeNode* grammar)
        {
            int* passed = stackalloc int[MAXRULES];
            TreeNode* rule;
            /* check left-recursive rules */
            for (rule = TreeNode.sib1(grammar); rule->tag == Tag.TRule; rule = TreeNode.sib2(rule))
            {
                if (rule->key == 0) continue;  /* unused rule */
                verifyrule(grammarPattern, TreeNode.sib1(rule), passed, 0, 0);
            }
            assert(rule->tag == Tag.TTrue);
            /* check infinite loops inside rules */
            for (rule = TreeNode.sib1(grammar); rule->tag == Tag.TRule; rule = TreeNode.sib2(rule))
            {
                if (rule->key == 0) continue;  /* unused rule */
                if (checkloops(TreeNode.sib1(rule)) != 0)
                {
                    throw new Exception("empty loop");
                    //TODO
                    //lua_rawgeti(L, -1, rule->key);  /* get rule's key */
                    //luaL_error(L, "empty loop in rule '%s'", val2str(L, -1));
                }
            }
            assert(rule->tag == Tag.TTrue);
        }

        /*
        ** Check whether a rule can be left recursive; raise an error in that
        ** case; otherwise return 1 iff pattern is nullable.
        ** The return value is used to check sequences, where the second pattern
        ** is only relevant if the first is nullable.
        ** Parameter 'nb' works as an accumulator, to allow tail calls in
        ** choices. ('nb' true makes function returns true.)
        ** Parameter 'passed' is a list of already visited rules, 'npassed'
        ** counts the elements in 'passed'.
        ** Assume ktable at the top of the stack.
        */
        static int verifyrule(Pattern grammarPattern, TreeNode* tree, int* passed, int npassed,
                               int nb)
        {
            tailcall:
            switch (tree->tag)
            {
                case Tag.TChar:
                case Tag.TSet:
                case Tag.TAny:
                case Tag.TFalse:
                    return nb;  /* cannot pass from here */
                case Tag.TTrue:
                case Tag.TBehind:  /* look-behind cannot have calls */
                    return 1;
                case Tag.TNot:
                case Tag.TAnd:
                case Tag.TRep:
                    /* return verifyrule(L, sib1(tree), passed, npassed, 1); */
                    tree = TreeNode.sib1(tree); nb = 1; goto tailcall;
                case Tag.TCapture:
                case Tag.TRunTime:
                    /* return verifyrule(L, sib1(tree), passed, npassed, nb); */
                    tree = TreeNode.sib1(tree); goto tailcall;
                case Tag.TCall:
                    /* return verifyrule(L, sib2(tree), passed, npassed, nb); */
                    tree = TreeNode.sib2(tree); goto tailcall;
                case Tag.TSeq:  /* only check 2nd child if first is nb */
                    if (verifyrule(grammarPattern, TreeNode.sib1(tree), passed, npassed, 0) == 0)
                        return nb;
                    /* else return verifyrule(L, sib2(tree), passed, npassed, nb); */
                    tree = TreeNode.sib2(tree); goto tailcall;
                case Tag.TChoice:  /* must check both children */
                    nb = verifyrule(grammarPattern, TreeNode.sib1(tree), passed, npassed, nb);
                    /* return verifyrule(L, sib2(tree), passed, npassed, nb); */
                    tree = TreeNode.sib2(tree); goto tailcall;
                case Tag.TRule:
                    if (npassed >= MAXRULES)
                        return verifyerror(grammarPattern, passed, npassed);
                    else
                    {
                        passed[npassed++] = tree->key;
                        /* return verifyrule(L, sib1(tree), passed, npassed); */
                        tree = TreeNode.sib1(tree); goto tailcall;
                    }
                case Tag.TGrammar:
                    return nullable(tree);  /* sub-grammar cannot be left recursive */
                default: throw new Exception("should not reach here");
            }
        }

        /*
        ** Check whether a tree has potential infinite loops
        */
        static int checkloops(TreeNode* tree)
        {
            tailcall:
            if (tree->tag == Tag.TRep && nullable(TreeNode.sib1(tree)) != 0)
                return 1;
            else if (tree->tag == Tag.TGrammar)
                return 0;  /* sub-grammars already checked */
            else
            {
                switch (numsiblings[(int)tree->tag])
                {
                    case 1:  /* return checkloops(sib1(tree)); */
                        tree = TreeNode.sib1(tree); goto tailcall;
                    case 2:
                        if (checkloops(TreeNode.sib1(tree)) != 0) return 1;
                        /* else return checkloops(sib2(tree)); */
                        tree = TreeNode.sib2(tree); goto tailcall;
                    default: assert(numsiblings[(int)tree->tag] == 0); return 0;
                }
            }
        }

        /*
        ** Give appropriate error message for 'verifyrule'. If a rule appears
        ** twice in 'passed', there is path from it back to itself without
        ** advancing the subject.
        */
        static int verifyerror(Pattern grammarPattern, int* passed, int npassed)
        {
            int i, j;
            for (i = npassed - 1; i >= 0; i--)
            {  /* search for a repetition */
                for (j = i - 1; j >= 0; j--)
                {
                    if (passed[i] == passed[j])
                    {
                        var key = grammarPattern.kTable[passed[i] - 1];/* get rule's key */
                        throw new Exception($"rule '{key}' may be left recursive");
                    }
                }
            }
            throw new Exception("too many left calls in grammar");
        }

        /*
        ** Fill a tree with an empty capture, using an empty (TTrue) sibling.
        ** (The 'key' field must be filled by the caller to finish the tree.)
        */
        static TreeNode* auxemptycap(TreeNode* tree, CapKind cap)
        {
            tree->tag = Tag.TCapture;
            tree->cap = cap;
            TreeNode.sib1(tree)->tag = Tag.TTrue;
            return tree;
        }

        /*
        ** Create a tree for an empty capture.
        */
        static Pattern newemptycap(CapKind cap, int key)
        {
            var pattern = NewTree(2);
            fixed(TreeNode* tree = pattern.treeNodes)
            {
                auxemptycap(tree, cap);
                tree->key = (ushort)key;
            }
            return pattern;
        }

        public static Pattern Cp()
        {
            return newemptycap(CapKind.Cposition, 0);
        }
    }
}
