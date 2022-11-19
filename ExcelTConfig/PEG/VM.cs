using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace PEG
{
    unsafe struct Stack
    {
        public byte* s;  /* saved position (or NULL for calls) */
        public Instruction* p;  /* next instruction */
        public int caplevel;
    }

    public unsafe struct Capture
    {
        public byte* s;  /* subject position */
        public ushort idx;  /* extra info (group name, arg index, etc.) */
        public CapKind kind;  /* kind of capture */
        public byte siz;  /* size of full capture + 1 (0 = not a full capture) */
    }

    public unsafe struct CapState
    {
        public Capture* cap;  /* current capture */
        public Capture* ocap;  /* (original) capture list */
        public int ptop;  /* index of last argument to 'match' */
        public byte* s;  /* original string */
        //public int valuecached;  /* value stored in cache slot */
        public int reclevel;  /* recursion level */
        public Pattern pattern;
        public List<object> runtimeCaptures;
        public string originalString;
    }

    public unsafe class ExecuteState : IDisposable
    {
        public IntPtr capture;
        public IntPtr stackCapture;

        public void Dispose()
        {
            if(capture != IntPtr.Zero && capture != stackCapture)
            {
                Marshal.FreeHGlobal(capture);
                capture = IntPtr.Zero;
            }
        }
    }

    public static unsafe partial class LPEG
    {
        public const int INITCAPSIZE = 32;

        private const int MAXRECLEVEL = 200;

        /*
        ** Main match function
        */
        internal static object[] lp_match(Pattern p, string content)
        {
            Capture* capture = stackalloc Capture[INITCAPSIZE];

            Instruction[] codes = p.code != null ? p.code : prepcompile(p);
            fixed (Instruction* code = codes)
            {
                var bytes = Encoding.UTF8.GetBytes(content);
                int l = bytes.Length;
                fixed (byte* s = bytes)
                {
                    int i = 0;
                    using (var executeState = new ExecuteState { capture = (IntPtr)capture, stackCapture = (IntPtr)capture })
                    {
                        byte* r = match(s, s + i, s + l, code, capture, p, new List<object>(), executeState, content);
                        if (r == null) return null;
                        return getcaptures(s, r, (Capture*)executeState.capture, p, content);
                    }
                }
            }
        }

        private static readonly Instruction pGiveup = new Instruction { i = new Inst { code = Opcode.IGiveup, aux = 0, key = 0 } };
        private const int MAXBACK = 400;
        private const int INITBACK = MAXBACK;
        private static int getoffset(Instruction* p) => (p + 1)->offset;
        private static bool testchar(Instruction* st, int c) => (((byte*)st)[c >> 3] & (1 << (c & 7))) != 0;
        private static int getkind(Instruction* op) => op->i.aux & 0xF;
        private static int getoff(Instruction* op) => (op->i.aux >> 4) & 0xF;
        private static Capture* growcap(Capture* capture, ref int capSize, int capTop, int n, ExecuteState executeState)
        {
            if (capSize - capTop > n) return capture;

            Capture* newc;
            int newsize = capTop + n + 1;  /* minimum size needed */
            if (newsize < int.MaxValue / (sizeof(Capture) * 2))
                newsize *= 2;  /* twice that size, if not too big */
            else if (newsize >= int.MaxValue / sizeof(Capture))
                throw new Exception("too many captures");

            newc = (Capture*)Marshal.AllocHGlobal(newsize * sizeof(Capture));
            for (int it = 0; it < capTop; it++) newc[it] = capture[it];
            capSize = newsize;

            if((IntPtr)capture != executeState.stackCapture)
            {
                Marshal.FreeHGlobal((IntPtr)capture);
            }
            executeState.capture = (IntPtr)newc;

            return newc;
        }
        
        /*
        ** Opcode interpreter
        */
        static byte* match (byte* o, byte* s, byte* e, Instruction* op, Capture* capture, Pattern pattern, List<object> runtimeCaptures, ExecuteState executeState, string originalString)
        {
            Stack* stackbase = stackalloc Stack[INITBACK];
            Stack* stacklimit = stackbase + INITBACK;
            Stack* stack = stackbase;  /* point to first empty slot in stack */
            int capsize = INITCAPSIZE;
            int captop = 0;  /* point to first empty slot in captures */
            int ndyncap = 0;  /* number of dynamic captures (in Lua stack) */
            Instruction* p = op;  /* current instruction */
            var giveup = pGiveup;
            stack->p = &giveup; stack->s = s; stack->caplevel = 0; stack++;
            Stack* storedStackBase = stackbase;
            for (; ; )
            {
                //#if defined(DEBUG)
                //      printf("-------------------------------------\n");
                //      printcaplist(capture, capture + captop);
                //      printf("s: |%s| stck:%d, dyncaps:%d, caps:%d  ",
                //             s, (int)(stack - getstackbase(L, ptop)), ndyncap, captop);
                //      printinst(op, p);
                //#endif
                //assert(stackidx(ptop) + ndyncap == lua_gettop(L) && ndyncap <= captop);
                switch ((Opcode)p->i.code)
                {
                    case Opcode.IEnd:
                        {
                            assert(stack == storedStackBase + 1);
                            capture[captop].kind = CapKind.Cclose;
                            capture[captop].s = null;
                            return s;
                        }
                    case Opcode.IGiveup:
                        {
                            assert(stack == storedStackBase);
                            return null;
                        }
                    case Opcode.IRet:
                        {
                            assert(stack > storedStackBase && (stack - 1)->s == null);
                            p = (--stack)->p;
                            continue;
                        }
                    case Opcode.IAny:
                        {
                            if (s < e) { p++; s++; }
                            else goto fail;
                            continue;
                        }
                    case Opcode.ITestAny:
                        {
                            if (s < e) p += 2;
                            else p += getoffset(p);
                            continue;
                        }
                    case Opcode.IChar:
                        {
                            if ((byte)*s == p->i.aux && s < e) { p++; s++; }
                            else goto fail;
                            continue;
                        }
                    case Opcode.ITestChar:
                        {
                            if ((byte)*s == p->i.aux && s < e) p += 2;
                            else p += getoffset(p);
                            continue;
                        }
                    case Opcode.ISet:
                        {
                            int c = (byte)*s;
                            if (testchar((p + 1), c) && s < e)
                            { p += Charset.CharsetInstSize; s++; }
                            else goto fail;
                            continue;
                        }
                    case Opcode.ITestSet:
                        {
                            int c = (byte)*s;
                            if (testchar((p + 2), c) && s < e)
                                p += 1 + Charset.CharsetInstSize;
                            else p += getoffset(p);
                            continue;
                        }
                    case Opcode.IBehind:
                        {
                            int n = p->i.aux;
                            if (n > s - o) goto fail;
                            s -= n; p++;
                            continue;
                        }
                    case Opcode.ISpan:
                        {
                            for (; s < e; s++)
                            {
                                int c = (byte)*s;
                                if (!testchar((p + 1), c)) break;
                            }
                            p += Charset.CharsetInstSize;
                            continue;
                        }
                    case Opcode.IJmp:
                        {
                            p += getoffset(p);
                            continue;
                        }
                    case Opcode.IChoice:
                        {
                            if (stack == stacklimit) throw new Exception("stack over flow"); //stack = doublestack(L, &stacklimit, ptop);
                            stack->p = p + getoffset(p);
                            stack->s = s;
                            stack->caplevel = captop;
                            stack++;
                            p += 2;
                            continue;
                        }
                    case Opcode.ICall:
                        {
                            if (stack == stacklimit) throw new Exception("stack over flow"); //stack = doublestack(L, &stacklimit, ptop);
                            stack->s = null;
                            stack->p = p + 2;  /* save return address */
                            stack++;
                            p += getoffset(p);
                            continue;
                        }
                    case Opcode.ICommit:
                        {
                            assert(stack > storedStackBase && (stack - 1)->s != null);
                            stack--;
                            p += getoffset(p);
                            continue;
                        }
                    case Opcode.IPartialCommit:
                        {
                            assert(stack > storedStackBase && (stack - 1)->s != null);
                            (stack - 1)->s = s;
                            (stack - 1)->caplevel = captop;
                            p += getoffset(p);
                            continue;
                        }
                    case Opcode.IBackCommit:
                        {
                            assert(stack > storedStackBase && (stack - 1)->s != null);
                            s = (--stack)->s;
                            captop = stack->caplevel;
                            p += getoffset(p);
                            continue;
                        }
                    case Opcode.IFailTwice:
                        assert(stack > storedStackBase);
                        stack--;
                        goto fail;
                    /* go through */
                    case Opcode.IFail:
                        fail:
                        { /* pattern failed: try to backtrack */
                            do
                            {  /* remove pending calls */
                                assert(stack > storedStackBase);
                                s = (--stack)->s;
                            } while (s == null);
                            if (ndyncap > 0)  /* is there matchtime captures? */
                                ndyncap -= removedyncap(capture, stack->caplevel, captop, runtimeCaptures);
                            captop = stack->caplevel;
                            p = stack->p;
                            //#if defined(DEBUG)
                            //        printf("**FAIL**\n");
                            //#endif
                            continue;
                        }
                    case Opcode.ICloseRunTime:
                        {
                            CapState cs = new CapState()
                            {
                                reclevel = 0,
                                s = o,
                                ocap = capture,
                                pattern = pattern,
                                runtimeCaptures = runtimeCaptures,
                                originalString = originalString,
                            };
                            int rem, res, n;
                            int fr = runtimeCaptures.Count + 1;  /* stack index of first result */
                            n = runtimecap(ref cs, capture + captop, s, out rem);  /* call function */
                            captop -= n;  /* remove nested captures */
                            ndyncap -= rem;  /* update number of dynamic captures */
                            fr -= rem;  /* 'rem' items were popped from Lua stack */
                            res = resdyncaptures(fr, (int)(s - o), (int)(e - o), runtimeCaptures);  /* get result */
                            if (res == -1)  /* fail? */
                                goto fail;
                            s = o + res;  /* else update current position */
                            n = runtimeCaptures.Count - fr + 1;  /* number of new captures */
                            ndyncap += n;  /* update number of dynamic captures */
                            if (n == 0)  /* no new captures? */
                                captop--;  /* remove open group */
                            else
                            {  /* new captures; keep original open group */
                                if (fr + n >= short.MaxValue)
                                    throw new Exception("too many results in match-time capture");
                                /* add new captures + close group to 'capture' list */
                                capture = growcap(capture, ref capsize, captop, n + 1, executeState);
                                adddyncaptures(s, capture + captop, n, fr);
                                captop += n + 1;  /* new captures + close group */
                            }
                            p++;
                            continue;
                        }
                    case Opcode.ICloseCapture:
                        {
                            byte* s1 = s;
                            assert(captop > 0);
                            /* if possible, turn capture into a full capture */
                            if (capture[captop - 1].siz == 0 &&
                                s1 - capture[captop - 1].s < byte.MaxValue)
                            {
                                capture[captop - 1].siz = (byte)(s1 - capture[captop - 1].s + 1);
                                p++;
                                continue;
                            }
                            else
                            {
                                capture[captop].siz = 1;  /* mark entry as closed */
                                capture[captop].s = s;
                                goto pushcapture;
                            }
                        }
                    case Opcode.IOpenCapture:
                        capture[captop].siz = 0;  /* mark entry as open */
                        capture[captop].s = s;
                        goto pushcapture;
                    case Opcode.IFullCapture:
                        capture[captop].siz = (byte)(getoff(p) + 1);  /* save capture size */
                        capture[captop].s = s - getoff(p);
                        /* goto pushcapture; */
                        pushcapture:
                        {
                            capture[captop].idx = (ushort)p->i.key;
                            capture[captop].kind = (CapKind)getkind(p);
                            captop++;
                            capture = growcap(capture, ref capsize, captop, 0, executeState);
                            p++;
                            continue;
                        }
                    default: throw new Exception("should not reach here");
                }
            }
        }

    }
}
