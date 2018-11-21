/// <summary>
/// Workers Class (Collects and Uses Snowballs).
/// </summary>
public class Workers : BaseWorker
{
    /// <summary>
    /// Is Our Workers on Lunch Break
    /// </summary>    
    private bool onLunchBreak = false;

    // The type is the name of the class
    protected Resources R;

    void Start()
    {
        // The type is the name of the class
        R = transform.root.GetComponent<Resources>();
    }

    /// <summary>
    /// Sends the signal to update snowball count
    /// </summary>
    public void CollectSnowBalls()
    {
        // TODO Add code to trigger snowball animation
        R.AddSnowballs();
    }

    /// <summary>
    /// Tells worker elfs to go to lunch break
    /// </summary>
    public void LunchBreak()
    {
        if (onLunchBreak == false) { onLunchBreak = true; }
        else { onLunchBreak = false; }
    }
}