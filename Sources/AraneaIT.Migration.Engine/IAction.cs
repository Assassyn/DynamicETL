using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AraneaIT.Migration.Engine
{
	public interface IAction : IDisposable
	{
		void Configure(IDictionary<string, string> parametersSet, IEntityDefinition entityDefinition);

        void SetConditions(IEnumerable<Condition> readConditions);
	}
}
