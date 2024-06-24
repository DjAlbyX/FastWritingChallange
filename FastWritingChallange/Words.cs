using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastWritingChallange
{
    internal abstract class Words
    {
        public virtual string Name { get; set; }

        public List<Words> words;

    }
}
