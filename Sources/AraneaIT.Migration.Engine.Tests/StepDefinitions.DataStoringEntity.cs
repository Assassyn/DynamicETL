using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechTalk.SpecFlow;
using Xunit;
using AraneaIT.Migration.Engine;

namespace AraneaIT.MIgration.Engine.Tests
{
	/// <remarks>
	/// Part of steps for DataStoringEntity tests
	/// </remarks>
	[Binding]
	public partial class StepDefinitions
	{
		[Given(@"I have dynamic Entity")]
		public void GivenIHaveDynamicEntity()
		{
			_entity = new Entity();
		}

		[When(@"I add ""(.*)"" value named ""(.*)"" equals to ""(.*)""")]
		public void WhenIAddValueNamedQualsTo(string type, string name, string value )
		{
			_entity.AddProperty(name, type, value);
		}

		[Then(@"the result entity exists")]
		public void ThenTheResultEntityExists()
		{
			Assert.NotNull(_entity);
		}

		[Then(@"the result entity has property called ""(.*)"" of type ""(.*)"" with value ""(.*)""")]
		public void ThenTheResultEntityHasPropertyCalledOfTypeWithValue(string name, string type, string value)
		{
			var convertedValue = Convert.ChangeType(value, Type.GetType(type));
			Assert.NotNull(_entity[name]);

			Assert.True(_entity[name].Equals(convertedValue));
		}

		private Entity _entity;
	}
}
