using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;

//피부
public struct Skin
{
    public float r, g, b;
};

//눈
public struct Eye
{
    public int type;
    public float r, g, b;
}

//입
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

    
    
    //직렬화
    public static byte[] Serialize(object ava)
    {
        AvatarInfo info = (AvatarInfo)ava;


        //스트림에 필요한 메모리 사이즈(Byte)
        int s = sizeof(float) * 3;
        int e = sizeof(int) + sizeof(float) * 3;
        int m = sizeof(int);
        int h = sizeof(int) * 2 + sizeof(float) * 3;
        int c = sizeof(int) * 4;
        MemoryStream ms = new MemoryStream(s + e + m + h + c);

        //각 변수들을 Byte 형식으로 변환, 마지막은 개별 사이즈
        ms.Write(BitConverter.GetBytes(info.skin.r), 0, sizeof(float));
        ms.Write(BitConverter.GetBytes(info.skin.g), 0, sizeof(float));
        ms.Write(BitConverter.GetBytes(info.skin.b), 0, sizeof(float));

        ms.Write(BitConverter.GetBytes(info.eye.type), 0, sizeof(int));
        ms.Write(BitConverter.GetBytes(info.eye.r), 0, sizeof(float));
        ms.Write(BitConverter.GetBytes(info.eye.r), 0, sizeof(float));
        ms.Write(BitConverter.GetBytes(info.eye.r), 0, sizeof(float));

        ms.Write(BitConverter.GetBytes(info.mouse.type), 0, sizeof(int));

        ms.Write(BitConverter.GetBytes(info.hair.front_type), 0, sizeof(int));
        ms.Write(BitConverter.GetBytes(info.hair.back_type), 0, sizeof(int));
        ms.Write(BitConverter.GetBytes(info.hair.r), 0, sizeof(float));
        ms.Write(BitConverter.GetBytes(info.hair.r), 0, sizeof(float));
        ms.Write(BitConverter.GetBytes(info.hair.r), 0, sizeof(float));

        ms.Write(BitConverter.GetBytes(info.cloth.top), 0, sizeof(int));
        ms.Write(BitConverter.GetBytes(info.cloth.bottom), 0, sizeof(int));
        ms.Write(BitConverter.GetBytes(info.cloth.shoes), 0, sizeof(int));
        ms.Write(BitConverter.GetBytes(info.cloth.acc), 0, sizeof(int));

        return ms.ToArray();
    }

    //역직렬화
    public static object Deserialize(byte[] bytes)
    {
        AvatarInfo info = new AvatarInfo();

        info.skin.r = BitConverter.ToSingle(bytes, 0);
        info.skin.g = BitConverter.ToSingle(bytes, sizeof(float));
        info.skin.b = BitConverter.ToSingle(bytes, sizeof(float) * 2);
        int next = sizeof(float) * 3;

        info.eye.type = BitConverter.ToInt32(bytes, next);          next += sizeof(int);
        info.eye.r = BitConverter.ToSingle(bytes, next);            next += sizeof(float);
        info.eye.g = BitConverter.ToSingle(bytes, next);            next += sizeof(float);
        info.eye.b = BitConverter.ToSingle(bytes, next);            next += sizeof(float);

        info.mouse.type = BitConverter.ToInt32(bytes, next);        next += sizeof(int);

        info.hair.front_type = BitConverter.ToInt32(bytes, next);   next += sizeof(int);
        info.hair.back_type = BitConverter.ToInt32(bytes, next);    next += sizeof(int);
        info.eye.r = BitConverter.ToSingle(bytes, next);            next += sizeof(float);
        info.eye.g = BitConverter.ToSingle(bytes, next);            next += sizeof(float);
        info.eye.b = BitConverter.ToSingle(bytes, next);            next += sizeof(float);

        info.cloth.top = BitConverter.ToInt32(bytes, next);         next += sizeof(int);
        info.cloth.bottom = BitConverter.ToInt32(bytes, next);      next += sizeof(int);
        info.cloth.shoes = BitConverter.ToInt32(bytes, next);       next += sizeof(int);
        info.cloth.acc = BitConverter.ToInt32(bytes, next);

        return info;
    }
    
}

