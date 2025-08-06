using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppOverview.Model
{
    public record Nodes(int id, string label, string department, string type, string description, string color, string owner, string url);
}
