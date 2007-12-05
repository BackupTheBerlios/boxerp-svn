using System;
using System.Collections.Generic;
using System.Text;

namespace Migrator
{
	public enum ParsingStatus
	{
		BeforeFindingClassDeclaration,
		AfterClassDeclarationLine,
		ClassBody
	}
}
