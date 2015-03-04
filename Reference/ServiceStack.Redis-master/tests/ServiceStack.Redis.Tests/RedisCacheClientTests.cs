using NUnit.Framework;
using ServiceStack.Caching;
using ServiceStack.Common.Tests.Models;

namespace ServiceStack.Redis.Tests
{
	[TestFixture]
	public class RedisCacheClientTests
	{
		private ICacheClient cacheClient;

		[SetUp]
		public void OnBeforeEachTest()
		{
			if (cacheClient != null)
				cacheClient.Dispose();

			cacheClient = new RedisClient(TestConfig.SingleHost);
			cacheClient.FlushAll();
		}

		[Test]
		public void Get_non_existant_value_returns_null()
		{
			var model = ModelWithIdAndName.Create(1);
			var cacheKey = model.CreateUrn();
			var existingModel = cacheClient.Get<ModelWithIdAndName>(cacheKey);
			Assert.That(existingModel, Is.Null);
		}


		[Test]
		public void Get_non_existant_generic_value_returns_null()
		{
			var model = ModelWithIdAndName.Create(1);
			var cacheKey = model.CreateUrn();
			var existingModel = cacheClient.Get<ModelWithIdAndName>(cacheKey);
			Assert.That(existingModel, Is.Null);
		}

		[Test]
		public void Can_store_and_get_model()
		{
			var model = ModelWithIdAndName.Create(1);
			var cacheKey = model.CreateUrn();
			cacheClient.Set(cacheKey, model);

			var existingModel = cacheClient.Get<ModelWithIdAndName>(cacheKey);
			ModelWithIdAndName.AssertIsEqual(existingModel, model);
		}


		[Test]
		public void Can_store_null_model()
		{
			cacheClient.Set<ModelWithIdAndName>("test-key", null);
		}
	    
		[Test]
		public void Can_Set_and_Get_key_with_all_byte_values()
		{
			const string key = "bytesKey";

			var value = new byte[256];
			for (var i = 0; i < value.Length; i++)
			{
				value[i] = (byte)i;
			}

			cacheClient.Set(key, value);
			var resultValue = cacheClient.Get<byte[]>(key);

			Assert.That(resultValue, Is.EquivalentTo(value));
		}

        [Test]
        public void Can_Replace_By_Pattern()
        {
            var model = ModelWithIdAndName.Create(1);
            string modelKey = "model:" + model.CreateUrn();
            cacheClient.Add(modelKey, model);

            model = ModelWithIdAndName.Create(2);
            string modelKey2 = "xxmodelxx:" + model.CreateUrn();
            cacheClient.Add(modelKey2, model);

            string s = "this is a string";
            cacheClient.Add("string1", s);

            cacheClient.RemoveByPattern("*model*");

            ModelWithIdAndName result = cacheClient.Get<ModelWithIdAndName>(modelKey);
            Assert.That(result, Is.Null);

            result = cacheClient.Get<ModelWithIdAndName>(modelKey2);
            Assert.That(result, Is.Null);

            string result2 = cacheClient.Get<string>("string1");
            Assert.That(result2, Is.EqualTo(s));

            cacheClient.RemoveByPattern("string*");

            result2 = cacheClient.Get<string>("string1");
            Assert.That(result2, Is.Null);
        }
	}
}
