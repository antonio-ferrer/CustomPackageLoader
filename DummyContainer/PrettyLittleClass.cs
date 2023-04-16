using CustomPackageLoader;
using DummyContainer.Properties;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace DummyContainer
{
    public class PrettyLittleClass
    {
        public int DoSomethingWithDummy1()
        {
            PackageContainer dummy1Pkg = new CustomPackageLoader.PackageContainer("Dummy1", () => Resources.Dummy1);
            PackageLoader.LoadPackage("Dummy1", dummy1Pkg);
            dynamic rDummy1 = PackageLoader.GetInstanceFromPackage("Dummy1", "Dummy1.Sum", new KeyValuePair<Type, object>(typeof(int), 1), new KeyValuePair<Type, object>(typeof(int), 2));
            int v1 = rDummy1.Val;
            Debug.WriteLine($"DoSomethingWithDummy1: {v1}");
            return v1;
        }

        public int DoSomehingWithDummy2AndReturnNine()
        {
            Func<byte[]> getDummy1 = () => Resources.Dummy1;
            KeyValuePair<string, Func<byte[]>> getDummy1Pkg = new KeyValuePair<string, Func<byte[]>>("Dummy1", getDummy1);
            PackageContainer dummy2Pkg = new PackageContainer("Dummy2", () => Resources.Dummy2, getDummy1Pkg);
            PackageLoader.LoadPackage("Dummy2", dummy2Pkg);
            dynamic rDummy2 = PackageLoader.GetInstanceFromPackage("Dummy2", "Dummy2.Multiply", new KeyValuePair<Type, object>(typeof(int), 3), new KeyValuePair<Type, object>(typeof(uint), (uint)3));
            int v1 = rDummy2.Result;
            Debug.WriteLine($"\r\n@@@@@@@@\tDoSomethingWithDummy2: {v1}\r\n");
            return v1;
        }

        public bool DoSomethingCrazyWithDummy3AndReturnBoolean()
        {
            PackageContainer dummy3Pkg = new PackageContainer("Dummy3", () => Resources.Dummy3,
                new KeyValuePair<string, Func<byte[]>>("Dummy1", () => Resources.Dummy1),
                 new KeyValuePair<string, Func<byte[]>>("Dummy2", () => Resources.Dummy2)
            );
            PackageLoader.LoadPackage("Dummy3", dummy3Pkg);
            dynamic v = PackageLoader.GetInstanceFromPackage("Dummy3", "Dummy3.YoNumber", new KeyValuePair<Type, object>(typeof(int), 2), new KeyValuePair<Type, object>(typeof(int), 2));
            int v1 = v.Sum;
            int v2 = v.Multiplied;
            Debug.WriteLine($"\r\n@@@@@@@@\tDoSomethingWithDummy3 v1: {v1}\r\n");
            Debug.WriteLine($"\r\n@@@@@@@@\tDoSomethingWithDummy3 v2: {v2}\r\n");
            return v1 == v2;
        }

    }
}
