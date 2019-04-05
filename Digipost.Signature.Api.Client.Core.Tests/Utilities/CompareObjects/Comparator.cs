﻿using System.Collections.Generic;
using System.Linq;
using KellermanSoftware.CompareNetObjects;

namespace Digipost.Signature.Api.Client.Core.Tests.Utilities.CompareObjects
{
    public class Comparator : IComparator
    {
        public bool AreEqual(object expected, object actual)
        {
            var compareLogic = new CompareLogic();
            compareLogic.Config.MaxMillisecondsDateDifference = 999;
            return compareLogic.Compare(expected, actual).AreEqual;
        }

        public bool AreEqual(object expected, object actual, out IEnumerable<IDifference> differences)
        {
            var compareLogic = new CompareLogic(new ComparisonConfig {MaxDifferences = 5});
            var compareResult = compareLogic.Compare(expected, actual);

            differences = compareResult.Differences.Select(d => new Difference
            {
                PropertyName = d.PropertyName,
                WhatIsCompared = d.GetWhatIsCompared(),
                ExpectedValue = d.Object1Value,
                ActualValue = d.Object2Value
            }).ToList<IDifference>();

            return compareResult.AreEqual;
        }
    }
}
