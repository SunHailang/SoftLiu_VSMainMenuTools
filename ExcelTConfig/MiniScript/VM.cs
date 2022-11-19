using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MiniScript.OpCodes;

namespace MiniScript
{
    public static unsafe class VM
    {
        public enum Type : int
        {
            Boolean,
            Integer,
            Float,
            Function,
        }

        public unsafe struct Value
        {
            public Type type { get; private set; }
            private int value;

            public static implicit operator Value(int value) => new Value { type = Type.Integer, value = value };
            public static implicit operator Value(float value) => new Value { type = Type.Float, value = *(int*)&value };
            public static implicit operator Value(bool value) => new Value { type = Type.Boolean, value = value ? 1 : 0 };

            public static Value operator +(Value v1, Value v2)
            {
                Value v;
                if (v1.type == Type.Integer)
                {
                    if (v2.type == Type.Integer) v = (int)v1 + (int)v2;
                    else if (v2.type == Type.Float) v = (int)v1 + (float)v2;
                    else throw new Exception($"unsuitable type {v2.type} for Operator +");
                }
                else if (v1.type == Type.Float)
                {
                    if (v2.type == Type.Integer) v = (float)v1 + (int)v2;
                    else if (v2.type == Type.Float) v = (float)v1 + (float)v2;
                    else throw new Exception($"unsuitable type {v2.type} for Operator +");
                }
                else throw new Exception($"unsuitable type {v1.type} for Operator +");
                return v;
            }

            public static Value operator -(Value v1, Value v2)
            {
                Value v;
                if (v1.type == Type.Integer)
                {
                    if (v2.type == Type.Integer) v = (int)v1 - (int)v2;
                    else if (v2.type == Type.Float) v = (int)v1 - (float)v2;
                    else throw new Exception($"unsuitable type {v2.type} for Operator -");
                }
                else if (v1.type == Type.Float)
                {
                    if (v2.type == Type.Integer) v = (float)v1 - (int)v2;
                    else if (v2.type == Type.Float) v = (float)v1 - (float)v2;
                    else throw new Exception($"unsuitable type {v2.type} for Operator -");
                }
                else throw new Exception($"unsuitable type {v1.type} for Operator -");
                return v;
            }

            public static Value operator - (Value v1)
            {
                Value v;
                if (v1.type == Type.Integer) v = -(int)v1;
                else if (v1.type == Type.Float) v = -(float)v1;
                else throw new Exception($"unsuitable type {v1.type} for Operator -");
                return v;
            }

            public static Value operator * (Value v1, Value v2)
            {
                Value v;
                if (v1.type == Type.Integer)
                {
                    if (v2.type == Type.Integer) v = (int)v1 * (int)v2;
                    else if (v2.type == Type.Float) v = (int)v1 * (float)v2;
                    else throw new Exception($"unsuitable type {v2.type} for Operator *");
                }
                else if (v1.type == Type.Float)
                {
                    if (v2.type == Type.Integer) v = (float)v1 * (int)v2;
                    else if (v2.type == Type.Float) v = (float)v1 * (float)v2;
                    else throw new Exception($"unsuitable type {v2.type} for Operator *");
                }
                else throw new Exception($"unsuitable type {v1.type} for Operator *");
                return v;
            }

            public static Value operator /(Value v1, Value v2)
            {
                Value v;
                if (v1.type == Type.Integer)
                {
                    if (v2.type == Type.Integer) v = (int)v1 / (int)v2;
                    else if (v2.type == Type.Float) v = (int)v1 / (float)v2;
                    else throw new Exception($"unsuitable type {v2.type} for Operator /");
                }
                else if (v1.type == Type.Float)
                {
                    if (v2.type == Type.Integer) v = (float)v1 / (int)v2;
                    else if (v2.type == Type.Float) v = (float)v1 / (float)v2;
                    else throw new Exception($"unsuitable type {v2.type} for Operator /");
                }
                else throw new Exception($"unsuitable type {v1.type} for Operator /");
                return v;
            }

