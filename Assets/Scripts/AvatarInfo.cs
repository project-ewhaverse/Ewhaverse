//ÇÇºÎ
public struct Skin
{
    public float r, g, b;
};

//´«
public struct Eye
{
    public int type;
    public float r, g, b;
}

//ÀÔ
public struct Mouse
{
    public int type;
}

public struct Hair
{
    public int front_type;
    public int back_type;
    public float r, g, b;
}

public struct Cloth
{
    public int top;
    public int bottom;
    public int shoes;
    public int acc;
}

public class AvatarInfo
{
    public Skin skin;
    public Eye eye;
    public Mouse mouse;
    public Hair hair;
    public Cloth cloth;

}

