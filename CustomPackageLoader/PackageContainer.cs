using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace CustomPackageLoader
{
    public class PackageContainer
    {
        public Assembly Assembly { get; private set; }

        public string Name { get; }

        public bool? IsReady { get; private set; }

        public bool Failed { get; private set; }

        public string FailMessage { get; private set; }

        private static ConcurrentDictionary<string, Assembly> DependentsAssemblies = new ConcurrentDictionary<string, Assembly>(StringComparer.OrdinalIgnoreCase);

        private PackageContainer()
        {
        }

        public PackageContainer(string name, Func<byte[]> getPackage)
        {
            this.Name = name;
            try
            {
                Assembly = Assembly.Load(getPackage.Invoke());
                Failed = false;
                FailMessage = String.Empty;
                IsReady = true;
            }
            catch (Exception ex)
            {
                IsReady = false;
                FailMessage = ex.Message;
                Failed = true;
            }
        }

        public PackageContainer(string name, Func<byte[]> getPackage, params KeyValuePair<string, Func<byte[]>>[] subPackages)
        {
            this.Name = name;
            this.IsReady = null;
            this.Failed = true;
            try
            {
                foreach (var sub in subPackages)
                {
                    if (!DependentsAssemblies.ContainsKey(sub.Key))
                    {
                        DependentsAssemblies.AddOrUpdate(sub.Key, Assembly.Load(sub.Value.Invoke()), (k, v) => Assembly.Load(sub.Value.Invoke()));
                    }
                }
                AppDomain.CurrentDomain.AssemblyResolve += ResolveAssemblyReference;
                Assembly = Assembly.Load(getPackage.Invoke());
                Failed = false;
                FailMessage = String.Empty;
                IsReady = true;
            }
            catch (Exception ex)
            {
                IsReady = false;
                FailMessage = ex.Message;
            }
        }

        private static Assembly ResolveAssemblyReference(object sender, ResolveEventArgs args)
        {
            AssemblyName assemblyName = new AssemblyName(args.Name);
            if (DependentsAssemblies.ContainsKey(assemblyName.Name))
            {
                return DependentsAssemblies[assemblyName.Name];
            }
            return null;
        }

        public override string ToString()
        {
            return $"asm:{Name}";
        }

    }
}
