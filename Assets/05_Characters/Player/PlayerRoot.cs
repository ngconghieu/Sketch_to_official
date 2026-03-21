public class PlayerRoot : State
{
    private PlayerContext ctx;
    private Grounded grounded;
    private AirBorne airBorne;

    public PlayerRoot(PlayerContext context)
    {
        ctx = context;
        grounded = new(this, ctx);
        airBorne = new(this, ctx);
    }

    public override State InitialState => grounded;

}

public class Grounded : State
{
    private PlayerContext ctx;

    public Grounded(State parent, PlayerContext ctx) : base(parent)
    {
        this.ctx = ctx;
    }
}

public class Idle : State
{
}

public class Move : State
{
}

public class AirBorne : State
{
    private PlayerContext ctx;

    public AirBorne(State parent, PlayerContext ctx) : base(parent)
    {
        this.ctx = ctx;
    }
}