            public static Value operator %(Value v1, Value v2)
            {
                Value v;
                if (v1.type == Type.Integer)
                {
                    if (v2.type == Type.Integer) v = (int)v1 % (int)v2;
                    else if (v2.type == Type.Float) v = (int)v1 % (float)v2;
                    else throw new Exception($"unsuitable type {v2.type} for Operator %");
                }
                else if (v1.type == Type.Float)
                {
                    if (v2.type == Type.Integer) v = (float)v1 % (int)v2;
                    else if (v2.type == Type.Float) v = (float)v1 % (float)v2;
                    else throw new Exception($"unsuitable type {v2.type} for Operator %");
                }
                else throw new Exception($"unsuitable type {v1.type} for Operator %");
                return v;
            }

            public static explicit operator int(Value value)
            {
                if (value.type == Type.Integer) return value.value;
                throw new Exception($"could not cast {value.type} to {Type.Integer}");
            }

            public static explicit operator float(Value value)
            {
                if (value.type == Type.Float) return *(float*)&value.value;
                throw new Exception($"could not cast {value.type} to {Type.Float}");
            }

            public static explicit operator bool(Value value)
            {
                if (value.type == Type.Boolean) return value.value != 0;
                throw new Exception($"could not cast {value.type} to {Type.Boolean}");
            }

            public override string ToString()
            {
                switch (type)
                {
                    case Type.Integer: return ((int)this).ToString();
                    case Type.Float: return ((float)this).ToString();
                    case Type.Boolean: return ((bool)this).ToString();
                    default: return $"type: {type}";
                }
            }
        }

        public static Value Evaluate<T>(T codes) where T: IList<byte>
        {
            arguments.Clear();
            return EvaluateInternal(codes);
        }

        public static Value Evaluate<T>(T codes, Value v0) where T : IList<byte>
        {
            arguments.Clear();
            arguments.Add(v0);
            return EvaluateInternal(codes);
        }

        public static Value Evaluate<T>(T codes, Value v0, Value v1) where T : IList<byte>
        {
            arguments.Clear();
            arguments.Add(v0); arguments.Add(v1);
            return EvaluateInternal(codes);
        }

        public static Value Evaluate<T>(T codes, Value v0, Value v1, Value v2) where T : IList<byte>
        {
            arguments.Clear();
            arguments.Add(v0); arguments.Add(v1); arguments.Add(v2);
            return EvaluateInternal(codes);
        }

        public static Value Evaluate<T>(T codes, Value v0, Value v1, Value v2, Value v3) where T : IList<byte>
        {
            arguments.Clear();
            arguments.Add(v0); arguments.Add(v1); arguments.Add(v2); arguments.Add(v3);
            return EvaluateInternal(codes);
        }

        public static Value Evaluate<T>(T codes, IList<Value> vi) where T : IList<byte>
        {
            arguments.Clear();
            for (int it = 0; it < vi.Count; it++) arguments.Add(vi[it]);
            return EvaluateInternal(codes);
        }

        private static Value[] stack;
        private static List<Value> arguments = new List<Value>();
        private static Value[] localValues;

