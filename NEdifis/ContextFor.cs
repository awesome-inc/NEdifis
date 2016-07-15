using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NSubstitute;

namespace NEdifis
{
    /// <summary>
    /// A builder for you 'system/class under test'. 
    /// This effectively reduces test maintenance by leveraging the 'test factory pattern' in a generic, reusable way.
    /// </summary>
    /// <typeparam name="T">The type to test.</typeparam>
    public sealed class ContextFor<T>
    {
        private readonly IList<ParamInfo> _ctorParameter = new List<ParamInfo>();

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
        /// Replace a constructor parameter with another instance.
        /// </summary>
        public TK Use<TK>(TK newInstance)
        {
            var paramInfo = GetParamInfo<TK>();
            ReplaceInstance(paramInfo, newInstance);
            return newInstance;
        }

        /// <summary>
        /// Replace a constructor parameter with another instance.
        /// </summary>
        public TK Use<TK>(TK newInstance, string parameter)
        {
            var paramInfo = GetParamInfo(parameter);
            ReplaceInstance(paramInfo, newInstance);
            return newInstance;
        }


        /// <summary>
        /// Get the substitute for the specified type <typeparamref name="T"/>.
        /// </summary>
        public TK For<TK>()
        {
            var paramInfo = GetParamInfo<TK>();
            return (TK)paramInfo.Instance;
        }

        /// <summary>
        /// Get the substitute for the specified parameter <paramref name="parameter"/>.
        /// </summary>
        public TK For<TK>(string parameter)
        {
            var paramInfo = GetParamInfo(parameter);
            return (TK)paramInfo.Instance;
        }

        /// <summary>
        /// Builds an returns instance for the 'system under test (SUT)', i.e. the class to test.
        /// </summary>
        public T BuildSut()
        {
            return (T)Activator.CreateInstance(typeof(T), _ctorParameter.Select(pi => pi.Instance).ToArray());
        }


        /// <summary>
        /// Initializes the constructor parameters using the specified <see cref="ConstructorInfo"/>.
        /// </summary>
        private void InitCtorParameterWith(ConstructorInfo ctor, bool substituteOptionalParameter)
        {
            // add each ctor parameter to an internal dictionary
            foreach (var info in ctor.GetParameters())
                _ctorParameter.Add(
                    new ParamInfo(
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
                        new ParamInfo(
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

                return Activator.CreateInstance(type, parameter.Select(p => p.Instance).ToArray());
            }
            catch { return null; }

        }

        private ParamInfo GetParamInfo<TK>()
        {
            var paramInfo = _ctorParameter.FirstOrDefault(pi => typeof(TK) == pi.Type);
            if (paramInfo == null)
                throw new ArgumentException(
                    $"The specified type '{typeof(TK).Name}' is not a constructor parameter of '{typeof(T).Name}'.");
            return paramInfo;
        }

        private ParamInfo GetParamInfo(string parameter)
        {
            var paramInfo = _ctorParameter.FirstOrDefault(pi => string.Compare(pi.Name, parameter.ToLower(), StringComparison.OrdinalIgnoreCase) == 0);
            if (paramInfo == null)
                throw new ArgumentException(
                    $"The specified parameter '{parameter}' is not a constructor parameter of '{typeof(T).Name}'.");
            return paramInfo;
        }

        private void ReplaceInstance<TK>(ParamInfo paramInfo, TK newInstance)
        {
            var index = _ctorParameter.IndexOf(paramInfo);
            _ctorParameter.Remove(paramInfo);
            var newTpl = new ParamInfo(paramInfo.Name, paramInfo.Type, newInstance);
            _ctorParameter.Insert(index, newTpl);
        }

        private class ParamInfo
        {
            public ParamInfo(string name, Type type, object instance)
            {
                Name = name;
                Type = type;
                Instance = instance;
            }

            public string Name { get;  }
            public Type Type { get; }
            public object Instance { get; }
        }
    }
}
