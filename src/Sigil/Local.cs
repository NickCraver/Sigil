﻿using Sigil.Impl;
using System;

namespace Sigil
{
    /// <summary>
    /// <para>Represents a variable local to the delegate being created.</para>
    /// <para>To create a Local, call DeclareLocal().</para>
    /// </summary>
    public class Local : IOwned, IDisposable
    {
        /// <summary>
        /// <para>The name of this local.</para>
        /// <para>If one is omitted during creation a random one is created instead.</para>
        /// <para>Names are purely for debugging aid, and will not appear in the generated delegate.</para>
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// The type stored in this local.
        /// </summary>
        public Type LocalType { get; private set; }

        internal TypeOnStack StackType { get; private set; }
        internal ushort Index { get; private set; }

        internal DeclareLocallDelegate LocalDel { get; private set; }

        internal LocalReusableDelegate Reusable { get; private set; }

        private object _Owner;
        object IOwned.Owner { get { return _Owner; } }

        internal int DeclaredAtIndex { get; private set; }
        internal int? ReleasedAtIndex { get; private set; }

        internal Local(object owner, ushort index, Type localType, DeclareLocallDelegate local, string name, LocalReusableDelegate reusable, int declaredAt)
        {
            _Owner = owner;
            LocalDel = local;
            Name = name;

            Index = index;
            LocalType = localType;
            StackType = TypeOnStack.Get(localType);

            Reusable = reusable;

            DeclaredAtIndex = declaredAt;
        }

        internal void SetOwner(object owner)
        {
            if (_Owner != null && owner != null) throw new Exception("Can't set ownership of an owned local");

            _Owner = owner;
        }

        internal void SetReleasedAt(int index)
        {
            if (ReleasedAtIndex.HasValue) throw new Exception("Can't call this method twice");

            ReleasedAtIndex = index;
        }

        /// <summary>
        /// Returns the type and name of this Local, in string form.
        /// </summary>
        public override string ToString()
        {
            return LocalType.FullName + " " + Name;
        }

        /// <summary>
        /// <para>Frees this local.</para>
        /// <para>While not strictly required, freeing a local allows it's index to be reused.</para>
        /// <para>Locals are only eligible for reuse when the new local is exactly the same type.</para>
        /// </summary>
        public void Dispose()
        {
            if (Reusable == null)
            {
                throw new InvalidOperationException(this + " double released");
            }

            Reusable(this);

            Reusable = null;
        }
    }
}