        private static Value EvaluateInternal<T>(T format) where T : IList<byte>
        {
            var arguments = VM.arguments;

            int it = 0;
            {
                int localVarCount = format[it++];
                if (localVarCount > 0)
                {
                    if (localValues == null) localValues = new Value[localVarCount];
                    else if (localValues.Length < localVarCount) Array.Resize(ref localValues, localVarCount);
                }

                int maxStack = format[it++];
                if (VM.stack == null) VM.stack = new Value[maxStack];
                else if (VM.stack.Length < maxStack) Array.Resize(ref VM.stack, maxStack);
            }

            var stack = VM.stack;
            int top = -1;

            int formatLength = format.Count;
            for(; it < formatLength; it++)
            {
                head: { }
                var opCode = (OpCode)format[it];

                switch(opCode)
                {
                    case OpCode.LoadArgument:
                        {
                            int index = format[++it];
                            stack[++top] = arguments[index];
                        }
                        break;

                    case OpCode.LoadLocalVariable:
                        {
                            int index = format[++it];
                            stack[++top] = localValues[index];
                        }
                        break;

                    case OpCode.LoadInteger32bit:
                        {
                            byte b0 = format[++it];
                            byte b1 = format[++it];
                            byte b2 = format[++it];
                            byte b3 = format[++it];
                            stack[++top] = b0 | (b1 << 8) | (b2 << 16) | (b3 << 24);
                        }
                        break;

                    case OpCode.LoadInteger16bit:
                        {
                            byte b0 = format[++it];
                            byte b1 = format[++it];
                            stack[++top] = (short)(b0 | (b1 << 8));
                        }
                        break;

                    case OpCode.LoadInteger8bit:
                        {
                            byte b0 = format[++it];
                            stack[++top] = (sbyte)b0;
                        }
                        break;

                    case OpCode.LoadFloat:
                        {
                            byte b0 = format[++it];
                            byte b1 = format[++it];
                            byte b2 = format[++it];
                            byte b3 = format[++it];
                            int constI = b0 | (b1 << 8) | (b2 << 16) | (b3 << 24);
                            stack[++top] = *(float*)&constI;
                        }
                        break;

                    case OpCode.LoadTrue: stack[++top] = true; break;
                    case OpCode.LoadFalse: stack[++top] = false; break;

                    case OpCode.StoreArgument:
                        {
                            int index = format[++it];
                            arguments[index] = stack[top--];
                        }
                        break;

                    case OpCode.StoreLocalVariable:
                        {
                            int index = format[++it];
                            localValues[index] = stack[top--];
                        }
                        break;

                    case OpCode.Add:
                        {
                            var v2 = stack[top];
                            var v1 = stack[top - 1];

                            stack[--top] = v1 + v2;
                        }
                        break;

                    case OpCode.Substract:
                        {
                            var v2 = stack[top];
                            var v1 = stack[top - 1];

                            stack[--top] = v1 - v2;
                        }
                        break;

                    case OpCode.Multify:
                        {
                            var v2 = stack[top];
                            var v1 = stack[top - 1];

                            stack[--top] = v1 * v2;
                        }
                        break;

                    case OpCode.Divide:
                        {
                            var v2 = stack[top];
                            var v1 = stack[top - 1];

                            stack[--top] = v1 / v2;
                        }
                        break;

                    case OpCode.Mods:
                        {
                            var v2 = stack[top];
                            var v1 = stack[top - 1];

                            stack[--top] = v1 % v2;
                        }
                        break;

                    case OpCode.Negate:
                        {
                            var v1 = stack[top];

                            stack[top] = -v1;
                        }
                        break;

                    case OpCode.GT:
                        {
                            var v2 = stack[top];
                            var v1 = stack[top - 1];

                            Value v;
                            if (v1.type == Type.Integer)
                            {
                                if (v2.type == Type.Integer) v = (int)v1 > (int)v2;
                                else if (v2.type == Type.Float) v = (int)v1 > (float)v2;
                                else throw new Exception($"unsuitable type {v2.type} for Operator {opCode}");
                            }
                            else if (v1.type == Type.Float)
                            {
                                if (v2.type == Type.Integer) v = (float)v1 > (int)v2;
                                else if (v2.type == Type.Float) v = (float)v1 > (float)v2;
                                else throw new Exception($"unsuitable type {v2.type} for Operator {opCode}");
                            }
                            else throw new Exception($"unsuitable type {v1.type} for Operator {opCode}");

                            stack[--top] = v;
                        }
                        break;

                    case OpCode.LT:
                        {
                            var v2 = stack[top];
                            var v1 = stack[top - 1];

                            Value v;
                            if (v1.type == Type.Integer)
                            {
                                if (v2.type == Type.Integer) v = (int)v1 < (int)v2;
                                else if (v2.type == Type.Float) v = (int)v1 < (float)v2;
                                else throw new Exception($"unsuitable type {v2.type} for Operator {opCode}");
                            }
                            else if (v1.type == Type.Float)
                            {
                                if (v2.type == Type.Integer) v = (float)v1 < (int)v2;
                                else if (v2.type == Type.Float) v = (float)v1 < (float)v2;
                                else throw new Exception($"unsuitable type {v2.type} for Operator {opCode}");
                            }
                            else throw new Exception($"unsuitable type {v1.type} for Operator {opCode}");

                            stack[--top] = v;
                        }
                        break;

                    case OpCode.GE:
                        {
                            var v2 = stack[top];
                            var v1 = stack[top - 1];

                            Value v;
                            if (v1.type == Type.Integer)
                            {
                                if (v2.type == Type.Integer) v = (int)v1 >= (int)v2;
                                else if (v2.type == Type.Float) v = (int)v1 >= (float)v2;
                                else throw new Exception($"unsuitable type {v2.type} for Operator {opCode}");
                            }
                            else if (v1.type == Type.Float)
                            {
                                if (v2.type == Type.Integer) v = (float)v1 >= (int)v2;
                                else if (v2.type == Type.Float) v = (float)v1 >= (float)v2;
                                else throw new Exception($"unsuitable type {v2.type} for Operator {opCode}");
                            }
                            else throw new Exception($"unsuitable type {v1.type} for Operator {opCode}");

                            stack[--top] = v;
                        }
                        break;

                    case OpCode.LE:
                        {
                            var v2 = stack[top];
                            var v1 = stack[top - 1];

                            Value v;
                            if (v1.type == Type.Integer)
                            {
                                if (v2.type == Type.Integer) v = (int)v1 <= (int)v2;
                                else if (v2.type == Type.Float) v = (int)v1 <= (float)v2;
                                else throw new Exception($"unsuitable type {v2.type} for Operator {opCode}");
                            }
                            else if (v1.type == Type.Float)
                            {
                                if (v2.type == Type.Integer) v = (float)v1 <= (int)v2;
                                else if (v2.type == Type.Float) v = (float)v1 <= (float)v2;
                                else throw new Exception($"unsuitable type {v2.type} for Operator {opCode}");
                            }
                            else throw new Exception($"unsuitable type {v1.type} for Operator {opCode}");

                            stack[--top] = v;
                        }
                        break;

                    case OpCode.EQ:
                        {
                            var v2 = stack[top];
                            var v1 = stack[top - 1];

                            Value v;
                            if (v1.type == Type.Integer)
                            {
                                if (v2.type == Type.Integer) v = (int)v1 == (int)v2;
                                else if (v2.type == Type.Float) v = (int)v1 == (float)v2;
                                else throw new Exception($"unsuitable type {v2.type} for Operator {opCode}");
                            }
                            else if (v1.type == Type.Float)
                            {
                                if (v2.type == Type.Integer) v = (float)v1 == (int)v2;
                                else if (v2.type == Type.Float) v = (float)v1 == (float)v2;
                                else throw new Exception($"unsuitable type {v2.type} for Operator {opCode}");
                            }
                            else throw new Exception($"unsuitable type {v1.type} for Operator {opCode}");

                            stack[--top] = v;
                        }
                        break;

                    case OpCode.NE:
                        {
                            var v2 = stack[top];
                            var v1 = stack[top - 1];

                            Value v;
                            if (v1.type == Type.Integer)
                            {
                                if (v2.type == Type.Integer) v = (int)v1 != (int)v2;
                                else if (v2.type == Type.Float) v = (int)v1 != (float)v2;
                                else throw new Exception($"unsuitable type {v2.type} for Operator {opCode}");
                            }
                            else if (v1.type == Type.Float)
                            {
                                if (v2.type == Type.Integer) v = (float)v1 != (int)v2;
                                else if (v2.type == Type.Float) v = (float)v1 != (float)v2;
                                else throw new Exception($"unsuitable type {v2.type} for Operator {opCode}");
                            }
                            else throw new Exception($"unsuitable type {v1.type} for Operator {opCode}");

                            stack[--top] = v;
                        }
                        break;

                    case OpCode.And:
                        {
                            var v2 = stack[top];
                            var v1 = stack[top - 1];

                            Value v;
                            if (v1.type != Type.Boolean) throw new Exception($"unsuitable type {v1.type} for Operator {opCode}");
                            if (v2.type != Type.Boolean) throw new Exception($"unsuitable type {v2.type} for Operator {opCode}");
                            v = (bool)v1 && (bool)v2;

                            stack[--top] = v;
                        }
                        break;

                    case OpCode.Or:
                        {
                            var v2 = stack[top];
                            var v1 = stack[top - 1];

                            Value v;
                            if (v1.type != Type.Boolean) throw new Exception($"unsuitable type {v1.type} for Operator {opCode}");
                            if (v2.type != Type.Boolean) throw new Exception($"unsuitable type {v2.type} for Operator {opCode}");
                            v = (bool)v1 || (bool)v2;

                            stack[--top] = v;
                        }
                        break;

                    case OpCode.Not:
                        {
                            var v1 = stack[top];

                            Value v;
                            if (v1.type != Type.Boolean) throw new Exception($"unsuitable type {v1.type} for Operator {opCode}");
                            v = !(bool)v1;

                            stack[top] = v;
                        }
                        break;

                    case OpCode.CallGlobal:
                        {
                            int index = format[++it];
                            int argCount = format[++it];
                            var funcInfo = GlobalFunctions.functions[index];
                            var callInfo = new CallInfo
                            {
                                stack = stack,
                                from = top + 1 - argCount,
                                to = top + 1,
                            };
                            funcInfo.func(ref callInfo);
                            top += 1 - argCount;
                            stack[top] = callInfo.returnValue;
                        }
                        break;

                    case OpCode.Ret:
                        {
                            var ret = stack[top--];
                            if (top != -1) throw new Exception("stack not balance!");
                            arguments.Clear();
                            return ret;
                        }

                    case OpCode.Pop:
                        {
                            top--;
                        }
                        break;

                    case OpCode.Dup:
                        {
                            stack[top + 1] = stack[top];
                            top++;
                        }
                        break;

                    case OpCode.GoTo:
                        {
                            byte b0 = format[++it];
                            byte b1 = format[++it];
                            ushort offset = (ushort)(b0 | (b1 << 8));
                            it = offset;
                            goto head;
                        }

                    case OpCode.GoToOnTrue:
                        {
                            byte b0 = format[++it];
                            byte b1 = format[++it];
                            ushort offset = (ushort)(b0 | (b1 << 8));
                            var v1 = stack[top--];
                            if (v1.type == Type.Boolean)
                            {
                                if((bool)v1)
                                {
                                    it = offset;
                                    goto head;
                                }
                            }
                            else throw new Exception($"unsuitable type {v1.type} for Operator {opCode}");
                        }
                        break;

                    case OpCode.GoToOnFalse:
                        {
                            byte b0 = format[++it];
                            byte b1 = format[++it];
                            ushort offset = (ushort)(b0 | (b1 << 8));
                            var v1 = stack[top--];
                            if (v1.type == Type.Boolean)
                            {
                                if (!(bool)v1)
                                {
                                    it = offset;
                                    goto head;
                                }
                            }
                            else throw new Exception($"unsuitable type {v1.type} for Operator {opCode}");
                        }
                        break;

                    default: throw new Exception($"unkonow opcode type: {opCode}");
                }
            }

            throw new Exception("should not reach here");
        }
    }
}
