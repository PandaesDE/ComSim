using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class UtilTests
{
    [Test]
    public void NumericEntryValidation_Passes()
    {
        Assert.False(Util.UI.isValidNumericEntry(""));
        Assert.False(Util.UI.isValidNumericEntry("a"));
        Assert.False(Util.UI.isValidNumericEntry("asdf"));
        Assert.False(Util.UI.isValidNumericEntry("\nd"));
        Assert.False(Util.UI.isValidNumericEntry("true"));
        Assert.False(Util.UI.isValidNumericEntry("$Ab,"));
        Assert.False(Util.UI.isValidNumericEntry("-"));
        Assert.False(Util.UI.isValidNumericEntry("."));
        Assert.False(Util.UI.isValidNumericEntry("-."));
        Assert.True(Util.UI.isValidNumericEntry("0"));
        Assert.True(Util.UI.isValidNumericEntry(".1"));
        Assert.True(Util.UI.isValidNumericEntry("-.1"));
        Assert.True(Util.UI.isValidNumericEntry("-.01"));
        Assert.True(Util.UI.isValidNumericEntry("10"));
        Assert.True(Util.UI.isValidNumericEntry("10.10"));
    }

    
}
