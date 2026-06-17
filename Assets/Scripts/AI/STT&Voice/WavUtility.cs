using System.IO;
using UnityEngine;

public static class WavUtility
{
    public static byte[] FromAudioClip(
        AudioClip clip)
    {
        MemoryStream stream =
            new MemoryStream();

        int samples =
            clip.samples;

        float[] data =
            new float[samples];

        clip.GetData(
            data,
            0);

        byte[] bytes =
            new byte[data.Length * 2];

        int offset = 0;

        foreach (float sample in data)
        {
            short value =
                (short)(sample * 32767);

            System.BitConverter
                .GetBytes(value)
                .CopyTo(
                    bytes,
                    offset);

            offset += 2;
        }

        WriteHeader(
            stream,
            bytes.Length,
            clip.frequency);

        stream.Write(
            bytes,
            0,
            bytes.Length);

        return stream.ToArray();
    }

    static void WriteHeader(
        Stream stream,
        int dataLength,
        int sampleRate)
    {
        BinaryWriter writer =
            new BinaryWriter(stream);

        writer.Write("RIFF".ToCharArray());
        writer.Write(36 + dataLength);
        writer.Write("WAVE".ToCharArray());
        writer.Write("fmt ".ToCharArray());
        writer.Write(16);
        writer.Write((short)1);
        writer.Write((short)1);
        writer.Write(sampleRate);
        writer.Write(sampleRate * 2);
        writer.Write((short)2);
        writer.Write((short)16);
        writer.Write("data".ToCharArray());
        writer.Write(dataLength);
    }
}