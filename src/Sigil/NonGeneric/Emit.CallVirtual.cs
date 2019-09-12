﻿using System;
using System.Reflection;

namespace Sigil.NonGeneric
{
    public partial class Emit
    {
        /// <summary>
        /// <para>Calls the given method virtually.  Pops its arguments in reverse order (left-most deepest in the stack), and pushes the return value if it is non-void.</para>
        /// <para>The `this` reference should appear before any arguments (deepest in the stack).</para>
        /// <para>The method invoked at runtime is determined by the type of the `this` reference.</para>
        /// <para>If the method invoked shouldn't vary (or if the method is static), use Call instead.</para>
        /// </summary>
        public Emit CallVirtual(MethodInfo method, Type constrained = null, Type[] arglist = null)
        {
            InnerEmit.CallVirtual(method, constrained, arglist);
            return this;
        }
    }
}
