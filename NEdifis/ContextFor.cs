using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NSubstitute;

namespace NEdifis
{
    public sealed class ContextFor<T>
    {
        private readonly IList<Tuple<string, Type, object>> _ctorParameter = new List<Tuple<string, Type, object>>();

        #region constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ContextFor{T}"/> class.
        /// </summary>
        public ContextFor() : this(null, true) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContextFor{T}"/> class.
        /// </summary>
        /// <param name="info">The constructor information</param>
        // ReSharper disable once IntroduceOptionalParameters.Global
        public ContextFor(ConstructorInfo info) : this(info, true) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContextFor{T}"/> class.
        /// </summary>
        /// <param name="substituteOptionalParameter"></param>
        // ReSharper disable once IntroduceOptionalParameters.Global
        public ContextFor(bool substituteOptionalParameter) : this(null, substituteOptionalParameter) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContextFor{T}"/> class.
        /// </summary>
        /// <param name="info">The constructor information</param>
        /// <param name="substituteOptionalParameter"></param>
        // ReSharper disable once MemberCanBePrivate.Global
        public ContextFor(ConstructorInfo info, bool substituteOptionalParameter)
        {
            // get the constructor with the most parameters
            var ctor = info ?? GetConstructor(typeof(T));
            InitCtorParameterWith(ctor, substituteOptionalParameter);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContextFor{T}"/> class.
        /// </summary>
        /// <param name="constructorTypes">types for the constructor to use</param>
        public ContextFor(params Type[] constructorTypes)
        {
            var ctor = typeof(T).GetConstructor(constructorTypes);
            InitCtorParameterWith(ctor, true);
        }

        /// <summary>
        /// Initializes the ctor parameter with the given constructor info.
        /// </summary>
        private void InitCtorParameterWith(ConstructorInfo ctor, bool substituteOptionalParameter)
        {
            // add each ctor parameter to an internal dictionary
            foreach (var info in ctor.GetParameters())
                _ctorParameter.Add(
                    new Tuple<string, Type, object>(
                        info.Name.ToLower(),
                        info.ParameterType,
                        CreateCtorInstance(info, substituteOptionalParameter)
                    ));
        }

        private static ConstructorInfo GetConstructor(Type type)
        {
            var ctors = type.GetConstructors();
            switch (ctors.Length)
            {
                case 0:
                    throw new ArgumentException($"Type '{typeof(T)}' has no public constructors.");
                case 1:
                    return ctors[0];
                default:
                    var maxCtorParameter = ctors.Max(c2 => c2.GetParameters().Length);
                    return ctors.First(c1 => c1.GetParameters().Length == maxCtorParameter);
            }
        }

        private static object CreateCtorInstance(ParameterInfo info, bool substituteOptionalParameter)
        {
            if (!substituteOptionalParameter && info.DefaultValue == null) return null;

            if (info.ParameterType.IsInterface) return Substitute.For(new[] { info.ParameterType }, null);

            return CreateInstanceWithSubstitutes(info.ParameterType, substituteOptionalParameter);
        }

        private static object CreateInstanceWithSubstitutes(Type type, bool substituteOptionalParameter)
        {

            // now we can get issues of creation does not work
            try
            {
                // get a constructor
                var ctor = GetConstructor(type);

                // add each ctor parameter to an internal dictionary
                var parameter = ctor
                    .GetParameters()
                    .Select(info =>
                        new Tuple<string, Type, object>(
                            info.Name.ToLower(),
                            info.ParameterType,
                            (!substituteOptionalParameter && info.DefaultValue == null)
                                ? null
                                : info.ParameterType.IsInterface
                                    ? Substitute.For(new[] { info.ParameterType }, null)
                                    : info.ParameterType.IsValueType
                                        ? Activator.CreateInstance(info.ParameterType)
                                        : CreateInstanceWithSubstitutes(info.ParameterType, substituteOptionalParameter)))
                    .ToList();

                return Activator.CreateInstance(type, parameter.Select(t => t.Item3).ToArray());
            }
            catch { return null; }

        }

        #endregion

        /// <summary>
        /// replace a constructor parameter with another instance.
        /// </summary>
        public TK Use<TK>(TK newInstance)
        {
            var tpl = _ctorParameter
                .FirstOrDefault(tuple => typeof(TK) == tuple.Item2);
            if (tpl == null)
                throw new KeyNotFoundException();

            var tplPos = _ctorParameter.IndexOf(tpl);
            _ctorParameter.Remove(tpl);
            var newTpl = new Tuple<string, Type, object>(tpl.Item1, tpl.Item2, newInstance);
            _ctorParameter.Insert(tplPos, newTpl);

            return newInstance;
        }

        /// <summary>
        /// get the substitute for the given type<see cref="TK"/>
        /// </summary>
        public TK For<TK>()
        {
            var tpl =
                _ctorParameter.FirstOrDefault(
                    tuple => typeof(TK) == tuple.Item2);

            if (tpl != null) return (TK)tpl.Item3;
            throw new ArgumentException("given type is not a constructor parameter for " + typeof(T).Name);
        }

        /// <summary>
        /// get the substitute for the <see cref="parameter"/>
        /// </summary>
        public TK For<TK>(string parameter)
        {
            var tpl =
                _ctorParameter.FirstOrDefault(
                    tuple =>
                        string.Compare(tuple.Item1, parameter.ToLower(), StringComparison.OrdinalIgnoreCase) == 0);

            if (tpl != null) return (TK)tpl.Item3;
            throw new ArgumentException("given type is not a constructor parameter for " + typeof(T).Name);
        }

        /// <summary>
        ///  create the SUT with the parameter from the dictionary
        /// </summary>
        public T BuildSut()
        {
            return (T)Activator.CreateInstance(typeof(T), _ctorParameter.Select(t => t.Item3).ToArray());
        }
    }
}
