using System;
using Behc.Navigation;
using Behc.Utils;
using UnityEngine;

namespace Features.ProxyLoader
{
    public class ProxyNavigationRegistry : INavigationRegistry
    {
        private readonly string _expectedName;
        private IFactory<object, INavigable> _innerFactory;

        public ProxyNavigationRegistry(string expectedName)
        {
            _expectedName = expectedName;
        }

        public IDisposable Register(string name, IFactory<object, INavigable> navigableFactory)
        {
            if (name != _expectedName)
            {
                Debug.LogError($"ProxyNavigationRegistry: Expected '{_expectedName}' got '{name}'");
                return GenericDisposable.Noop;
            }

            if (_innerFactory != null)
            {
                Debug.LogError($"ProxyNavigationRegistry: Multiple navigables found!");
                return GenericDisposable.Noop;
            }

            _innerFactory = navigableFactory;
            return new GenericDisposable(() => { _innerFactory = null; });
        }

        public INavigable Create(string name, object parameters)
        {
            return _innerFactory.Create(parameters);
        }

        public INavigable Create(object parameters) => Create(_expectedName, parameters);
    }
}