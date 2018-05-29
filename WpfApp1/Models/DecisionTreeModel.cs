using WpfApp1.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Models
{
    public class DecisionTreeModel
    {
        public string Attribute;

        public List<DecisionTreeModel> DecisionTreeModels;

        public List<Predicate<string>> Predicates;
    }
}
