using System;
using System.Collections.Generic;

namespace Cogs.Common
{
	public interface IComponentContainer : IDisposable
	{
		void Add<TService, TImplementation>()
			where TImplementation : TService;

		void RemoveAll<T>();

		T Get<T>();
		IEnumerable<T> GetAll<T>();

		object Get(Type service);
		IEnumerable<object> GetAll(Type service);
	}
}