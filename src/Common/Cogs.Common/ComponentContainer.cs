using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Cogs.Common
{
	public class ComponentContainer : IComponentContainer
	{
		private readonly Multimap<Type, Type> _mappings = new Multimap<Type, Type>();
		private readonly Dictionary<Type, object> _instances = new Dictionary<Type, object>();

		public void Dispose()
		{
			foreach (Type service in _mappings.Keys)
				RemoveAll(service);
		}

		public void Add<TService, TImplementation>()
			where TImplementation : TService
		{
			_mappings.Add(typeof(TService), typeof(TImplementation));
		}

		public void RemoveAll<T>()
		{
			RemoveAll(typeof(T));
		}

		public void RemoveAll(Type service)
		{
			foreach (object instance in _instances.Values)
			{
				var disposable = instance as IDisposable;

				if (disposable != null)
					disposable.Dispose();
			}

			_instances.Remove(service);
			_mappings.RemoveAll(service);
		}

		public T Get<T>()
		{
			return (T) Get(typeof(T));
		}

		public IEnumerable<T> GetAll<T>()
		{
			return GetAll(typeof(T)).Cast<T>();
		}

		public object Get(Type service)
		{
			return GetAll(service).FirstOrDefault();
		}

		public IEnumerable<object> GetAll(Type service)
		{
			if (!_mappings.ContainsKey(service))
			{
				if (service.IsClass && !service.IsAbstract && !service.IsGenericTypeDefinition)
					_mappings[service].Add(service);
			}

			foreach (Type implementation in _mappings[service])
				yield return ResolveInstance(implementation);
		}

		private object ResolveInstance(Type type)
		{
			if (_instances.ContainsKey(type))
				return _instances[type];

			object instance = CreateNewInstance(type);
			_instances.Add(type, instance);

			return instance;
		}

		private object CreateNewInstance(Type type)
		{
			ConstructorInfo constructor = SelectConstructor(type);
			ParameterInfo[] parameters = constructor.GetParameters();

			var arguments = new object[parameters.Length];

			for (int idx = 0; idx < parameters.Length; idx++)
				arguments[idx] = GetInstanceForParameter(parameters[idx]);

			return constructor.Invoke(arguments);
		}

		private object GetInstanceForParameter(ParameterInfo parameter)
		{
			Type service = parameter.ParameterType;

			if (service.IsArray)
				return GetAllAsArraySlow(service.GetElementType());

			if (service.IsGenericType)
			{
				Type gtd = service.GetGenericTypeDefinition();

				if (typeof(List<>).IsAssignableFrom(gtd))
					return GetAllAsListSlow(service.GetGenericArguments()[0]);

				if (gtd.IsInterface && typeof(ICollection<>).IsAssignableFrom(gtd))
					return GetAllAsListSlow(service.GetGenericArguments()[0]);

				if (gtd.IsInterface && typeof(IEnumerable<>).IsAssignableFrom(gtd))
					return GetAllSlow(service.GetGenericArguments()[0]);
			}

			return Get(service);
		}

		private ConstructorInfo SelectConstructor(Type type)
		{
			return type.GetConstructors().OrderByDescending(c => c.GetParameters().Length).FirstOrDefault();
		}

		private object GetAllAsArraySlow(Type service)
		{
			var method = typeof(Enumerable).GetMethod("ToArray").MakeGenericMethod(service);
			return method.Invoke(null, new[] { GetAllSlow(service) });
		}

		private object GetAllAsListSlow(Type service)
		{
			var method = typeof(Enumerable).GetMethod("ToList").MakeGenericMethod(service);
			return method.Invoke(null, new[] { GetAllSlow(service) });
		}

		private object GetAllSlow(Type service)
		{
			var method = GetType().GetMethod("GetAll", Type.EmptyTypes).MakeGenericMethod(service);
			return method.Invoke(this, null);
		}
	}
}