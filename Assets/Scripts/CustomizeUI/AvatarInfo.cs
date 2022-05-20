//ÇÇºÎ
struct Skin
{
    public float r, g, b;
};

//´«
struct Eye
{
    public int type;
    public float r, g, b;
}

//ÀÔ
struct Mouse
{
    public int type;
}

struct Hair
{
    public int front_type;
    public int back_type;
    public float r, g, b;
}

struct Cloth
{
    public int top;
    public int bottom;
    public int shoes;
    public int acc;
}

class AvatarInfo
{
    public Skin skin;
    public Eye eye;
    public Mouse mouse;
    public Hair hair;
    public Cloth cloth;

}

