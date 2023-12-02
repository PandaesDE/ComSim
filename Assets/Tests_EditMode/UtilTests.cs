/*  Head
 *      Author:             Schneider Erik
 *      1st Supervisor:     Prof.Dr Ralph Lano
 *      2nd Supervisor:     Prof.Dr Matthias Hopf
 *      Project-Title:      ComSim
 *      Bachelor-Title:     "Erschaffung einer digitalen Evolutionssimulation mit Vertiefung auf Sozialverhalten"
 *      University:         Technische Hochschule Nürnberg
 *  
 *  Description:
 *      - generic unit tests
 *  
 *  References:
 *      Scene:
 *          - scene independent
 *      Script:
 *          - 
 *          
 *  Notes:
 *      - To watch:
 *          - https://www.youtube.com/watch?v=qCghhGLUa-Y
 *          - https://www.youtube.com/watch?v=QIFxIRQUQzs
 *          - https://www.youtube.com/watch?v=043EY6H5424
 *  
 *  Sources:
 *      - https://docs.unity.cn/Packages/com.unity.test-framework@1.3/manual/index.html
 */

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
        Assert.False(Util.UI.IsValidNumericEntry(""));
        Assert.False(Util.UI.IsValidNumericEntry("a"));
        Assert.False(Util.UI.IsValidNumericEntry("asdf"));
        Assert.False(Util.UI.IsValidNumericEntry("\nd"));
        Assert.False(Util.UI.IsValidNumericEntry("true"));
        Assert.False(Util.UI.IsValidNumericEntry("$Ab,"));
        Assert.False(Util.UI.IsValidNumericEntry("-"));
        Assert.False(Util.UI.IsValidNumericEntry("."));
        Assert.False(Util.UI.IsValidNumericEntry("-."));
        Assert.True(Util.UI.IsValidNumericEntry("0"));
        Assert.True(Util.UI.IsValidNumericEntry(".1"));
        Assert.True(Util.UI.IsValidNumericEntry("-.1"));
        Assert.True(Util.UI.IsValidNumericEntry("-.01"));
        Assert.True(Util.UI.IsValidNumericEntry("10"));
        Assert.True(Util.UI.IsValidNumericEntry("10.10"));
    }
}
