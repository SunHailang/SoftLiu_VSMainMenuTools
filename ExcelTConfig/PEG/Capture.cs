using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace PEG
{
    public unsafe delegate void MatchTimeFunction(string content, int index, ref CaptureResult captures);
    public delegate void CaptureFunction(ref CaptureResult captures);
    public delegate void FoldFunction(ref CaptureResult captures);

    public struct CaptureResult
    {
        public int captureCount => to - from;
        public object this[int index]
        {
            get
            {
                if ((uint)index >= to - from) throw new IndexOutOfRangeException();
                return argList[index + from];
            }
        }
        public void PushResult(object result)
        {
            retList.Add(result);
            retCount++;
        }
        
        internal int retCount { get; private set; }
        internal int from;
        internal int to;
        internal IList<object> argList;
        internal IList<object> retList;

        internal CaptureResult(IList<object> argList)
        {
            this.argList = argList;
            from = 0;
            to = argList.Count;
            retCount = 0;
            retList = argList;
        }

        internal CaptureResult(IList<object> argList, IList<object> retList)
        {
            this.argList = argList;
            from = 0;
            to = argList.Count;
            retCount = 0;
            this.retList = retList;
        }

        internal void ClearArgs()
        {
            if (argList is List<object> rList)
            {
                rList.RemoveRange(from, to - from);
            }
            else throw new NotSupportedException();
        }
    }

    unsafe struct StrAux
    {
        public int isstring;
        public Capture* cp;
        public byte* s;
        public byte* e;
    }

    public static unsafe partial class LPEG
    {
        private static CapKind captype(Capture* capture) => capture->kind;
        private static bool isclosecap(Capture* capture) => captype(capture) == CapKind.Cclose;
        private static bool isfullcap(Capture* cap) => cap->siz != 0;

        private const int MAXSTRCAPS = 10;

        private static object GetObjectValue(ref CapState captureState)
        {
            return captureState.pattern.kTable[captureState.cap->idx - 1];
        }

        private static object getfromktable(ref CapState captureState, int idx)
        {
            return captureState.pattern.kTable[idx - 1];
        }

        /*
        ** Calls a runtime capture. Returns number of captures "removed" by the
        ** call, that is, those inside the group capture. Captures to be added
        ** are on the Lua stack.
        */
        static int runtimecap(ref CapState cs, Capture* close, byte* s, out int rem)
        {
            int n, id;
            int otop = cs.runtimeCaptures.Count;
            Capture* open = findopen(close);  /* get open group capture */
            assert(captype(open) == CapKind.Cgroup);
            id = finddyncap(open, close);  /* get first dynamic capture argument */
            close->kind = CapKind.Cclose;  /* closes the group */
            close->s = s;
            cs.cap = open;                 /* prepare capture state */

            var matchTimeFunction = GetObjectValue(ref cs) as MatchTimeFunction;
            var pattern = cs.pattern;
            int currentPosition = (int)(s - cs.s);
            //var nestedCaptures = pushnestedvalues(ref cs, false);  /* push nested captures */

            //TODO!!!!!!
            var captureResult = new CaptureResult();
            matchTimeFunction(cs.originalString, currentPosition, ref captureResult);
            //var results = nestedCaptures
            throw new Exception("TODO!");

            if (id > 0)
            {  /* are there old dynamic captures to be removed? */
                int i;
                for (i = id; i <= otop; i++)
                    cs.runtimeCaptures.RemoveAt(i);  /* remove old dynamic captures */
                rem = otop - id + 1;  /* total number of dynamic captures removed */
            }
            else
                rem = 0;  /* no dynamic captures removed */
            return (int)(close - open - 1);  /* number of captures to be removed */
        }

        /*
        ** Goes back in a list of captures looking for an open capture
        ** corresponding to a close
        */
        static Capture* findopen(Capture* cap)
        {
            int n = 0;  /* number of closes waiting an open */
            for (; ; )
            {
                cap--;
                if (isclosecap(cap)) n++;  /* one more open to skip */
                else if (!isfullcap(cap))
                {
                    if (n-- == 0) return cap;
                }
            }
        }

        /*
        ** Return the stack index of the first runtime capture in the given
        ** list of captures (or zero if no runtime captures)
        */
        static int finddyncap(Capture* cap, Capture* last)
        {
            for (; cap < last; cap++)
            {
                if (cap->kind == CapKind.Cruntime)
                    return cap->idx;  /* stack position of first capture */
            }
            return 0;  /* no dynamic captures in this segment */
        }

        /*
        ** Push on the Lua stack all values generated by nested captures inside
        ** the current capture. Returns number of values pushed. 'addextra'
        ** makes it push the entire match after all captured values. The
        ** entire match is pushed also if there are no other nested values,
        ** so the function never returns zero.
        */
        static int pushnestedvalues(ref CapState cs, bool addextra, List<object> results)
        {
            Capture* co = cs.cap;
            if (isfullcap(cs.cap++))
            {  /* no nested captures? */
                results.Add(Encoding.UTF8.GetString(co->s, co->siz - 1));/* push whole match */
                return 1;
            }
            else
            {
                int n = 0;
                while (!isclosecap(cs.cap))  /* repeat for all nested patterns */
                    n += pushcapture(ref cs, results);
                if (addextra || n == 0)
                {  /* need extra? */
                    results.Add(Encoding.UTF8.GetString(co->s, (int)(cs.cap->s - co->s)));/* push whole match */
                    n++;
                }
                cs.cap++;  /* skip close entry */
                return n;
            }
        }

        /*
        ** Push all values of the current capture into the stack; returns
        ** number of values pushed
        */
        static int pushcapture(ref CapState cs, List<object> results)
        {
            int res;
            if (cs.reclevel++ > MAXRECLEVEL)
                throw new Exception("subcapture nesting too deep");
            switch (captype(cs.cap))
            {
                case CapKind.Cposition:
                    {
                        results.Add((int)(cs.cap->s - cs.s + 1));
                        cs.cap++;
                        res = 1;
                        break;
                    }
                case CapKind.Cconst:
                    {
                        results.Add(GetObjectValue(ref cs));
                        cs.cap++;
                        res = 1;
                        break;
                    }
                case CapKind.Carg:
                    {
                        throw new Exception("Carg TODO!!!!!!!");
                        //int arg = (cs.cap++)->idx;
                        //if (arg + FIXEDARGS > cs->ptop)
                        //    return luaL_error(L, "reference to absent extra argument #%d", arg);
                        //lua_pushvalue(L, arg + FIXEDARGS);
                        //res = 1;
                        //break;
                    }
                case CapKind.Csimple:
                    {
                        int k = pushnestedvalues(ref cs, true, results);
                        var last = results[results.Count - 1];
                        results.RemoveAt(results.Count - 1);
                        results.Insert(results.Count - k + 1, last); /* make whole match be first result */
                        res = k;
                        break;
                    }
                case CapKind.Cruntime:
                    {
                        results.Add(cs.runtimeCaptures[(cs.cap++)->idx]);
                        res = 1;
                        break;
                    }
                case CapKind.Cstring:
                    {
                        var sb = new StringBuilder();
                        stringcap(sb, ref cs);
                        results.Add(sb.ToString());
                        res = 1;
                        break;
                    }
                case CapKind.Csubst:
                    {
                        var sb = new StringBuilder();
                        substcap(sb, ref cs);
                        results.Add(sb.ToString());
                        res = 1;
                        break;
                    }
                case CapKind.Cgroup:
                    {
                        if (cs.cap->idx == 0)  /* anonymous group? */
                        {
                            res = pushnestedvalues(ref cs, false, results);/* add all nested values */
                        }
                        else
                        {  /* named group: add no values */
                            nextcap(ref cs);  /* skip capture */
                            res = 0;
                        }
                        break;
                    }
                case CapKind.Cbackref: res = backrefcap(ref cs, results); break;
                case CapKind.Ctable: res = tablecap(ref cs, results); break;
                case CapKind.Cfunction: res = functioncap(ref cs, results); break;
                case CapKind.Cnum: res = numcap(ref cs, results); break;
                case CapKind.Cquery: res = querycap(ref cs, results); break;
                case CapKind.Cfold: res = foldcap(ref cs, results); break;
                default: throw new Exception("should not reach here");
            }
            cs.reclevel--;
            return res;
        }

        /*
        ** Interpret the result of a dynamic capture: false -> fail;
        ** true -> keep current position; number -> next position.
        ** Return new subject position. 'fr' is stack index where
        ** is the result; 'curr' is current subject position; 'limit'
        ** is subject's size.
        */
        static int resdyncaptures(int firstIndex, int curr, int limit, List<object> runtimeCaptures)
        {
            int res;
            var firstResultValue = runtimeCaptures[firstIndex];
            if(firstResultValue is bool bValue)
            {
                if (bValue) res = curr;
                else
                {
                    runtimeCaptures.RemoveRange(firstIndex, runtimeCaptures.Count - firstIndex);
                    return -1;
                }
            }
            else
            {
                res = (int)firstResultValue;
                if (res < curr || res > limit) throw new Exception("invalid position returned by match-time capture");
            }
            runtimeCaptures.RemoveAt(firstIndex);  /* remove first result (offset) */
            return res;
        }

        /*
        ** Add capture values returned by a dynamic capture to the list
        ** 'capture', nested inside a group. 'fd' indexes the first capture
        ** value, 'n' is the number of values (at least 1). The open group
        ** capture is already in 'capture', before the place for the new entries.
        */
        static void adddyncaptures(byte* s, Capture* capture, int n, int fd)
        {
            int i;
            assert(capture[-1].kind == CapKind.Cgroup && capture[-1].siz == 0);
            capture[-1].idx = 0;  /* make group capture an anonymous group */
            for (i = 0; i < n; i++)
            {  /* add runtime captures */
                capture[i].kind = CapKind.Cruntime;
                capture[i].siz = 1;  /* mark it as closed */
                capture[i].idx = (ushort)(fd + i);  /* stack index of capture value */
                capture[i].s = s;
            }
            capture[n].kind = CapKind.Cclose;  /* close group */
            capture[n].siz = 1;
            capture[n].s = s;
        }

        /*
        ** Remove dynamic captures from the Lua stack (called in case of failure)
        */
        static int removedyncap(Capture* capture, int level, int last, List<object> runtimeCaptures)
        {
            int id = finddyncap(capture + level, capture + last);  /* index of 1st cap. */
            int top = runtimeCaptures.Count;
            if (id == 0) return 0;  /* no dynamic captures? */
            runtimeCaptures.RemoveRange(id, top - id);
            return top - id + 1;  /* number of values removed */
        }

        /*
        ** Prepare a CapState structure and traverse the entire list of
        ** captures in the stack pushing its results. 's' is the subject
        ** string, 'r' is the final position of the match, and 'ptop' 
        ** the index in the stack where some useful values were pushed.
        ** Returns the number of results pushed. (If the list produces no
        ** results, push the final position of the match.)
        */
        static object[] getcaptures(byte* s, byte* r, Capture* capture, Pattern pattern, string originalString)
        {
            var results = new List<object>();
            int n = 0;
            if (!isclosecap(capture))
            {  /* is there any capture? */
                CapState cs = new CapState
                {
                    ocap = capture,
                    cap = capture,
                    reclevel = 0,
                    s = s,
                    pattern = pattern,
                    originalString = originalString,
                };

                do
                {  /* collect their values */
                    n += pushcapture(ref cs, results);
                } while (!isclosecap(cs.cap));
            }

            if (n == 0)
            {  /* no capture values? */
                return new object[] { (int)(r - s + 1) };
            }
            return results.ToArray();
        }


        /*
        ** String capture: add result to buffer 'b' (instead of pushing
        ** it into the stack)
        */
        static void stringcap(StringBuilder sb, ref CapState cs)
        {
            StrAux* cps = stackalloc StrAux[MAXSTRCAPS];
            int n;
            int i;

            string fmt = GetObjectValue(ref cs) as string;
            int len = fmt.Length;

            n = getstrcaps(ref cs, cps, 0) - 1;  /* collect nested captures */

            for (i = 0; i < len; i++)
            {  /* traverse them */
                if (fmt[i] != '%')  /* not an escape? */
                    sb.Append(fmt[i]);  /* add it to buffer */
                else if (fmt[++i] < '0' || fmt[i] > '9')  /* not followed by a digit? */
                    sb.Append(fmt[i]);  /* add to buffer */
                else
                {
                    int l = fmt[i] - '0';  /* capture index */
                    if (l > n) throw new Exception($"invalid capture index ({l})");
                    else if (cps[l].isstring != 0)
                    {
                        byte* start = cps[1].s;
                        byte* end = cps[1].e;
                        for(byte* cursor = start; cursor != end; cursor++)
                        {
                            sb.Append((char)(*cursor));
                        }
                    }
                    else
                    {
                        Capture* curr = cs.cap;
                        cs.cap = cps[l].cp;  /* go back to evaluate that nested capture */
                        if (addonestring(sb, ref cs, "capture") == 0)
                            throw new Exception($"no values in capture index {l}");
                        cs.cap = curr;  /* continue from where it stopped */
                    }
                }
            }
        }

        /*
        ** Put at the cache for Lua values the value indexed by 'v' in ktable
        ** of the running pattern (if it is not there yet); returns its index.
        */
        //static int updatecache(ref CapState cs, int v)
        //{
        //    int idx = cs->ptop + 1;  /* stack index of cache for Lua values */
        //    if (v != cs.valuecached)
        //    {  /* not there? */
        //        getfromktable(cs, v);  /* get value from 'ktable' */
        //        lua_replace(cs->L, idx);  /* put it at reserved stack position */
        //        cs.valuecached = v;  /* keep track of what is there */
        //    }
        //    return idx;
        //}

        static byte* closeaddr(Capture* c) => c->s + c->siz - 1;

        /*
        ** Collect values from current capture into array 'cps'. Current
        ** capture must be Cstring (first call) or Csimple (recursive calls).
        ** (In first call, fills %0 with whole match for Cstring.)
        ** Returns number of elements in the array that were filled.
        */
        static int getstrcaps(ref CapState cs, StrAux* cps, int n)
        {
            int k = n++;
            cps[k].isstring = 1;  /* get string value */
            cps[k].s = cs.cap->s;  /* starts here */
            if (!isfullcap(cs.cap++))
            {  /* nested captures? */
                while (!isclosecap(cs.cap))
                {  /* traverse them */
                    if (n >= MAXSTRCAPS)  /* too many captures? */
                        nextcap(ref cs);  /* skip extra captures (will not need them) */
                    else if (captype(cs.cap) == CapKind.Csimple)  /* string? */
                        n = getstrcaps(ref cs, cps, n);  /* put info. into array */
                    else
                    {
                        cps[n].isstring = 0;  /* not a string */
                        cps[n].cp = cs.cap;  /* keep original capture */
                        nextcap(ref cs);
                        n++;
                    }
                }
                cs.cap++;  /* skip close */
            }
            cps[k].e = closeaddr(cs.cap - 1);  /* ends here */
            return n;
        }


        /*
        ** Go to the next capture
        */
        static void nextcap(ref CapState cs)
        {
            Capture* cap = cs.cap;
            if (!isfullcap(cap))
            {  /* not a single capture? */
                int n = 0;  /* number of opens waiting a close */
                for (; ; )
                {  /* look for corresponding close */
                    cap++;
                    if (isclosecap(cap))
                    {
                        if (n-- == 0) break;
                    }
                    else if (!isfullcap(cap)) n++;
                }
            }
            cs.cap = cap + 1;  /* + 1 to skip last close (or entire single capture) */
        }

        /*
        ** Evaluates a capture and adds its first value to buffer 'b'; returns
        ** whether there was a value
        */
        static int addonestring(StringBuilder b, ref CapState cs, string what)
        {
            switch (captype(cs.cap))
            {
                case CapKind.Cstring:
                    stringcap(b, ref cs);  /* add capture directly to buffer */
                    return 1;
                case CapKind.Csubst:
                    substcap(b, ref cs);  /* add capture directly to buffer */
                    return 1;
                default:
                    {
                        var results = new List<object>();
                        int n = pushcapture(ref cs, results);
                        if (n > 0)
                        {
                            //if (n > 1) lua_pop(L, n - 1);  /* only one result */
                            var result = results[0] as string;
                            if (result == null)
                            {
                                var resultType = results[0] == null ? "null" : results[0].GetType().ToString();
                                throw new Exception($"invalid {what} value (a {resultType})");
                            }
                            b.Append(result);
                        }
                        return n;
                    }
            }
        }

        /*
        ** Substitution capture: add result to buffer 'b'
        */
        static void substcap(StringBuilder b, ref CapState cs)
        {
            byte* curr = cs.cap->s;
            if (isfullcap(cs.cap))  /* no nested captures? */
            {
                for (int it = 0; it < cs.cap->siz - 1; it++) b.Append((char)*(curr + it));  /* keep original text */
            }
            else
            {
                cs.cap++;  /* skip open entry */
                while (!isclosecap(cs.cap))
                {  /* traverse nested captures */
                    byte* next = cs.cap->s;
                    for (int it = 0; it < next - curr; it++) b.Append((char)*(curr + it)); /* add text up to capture */
                    if (addonestring(b, ref cs, "replacement") != 0)
                        curr = closeaddr(cs.cap - 1);  /* continue after match */
                    else  /* no capture value */
                        curr = next;  /* keep original text in final result */
                }
                for (int it = 0; it < cs.cap->s - curr; it++) b.Append((char)*(curr + it)); /* add last piece of text */
            }
            cs.cap++;  /* go to next capture */
        }

        /*
        ** Push only the first value generated by nested captures
        */
        static void pushonenestedvalue(ref CapState cs, List<object> results)
        {
            int n = pushnestedvalues(ref cs, false, results);

            /* pop extra values */
            if (n > 1) results.RemoveRange(results.Count - n + 1, n - 1);
        }


        /*
        ** Table capture: creates a new table and populates it with nested
        ** captures.
        */
        static int tablecap(ref CapState cs, List<object> results)
        {
            int n = 0;
            var nestResults = new List<object>();
            if (isfullcap(cs.cap++))/* table is empty */
            {
                results.Add(nestResults.ToArray());
                return 1;
            }
            while (!isclosecap(cs.cap))
            {
                if (captype(cs.cap) == CapKind.Cgroup && cs.cap->idx != 0)
                {  /* named group? */
                    var groupName = GetObjectValue(ref cs);  /* push group name */
                    pushonenestedvalue(ref cs, nestResults);
                    var groupValue = nestResults[nestResults.Count - 1];
                    nestResults.RemoveAt(nestResults.Count - 1);
                    nestResults.Add(new object[] { groupName, groupValue });
                }
                else
                {  /* not a named group */
                    int i;
                    int k = pushcapture(ref cs, nestResults);/* store all values into table */
                    n += k;
                }
            }
            cs.cap++;  /* skip close entry */
            results.Add(nestResults.ToArray());
            return 1;  /* number of values pushed (only the table) */
        }


        /*
        ** Function capture
        */
        static int functioncap(ref CapState cs, List<object> results)
        {
            var captureFunction = GetObjectValue(ref cs) as CaptureFunction;  /* push function */
            int prevCount = results.Count;

            int n = pushnestedvalues(ref cs, false, results);  /* push nested captures */

            var captureResult = new CaptureResult
            {
                argList = results,
                from = prevCount,
                to = prevCount + n,
                retList = results,
            };

            captureFunction(ref captureResult);  /* call function */

            results.RemoveRange(prevCount, n);

            return results.Count - prevCount; /* return function's results */
        }

        /*
        ** Select capture
        */
        static int numcap(ref CapState cs, List<object> results)
        {
            int idx = cs.cap->idx;  /* value to select */
            if (idx == 0)
            {  /* no values? */
                nextcap(ref cs);  /* skip entire capture */
                return 0;  /* no value produced */
            }
            else
            {
                int n = pushnestedvalues(ref cs, false, results);
                if (n < idx)  /* invalid index? */
                    throw new Exception($"no capture '{idx}'");
                else
                {
                    var result = results[results.Count - n + idx - 1];
                    results.RemoveRange(results.Count - n, n);
                    results.Add(result);
                    //lua_pushvalue(cs->L, -(n - idx + 1));  /* get selected capture */
                    //lua_replace(cs->L, -(n + 1));  /* put it in place of 1st capture */
                    //lua_pop(cs->L, n - 1);  /* remove other captures */
                    return 1;
                }
            }
        }

        /*
        ** Table-query capture
        */
        static int querycap(ref CapState cs, List<object> results)
        {
            int idx = cs.cap->idx;
            pushonenestedvalue(ref cs, results);  /* get nested capture */
            //lua_gettable(cs->L, updatecache(cs, idx));  /* query cap. value at table */

            var nestCapture = results[results.Count - 1];
            results.RemoveAt(results.Count - 1);

            var dict = GetObjectValue(ref cs) as Dictionary<object, object>;
            object result;
            if(dict.TryGetValue(nestCapture, out result))
            {
                results.Add(result);
                return 1;
            }
            else return 0;
        }

        /*
        ** Fold capture
        */
        static int foldcap(ref CapState cs, List<object> results)
        {
            var foldList = new List<object>();
            int n;
            int idx = cs.cap->idx;
            if (isfullcap(cs.cap++) ||  /* no nested captures? */
                isclosecap(cs.cap) ||  /* no nested captures (large subject)? */
                (n = pushcapture(ref cs, foldList)) == 0)  /* nested captures with no values? */
                throw new Exception("no initial value for fold capture");
            if (n > 1)
            {
                foldList.RemoveRange(1, n - 1); /* leave only one result for accumulator */
            }

            var foldFunction = GetObjectValue(ref cs) as FoldFunction;/* get folding function */

            while (!isclosecap(cs.cap))
            {
                n = pushcapture(ref cs, foldList);  /* get next capture's values */
                var captureResult = new CaptureResult(foldList);
                foldFunction(ref captureResult);/* call folding function */
                foldList.RemoveRange(0, foldList.Count - captureResult.retCount);
            }
            cs.cap++;  /* skip close entry */
            results.Add(foldList);
            return 1;  /* only accumulator left on the stack */
        }

        /*
        ** Back-reference capture. Return number of values pushed.
        */
        static int backrefcap(ref CapState cs, List<object> results)
        {
            int n;
            Capture* curr = cs.cap;
            var referenceName = GetObjectValue(ref cs);  /* reference name */
            cs.cap = findback(ref cs, curr, referenceName);  /* find corresponding group */

            n = pushnestedvalues(ref cs, false, results);  /* push group's values */
            cs.cap = curr + 1;
            return n;
        }

        /*
        ** Try to find a named group capture with the name given at the top of
        ** the stack; goes backward from 'cap'.
        */
        static Capture* findback(ref CapState cs, Capture* cap, object referenceName)
        {
            while (cap-- > cs.ocap)
            {  /* repeat until end of list */
                if (isclosecap(cap))
                    cap = findopen(cap);  /* skip nested captures */
                else if (!isfullcap(cap))
                    continue; /* opening an enclosing capture: skip and get previous */
                if (captype(cap) == CapKind.Cgroup)
                {
                    if (referenceName.Equals(getfromktable(ref cs, cap->idx)))
                    {  /* right group? */
                        return cap;
                    }
                }
            }
            throw new Exception($"back reference '{referenceName}' not found");
        }
    }
}
