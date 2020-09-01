using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using Roomba.Systems;
using UnityEngine;
using UnityEngine.TestTools;
using Zenject;

namespace Tests
{
    public class DataIo : SceneTestFixture
    {
        [Test]
        public void DataIoSimplePasses()
        {
            var container = ProjectContext.Instance.Container;

            var repo = container.Resolve<IDataRepository>();
            
            repo.Save("test", 12345);
            repo.Save("test-2", new Vector3(1,2,3).ToString());
            
            Debug.Log(repo.Load<int>("test"));
            Debug.Log(repo.Load<Vector3>("test-2"));
        }

        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        [UnityTest]
        public IEnumerator DataIoWithEnumeratorPasses()
        {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.
            yield return null;
        }
    }
}
