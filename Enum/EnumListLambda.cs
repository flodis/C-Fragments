using System.Collections;

class IENumDemo
{


    /// <summary>
    /// Create a cosinus table enumerator with 0..360 deg values
    /// </summary>
    private IEnumerable<float> cosdata = new Func<IEnumerable<float>>(() =>
    {
        for (int v = 0; v < 360; v++)
        {
            yield return (float)Math.Cos(v * Math.PI / 180);
        }
    }
        )();

    /// <summary>
    /// Create a cosinus table IEnumerator from the cosdata table
    /// </summary>
    private IEnumerator cosenum = cosdata.GetEnumerator();


    /// <summary>
    /// Demonstrates eternal fetch of one value at a time from an IEnumerator
    /// If end of list the enumerator is reset to the enumeration start
    /// </summary>
    /// <returns></returns>
    private float GetaNum()
    {
        //Advance to next item
        if (!cosenum.MoveNext())
        {
            //End of list - reset and advance to first
            cosenum.Reset();
            cosenum.MoveNext();
        }

        //Return Enum current value
        return cosenum.Current;


    }


    /// <summary>
    /// Basic demo how to construct an IEnumerable from a loop
    /// Can be useful if only a few values at the begining of a collection is normally used
    /// or to deliver continuos chunks of data from a source in a streaming manner.
    /// </summary>
    /// <returns></returns>
    private IEnumerable<float> GetaList()
    {
        for (int v = 0; v < 360; v++)
      
            //Yield value and execution to calling routine
            yield return (float)Math.Cos(v * Math.PI / 180);
        }

    }


}
