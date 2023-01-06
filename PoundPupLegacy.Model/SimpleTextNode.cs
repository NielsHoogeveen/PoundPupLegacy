using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoundPupLegacy.Model;

public interface SimpleTextNode: Node
{
    string Text { get; }
}
