using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NSubstitute;

namespace NEdifis
{
    [TestedBy(typeof(ContextFor_Should))]
    public class ContextFor<T>
    {
        protected readonly IList<Tuple<string, Type, object>> CtorParameter = new List<Tuple<string, Type, object>>();

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
            // get the constructor with the most parameter
            var ctor = info;
            if (ctor == null)
            {
                var ctors = typeof(T).GetConstructors();
                var maxCtorParameter = ctors.Max(c2 => c2.GetParameters().Count());
                ctor = ctors.First(c1 => c1.GetParameters().Count() == maxCtorParameter);
            }

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
                CtorParameter.Add(
                    new Tuple<string, Type, object>(
                        info.Name.ToLower(),
                        info.ParameterType,
                        CreateCtorInstance(substituteOptionalParameter, info)
                    ));
        }

        private object CreateCtorInstance(bool substituteOptionalParameter, ParameterInfo info)
        {
            if (!substituteOptionalParameter && info.DefaultValue == null) return null;

            if (info.ParameterType.IsInterface) return Substitute.For(new[] { info.ParameterType }, null);

            return CreateInstanceWithSubstitutes(info.ParameterType, substituteOptionalParameter);
        }

        private object CreateInstanceWithSubstitutes(Type type, bool substituteOptionalParameter)
        {
            // get a constructor
            var ctors = type.GetConstructors();
            var maxCtorParameter = ctors.Max(c2 => c2.GetParameters().Count());
            var ctor = ctors.First(c1 => c1.GetParameters().Count() == maxCtorParameter);

            // now we can get issues of creation does not work
            try
            {
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
        public virtual TK Use<TK>(TK newInstance)
        {
            var tpl = CtorParameter.FirstOrDefault(tuple => typeof(TK) == tuple.Item2);
            if (tpl == null) throw new KeyNotFoundException();

            var tplPos = CtorParameter.IndexOf(tpl);
            CtorParameter.Remove(tpl);
            var newTpl = new Tuple<string, Type, object>(tpl.Item1, tpl.Item2, newInstance);
            CtorParameter.Insert(tplPos, newTpl);

            return newInstance;
        }

        /// <summary>
        /// get the substitute for the given type<see cref="TK"/>
        /// </summary>
        public virtual TK For<TK>()
        {
            var tpl =
                CtorParameter.FirstOrDefault(
                    tuple => typeof(TK) == tuple.Item2);

            if (tpl != null) return (TK)tpl.Item3;
            throw new ArgumentException("given type is not a constructor parameter for " + typeof(T).Name);
        }

        /// <summary>
        /// get the substitute for the <see cref="parameter"/>
        /// </summary>
        public virtual TK For<TK>(string parameter)
        {
            var tpl =
                CtorParameter.FirstOrDefault(
                    tuple =>
                        string.Compare(tuple.Item1, parameter.ToLower(), StringComparison.OrdinalIgnoreCase) == 0);

            if (tpl != null) return (TK)tpl.Item3;
            throw new ArgumentException("given type is not a constructor parameter for " + typeof(T).Name);
        }

        /// <summary>
        ///  create the SUT with the parameter from the dictionary
        /// </summary>
        public virtual T BuildSut()
        {
            return (T)Activator.CreateInstance(typeof(T), CtorParameter.Select(t => t.Item3).ToArray());
        }
    }
}
