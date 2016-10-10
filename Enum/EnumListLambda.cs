using System.Collections;

class IENumDemo
{

    /// <summary>
    /// Create a cosinus table enumerator with 0..360 deg values
    /// </summary>
    private IEnumerator costable = new Func<List<float>>(() =>
        {
            List<float> nn = new List<float>();
            for (int v = 0; v < 360; v++)
            {
                nn.Add((float)Math.Cos(v * Math.PI / 180));
            }

            return nn;
        }

        )().GetEnumerator();


    /// <summary>
    /// Demonstrates eternal fetch of next value from an IEnumerator
    /// At end of list the enumerator is reset to start of list
    /// </summary>
    /// <returns></returns>
    private float GetaNum()
    {
        //Advance to next item
        if (!costable.MoveNext())
        {
            //End of list - reset and advance to first
            costable.Reset();
            costable.MoveNext();
        }

        //Return Enum current value
        yield return costable.Current;
    
    
    }

}
