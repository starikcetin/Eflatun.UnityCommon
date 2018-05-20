using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Eflatun.UnityCommon.Utils.Common;

namespace Eflatun.UnityCommon.Utils.CodePatterns
{
    /// <summary>
    /// Base class for types that follow Static-Object-Enum pattern. <para />
    /// Features: <para />
    /// 1- Keeps a list of all instances which can be accessed via <see cref="All"/> static property. <para />
    /// 2- Enforces a name argument on constructor and assigns it to <see cref="Name"/> instance property. <para />
    /// 3- Enforces a unique name for each instance, throws an exception at constructor if an instance with same name already exists. <para />
    /// 4- Has a <see cref="Parse"/> method which takes a string and returns a single instance by comparing <see cref="Name"/> instance properties. <para />
    /// 5- Overrides <see cref="ToString"/> method to return <see cref="Name"/> instance property instead of type name.
    /// </summary>
    ///
    /// <typeparam name="T">
    /// Type of implementor/inheritor/derived class. <para />
    /// Example declaration: <para />
    /// <code>public class Foo : StaticObjectEnum&lt;Foo&gt;</code>
    /// </typeparam>
    ///
    /// <remarks>
    /// -> Make sure to keep the implementor's constructor private, so no other instance can be created. <para />
    /// -> The All property is type of ReadOnlyCollection for good reasons.
    /// </remarks>
    public abstract class StaticObjectEnum<T> where T : StaticObjectEnum<T>
    {
        private static readonly List<T> _all = new List<T>();

        /// <summary>
        /// All instances <typeparamref name="T"/> has.
        /// </summary>
        public static ReadOnlyCollection<T> All
        {
            get { return _all.AsReadOnly(); }
        }

        /// <summary>
        /// Name of this instance.
        /// </summary>
        public string Name { get; private set; }

        /// <remarks>
        /// Here we have some magic going on. As a nature of static fields, they don't get initialized until the type
        /// is referenced. This is usually not a problem, but when you access the class via a generic base class with
        /// the type parameter, the type itself doesn't get referenced directly, so the static fields don't get
        /// initialized.
        ///
        /// In our case, the <see cref="StringUtils.CastSOE{T}"/> method takes a type
        /// parameter to cast to. And in there we call the Parse method via the generic base class using the type
        /// parameter. But since the type itself has not yet referenced, none of the static fields was being
        /// initialized at the time we call Parse method. So the <see cref="_all"/> list had no members in it.
        ///
        /// But here, we manually run the static constructor on the implementor type itself in the static constructor
        /// of base class. So the fields of implementor type will get initialized when we reference the base generic
        /// type as if we are referencing the implementor type itself.
        /// </remarks>
        static StaticObjectEnum()
        {
            RuntimeHelpers.RunClassConstructor(typeof(T).TypeHandle);
        }

        protected StaticObjectEnum(string name)
        {
            if (_all.Any(a => a.Name == name))
            {
                throw new ArgumentException("An instance with the same name already exists!", "name");
            }

            Name = name;
            _all.Add((T) this);
        }

        /// <summary>
        /// Returns the single instance of <typeparamref name="T"/> whose <see cref="Name"/> equals to <paramref name="name"/>.
        /// </summary>
        public static T Parse(string name)
        {
            return _all.Single(a => a.Name == name);
        }

        #region Overrides of Object

        /// <summary>
        /// Returns <see cref="Name"/> of this instance.
        /// </summary>
        public override string ToString()
        {
            return Name;
        }

        #endregion
    }
}
