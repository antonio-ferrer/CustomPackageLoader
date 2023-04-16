using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CustomPackageLoader
{
    public  static class PackageLoader
    {
        private static readonly ConcurrentDictionary<string, PackageContainer> _packages = new ConcurrentDictionary<string, PackageContainer>(); 
        private static readonly ConcurrentDictionary<string, ConstructorInfo> _constructors = new ConcurrentDictionary<string, ConstructorInfo>();

        public static void LoadPackage(string packageName, PackageContainer package)
        {
            _packages.AddOrUpdate(packageName, package, (k, v) =>package);
            if(_constructors.Count > 0)
            {
                var affected = _constructors.Keys.Where(k => k.StartsWith(packageName));
                if (affected.Any())
                {
                    foreach(var affectedConstructor in affected)
                    {
                        _constructors.TryRemove(affectedConstructor, out ConstructorInfo deprecatedConstructor);
                    }
                }
            }
        }

        public static object GetInstanceFromPackage(string packageName, string qualifiedTypeName, params KeyValuePair<Type, object>[] contructorParameters)
        {
            if(string.IsNullOrEmpty(packageName))
                throw new ArgumentNullException(nameof(packageName));
            if (string.IsNullOrEmpty(qualifiedTypeName))
                throw new ArgumentNullException(nameof(qualifiedTypeName));
            if(!_packages.ContainsKey(packageName))
                throw new DllNotFoundException(packageName);
            object instance = null;
            
            try
            {
                ConstructorInfo ctor = null;
                var parameters = contructorParameters.Select(p => p.Value).ToArray();
                if (_constructors.ContainsKey(qualifiedTypeName) && _constructors.TryGetValue(qualifiedTypeName, out ctor))
                {
                    instance = ctor.Invoke(parameters);
                }
                else
                {
                    PackageContainer package = _packages[packageName];
                    Type type = null;
                    if (package.Assembly.Modules.FirstOrDefault(m => (type = m.GetType(qualifiedTypeName)) != null) != null)
                    {
                        ctor = type.GetConstructor(contructorParameters.Select(p => p.Key).ToArray());
                        if (ctor != null)
                        {
                            _constructors.TryAdd(qualifiedTypeName, ctor);
                            instance = ctor.Invoke(parameters);
                        }
                    }
                }
            }
            catch
            {
                return instance = null;
            }
            return instance;
        }
    }


}
